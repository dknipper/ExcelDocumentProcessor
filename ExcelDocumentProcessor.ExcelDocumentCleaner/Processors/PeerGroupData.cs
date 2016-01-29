using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ExcelDocumentProcessor.ExcelDocumentCleaner.Processors
{
    public class PeerGroupData : Processor
    {
        public override DataSet GetDataSetFromExcelTab(DocumentFormat.OpenXml.OpenXmlReader reader, DocumentFormat.OpenXml.Packaging.WorkbookPart workbookPart)
        {
            var rtrn = new DataSet();
            var dataTable = new DataTable(SheetName);
            var dataTableBenchmarkPa = new DataTable(string.Format("{0} Benchmark Set PA", SheetName.Substring(0, 7)));

            dataTableBenchmarkPa.Columns.Add("Equity Benchmark Set");
            dataTableBenchmarkPa.Columns.Add("Set Name");
            dataTableBenchmarkPa.Columns.Add("Set ID");
            dataTableBenchmarkPa.Columns.Add("Lookup Row");

            rtrn.Tables.Add(dataTable);
            rtrn.Tables.Add(dataTableBenchmarkPa);

            while (reader.Read())
            {
                if (reader.ElementType != typeof(Row))
                {
                    continue;
                }

                var row = (Row)reader.LoadCurrentElement();

                if (row.RowIndex >= 3)
                {
                    var blankRow = new List<object>();
                    for (var i = 0; i < dataTable.Columns.Count; i++)
                    {
                        blankRow.Add(null);
                    }
                    dataTable.Rows.Add(blankRow.ToArray());
                }

                var currentSet = string.Empty;
                foreach (var openXmlElement in row.ChildElements)
                {
                    var cell = (Cell) openXmlElement;
                    var withoutNumbers = Regex.Replace(cell.CellReference, "[0-9]", "");
                    var excelColumnNumber = ExcelColumnNameToNumber(withoutNumbers);

                    if (row.RowIndex >= 2 && excelColumnNumber <= 12)
                    {
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
                        else if (row.RowIndex >= 3)
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


                    if (row.RowIndex%3 != 0 || excelColumnNumber < 13 || excelColumnNumber > 28)
                    {
                        continue;
                    }

                    if (excelColumnNumber == 13)
                    {
                        var cellValue = GetCellValue(workbookPart, cell);
                        currentSet = cellValue.Trim();
                    }

                    if (string.IsNullOrEmpty(currentSet))
                    {
                        continue;
                    }

                    var setId = excelColumnNumber - 14;
                    if (setId < 1 || setId > 14)
                    {
                        continue;
                    }

                    var cellValueX = GetCellValue(workbookPart, cell);
                    dataTableBenchmarkPa.Rows.Add(currentSet, cellValueX.Trim(), setId, row.RowIndex);
                }
            }

            DumpEmptyColumns(rtrn);
            DumpEmptyRows(rtrn);

            return rtrn;
        }
    }
}
