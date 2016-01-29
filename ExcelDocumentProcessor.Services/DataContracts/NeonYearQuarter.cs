
using System.Runtime.Serialization;

namespace ExcelDocumentProcessor.Services.DataContracts
{
    [DataContract]
    public class NeonYearQuarter
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Quarter { get; set; }

        [DataMember]
        public string LastMonth { get; set; }

        [DataMember]
        public string EndDate { get; set; }
    }
}