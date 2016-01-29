
using ExcelDocumentProcessor.Business.Enumerations;

namespace ExcelDocumentProcessor.Business.Entities
{
    public class BusinessUniverseMap
    {
        public string InputTable { get; set; }
        public string UniverseTemplateKey { get; set; }
        public BusinessISGDatabaseType LoadISGDatabase { get; set; }
        public bool QuarterInName { get; set; }
    }
}
