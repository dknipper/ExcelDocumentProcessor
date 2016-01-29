
using System.Runtime.Serialization;

namespace ExcelDocumentProcessor.Services.DataContracts
{
    [DataContract]
    public class NeonClient
    {
        [DataMember]
        public int ClientKey { get; set; }

        [DataMember]
        public string ClientName { get; set; }

        [DataMember]
        public string RecordStatus { get; set; }

        [DataMember]
        public string InputTable { get; set; }

        [DataMember]
        public string OutputTable { get; set; }

        [DataMember]
        public string TransientTable { get; set; }
    }
}