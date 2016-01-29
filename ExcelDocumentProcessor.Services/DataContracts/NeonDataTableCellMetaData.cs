using System.Runtime.Serialization;

namespace ExcelDocumentProcessor.Services.DataContracts
{
    [DataContract]
    public class NeonDataTableCellMetaData
    {
        [DataMember]
        public string ColumnName { get; set; }

        [DataMember]
        public bool IsPrimaryKey { get; set; }

        [DataMember]
        public string TypeName { get; set; }

        [DataMember]
        public object Value { get; set; }
    }
}
