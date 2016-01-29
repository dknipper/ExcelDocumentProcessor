using System.Collections.Generic;
using ExcelDocumentProcessor.Web.ApplicationLogic.Enumerations;
using ExcelDocumentProcessor.Web.ApplicationLogic.UserInterfaceHelpers;
using ExcelDocumentProcessor.Web.Models.Interfaces;

namespace ExcelDocumentProcessor.Web.Models
{
    public class FundModel : IParentModel
    {
        private GridModel _gridModel;
        public WebISGDatabaseType DatabaseType { get; set; }
        public bool HasLoaded { get; set; }

        public string FriendlyName
        {
            get { return "Fund"; }
        }

        public string ControllerName
                                {
            get { return "Fund"; }
                }

        public GridModel GridModel
        {
            get
            {
                _gridModel =
                    _gridModel ?? new GridModel
                        {
                            Tabs = new List<GridTab>
                                        {
                            new GridTab("Master", "quantmastermaster"),
                            new GridTab("Fund", "fundmastermaster"),
                            new GridTab("Client", "clientmastermaster"),
                            new GridTab("All Funds", "mastermasterlist")
                        }
                    };
                return _gridModel;
            }
        }
    }
}