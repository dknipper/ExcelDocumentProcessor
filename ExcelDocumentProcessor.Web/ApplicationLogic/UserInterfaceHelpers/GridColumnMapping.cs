
namespace ExcelDocumentProcessor.Web.ApplicationLogic.UserInterfaceHelpers
{
    public class GridColumnMapping
    {
        public string DBTableName { get; set; }
        public string FriendlyName { get; set; }
        public bool Hidden { get; set; }
        public long Order { get; set; }
        public long? Width { get; set; }
        public bool Sortable { get; set; }
        public bool Editable { get; set; }
        public string SearchType { get; set; }
        public bool Frozen { get; set; }
        public string DBTableColumnName { get; set; }
    }
}