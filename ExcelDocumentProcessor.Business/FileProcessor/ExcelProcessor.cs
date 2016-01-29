using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using AutoMapper;
using Excel;
using ExcelDocumentProcessor.Business.AppConfig;
using ExcelDocumentProcessor.Business.Entities;
using ExcelDocumentProcessor.Business.Enumerations;
using ExcelDocumentProcessor.Data.Entities.Custom;
using ExcelDocumentProcessor.Data.Enumerations;

namespace ExcelDocumentProcessor.Business.FileProcessor
{
    public class ExcelProcessor
    {
        public static List<BusinessExcelValidationRecord> ValidateRequiredColumns(DataTable changeTable, BusinessDataTableMetaData metaData)
        {
            var rtrn = new List<BusinessExcelValidationRecord>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var metaColumn in metaData.Columns.Where(x => x.IsNullable == false && x.IsReadonlyIdentity == false))
            {
                if (changeTable.Columns[metaColumn.ExcelColumn] == null && changeTable.Columns[metaColumn.Name] == null)
                {
                    rtrn.Add(new BusinessExcelValidationRecord(changeTable.TableName, metaColumn.FriendlyName, BusinessExcelValidationResultType.RequiredColumnMissing));
                }
            }
            return rtrn;
        }

        public static List<BusinessExcelValidationRecord> ValidateDataTypes(DataTable changeTable, BusinessDataTableMetaData metaData)
        {
            var rtrn = new List<BusinessExcelValidationRecord>();
            for (var i = 0; i < changeTable.Rows.Count; i++)
            {
                var row = changeTable.Rows[i];
                var excelRowIndex = i + 2;

                foreach (DataColumn column in changeTable.Columns)
                {
                    var metaDataColumn = metaData.Columns.FirstOrDefault(x => string.Equals(x.ExcelColumn, column.ColumnName, StringComparison.CurrentCultureIgnoreCase) && x.StrongType != typeof(string));
                    if ((metaDataColumn == null) || ((string.IsNullOrEmpty(row[metaDataColumn.ExcelColumn].ToString()) && metaDataColumn.IsNullable)))
                    {
                        continue;
                    }

                    var valid = true;
                    if (metaDataColumn.StrongType == typeof(DateTime))
                    {
                        DateTime testValue;
                        valid = DateTime.TryParse(row[metaDataColumn.ExcelColumn].ToString(), out testValue);
                    }
                    else if (metaDataColumn.StrongType == typeof(long))
                    {
                        long testValue;
                        row[metaDataColumn.ExcelColumn] = UpdateWholeNumberTemplates(row[metaDataColumn.ExcelColumn].ToString());
                        valid = long.TryParse(row[metaDataColumn.ExcelColumn].ToString().ToLower().Replace("%", ""), NumberStyles.Any, null, out testValue);
                        if (valid && row[metaDataColumn.ExcelColumn].ToString().Contains("%"))
                        {
                            row[metaDataColumn.ExcelColumn] = testValue / 100;
                        }
                    }
                    else if (metaDataColumn.StrongType == typeof(bool))
                    {
                        bool testValue;
                        valid = bool.TryParse(row[metaDataColumn.ExcelColumn].ToString(), out testValue);
                    }
                    else if (metaDataColumn.StrongType == typeof(DateTimeOffset))
                    {
                        DateTimeOffset testValue;
                        valid = DateTimeOffset.TryParse(row[metaDataColumn.ExcelColumn].ToString(), out testValue);
                    }
                    else if (metaDataColumn.StrongType == typeof(decimal))
                    {
                        decimal testValue; 
                        row[metaDataColumn.ExcelColumn] = UpdateDecimalTemplates(row[metaDataColumn.ExcelColumn].ToString());
                        valid = decimal.TryParse(row[metaDataColumn.ExcelColumn].ToString().ToLower().Replace("%", ""), NumberStyles.Any, null, out testValue);
                        if (valid && row[metaDataColumn.ExcelColumn].ToString().Contains("%"))
                        {
                            row[metaDataColumn.ExcelColumn] = testValue / 100;
                        }
                    }
                    else if (metaDataColumn.StrongType == typeof(double))
                    {
                        double testValue;
                        row[metaDataColumn.ExcelColumn] = UpdateDecimalTemplates(row[metaDataColumn.ExcelColumn].ToString());
                        valid = double.TryParse(row[metaDataColumn.ExcelColumn].ToString().ToLower().Replace("%", ""), NumberStyles.Any, null, out testValue); 
                        if (valid && row[metaDataColumn.ExcelColumn].ToString().Contains("%"))
                        {
                            row[metaDataColumn.ExcelColumn] = testValue / 100;
                        }
                    }
                    else if (metaDataColumn.StrongType == typeof(int))
                    {
                        int testValue;
                        row[metaDataColumn.ExcelColumn] = UpdateWholeNumberTemplates(row[metaDataColumn.ExcelColumn].ToString());
                        valid = int.TryParse(row[metaDataColumn.ExcelColumn].ToString().ToLower().Replace("%", ""), NumberStyles.Any, null, out testValue);
                        if (valid && row[metaDataColumn.ExcelColumn].ToString().Contains("%"))
                        {
                            row[metaDataColumn.ExcelColumn] = testValue / 100;
                        }
                    }
                    else if (metaDataColumn.StrongType == typeof(short))
                    {
                        short testValue; 
                        row[metaDataColumn.ExcelColumn] = UpdateWholeNumberTemplates(row[metaDataColumn.ExcelColumn].ToString());
                        valid = short.TryParse(row[metaDataColumn.ExcelColumn].ToString().ToLower().Replace("%", ""), NumberStyles.Any, null, out testValue);
                        if (valid && row[metaDataColumn.ExcelColumn].ToString().Contains("%"))
                        {
                            row[metaDataColumn.ExcelColumn] = testValue / 100;
                        }
                    }
                    else if (metaDataColumn.StrongType == typeof(TimeSpan))
                    {
                        TimeSpan testValue;
                        valid = TimeSpan.TryParse(row[metaDataColumn.ExcelColumn].ToString(), out testValue);
                    }
                    else if (metaDataColumn.StrongType == typeof(Guid))
                    {
                        Guid testValue;
                        valid = Guid.TryParse(row[metaDataColumn.ExcelColumn].ToString(), out testValue);
                    }

                    if (!valid)
                    {
                        rtrn.Add(
                            new BusinessExcelValidationRecord(changeTable.TableName, column.ColumnName, excelRowIndex,metaDataColumn.StrongType,BusinessExcelValidationResultType.DataTypeValidation)
                                {
                                    BadValue = row[metaDataColumn.ExcelColumn].ToString()
                                });
                    }
                }
            }
            return rtrn;
        }

