using System;
using System.Data;
using System.Globalization;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelDocumentProcessor.ExcelDocumentCleaner.Processors.Interfaces;

namespace ExcelDocumentProcessor.ExcelDocumentCleaner.Processors
{
    public abstract class Processor : IProcessor
    {
        public string SheetName { get; set; }

        public abstract DataSet GetDataSetFromExcelTab(OpenXmlReader reader, WorkbookPart workbookPart);

        public static WorksheetPart GetWorksheetFromSheetName(WorkbookPart workbookPart, string sheetName)
        {
            var sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetName);
            if (sheet == null)
            {
                return null;
            }
            return workbookPart.GetPartById(sheet.Id) as WorksheetPart;
        }

        protected string GetCellValue(WorkbookPart wbPart, Cell theCell)
        {
            if (theCell == null)
            {
                return null;
            }

            var value = theCell.InnerText;
            if (theCell.DataType == null)
            {
                return value;
            }

            switch (theCell.DataType.Value)
            {
                case (CellValues.SharedString):
                {
                    var stringTable = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                    if (stringTable != null)
                    {
                        value = stringTable.SharedStringTable.ElementAt(int.Parse(value)).InnerText;
                    }
                    break;
                }
                case (CellValues.Boolean):
                {
                    switch (value)
                    {
                        case "0":
                        {
                            value = "FALSE";
                            break;
                        }
                        default:
                        {
                            value = "TRUE";
                            break;
                        }
                    }
                    break;
                }
                case (CellValues.Date):
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        value = DateTime.FromOADate(Convert.ToDouble(value)).ToString(CultureInfo.CurrentCulture);
                    }
                    break;
                }
            }

            return value;
        }

        protected int ExcelColumnNameToNumber(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
            {
                return 0;
            }

            columnName = columnName.ToUpperInvariant();
            var sum = 0;
            foreach (var columnCharInName in columnName.ToCharArray())
            {
                sum *= 26;
                sum += (columnCharInName - 'A' + 1);
            }

            return sum;
        }

        protected void DumpEmptyColumns(DataSet dataSet)
        {
            foreach (DataTable dataTable in dataSet.Tables)
            {
                for (var i = dataTable.Columns.Count - 1; i >= 0; i--)
                {
                    if (dataTable.Columns[i].ColumnName.StartsWith("Column", StringComparison.CurrentCultureIgnoreCase))
                    {
                        dataTable.Columns.Remove(dataTable.Columns[i]);
                    }
                }
            }
        }

        protected void DumpEmptyRows(DataSet dataSet)
        {
            foreach (DataTable dataTable in dataSet.Tables)
            {
                var columnCount = dataTable.Columns.Count;
                for (var i = dataTable.Rows.Count - 1; i >= 0; i--)
                {
                    var allNull = true;
                    for (var j = 0; j < columnCount; j++)
                    {
                        if (dataTable.Rows[i][j] == DBNull.Value)
                        {
                            continue;
                        }
                        allNull = false;
                        break;
                    }
                    if (allNull)
                    {
                        dataTable.Rows[i].Delete();
                    }
                }
                dataTable.AcceptChanges();
            }
        }

        public static void CreateExcelDocument(DataSet dataSet, string excelFilename)
        {
            using (var spreadsheet = SpreadsheetDocument.Create(excelFilename, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = spreadsheet.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();
                var sheets = workbookPart.Workbook.AppendChild(new Sheets());

                uint index = 1;
                foreach (DataTable datatable in dataSet.Tables)
                {
                    var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    var sheet =
                        new Sheet
                        {
                            Id = spreadsheet.WorkbookPart.GetIdOfPart(worksheetPart),
                            SheetId = index++,
                            Name = datatable.TableName
                        };

                    // ReSharper disable once PossiblyMistakenUseOfParamsMethod
                    sheets.Append(sheet);

                    using (var writer = OpenXmlWriter.Create(worksheetPart))
                    {
                        writer.WriteStartElement(new Worksheet());
                        writer.WriteStartElement(new SheetData());

                        writer.WriteStartElement(new Row());
                        foreach (DataColumn column in datatable.Columns)
                        {
                            writer.WriteElement(
                                new Cell
                                {
                                    CellValue = new CellValue(column.ColumnName),
                                    DataType = CellValues.String
                                });
                        }
                        writer.WriteEndElement();

                        foreach (DataRow row in datatable.Rows)
                        {
                            writer.WriteStartElement(new Row());
                            foreach (var val in row.ItemArray)
                            {
                                writer.WriteElement(
                                    new Cell
                                    {
                                        CellValue = new CellValue(val.ToString()),
                                        DataType = CellValues.String
                                    });
                            }
                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }
                }
            }
        }
    }
}
