using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ExcelDocumentProcessor.FileServices.DataContracts
{
    [DataContract]
    public class NeonFSDataTableMetaData
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<NeonFSDataTableColumn> Columns { get; set; }

        [DataMember]
        public NeonFSISGDatabaseType DatabaseType { get; set; }

        [DataMember]
        public NeonFSDataTableMetaData MetaData { get; set; }
    }
}
