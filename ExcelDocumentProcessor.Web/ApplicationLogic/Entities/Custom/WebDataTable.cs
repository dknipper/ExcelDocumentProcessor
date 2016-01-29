using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ExcelDocumentProcessor.Web.ApplicationLogic.AppConfig;
using ExcelDocumentProcessor.Web.ApplicationLogic.Enumerations;
using ExcelDocumentProcessor.Web.ApplicationLogic.Proxies.DataService;

namespace ExcelDocumentProcessor.Web.ApplicationLogic.Entities.Custom
{
    public class WebDataTable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<WebDataTableRow> Rows { get; set; }
        public List<WebDataTableColumn> Columns { get; set; }

        public static long GetRowCount(string tableName, FilterDataObject filterData, WebISGDatabaseType databaseType)
        {
            long rtrn;
            using (var serviceClient = new NeonISGDataServiceClient(Configuration.ActiveNeonDataServiceEndpoint))
            {
                var database = Mapper.Map<WebISGDatabaseType, NeonISGDatabaseType>(databaseType);
                rtrn = serviceClient.GetRowCount(tableName, filterData.FilterQuery, database);
            }
            return rtrn;
        }

        public static bool SaveData(string tableName, NeonDataTableRowMetaData row, WebISGDatabaseType databaseType)
        {
            try
            {
                using (var serviceClient = new NeonISGDataServiceClient(Configuration.ActiveNeonDataServiceEndpoint))
                {
                    var neonDatabase = Mapper.Map<WebISGDatabaseType, NeonISGDatabaseType>(databaseType);
                    // ReSharper disable once UnusedVariable
                    var rtrn = serviceClient.SaveRow(tableName, row, neonDatabase);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Unable to save information to database for table {0}", tableName), ex);
            }
        }

        public WebDataTable(string name, string sortIndex, string sortOrder, long pageIndex, long pageSize, FilterDataObject filterData, WebISGDatabaseType databaseType)
        {
            Name = name;
            Rows = new List<WebDataTableRow>();

            // The "Actions" column will be the same across every table
            Columns =
                new List<WebDataTableColumn>
                    {
                        new WebDataTableColumn("act")
                            {
                                ParentTable = name,
                                DatabaseType = databaseType
                            }
                    };

            using (var serviceClient = new NeonISGDataServiceClient(Configuration.ActiveNeonDataServiceEndpoint))
            {
                var database = Mapper.Map<WebISGDatabaseType, NeonISGDatabaseType>(databaseType);
                var rtrn = serviceClient.GetData(Name, sortIndex, sortOrder, pageIndex, pageSize, filterData.FilterQuery, database);

                foreach (var column in rtrn.Columns)
                {
                    Columns.Add(
                        new WebDataTableColumn(column.Name)
                            {
                                IsNullable = column.IsNullable,
                                Length = column.Length,
                                Precision = column.Precision,
                                Scale = column.Scale,
                                TypeName = column.TypeName,
                                ParentTable = name,
                                IsPrimaryKey = column.IsPrimaryKey,
                                IsReadonlyIdentity = column.IsReadonlyIdentity,
                                DatabaseType = databaseType
                            });
                }
                Columns = Columns.OrderBy(x => x.Order).ThenBy(x => x.FriendlyName).ToList();

                foreach (var row in rtrn.Rows)
                {
                    var dataRow = new WebDataTableRow(row.RowId);
                    foreach (var column in Columns.Where(c => !c.Name.Equals("act")))
                    {
                        var cell = row.Cells.FirstOrDefault(x => string.Equals(x.ColumnName, column.Name, StringComparison.CurrentCultureIgnoreCase));
                        var value = (cell != null) ? cell.Value : string.Empty;

                        dataRow.Cells.Add(
                            new WebDataTableCell(column)
                            {
                                Value = value
                            });
                    }
                    Rows.Add(dataRow);
                }
            }
        }
    }
}