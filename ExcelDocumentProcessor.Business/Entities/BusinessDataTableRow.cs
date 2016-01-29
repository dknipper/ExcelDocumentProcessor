using System.Collections.Generic;

namespace ExcelDocumentProcessor.Business.Entities
{
    public class BusinessDataTableRow
    {
        public string RowId { get; set; }
        public List<BusinessDataTableCell> Cells { get; set; }
    }
}
