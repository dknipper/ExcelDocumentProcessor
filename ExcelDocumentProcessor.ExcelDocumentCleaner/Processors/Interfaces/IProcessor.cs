using System.Data;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;

namespace ExcelDocumentProcessor.ExcelDocumentCleaner.Processors.Interfaces
{
    public interface IProcessor
    {
        DataSet GetDataSetFromExcelTab(OpenXmlReader reader, WorkbookPart workbookPart);
        string SheetName { get; set; }
    }
}
