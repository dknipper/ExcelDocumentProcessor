
using System.Runtime.Serialization;

namespace ExcelDocumentProcessor.FileServices.DataContracts
{
    [DataContract]
    public enum NeonFSISGDatabaseType
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
