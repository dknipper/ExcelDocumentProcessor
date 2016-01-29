using System.Data;
using System.IO;
using System.Windows;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelDocumentProcessor.ExcelDocumentCleaner.Processors;

namespace ExcelDocumentProcessor.ExcelDocumentCleaner
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBrowse_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog =
                new Microsoft.Win32.OpenFileDialog
                    {
                        DefaultExt = ".xlsx", 
                        Filter = "Excel documents (.xlsx)|*.xlsx"
                    };
            
            var result = fileDialog.ShowDialog();

            if (result != null && !result.Value)
            {
                return;
            }

            FileNameTextBox.Text = fileDialog.FileName;
        }

        private void ButtonProcessDocument_Click(object sender, RoutedEventArgs e)
        {
            if (FileNameTextBox.Text == "")
            {
                return;
            }

            var path = Path.GetDirectoryName(FileNameTextBox.Text);
            var fileName = Path.GetFileNameWithoutExtension(FileNameTextBox.Text);
            var newFileName = string.Format("{0}\\{1}_cleaned.xlsx", path, fileName);

            var dataSet = new DataSet("ExcelData");

            using (var spreadsheetDocument = SpreadsheetDocument.Open(FileNameTextBox.Text, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var allSheets = workbookPart.Workbook.Descendants<Sheet>();

                foreach (var sheet in allSheets)
                {
                    var worksheetPart = Processor.GetWorksheetFromSheetName(workbookPart, sheet.Name);

                    using (var reader = OpenXmlReader.Create(worksheetPart))
                    {
                        var processor = ProcessorFactory.GetProcessor(sheet.Name, fileName);

                        if (processor == null)
                        {
                            continue;
                        }

                        var excelData = processor.GetDataSetFromExcelTab(reader, workbookPart);
                        if (excelData != null)
                        {
                             dataSet.Merge(excelData, false);
                        }
                    }
                }
            }

            if (dataSet.Tables.Count != 0)
            {
                Processor.CreateExcelDocument(dataSet, newFileName);
            }
        }
    }
}
