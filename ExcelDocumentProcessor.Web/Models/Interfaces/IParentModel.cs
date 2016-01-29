
using ExcelDocumentProcessor.Web.ApplicationLogic.Enumerations;

namespace ExcelDocumentProcessor.Web.Models.Interfaces
{
    public interface IParentModel
    {
        WebISGDatabaseType DatabaseType { get; set; }
        string FriendlyName { get; }
        string ControllerName { get; }
        bool HasLoaded { get; set; }
    }
}