        public static List<BusinessExcelValidationRecord> ValidateRequiredFields(DataTable changeTable, BusinessDataTableMetaData metaData)
        {
            var rtrn = new List<BusinessExcelValidationRecord>();
            for (var i = 0; i < changeTable.Rows.Count; i++)
            {
                var row = changeTable.Rows[i];
                var excelRowIndex = i + 2;

                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (DataColumn column in changeTable.Columns)
                {
                    var metaDataColumn = metaData.Columns.FirstOrDefault(x => string.Equals(x.ExcelColumn, column.ColumnName, StringComparison.CurrentCultureIgnoreCase) && x.IsNullable == false && x.IsReadonlyIdentity == false);
                    if ((metaDataColumn != null) && (string.IsNullOrEmpty(row[metaDataColumn.ExcelColumn].ToString())))
                    {
                        rtrn.Add(new BusinessExcelValidationRecord(changeTable.TableName, column.ColumnName, excelRowIndex, BusinessExcelValidationResultType.RequiredFieldWithNoValue));
                    }
                }
            }
            return rtrn;
        }

        public static List<BusinessExcelValidationRecord> ValidateMaxLength(DataTable changeTable, BusinessDataTableMetaData metaData)
        {
            var rtrn = new List<BusinessExcelValidationRecord>();
            for (var i = 0; i < changeTable.Rows.Count; i++)
            {
                var row = changeTable.Rows[i];
                var excelRowIndex = i + 2;

                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (DataColumn column in changeTable.Columns)
                {
                    var metaDataColumn = metaData.Columns.FirstOrDefault(x => string.Equals(x.ExcelColumn, column.ColumnName, StringComparison.CurrentCultureIgnoreCase) && x.IsNullable == false && x.IsReadonlyIdentity == false && x.StrongType == typeof(string));
                    if ((metaDataColumn != null) && (!string.IsNullOrEmpty(row[metaDataColumn.ExcelColumn].ToString())) && (row[metaDataColumn.ExcelColumn].ToString().Length > metaDataColumn.Length))
                    {
                        rtrn.Add(
                            new BusinessExcelValidationRecord(changeTable.TableName, column.ColumnName, excelRowIndex,BusinessExcelValidationResultType.MaximumStringLengthViolation)
                                {
                                    BadValue = row[metaDataColumn.ExcelColumn].ToString()
                                });
                    }
                }
            }
            return rtrn;
        }

