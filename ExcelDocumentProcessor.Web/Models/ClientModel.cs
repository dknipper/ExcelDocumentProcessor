using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using ExcelDocumentProcessor.Web.ApplicationLogic.AppConfig;
using ExcelDocumentProcessor.Web.ApplicationLogic.Entities.Custom;
using ExcelDocumentProcessor.Web.ApplicationLogic.Enumerations;
using ExcelDocumentProcessor.Web.ApplicationLogic.UserInterfaceHelpers;
using ExcelDocumentProcessor.Web.Models.Interfaces;

namespace ExcelDocumentProcessor.Web.Models
{
    public class ClientModel : IParentModel
    {
        private GridModel _gridModel;
        private int _selectedClientListItemIndex = -1;
        private IList<SelectListItem> _clientListItems;
        public WebISGDatabaseType DatabaseType { get; set; }
        public bool HasLoaded { get; set; }

        public string FriendlyName
        {
            get { return "Client Edit"; }
        }

        public string ControllerName
        {
            get { return "Client"; }
        }

        public GridModel GridModel
        {
            get
            {
                _gridModel = _gridModel ?? new GridModel();
                return _gridModel;
            }
            set { _gridModel = value; }
        }

        public int SelectClientListItemIndex
        {
            get
            {
                _selectedClientListItemIndex =
                    (_selectedClientListItemIndex == -1)
                        ? SelectLists.DefaultClient
                        : _selectedClientListItemIndex;

                return _selectedClientListItemIndex;
            }
            set
            {
                _selectedClientListItemIndex = value;
            }
        }

        [Display(Name = "Clients:")]
        public IList<SelectListItem> ClientListItems
        {
            get
            {
                _clientListItems = _clientListItems ?? SelectLists.Clients;
                return _clientListItems;
            }
        }

        public WebClient SelectedClient
        {
            get
            {
                return WebCache.Clients.FirstOrDefault(x => x.ClientKey == SelectClientListItemIndex);
            }
        }
    }
}