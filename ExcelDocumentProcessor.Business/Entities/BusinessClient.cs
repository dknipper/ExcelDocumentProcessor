using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using AutoMapper;
using ExcelDocumentProcessor.Business.Constants;
using ExcelDocumentProcessor.Business.Enumerations;
using ExcelDocumentProcessor.Business.FileProcessor;
using ExcelDocumentProcessor.Data.Entities.Custom;
using ExcelDocumentProcessor.Data.Enumerations;

namespace ExcelDocumentProcessor.Business.Entities
{
    public class BusinessClient
    {
        public int ClientKey { get; set; }
        public string ClientName { get; set; }
        public string RecordStatus { get; set; }
        public string InputTable { get; set; }
        public string OutputTable { get; set; }
        public string TransientTable { get; set; }

        public static List<BusinessClient> GetActiveClients()
        {
            var dalClients = DALClient.GetClients();
            var allClients = Mapper.Map<List<DALClient>, List<BusinessClient>>(dalClients);
            return allClients.Where(x => string.Equals(x.RecordStatus, BusinessClientStatus.Active, StringComparison.CurrentCultureIgnoreCase)).ToList();
        }

        public static void ProcessClientChangeFile(Stream fileStream, List<BusinessDataColumnBuilder> extraColumns, BusinessDataTableMetaData clientMetaData, string uploadCode, string sheetName, string fileName)
        {
            try
            {
                var excelDataSet = ExcelProcessor.GetDataSetFromExcelStream(fileStream);
                if (excelDataSet == null)
                {
                    return;
                }

                var excelDataTable = excelDataSet.Tables[sheetName];
                if (excelDataTable == null)
                {
                    return;
                }

                var clientChangeTable = (excelDataTable.Rows.Cast<DataRow>().Where(row => row.ItemArray.Any(field => !(field is DBNull)))).CopyToDataTable();

                ExcelProcessor.DropEmptyColumns(clientChangeTable);
                ExcelProcessor.AddExtraColumns(clientChangeTable, extraColumns);
                ExcelProcessor.DropUnmatchedColumns(clientChangeTable, clientMetaData);

                var validationErrors = new List<BusinessExcelValidationRecord>();
                validationErrors.AddRange(ExcelProcessor.ValidateRequiredColumns(clientChangeTable, clientMetaData));
                validationErrors.AddRange(ExcelProcessor.ValidateDataTypes(clientChangeTable, clientMetaData));
                validationErrors.AddRange(ExcelProcessor.ValidateRequiredColumns(clientChangeTable, clientMetaData));

                if (validationErrors.Count > 0)
                {
                    ExcelProcessor.WriteExcelValidationErrors(validationErrors, uploadCode, fileName);
                }
                else
                {
                    DynamicEntity.ClearTable(clientMetaData.Name, DALISGDatabaseType.ISGInput);
                    ExcelProcessor.BulkInsert(clientChangeTable, clientMetaData, clientMetaData.Name, BusinessISGDatabaseType.ISGInput);

                    if (clientChangeTable.Columns.Contains("YearQuater") && clientChangeTable.Rows != null && clientChangeTable.Rows.Count > 0)
                    {
                        var yearQuarter = clientChangeTable.Rows[0]["YearQuater"];
                        DALClient.ClientMaintenance(clientMetaData.Name, (yearQuarter == null) ? string.Empty : yearQuarter.ToString());
                    }

                    ExcelProcessor.CheckForExcelValidationSuccess(uploadCode);
                }
                
                excelDataSet.Dispose();
            }
            catch (Exception ex)
            {
                ExcelProcessor.WriteExcelValidationSystemException(ex, uploadCode);
            }
        }
    }
}
