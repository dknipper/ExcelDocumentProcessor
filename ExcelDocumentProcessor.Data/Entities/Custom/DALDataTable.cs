using System.Collections.Generic;

namespace ExcelDocumentProcessor.Data.Entities.Custom
{
    public class DALDataTable
    {
        public string Name { get; set; }
        public List<DALDataTableRow> Rows { get; set; }
        public List<DALDataTableColumn> Columns { get; set; }
    }
}
