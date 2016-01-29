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
using ExcelDocumentProcessor.Web.Models.Interfaces;

namespace ExcelDocumentProcessor.Web.Models
{
    public class UniverseUploadModel : UploadModel, IParentModel
    {
        private string _selectedUniverseListItemValue = "";
        private IList<SelectListItem> _universeListItems;

        public bool HasLoaded { get; set; }
        public WebISGDatabaseType DatabaseType { get; set; }

        public string FriendlyName
        {
            get { return "Universe File Upload"; }
        }

        public string ControllerName
        {
            get { return "UniverseUpload"; }
        }

        public override string UploadControlName
        {
            get { return "Universe File"; }
        }

        public WebUniverse SelectedUniverse
        {
            get
            {
                return WebCache.Universes.FirstOrDefault(x => string.Equals(x.UniverseName, SelectUniverseListItemValue, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        [Display(Name = "Universes:")]
        public IList<SelectListItem> Universes
        {
            get
            {
                if (_universeListItems == null)
                {
                    _universeListItems = new List<SelectListItem>();
                    foreach (var universe in WebCache.Universes)
                    {
                        _universeListItems.Add(
                            new SelectListItem
                            {
                                Value = universe.UniverseName,
                                Text = universe.UniverseName
                            });
                    }
                }
                return _universeListItems;
            }
        }

        public string SelectUniverseListItemValue
        {
            get
            {
                if (string.Equals(_selectedUniverseListItemValue, string.Empty, StringComparison.CurrentCultureIgnoreCase))
                {
                    var firstUniverse = WebCache.Universes.FirstOrDefault();
                    if (firstUniverse != null)
                    {
                        _selectedUniverseListItemValue = firstUniverse.UniverseName;
                    }
                }
                return _selectedUniverseListItemValue;
            }
            set
            {
                _selectedUniverseListItemValue = value;
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
                    UploadCode = Guid.NewGuid().ToString().Substring(0, 6);
                    var fileSuffix = file.FileName.Split('.').FirstOrDefault();
                    var defaultYearQuarter = UploadCode;
                    if (!string.IsNullOrEmpty(fileSuffix))
                    {
                        defaultYearQuarter = (fileSuffix.Length >= 7) ? fileSuffix.Substring(0, 7) : fileSuffix;
                    }

                    var extraColumns =
                        new List<WebDataColumnBuilder>
                            {
                                new WebDataColumnBuilder(false, "DataLoaderId", typeof (int), "0"),
                                new WebDataColumnBuilder(false, "DataLoaderDate", typeof (DateTime), DateTime.Now.ToString(CultureInfo.CurrentCulture)),
                                new WebDataColumnBuilder(false, "YearQuater", typeof (string), defaultYearQuarter),
                                new WebDataColumnBuilder(false, "CreatedBy", typeof (string), "ISG Web"),
                                new WebDataColumnBuilder(false, "CreatedOn", typeof (DateTime), DateTime.Now.ToString(CultureInfo.CurrentCulture)),
                                new WebDataColumnBuilder(false, "ModifiedBy", typeof (string), "ISG Web"),
                                new WebDataColumnBuilder(false, "ModifiedOn", typeof (DateTime), DateTime.Now.ToString(CultureInfo.CurrentCulture))
                            };
                    
                    var NeonExtraColumns = Mapper.Map<List<WebDataColumnBuilder>,List<NeonFSDataColumnBuilder>>(extraColumns);
                    NeonExtraColumns.ForEach(x => x.DataTypeName = x.DataTypeName);

                    var inputTables = new List<string>();
                    var NeonUniverse = Mapper.Map<WebUniverse,NeonFSUniverse>(SelectedUniverse);
                    NeonUniverse.UniverseMaps.ForEach(x => inputTables.Add(x.InputTable.ToLower()));

                    var NeonMetaData = Mapper.Map<List<WebDataTableMetaData>, List<NeonFSDataTableMetaData>>(WebCache.IsgMetaData.Where(x => inputTables.Contains(x.Name.ToLower())).ToList());
                    fileServiceClient.UploadUniverseFile(NeonExtraColumns, file.FileName, NeonMetaData, NeonUniverse, UploadCode, file.InputStream);
                }
            }
            catch (Exception ex)
            {
                ValidationErrors.Add(string.Format("System Exception: {0}", ex.Message));
            }
        }
    }
}