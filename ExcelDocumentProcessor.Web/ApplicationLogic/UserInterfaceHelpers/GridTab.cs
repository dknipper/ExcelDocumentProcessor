
namespace ExcelDocumentProcessor.Web.ApplicationLogic.UserInterfaceHelpers
{
    public class GridTab
    {
        private readonly string _friendlyName;
        private readonly string _dbTableName;

        public GridTab(string friendlyName, string dbTableName)
        {
            _friendlyName = friendlyName;
            _dbTableName = dbTableName;
        }

        public string FriendlyName
        {
            get { return _friendlyName; }
        }
        public string DBTableName
        {
            get { return _dbTableName; }
        }
    }
}