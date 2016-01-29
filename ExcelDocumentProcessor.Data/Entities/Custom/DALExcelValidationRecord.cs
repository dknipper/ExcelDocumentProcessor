using System;
using System.Collections.Generic;
using System.Linq;
using ExcelDocumentProcessor.Data.Entities.SystemGenerated;
using ExcelDocumentProcessor.Data.Enumerations;

namespace ExcelDocumentProcessor.Data.Entities.Custom
{
    public class DALExcelValidationRecord
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
        public DALExcelValidationDataType ExcelValidationDataType { get; set; }
        public DALExcelValidationResultType ExcelValidationResultType { get; set; }

        public static List<DALExcelValidationRecord> GetExcelValidationRecordsByCode(string recordCode)
        {
            List<DALExcelValidationRecord> rtrn = null;
            using (var context = new ISGAdminEntities())
            {
                var excelValidationRecords = context.ExcelValidationRecords.Where(x => string.Equals(x.Code, recordCode, StringComparison.CurrentCultureIgnoreCase));
                if (excelValidationRecords.Any())
                {
                    rtrn = TransformDBtoDALExcelValidationRecords(excelValidationRecords.ToList());
                }
            }
            return rtrn;
        }

        public static List<DALExcelValidationRecord> GetExcelValidationRecordsByFileName(string fileName)
        {
            List<DALExcelValidationRecord> rtrn = null;
            using (var context = new ISGAdminEntities())
            {
                var excelValidationRecords = context.ExcelValidationRecords.Where(x => string.Equals(x.FileName, fileName, StringComparison.CurrentCultureIgnoreCase));
                if (excelValidationRecords.Any())
                {
                    rtrn = TransformDBtoDALExcelValidationRecords(excelValidationRecords.ToList());
                }
            }
            return rtrn;
        }

        public static List<DALExcelValidationRecord> GetAllExcelValidationRecords()
        {
            List<DALExcelValidationRecord> rtrn = null;
            using (var context = new ISGAdminEntities())
            {
                var excelValidationRecords = context.ExcelValidationRecords;
                if (excelValidationRecords.Any())
                {
                    rtrn = TransformDBtoDALExcelValidationRecords(excelValidationRecords.ToList());
                }
            }
            return rtrn;
        }

        public static void AddExcelValidationRecords(List<DALExcelValidationRecord> records)
        {
            using (var context = new ISGAdminEntities())
            {
                records.ForEach(
                    x => context.ExcelValidationRecords.Add(
                        new ExcelValidationRecord
                            {
                                Code = x.Code,
                                ExcelColumn = x.ExcelColumn,
                                ExcelRow = x.ExcelRow,
                                ExcelSheet = x.ExcelSheet,
                                ExcelValidationDataType = context.ExcelValidationDataTypes.FirstOrDefault(y => y.Id == (int)x.ExcelValidationDataType),
                                ExcelValidationResultType = context.ExcelValidationResultTypes.FirstOrDefault(y => y.Id == (int)x.ExcelValidationResultType),
                                MaxStringLength = x.MaxStringLength,
                                CreationDate = DateTime.Now,
                                Message = x.Message,
                                FileName = x.FileName,
                                BadValue = x.BadValue
                            }));
                context.SaveChanges();
            }
        }

        private static List<DALExcelValidationRecord> TransformDBtoDALExcelValidationRecords(IEnumerable<ExcelValidationRecord> excelValidationRecords)
        {
            List<DALExcelValidationRecord> rtrn = null;
            foreach (var record in excelValidationRecords.ToList())
            {
                rtrn = rtrn ?? new List<DALExcelValidationRecord>();

                var excelValidationDataType =
                    (record.ExcelValidationDataType != null)
                        ? (DALExcelValidationDataType)record.ExcelValidationDataType.Id
                        : DALExcelValidationDataType.NotSet;

                var excelValidationResultType =
                    (record.ExcelValidationResultType != null)
                        ? (DALExcelValidationResultType)record.ExcelValidationResultType.Id
                        : DALExcelValidationResultType.Success;

                rtrn.Add(
                    new DALExcelValidationRecord
                        {
                            Code = record.Code,
                            ExcelColumn = record.ExcelColumn,
                            ExcelRow = record.ExcelRow,
                            ExcelSheet = record.ExcelSheet,
                            ExcelValidationDataType = excelValidationDataType,
                            ExcelValidationResultType = excelValidationResultType,
                            Id = record.Id,
                            MaxStringLength = record.MaxStringLength,
                            FileName = record.FileName,
                            BadValue = record.BadValue
                        });
            }
            return rtrn;
        }
    }
}
