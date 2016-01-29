
using System.Runtime.Serialization;

namespace ExcelDocumentProcessor.Services.DataContracts
{
    [DataContract]
    public enum NeonExcelValidationResultType
    {
        [EnumMember]
        Success = 1,

        [EnumMember]
        RequiredColumnMissing = 2,

        [EnumMember]
        RequiredFieldWithNoValue = 3,

        [EnumMember]
        DataTypeValidation = 4,

        [EnumMember]
        MaximumStringLengthViolation = 5,

        [EnumMember]
        SystemException = 6
    }
}