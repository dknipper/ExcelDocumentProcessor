using System.Runtime.Serialization;

namespace ExcelDocumentProcessor.Services.DataContracts
{
    [DataContract]
    public class NeonDataTableColumn
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string TypeName { get; set; }

        [DataMember]
        public string StrongTypeName { get; set; }

        [DataMember]
        public short? Precision { get; set; }

        [DataMember]
        public short? ColumnOrdinal { get; set; }

        [DataMember]
        public int? Scale { get; set; }

        [DataMember]
        public short Length { get; set; }

        [DataMember]
        public bool IsNullable { get; set; }

        [DataMember]
        public string ParentTable { get; set; }

        [DataMember]
        public string ParentSourceTable { get; set; }

        [DataMember]
        public bool IsPrimaryKey { get; set; }

        [DataMember]
        public bool IsReadonlyIdentity { get; set; }

        [DataMember]
        public string ExcelColumn { get; set; }

        [DataMember]
        public string FriendlyName { get; set; }
    }
}
