
namespace ExcelDocumentProcessor.Web.ApplicationLogic.Entities.Custom
{
    public class WebDataTableCell
    {
        public WebDataTableColumn Column { get; set; }
        public object Value { get; set; }

        public WebDataTableCell(WebDataTableColumn column)
        {
            Column = column;
        }
    }
}