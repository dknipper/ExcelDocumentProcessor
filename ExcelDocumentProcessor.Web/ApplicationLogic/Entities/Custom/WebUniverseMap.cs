
using ExcelDocumentProcessor.Web.ApplicationLogic.Enumerations;

namespace ExcelDocumentProcessor.Web.ApplicationLogic.Entities.Custom
{
    public class WebUniverseMap
    {
        public string InputTable { get; set; }
        public string UniverseTemplateKey { get; set; }
        public WebISGDatabaseType LoadISGDatabase { get; set; }
        public bool QuarterInName { get; set; }
    }
}