using System.Runtime.Serialization;

namespace ExcelDocumentProcessor.FileServices.DataContracts
{
    [DataContract]
    public class NeonFSDataColumnBuilder
    {
        [DataMember]
        public bool AllowDBNull { get; set; }

        [DataMember]
        public string ColumnName { get; set; }

        [DataMember]
        public string DataTypeName { get; set; }

        [DataMember]
        public string DefaultValueString { get; set; }
    }
}