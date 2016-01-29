using System.Collections.Generic;

namespace ExcelDocumentProcessor.Web.ApplicationLogic.Entities.Custom
{
    public class WebDataTableRow
    {
        public string RowId { get; set; }

        public string[] CellValueArray
        {
            get
            {
                var rtrn = new List<string> { "" };
                Cells.ForEach(x => rtrn.Add((x.Value == null) ? string.Empty : JsonEncode(x.Value.ToString())));
                return rtrn.ToArray();
            }
        }

        public List<WebDataTableCell> Cells { get; set; }

        public WebDataTableRow(string rowId)
        {
            RowId = rowId;
            Cells = new List<WebDataTableCell>();
        }

        public static string JsonEncode(string encodeMe)
        {
            encodeMe = encodeMe.Replace("\n", "").Replace("\r", "");
            encodeMe = encodeMe.Replace("\\", "\\\\").Replace("\"", "\"\"");
            return encodeMe;
        }
    }
}