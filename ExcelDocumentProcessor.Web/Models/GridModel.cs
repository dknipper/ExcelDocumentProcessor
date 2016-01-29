using System.Collections.Generic;
using ExcelDocumentProcessor.Web.ApplicationLogic.Enumerations;
using ExcelDocumentProcessor.Web.ApplicationLogic.UserInterfaceHelpers;

namespace ExcelDocumentProcessor.Web.Models
{
    public class GridModel
    {
        private List<GridModel> _grids;

        public string ControllerName { get; set; }
        public string JqGridNonEditableColumns { get; set; }
        public string JqGridColumnNames { get; set; }
        public string JqGridColumns { get; set; }
        public string TableName { get; set; }
        public string FriendlyTableName { get; set; }
        public WebISGDatabaseType DatabaseType { get; set; }
        public List<GridTab> Tabs { get; set; }

        public List<GridModel> Grids
        {
            get
            {
                if (_grids == null)
                {
                    _grids = new List<GridModel>();
                    if (Tabs != null)
                    {
                        foreach (var t in Tabs)
                        {
                            _grids.Add(new GridModel()
                            {
                                FriendlyTableName = t.FriendlyName,
                                TableName = t.DBTableName
                            });
                        }
                    }
                }
                return _grids;
            }
            set { _grids = value; }
        }
    }
}