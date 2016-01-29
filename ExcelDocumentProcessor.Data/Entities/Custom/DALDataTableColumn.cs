
namespace ExcelDocumentProcessor.Data.Entities.Custom
{
    public class DALDataTableColumn
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public string StrongTypeName { get; set; }
        public short? Precision { get; set; }
        public short? ColumnOrdinal { get; set; }
        public int? Scale { get; set; }
        public short Length { get; set; }
        public bool IsNullable { get; set; }
        public string ParentTable { get; set; }
        public string ParentSourceTable { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsReadOnlyIdentity { get; set; }
        public bool Hidden { get; set; }
    }
}
