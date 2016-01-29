using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ExcelDocumentProcessor.ExcelDocumentCleaner.Processors
{
    public class Standard : Processor
    {
        public int HeaderRowIndex { get; set; }

        public int BodyStartRowIndex { get; set; }

        public override DataSet GetDataSetFromExcelTab(DocumentFormat.OpenXml.OpenXmlReader reader, DocumentFormat.OpenXml.Packaging.WorkbookPart workbookPart)
        {
            var rtrn = new DataSet();
            var dataTable = new DataTable(SheetName);
            rtrn.Tables.Add(dataTable);

            while (reader.Read())
            {
                if (reader.ElementType != typeof(Row))
                {
                    continue;
                }

                var row = (Row)reader.LoadCurrentElement();

                if (row.RowIndex >= BodyStartRowIndex)
                {
                    var blankRow = new List<object>();
                    for (var i = 0; i < dataTable.Columns.Count; i++)
                    {
                        blankRow.Add(null);
                    }
                    dataTable.Rows.Add(blankRow.ToArray());
                }

                // ReSharper disable once LoopCanBePartlyConvertedToQuery
                foreach (var openXmlElement in row.ChildElements)
                {
                    var cell = (Cell) openXmlElement;
                    if (row.RowIndex == HeaderRowIndex)
                    {
                        var cellValue = GetCellValue(workbookPart, cell);
                        if (dataTable.Columns.Contains(cellValue))
                        {
                            for (var i = 1; i < int.MaxValue; i++)
                            {
                                var columnName = string.Format("{0}_{1}", cellValue, i);
                                if (dataTable.Columns.Contains(columnName))
                                {
                                    continue;
                                }
                                dataTable.Columns.Add(columnName);
                                break;
                            }
                        }
                        else
                        {
                            dataTable.Columns.Add(cellValue);
                        }
                    }
                    else if (row.RowIndex >= BodyStartRowIndex)
                    {
                        var cellValue = GetCellValue(workbookPart, cell);

                        if (string.IsNullOrEmpty(cellValue))
                        {
                            continue;
                        }

                        var withoutNumbers = Regex.Replace(cell.CellReference, "[0-9]", "");
                        var columnIndex = (ExcelColumnNameToNumber(withoutNumbers) - 1);

                        if (columnIndex < dataTable.Columns.Count)
                        {
                            dataTable.Rows[dataTable.Rows.Count - 1][columnIndex] = cellValue.Trim();
                        }
                    }
                }
            }

            DumpEmptyColumns(rtrn);
            DumpEmptyRows(rtrn);

            return rtrn;
        }
    }
}