        public static void DropEmptyColumns(DataTable changeTable)
        {
            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (var column in changeTable.Columns.Cast<DataColumn>().ToArray())
            {
                if (changeTable.AsEnumerable().All(dr => dr.IsNull(column)))
                {
                    changeTable.Columns.Remove(column);
                }
            }
        }

        public static void DropUnmatchedColumns(DataTable changeTable, BusinessDataTableMetaData metaData)
        {
            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (var column in changeTable.Columns.Cast<DataColumn>().ToArray())
            {
                var metaDataColumnExists = metaData.Columns.Any(x => string.Equals(x.ExcelColumn, column.ColumnName, StringComparison.CurrentCultureIgnoreCase) || string.Equals(x.Name, column.ColumnName, StringComparison.CurrentCultureIgnoreCase));
                if (!metaDataColumnExists)
                {
                    changeTable.Columns.Remove(column);
                }
            }
        }

        public static void AddExtraColumns(DataTable clientChangeTable, List<BusinessDataColumnBuilder> extraColumns)
        {
            if (extraColumns == null)
            {
                return;
            }

            foreach (var column in extraColumns)
            {
                if (clientChangeTable.Columns.Contains(column.ColumnName))
                {
                    continue;
                }

                var convertedValue = Convert.ChangeType(column.DefaultValue, Type.GetType(column.DataTypeName) ?? typeof(string));
                clientChangeTable.Columns.Add(
                    new DataColumn
                        {
                            AllowDBNull = column.AllowDBNull,
                            DataType = Type.GetType(column.DataTypeName),
                            ColumnName = column.ColumnName,
                            DefaultValue = convertedValue
                        });
            }
        }

        public static void BulkInsert(DataTable changeTable, BusinessDataTableMetaData metaData, string tableName, BusinessISGDatabaseType databaseType)
        {
            var columns = new List<BusinessDataTableColumn>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (DataColumn column in changeTable.Columns)
            {
                var selectedColumn = metaData.Columns.FirstOrDefault(x => string.Equals(x.ExcelColumn, column.ColumnName, StringComparison.CurrentCultureIgnoreCase));

                var isNullable = (selectedColumn == null || selectedColumn.IsNullable);
                var typeName = (selectedColumn != null) ? selectedColumn.StrongType.ToString() : column.DataType.ToString();
                var length = (selectedColumn == null || selectedColumn.Length == 0) ? Convert.ToInt16(-1) : selectedColumn.Length;
                
                var columnName = (selectedColumn != null) ? selectedColumn.Name : column.ColumnName;
                columns.Add(
                    new BusinessDataTableColumn
                    {
                        Name = columnName,
                        IsNullable = isNullable,
                        TypeName = typeName,
                        Length = length
                    });
            }

            var rows = new List<BusinessDataTableRow>();
            foreach (DataRow row in changeTable.Rows)
            {
                var cells = new List<BusinessDataTableCell>();
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var column in columns)
                {
                    var selectedColumn = metaData.Columns.FirstOrDefault(x => string.Equals(x.Name, column.Name, StringComparison.CurrentCultureIgnoreCase));
                    var columnName = (selectedColumn != null && !string.IsNullOrEmpty(selectedColumn.ExcelColumn)) ? selectedColumn.ExcelColumn : column.Name;
                    cells.Add(
                        new BusinessDataTableCell
                        {
                            ColumnName = column.Name,
                            Value = row[columnName].ToString()
                        });
                }

                rows.Add(
                    new BusinessDataTableRow
                    {
                        Cells = cells
                    });
            }

