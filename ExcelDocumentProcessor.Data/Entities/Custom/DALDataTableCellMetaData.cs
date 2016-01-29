
namespace ExcelDocumentProcessor.Data.Entities.Custom
{
    public class DALDataTableCellMetaData
    {
        public string ColumnName { get; set; }
        public string TypeName { get; set; }
        public bool IsPrimaryKey { get; set; }
        public object Value { get; set; }
    }
}
