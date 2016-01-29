using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ExcelDocumentProcessor.ExcelDocumentCleaner.Processors
{
    public class ParentMigrationVA : Processor
    {
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

                if (row.RowIndex >= 4)
                {
                    var blankRow = new List<object>();
                    for (var i = 0; i < dataTable.Columns.Count; i++)
                    {
                        blankRow.Add(null);
                    }
                    dataTable.Rows.Add(blankRow.ToArray());
                }

                foreach (var openXmlElement in row.ChildElements)
                {
                    var cell = (Cell) openXmlElement;
                    var withoutNumbers = Regex.Replace(cell.CellReference, "[0-9]", "");
                    var excelColumnNumber = ExcelColumnNameToNumber(withoutNumbers);

                    if (row.RowIndex == 2)
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
                    else if (row.RowIndex >= 4)
                    {
                        var cellValue = GetCellValue(workbookPart, cell);

                        if (string.IsNullOrEmpty(cellValue))
                        {
                            continue;
                        }

                        var columnIndex = excelColumnNumber - 1;
                        if (columnIndex < dataTable.Columns.Count)
                        {
                            dataTable.Rows[dataTable.Rows.Count - 1][columnIndex] = cellValue.Trim();
                        }
                    }
                }
            }

            DumpEmptyColumns(rtrn);
            DumpEmptyRows(rtrn);
            
            for (var i = dataTable.Rows.Count - 1; i >= 0; i--)
            {
                var distinctValue = dataTable.Rows[i][1].ToString();
                var name = dataTable.Rows[i][3].ToString();
                if (((distinctValue != "yes" && distinctValue != "no" && !string.IsNullOrEmpty(distinctValue))) || (name.ToLower().Contains("copy and paste")))
                {
                    dataTable.Rows[i].Delete();
                }
            }
            dataTable.AcceptChanges();

            return rtrn;
        }
    }
}
