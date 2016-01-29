using System.Data.Entity.Core.EntityClient;
using ExcelDocumentProcessor.Data.AppConfig;
using ExcelDocumentProcessor.Data.Enumerations;

namespace ExcelDocumentProcessor.Data.Utilities
{
    public static class Database
    {
        public static string GetConnectionString(DALISGDatabaseType databaseType)
        {
            var rtrn = string.Empty;
            switch (databaseType)
            {
                case (DALISGDatabaseType.ISGAdmin):
                    {
                        rtrn = Configuration.ISGAdminConnectionString;
                        break;
                    }
                case (DALISGDatabaseType.ISGClient):
                    {
                        rtrn = Configuration.ISGClientConnectionString;
                        break;
                    }
                case (DALISGDatabaseType.ISGInput):
                    {
                        rtrn = Configuration.ISGInputConnectionString;
                        break;
                    }
                case (DALISGDatabaseType.ISGOutput):
                    {
                        rtrn = Configuration.ISGOutputConnectionString;
                        break;
                    }
                case (DALISGDatabaseType.ISGTransient):
                    {
                        rtrn = Configuration.ISGTransientConnectionString;
                        break;
                    }
            }
            return rtrn;
        }

        public static string GetEFConnectionString(DALISGDatabaseType databaseType)
        {
            var entityConnectionStringBuilder =
                new EntityConnectionStringBuilder
                {
                    Provider = "System.Data.SqlClient",
                    ProviderConnectionString = GetConnectionString(databaseType),
                    Metadata = GetEFConnectionStringMetadata(databaseType)
                };

            return entityConnectionStringBuilder.ToString();
        }

        public static string GetDABConnectionKey(DALISGDatabaseType databaseType)
        {
            var rtrn = string.Empty;
            switch (databaseType)
            {
                case (DALISGDatabaseType.ISGAdmin):
                    {
                        rtrn = Configuration.ISGAdminConnectionStringKey;
                        break;
                    }
                case (DALISGDatabaseType.ISGClient):
                    {
                        rtrn = Configuration.ISGClientConnectionStringKey;
                        break;
                    }
                case (DALISGDatabaseType.ISGInput):
                    {
                        rtrn = Configuration.ISGInputConnectionStringKey;
                        break;
                    }
                case (DALISGDatabaseType.ISGOutput):
                    {
                        rtrn = Configuration.ISGOutputConnectionStringKey;
                        break;
                    }
                case (DALISGDatabaseType.ISGTransient):
                    {
                        rtrn = Configuration.ISGTransientConnectionStringKey;
                        break;
                    }
            }
            return rtrn;
        }

        public static string GetEFConnectionStringMetadata(DALISGDatabaseType databaseType)
        {
            var rtrn = string.Empty;
            switch (databaseType)
            {
                case (DALISGDatabaseType.ISGInput):
                    {
                        rtrn = Configuration.ISGInputConnectionMetadata;
                        break;
                    }
                case (DALISGDatabaseType.ISGOutput):
                    {
                        rtrn = Configuration.ISGOutputConnectionMetadata;
                        break;
                    }
                case (DALISGDatabaseType.ISGTransient):
                    {
                        rtrn = Configuration.ISGTransientConnectionMetadata;
                        break;
                    }
            }
            return rtrn;
        }
    }
}
