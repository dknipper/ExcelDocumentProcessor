using System.Configuration;

namespace ExcelDocumentProcessor.Data.AppConfig
{
    public class Configuration
    {
        public static string ISGAdminConnectionStringKey
        {
            get { return "ISGAdmin"; }
        }
        public static string ISGClientConnectionStringKey
        {
            get { return "ISGClient"; }
        }
        public static string ISGInputConnectionStringKey
        {
            get { return "ISGInput"; }
        }
        public static string ISGOutputConnectionStringKey
        {
            get { return "ISGOutput"; }
        }
        public static string ISGTransientConnectionStringKey
        {
            get { return "ISGTransient"; }
        }

        public static string ISGAdminConnectionString
        {
            get
            {
                return GetConnectionString(ISGAdminConnectionStringKey);
            }
        }

        public static string ISGClientConnectionString
        {
            get
            {
                return GetConnectionString(ISGClientConnectionStringKey);
            }
        }

        public static string ISGInputConnectionString
        {
            get
            {
                return GetConnectionString(ISGInputConnectionStringKey);
            }
        }

        public static string ISGOutputConnectionString
        {
            get
            {
                return GetConnectionString(ISGOutputConnectionStringKey);
            }
        }

        public static string ISGTransientConnectionString
        {
            get
            {
                return GetConnectionString(ISGTransientConnectionStringKey);
            }
        }

        public static string ISGClientConnectionMetadata
        {
            get
            {
                var rtrn = GetConfigEntry("ISGClientConnectionMetadata");
                return rtrn;
            }
        }

        public static string ISGAdminConnectionMetadata
        {
            get
            {
                var rtrn = GetConfigEntry("ISGAdminConnectionMetadata");
                return rtrn;
            }
        }

        public static string ISGOutputConnectionMetadata
        {
            get
            {
                var rtrn = GetConfigEntry("ISGOutputConnectionMetadata");
                return rtrn;
            }
        }

        public static string ISGInputConnectionMetadata
        {
            get
            {
                var rtrn = GetConfigEntry("ISGInputConnectionMetadata");
                return rtrn;
            }
        }

        public static string ISGTransientConnectionMetadata
        {
            get
            {
                var rtrn = GetConfigEntry("ISGTransientConnectionMetadata");
                return rtrn;
            }
        }

        private static string GetConnectionString(string configKey)
        {
            var rtrn = string.Empty;
            if (ConfigurationManager.ConnectionStrings[configKey].ConnectionString != null)
            {
                rtrn = ConfigurationManager.ConnectionStrings[configKey].ConnectionString;
            }
            return rtrn;
        }

        private static string GetConfigEntry(string key)
        {
            return ConfigurationManager.AppSettings[key] ?? string.Empty;
        }
    }
}