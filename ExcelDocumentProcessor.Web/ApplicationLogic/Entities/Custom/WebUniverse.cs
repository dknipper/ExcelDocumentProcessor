
using System.Collections.Generic;

namespace ExcelDocumentProcessor.Web.ApplicationLogic.Entities.Custom
{
    public class WebUniverse
    {
        public string UniverseName { get; set; }
        public List<WebUniverseMap> UniverseMaps { get; set; } 
    }
}
