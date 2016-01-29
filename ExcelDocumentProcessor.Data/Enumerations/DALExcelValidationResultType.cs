
namespace ExcelDocumentProcessor.Data.Enumerations
{
    public enum DALExcelValidationResultType
    {
        Success = 1,
        RequiredColumnMissing = 2,
        RequiredFieldWithNoValue = 3,
        DataTypeValidation = 4,
        MaximumStringLengthViolation = 5,
        SystemException = 6
    }
}