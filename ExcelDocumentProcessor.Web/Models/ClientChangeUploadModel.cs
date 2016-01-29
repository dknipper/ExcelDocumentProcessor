using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using ExcelDocumentProcessor.Web.ApplicationLogic.AppConfig;
using ExcelDocumentProcessor.Web.ApplicationLogic.Entities.Custom;
using ExcelDocumentProcessor.Web.ApplicationLogic.Enumerations;
using ExcelDocumentProcessor.Web.ApplicationLogic.Proxies.FileService;
using ExcelDocumentProcessor.Web.ApplicationLogic.UserInterfaceHelpers;
using ExcelDocumentProcessor.Web.Models.Interfaces;

namespace ExcelDocumentProcessor.Web.Models
{
    public class ClientChangeUploadModel : UploadModel, IParentModel
    {
        private string _selectedYearQuarterListItemValue = "";
        private IList<SelectListItem> _yearQuarterListItems;
        private int _selectedClientListItemIndex = -1;
        private IList<SelectListItem> _clientListItems;

        public WebISGDatabaseType DatabaseType { get; set; }
        public bool HasLoaded { get; set; }

        public string FriendlyName
        {
            get { return "Client Change Upload"; }
        }

        public string ControllerName
        {
            get { return "ClientChangeUpload"; }
        }

        public override string UploadControlName
        {
            get { return "Client Change File"; }
        }

        [Display(Name = "Year/Quarter:")]
        public IList<SelectListItem> YearQuarters
        {
            get
            {
                _yearQuarterListItems = _yearQuarterListItems ?? SelectLists.YearQuarters;
                return _yearQuarterListItems;
            }
        }

        public string SelectYearQuarterListItemValue
        {
            get
            {
                _selectedYearQuarterListItemValue =
                    (string.IsNullOrEmpty(_selectedYearQuarterListItemValue))
                        ? SelectLists.DefaultYearQuarter
                        : _selectedYearQuarterListItemValue;

                return _selectedYearQuarterListItemValue;
            }
            set
            {
                _selectedYearQuarterListItemValue = value;
            }
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

        public override void UploadFile(HttpPostedFileBase file)
        {
            ValidationErrors.Clear();

            ProcessExcelPreFlightErrorMessages(file);
            if (ValidationErrors.Count != 0 || file == null)
            {
                return;
            }
            
            try
            {
                using (var fileServiceClient = new NeonISGFileServiceClient(Configuration.ActiveNeonFileServiceEndpoint))
                {
                    var extraColumns =
                        new List<WebDataColumnBuilder>
                            {
                                new WebDataColumnBuilder(false, "DataLoaderId", typeof (int), "0"),
                                new WebDataColumnBuilder(false, "DataLoaderDate", typeof (DateTime), DateTime.Now.ToString(CultureInfo.CurrentCulture)),
                                new WebDataColumnBuilder(false, "YearQuater", typeof (string), SelectYearQuarterListItemValue)
                            };

                    UploadCode = Guid.NewGuid().ToString().Substring(0, 6);
                    var NeonExtraColumns = Mapper.Map<List<WebDataColumnBuilder>, List<NeonFSDataColumnBuilder>>(extraColumns);
                    NeonExtraColumns.ForEach(x => x.DataTypeName = x.DataTypeName);

                    var NeonMetaData = Mapper.Map<WebDataTableMetaData, NeonFSDataTableMetaData>(WebCache.IsgMetaData.FirstOrDefault(x => x.DatabaseType == WebISGDatabaseType.ISGInput && string.Equals(SelectedClient.InputTable, x.Name, StringComparison.CurrentCultureIgnoreCase)));
                    fileServiceClient.UploadClientChangeFile(NeonExtraColumns, file.FileName, NeonMetaData, Configuration.ExcelSpreadSheetNameForImports, UploadCode, file.InputStream);
                }
            }
            catch (Exception ex)
            {
                ValidationErrors.Add(string.Format("System Exception: {0}", ex.Message));
            }
        }
    }
}