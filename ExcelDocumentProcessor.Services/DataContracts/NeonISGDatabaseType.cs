
using System.Runtime.Serialization;

namespace ExcelDocumentProcessor.Services.DataContracts
{
    [DataContract]
    public enum NeonISGDatabaseType
    {
        [EnumMember]
        ISGAdmin = 0,

        [EnumMember]
        ISGClient = 1,

        [EnumMember]
        ISGInput = 2,

        [EnumMember]
        ISGOutput = 3,

        [EnumMember]
        ISGTransient = 4
    }
}
