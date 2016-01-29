using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using AutoMapper;
using ExcelDocumentProcessor.Business.Enumerations;
using ExcelDocumentProcessor.Business.FileProcessor;
using ExcelDocumentProcessor.Data.Entities.Custom;
using ExcelDocumentProcessor.Data.Enumerations;

namespace ExcelDocumentProcessor.Business.Entities
{
    public class BusinessUniverse
    {
        public string UniverseName { get; set; }
        public List<BusinessUniverseMap> UniverseMaps { get; set; }

        public static void ProcessUniverseFile(Stream fileStream, List<BusinessDataColumnBuilder> extraColumns, BusinessUniverse universe, List<BusinessDataTableMetaData> allMetaData, string uploadCode, string fileName)
        {
            try
            {
                var excelDataSet = ExcelProcessor.GetDataSetFromExcelStream(fileStream);
                if (excelDataSet == null)
                {
                    return;
                }

                var yearQuarters = BusinessYearQuarter.GetYearQuarters();

                foreach (var universeMap in universe.UniverseMaps)
                {
                    var database = Mapper.Map<BusinessISGDatabaseType, DALISGDatabaseType>(universeMap.LoadISGDatabase);
                    DynamicEntity.ClearTable(universeMap.InputTable, database);
                }

                for (var i = excelDataSet.Tables.Count - 1; i >= 0; i--)
                {
                    var sheet = excelDataSet.Tables[i];
                    var sheetName = sheet.TableName.ToLower().Replace(" ", "");

                    foreach (var universeMap in universe.UniverseMaps)
                    {
                        if (!universeMap.QuarterInName && (sheetName.Contains(universeMap.UniverseTemplateKey.ToLower())))
                        {
                            var universeTable = (sheet.Rows.Cast<DataRow>().Where(row => row.ItemArray.Any(field => !(field is DBNull)))).CopyToDataTable();
                            if (universeTable == null)
                            {
                                continue;
                            }
                            universeTable.TableName = sheet.TableName;

                            var metaData = allMetaData.FirstOrDefault(x => string.Equals(x.Name, universeMap.InputTable, StringComparison.CurrentCultureIgnoreCase) && x.DatabaseType == universeMap.LoadISGDatabase);
                            AddDropColumns(universeTable, metaData, extraColumns); 
                            ValidateAndProcessColumns(universeTable, metaData, universeMap, uploadCode, fileName);
                        }
                        else
                        {
                            foreach (var yearQuarter in yearQuarters)
                            {
                                if (!(sheetName.Contains(universeMap.UniverseTemplateKey.ToLower()) && sheetName.Contains(yearQuarter.Quarter.ToLower())))
                                {
                                    continue;
                                }

                                var universeTable = (sheet.Rows.Cast<DataRow>().Where(row => row.ItemArray.Any(field => !(field is DBNull)))).CopyToDataTable();
                                if (universeTable == null)
                                {
                                    continue;
                                }
                                universeTable.TableName = sheet.TableName;

                                var metaData = allMetaData.FirstOrDefault(x => string.Equals(x.Name, universeMap.InputTable, StringComparison.CurrentCultureIgnoreCase) && x.DatabaseType == universeMap.LoadISGDatabase);
                                AddDropColumns(universeTable, metaData, extraColumns);

                                if (universeTable.Columns.Contains("YearQuater"))
                                {
                                    foreach (DataRow row in universeTable.Rows)
                                    {
                                        row["YearQuater"] = yearQuarter.Quarter;
                                    }
                                }

                                ValidateAndProcessColumns(universeTable, metaData, universeMap, uploadCode, fileName);
                            }
                        }
                    }
                    excelDataSet.Tables.Remove(sheet);
                }
            }
            catch (Exception ex)
            {
                ExcelProcessor.WriteExcelValidationSystemException(ex, uploadCode);
            }

            ExcelProcessor.CheckForExcelValidationSuccess(uploadCode);
        }

        private static void AddDropColumns(DataTable universeTable, BusinessDataTableMetaData metaData, List<BusinessDataColumnBuilder> extraColumns)
        {
            ExcelProcessor.DropEmptyColumns(universeTable);
            ExcelProcessor.AddExtraColumns(universeTable, extraColumns);
            ExcelProcessor.DropUnmatchedColumns(universeTable, metaData);
        }

        private static void ValidateAndProcessColumns(DataTable universeTable, BusinessDataTableMetaData metaData, BusinessUniverseMap universeMap, string uploadCode, string fileName)
        {
            var validationErrors = new List<BusinessExcelValidationRecord>();
            validationErrors.AddRange(ExcelProcessor.ValidateRequiredColumns(universeTable, metaData));
            validationErrors.AddRange(ExcelProcessor.ValidateDataTypes(universeTable, metaData));
            validationErrors.AddRange(ExcelProcessor.ValidateMaxLength(universeTable, metaData));
            validationErrors.AddRange(ExcelProcessor.ValidateRequiredFields(universeTable, metaData));

            if (validationErrors.Count > 0)
            {
                ExcelProcessor.WriteExcelValidationErrors(validationErrors, uploadCode, fileName);
            }
            else
            {
                ExcelProcessor.BulkInsert(universeTable, metaData, universeMap.InputTable, universeMap.LoadISGDatabase);
            }
        }
    }
}
