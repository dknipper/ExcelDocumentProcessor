using System.Configuration;
using System.Web;
using System.Web.Hosting;

namespace ExcelDocumentProcessor.Business.AppConfig
{
    public class Configuration
    {
        public static string TemporaryPath
        {
            get
            {
                const string key = "TemporaryDirectory";
                return (HttpContext.Current != null)
                    ? HttpContext.Current.Server.MapPath(GetConfigEntry(key)) 
                    : HostingEnvironment.MapPath(GetConfigEntry(key));
            }
        }

        private static string GetConfigEntry(string key)
        {
            return ConfigurationManager.AppSettings[key] ?? string.Empty;
        }
    }
}