using System.Text;
using ExcelDocumentProcessor.Web.ApplicationLogic.Enumerations;
using ExcelDocumentProcessor.Web.Models.Interfaces;

namespace ExcelDocumentProcessor.Web.Models
{
    public class HomeModel : IParentModel
    {
        private GridModel _gridModel;

        public WebISGDatabaseType DatabaseType { get; set; }
        public bool HasLoaded { get; set; }

        public string FriendlyName
        {
            get { return "Home"; }
        }

        public string ControllerName
        {
            get { return "Home"; }
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

        public string JqGridColumnNames
        {
            get { return "[\"ProcessName\",\"Status\",\"Start Date\",\"End Date\"]"; }
        }

        public string JqGridColumns
        {
            get
            {
                var retVal = new StringBuilder();
                retVal.Append("[{ \"name\": \"ProcessName\", \"label\": \"ProcessName\", \"sortable\": true, \"width\": \"500px\", \"align\": \"left\"},");
                retVal.Append("{ \"name\": \"Status\", \"label\": \"Status\", \"sortable\": true, \"align\": \"left\"},");
                retVal.Append("{ \"name\": \"Start Date\", \"label\": \"Start Date\", \"sortable\": true, \"align\": \"left\"},");
                retVal.Append("{ \"name\": \"End Date\", \"label\": \"End Date\", \"sortable\": true, \"align\": \"left\"}]");
                return retVal.ToString();
            }
        }
    }
}