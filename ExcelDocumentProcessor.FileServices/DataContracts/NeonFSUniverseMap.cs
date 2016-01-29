
using System.Runtime.Serialization;

namespace ExcelDocumentProcessor.FileServices.DataContracts
{
    [DataContract]
    public class NeonFSUniverseMap
    {
        [DataMember]
        public string InputTable { get; set; }

        [DataMember]
        public string UniverseTemplateKey { get; set; }

        [DataMember]
        public NeonFSISGDatabaseType LoadISGDatabase { get; set; }

        [DataMember]
        public bool QuarterInName { get; set; }
    }
}
