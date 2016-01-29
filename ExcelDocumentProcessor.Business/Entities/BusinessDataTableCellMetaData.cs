
namespace ExcelDocumentProcessor.Business.Entities
{
    public class BusinessDataTableCellMetaData
    {
        public string ColumnName { get; set; }
        public bool IsPrimaryKey { get; set; }
        public string TypeName { get; set; }
        public object Value { get; set; }
    }
}
