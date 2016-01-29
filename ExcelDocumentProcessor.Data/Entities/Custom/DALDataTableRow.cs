using System.Collections.Generic;

namespace ExcelDocumentProcessor.Data.Entities.Custom
{
    public class DALDataTableRow
    {
        public string RowId { get; set; }
        public List<DALDataTableCell> Cells { get; set; }
    }
}
