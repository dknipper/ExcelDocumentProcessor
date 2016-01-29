using System;
using System.Collections.Generic;
using AutoMapper;
using ExcelDocumentProcessor.Business.Enumerations;
using ExcelDocumentProcessor.Data.Entities.Custom;

namespace ExcelDocumentProcessor.Business.Entities
{
    public class BusinessExcelValidationRecord
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string ExcelSheet { get; set; }
        public string ExcelColumn { get; set; }
        public int? MaxStringLength { get; set; }
        public int? ExcelRow { get; set; }
        public string Message { get; set; }
        public string FileName { get; set; }
        public string BadValue { get; set; }
        public BusinessExcelValidationDataType ExcelValidationDataType { get; set; }
        public BusinessExcelValidationResultType ExcelValidationResultType { get; set; }

        public BusinessExcelValidationRecord(){}

        public BusinessExcelValidationRecord(string systemException)
        {
            Message = systemException;
            ExcelValidationResultType = BusinessExcelValidationResultType.SystemException;
        }

        public BusinessExcelValidationRecord(string sheetName, string columnName, BusinessExcelValidationResultType excelValidationErrorType)
        {
            ExcelSheet = sheetName;
            ExcelColumn = columnName;
            ExcelValidationResultType = excelValidationErrorType;
        }

        public BusinessExcelValidationRecord(string sheetName, string columnName, int row, BusinessExcelValidationResultType excelValidationErrorType)
        {
            ExcelSheet = sheetName;
            ExcelColumn = columnName;
            ExcelRow = row;
            ExcelValidationResultType = excelValidationErrorType;
        }

        public BusinessExcelValidationRecord(string sheetName, string columnName, int row, Type dataType, BusinessExcelValidationResultType excelValidationErrorType)
        {
            ExcelSheet = sheetName;
            ExcelColumn = columnName;
            ExcelRow = row;
            ExcelValidationResultType = excelValidationErrorType;

            if (dataType == typeof(DateTime))
            {
                ExcelValidationDataType = BusinessExcelValidationDataType.Date;
            }
            else if (dataType == typeof(bool))
            {
                ExcelValidationDataType = BusinessExcelValidationDataType.Boolean;
            }
            else if (dataType == typeof(DateTimeOffset))
            {
                ExcelValidationDataType = BusinessExcelValidationDataType.DateTimeOffset;
            }
            else if ((dataType == typeof(decimal)) || (dataType == typeof(double)))
            {
                ExcelValidationDataType = BusinessExcelValidationDataType.Decimal;
            }
            else if ((dataType == typeof(short)) || (dataType == typeof(int)) || (dataType == typeof(long)))
            {
                ExcelValidationDataType = BusinessExcelValidationDataType.WholeNumber;
            }
            else if (dataType == typeof(TimeSpan))
            {
                ExcelValidationDataType = BusinessExcelValidationDataType.WholeNumber;
            }
            else if (dataType == typeof(Guid))
            {
                ExcelValidationDataType = BusinessExcelValidationDataType.Guid;
            }
        }

        public BusinessExcelValidationRecord(string sheetName, string columnName, int row, int maxLength, BusinessExcelValidationResultType excelValidationErrorType)
        {
            ExcelSheet = sheetName;
            ExcelColumn = columnName;
            ExcelRow = row;
            ExcelValidationResultType = excelValidationErrorType;
            MaxStringLength = maxLength;
        }

        public static List<BusinessExcelValidationRecord> GetExcelValidationRecordsByCode(string recordCode)
        {
            var dalRecords = DALExcelValidationRecord.GetExcelValidationRecordsByCode(recordCode);
            var businessRecords = Mapper.Map<List<DALExcelValidationRecord>, List<BusinessExcelValidationRecord>>(dalRecords);
            return businessRecords;
        }

        public static List<BusinessExcelValidationRecord> GetExcelValidationRecordsByFileName(string fileName)
        {
            var dalRecords = DALExcelValidationRecord.GetExcelValidationRecordsByFileName(fileName);
            var businessRecords = Mapper.Map<List<DALExcelValidationRecord>, List<BusinessExcelValidationRecord>>(dalRecords);
            return businessRecords;
        }

        public static List<BusinessExcelValidationRecord> GetAllExcelValidationRecords()
        {
            var dalRecords = DALExcelValidationRecord.GetAllExcelValidationRecords();
            var businessRecords = Mapper.Map<List<DALExcelValidationRecord>, List<BusinessExcelValidationRecord>>(dalRecords);
            return businessRecords;
        }
    }
}
