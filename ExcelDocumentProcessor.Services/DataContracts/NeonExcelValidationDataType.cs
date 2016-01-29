
using System.Runtime.Serialization;

namespace ExcelDocumentProcessor.Services.DataContracts
{
    [DataContract]
    public enum NeonExcelValidationDataType
    {
        [EnumMember]
        NotSet = 0,

        [EnumMember]
        Date = 1,

        [EnumMember]
        Boolean = 2,

        [EnumMember]
        DateTimeOffset = 3,

        [EnumMember]
        Decimal = 4,

        [EnumMember]
        WholeNumber = 5,

        [EnumMember]
        TimeSpan = 6,

        [EnumMember]
        Guid = 7
    }
}