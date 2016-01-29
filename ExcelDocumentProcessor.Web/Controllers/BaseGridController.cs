using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using ClosedXML.Excel;
using ExcelDocumentProcessor.Web.ApplicationLogic.AppConfig;
using ExcelDocumentProcessor.Web.ApplicationLogic.Entities.Custom;
using ExcelDocumentProcessor.Web.ApplicationLogic.Enumerations;
using ExcelDocumentProcessor.Web.ApplicationLogic.Proxies.DataService;
using Newtonsoft.Json;

namespace ExcelDocumentProcessor.Web.Controllers
{
    public class BaseGridController : BaseController
    {
        public virtual ActionResult SaveGridData(string tableName, string postData, string databaseType)
        {
            if (tableName == null || postData == null)
            {
                return new EmptyResult();
            }
            var table = JsonConvert.DeserializeObject<string>(tableName);
            var saveData = JsonConvert.DeserializeObject<Dictionary<string, string>>(postData);
            var database = JsonConvert.DeserializeObject<WebISGDatabaseType>(databaseType);

            // Remove any data that does not have a corresponding database column
            saveData.Remove(table + "GridTable_id");
            saveData.Remove("id");
            saveData.Remove("oper");
            saveData.Remove("RowId");

            var metaData = WebCache.IsgMetaData.FirstOrDefault(x => string.Equals(x.Name, table, StringComparison.CurrentCultureIgnoreCase) && x.DatabaseType == database);
            var row = 
                new NeonDataTableRowMetaData
                {
                    Cells = new List<NeonDataTableCellMetaData>()
                };

            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (var val in saveData)
            {
                if (metaData == null)
                {
                    continue;
                }
                var column = metaData.Columns.FirstOrDefault(col => string.Equals(col.Name, val.Key, StringComparison.CurrentCultureIgnoreCase));
                if (column == null)
                {
                    continue;
                }
                var cell = new NeonDataTableCellMetaData
                {
                    ColumnName = column.Name,
                    IsPrimaryKey = column.IsPrimaryKey,
                    TypeName = column.TypeName,
                    Value = val.Value
                };
                row.Cells.Add(cell);
            }
            // ReSharper disable once UnusedVariable
            var success = metaData != null && WebDataTable.SaveData((metaData.SourceName ?? tableName), row, database);

            // TODO: handle the success/failure?
            return new EmptyResult();
        }

        public ActionResult GetGridData(string sidx, string sord, string filters, int page, int rows, string tableName, WebISGDatabaseType databaseType)
        {
            var jsonData = new JsonResult();
            if (string.IsNullOrEmpty(tableName))
            {
                return jsonData;
            }
            var filterData = filters != null ? JsonConvert.DeserializeObject<FilterDataObject>(filters) : new FilterDataObject();

            var rowCount = WebDataTable.GetRowCount(tableName, filterData, databaseType);

            var currentPage = page;
            var pageIndex = currentPage - 1;
            var pageSize = rows != -1 ? rows : rowCount;
            var totalRecords = rowCount;
            var totalPages = (int)Math.Ceiling(totalRecords / (float)pageSize);

            var dataTable = new WebDataTable(tableName, sidx, sord, pageIndex, pageSize, filterData, databaseType);

            var rowData = new
            {
                total = totalPages,
                page = currentPage,
                records = totalRecords,
                rows =
                    (
                        from row in dataTable.Rows
                        select new
                        {
                            id = row.RowId,
                            cell = row.CellValueArray
                        }
                        ).ToArray()
            };

            jsonData = Json(rowData, JsonRequestBehavior.AllowGet);
            return jsonData;
        }

        /// <summary>
        ///  This method is called to allow the user to download the entire table as a CSV file.
        /// </summary>
        /// <returns></returns>
        public FileStreamResult DownloadFullExcel(string id, WebISGDatabaseType databaseType, bool showHiddenColumns = false)
        {
            //Create a workbook & worksheet
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add(id);

            if (!string.IsNullOrEmpty(id))
            {
                var filterData = new FilterDataObject();
                var rowCount = WebDataTable.GetRowCount(id, filterData, databaseType);
                var dataTable = new WebDataTable(id, "", "asc", 0, rowCount, filterData, databaseType);

                // get column headers
                var colHeaders = dataTable.Columns
                    .Where(c => !c.Name.Equals("act") &&
                        (showHiddenColumns || c.Hidden.ToLower() != "true"))
                    .Select(cd => cd.FriendlyName ?? string.Empty)
                    .ToArray();

                // get row data
                var rowsToInsert = new List<Array>();
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var row in dataTable.Rows)
                {
                    rowsToInsert.Add(
                        row.Cells
                        .Where(cd => showHiddenColumns || cd.Column.Hidden.ToLower() != "true")
                        .Select(cd => cd.Value ?? string.Empty)
                        .ToArray()
                    );
                }

                // add data to worksheet
                for (var i = 0; i < colHeaders.Length; i++)
                {
                    ws.Cell(1, i + 1).Value = colHeaders[i];
                }
                // ReSharper disable once UnusedVariable
                var table = ws.Cell(1, 1).InsertTable(rowsToInsert);
                ws.Columns().AdjustToContents(1,99);
            }

            var memoryStream = new MemoryStream();
            wb.SaveAs(memoryStream);
            memoryStream.Flush();
            memoryStream.Position = 0;

            return File(memoryStream, "application/vnd.ms-excel", string.Format("{0}.xlsx", id));
        }

        public ActionResult GetDropdownOptions(string table, string column, WebISGDatabaseType databaseType)
        {
            List<string> distinctValues;
            using (var serviceClient = new NeonISGDataServiceClient(Configuration.ActiveNeonDataServiceEndpoint))
            {
                var database = Mapper.Map<WebISGDatabaseType, NeonISGDatabaseType>(databaseType);
                distinctValues = serviceClient.GetDistinctValues(table, column, database);
            }
            var jsonData = Json(distinctValues, JsonRequestBehavior.AllowGet);
            return jsonData;
        }

    }
}
