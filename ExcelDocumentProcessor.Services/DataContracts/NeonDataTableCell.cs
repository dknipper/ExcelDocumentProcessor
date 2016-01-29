using System.Runtime.Serialization;

namespace ExcelDocumentProcessor.Services.DataContracts
{
    [DataContract]
    public class NeonDataTableCell
    {
        [DataMember]
        public string ColumnName { get; set; }

        [DataMember]
        public string Value { get; set; }
    }
}