            var businessDataTable =
                new BusinessDataTable
                {
                    Name = tableName,
                    Columns = columns,
                    Rows = rows
                };

            var database = Mapper.Map<BusinessISGDatabaseType, DALISGDatabaseType>(databaseType);
          
            var dalDataTable = Mapper.Map<BusinessDataTable, DALDataTable>(businessDataTable);
            DynamicEntity.BulkInsert(dalDataTable, database);
        }

        public static string UpdateWholeNumberTemplates(string number)
        {
            number = number.Replace(" ", "").ToLower();
            switch (number)
            {
                case ("*---"):
                    {
                        number = "111111";
                        break;
                    }
                case ("**---"):
                    {
                        number = "666666";
                        break;
                    }
                case ("excluded"):
                    {
                        number = "777777";
                        break;
                    }
                case ("index"):
                    {
                        number = "888888";
                        break;
                    }
            }
            return number;
        }

        public static string UpdateDecimalTemplates(string number)
        {
            number = number.Replace(" ", "").ToLower();
            switch (number)
            {
                case ("*---"):
                    {
                        number = "1.11111";
                        break;
                    }
                case ("**---"):
                    {
                        number = "6.66666";
                        break;
                    }
                case ("excluded"):
                    {
                        number = "7.77777";
                        break;
                    }
                case ("index"):
                    {
                        number = "8.88888";
                        break;
                    }
            }
            return number;
        }

        public static DataSet GetDataSetFromExcelStream(Stream fileStream)
        {
            var tempFileName = Guid.NewGuid().ToString().Substring(0, 6);

            const int bufferSize = 2048;
            var buffer = new byte[bufferSize];
            var tempFilePath = string.Format("{0}{1}.xlsx", Configuration.TemporaryPath, tempFileName);

            using (var outputStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
            {
                var bytesRead = fileStream.Read(buffer, 0, bufferSize);
                while (bytesRead > 0)
                {
                    outputStream.Write(buffer, 0, bytesRead);
                    bytesRead = fileStream.Read(buffer, 0, bufferSize);
                }

                outputStream.Close();
                fileStream.Close();
            }

            DataSet excelDataSet;
            using (var stream = File.Open(tempFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream))
                {
                    excelReader.IsFirstRowAsColumnNames = true;
                    excelDataSet = excelReader.AsDataSet();
                }
                stream.Close();
            }
            File.Delete(tempFilePath);
            return excelDataSet;
        }

        public static void WriteExcelValidationSystemException(Exception ex, string uploadCode)
        {
            DALExcelValidationRecord.AddExcelValidationRecords(
                new List<DALExcelValidationRecord>
                    {
                        new DALExcelValidationRecord
                            {
                                Code = uploadCode,
                                ExcelValidationResultType = DALExcelValidationResultType.SystemException,
                                Message = ex.Message
                            }
                    });
        }

        public static void CheckForExcelValidationSuccess(string uploadCode)
        {
            var validationErrorsForCurrentProcess = DALExcelValidationRecord.GetExcelValidationRecordsByCode(uploadCode);
            if (validationErrorsForCurrentProcess == null || validationErrorsForCurrentProcess.Count == 0)
            {
                DALExcelValidationRecord.AddExcelValidationRecords(
                    new List<DALExcelValidationRecord>
                        {
                            new DALExcelValidationRecord
                                {
                                    Code = uploadCode,
                                    ExcelValidationResultType = DALExcelValidationResultType.Success
                                }
                        });
            }
        }

        public static void WriteExcelValidationErrors(List<BusinessExcelValidationRecord> businessExcelValidationRecords, string uploadCode, string fileName)
        {
            businessExcelValidationRecords.ForEach(x => x.Code = uploadCode);
            businessExcelValidationRecords.ForEach(x => x.FileName = fileName);
            var dalValidationErrors = Mapper.Map<List<BusinessExcelValidationRecord>, List<DALExcelValidationRecord>>(businessExcelValidationRecords);
            DALExcelValidationRecord.AddExcelValidationRecords(dalValidationErrors);
        }
    }
}