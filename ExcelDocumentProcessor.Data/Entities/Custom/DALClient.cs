using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using ExcelDocumentProcessor.Data.Entities.SystemGenerated;
using ExcelDocumentProcessor.Data.Enumerations;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Database = ExcelDocumentProcessor.Data.Utilities.Database;

namespace ExcelDocumentProcessor.Data.Entities.Custom
{
    public class DALClient
    {
        public int ClientKey { get; set; }
        public string ClientName { get; set; }
        public string RecordStatus { get; set; }
        public string InputTable { get; set; }
        public string OutputTable { get; set; }
        public string TransientTable { get; set; }

        public static List<DALClient> GetClients()
        {
            List<DALClient> rtrn;
            using (var context = new ISGClientEntities())
            {
                var clients = context.ClientInformations.ToList();
                rtrn = new List<DALClient>();
                clients.ForEach(
                    x =>
                    rtrn.Add(new DALClient
                        {
                            ClientKey = x.client_key,
                            ClientName = x.client_name,
                            RecordStatus = x.record_status,
                            InputTable = x.inputTable,
                            OutputTable = x.outputTable,
                            TransientTable = x.transientTable
                        }));
            }
            return rtrn;
        }

        public static void ClientMaintenance(string tableName, string yearQuarter)
        {
            const string storedProcedureName = "maint";
            const int transformerLogId = 8;

            var parameters =
                new List<DbParameter>
                    {
                        new SqlParameter {ParameterName = "@YearQuater", DbType = DbType.String, Value = yearQuarter},
                        new SqlParameter {ParameterName = "@TransformerLogId", DbType = DbType.Int32, Value = transformerLogId}
                    };

            ClientExecuteStoredProcedure(storedProcedureName, tableName, parameters);
        }

        public static void ClientReport(string tableName, string yearQuarter)
        {
            const string storedProcedureName = "Report";

            var parameters =
                new List<DbParameter>
                    {
                        new SqlParameter {ParameterName = "@YearQuater", DbType = DbType.String, Value = yearQuarter}
                    };

            ClientExecuteStoredProcedure(storedProcedureName, tableName, parameters);
        }

        public static void ClientWriteback(string tableName, string yearQuarter)
        {
            const int transformerLogId = 8;
            const int debugLevel = 0;
            const string storedProcedureName = "writeback";

            var parameters =
                new List<DbParameter>
                    {
                        new SqlParameter {ParameterName = "@YearQuater", DbType = DbType.String, Value = yearQuarter},
                        new SqlParameter {ParameterName = "@TransformerLogId", DbType = DbType.Int32, Value = transformerLogId},
                        new SqlParameter {ParameterName = "@debug_level", DbType = DbType.Int32, Value = debugLevel}
                    };

            ClientExecuteStoredProcedure(storedProcedureName, tableName, parameters);
        }


        private static void ClientExecuteStoredProcedure(string storedProcedureName, string tableName, IEnumerable<DbParameter> parameters)
        {
            var db = new DatabaseProviderFactory().Create(Database.GetDABConnectionKey(DALISGDatabaseType.ISGTransient));
            using (var cmd = db.GetStoredProcCommand(tableName.Replace("_", string.Format("_{0}_", storedProcedureName))))
            {
                foreach (var parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }
                db.ExecuteNonQuery(cmd);
            }
        }
    }
}
