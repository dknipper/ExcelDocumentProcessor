using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ExcelDocumentProcessor.Services.DataContracts
{
    [DataContract]
    public class NeonDataTableMetaData
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<NeonDataTableColumn> Columns { get; set; }

        [DataMember]
        public NeonISGDatabaseType DatabaseType { get; set; }

        [DataMember]
        public NeonDataTableMetaData MetaData { get; set; }
    }
}
