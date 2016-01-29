using System.Data.Entity.Core.EntityClient;
using System.Web;
using System.Web.Configuration;

namespace ExcelDocumentProcessor.Web.ApplicationLogic.AppConfig
{
    public static class Configuration
    {
        public static string ActiveNeonDataServiceEndpoint
        {
            get
            {
                var rtrn = GetConfigEntry("ActiveNeonDataServiceEndpoint");
                return rtrn;
            }
        }

        public static string ActiveNeonFileServiceEndpoint
        {
            get
            {
                var rtrn = GetConfigEntry("ActiveNeonFileServiceEndpoint");
                return rtrn;
            }
        }

        public static string ExcelSpreadSheetNameForImports
        {
            get
            {
                var rtrn = GetConfigEntry("ExcelSpreadSheetNameForImports");
                return rtrn;
            }
        }

        public static string UIConfigFileLocation
        {
            get
            {
                var rtrn = HttpContext.Current.Server.MapPath(GetConfigEntry("UIConfigFileLocation"));
                return rtrn;
            }
        }

        public static string UIConfigEFConnectionString 
        {
            get
            {
                var entityConnectionStringBuilder =
                    new EntityConnectionStringBuilder
                        {
                            Provider = "System.Data.SQLite",
                            ProviderConnectionString = string.Format("datasource=\"{0}\"", UIConfigFileLocation),
                            Metadata = GetConfigEntry("UIConfigConnectionMetadata")
                        };

                return entityConnectionStringBuilder.ToString();
            }
        }

        private static string GetConfigEntry(string key)
        {
            return WebConfigurationManager.AppSettings[key] ?? string.Empty;
        }
    }
}