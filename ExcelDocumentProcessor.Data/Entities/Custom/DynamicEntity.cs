using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Dapper;
using ExcelDocumentProcessor.Data.Entities.SystemGenerated;
using ExcelDocumentProcessor.Data.Enumerations;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Database = ExcelDocumentProcessor.Data.Utilities.Database;

namespace ExcelDocumentProcessor.Data.Entities.Custom
{
    public class DynamicEntity
    {
        public static DALDataTable GetData(string tableName, string sortIndex, string sortOrder, long pageIndex, long pageSize, string filterQuery, DALISGDatabaseType databaseType)
        {
            var dataTable =
                new DALDataTable
                    {
                        Name = tableName,
                        Columns = new List<DALDataTableColumn>(),
                        Rows = new List<DALDataTableRow>()
                    };

            using (var context = new ISGOutputEntities(Database.GetEFConnectionString(databaseType)))
            {
                var metaDatas = context.TableMetaData.Where(f => string.Equals(f.tableName, tableName, StringComparison.CurrentCultureIgnoreCase)).ToList();

                if (metaDatas.Any())
                {
                    foreach (var metaData in metaDatas)
                    {
                        dataTable.Columns.Add(
                            new DALDataTableColumn
                            {
                                Name = metaData.columnName,
                                IsNullable = (metaData.columnIsNullable.HasValue && metaData.columnIsNullable == 1),
                                Length = metaData.columnLength,
                                Precision = metaData.columnPrecision,
                                Scale = metaData.columnScale,
                                TypeName = metaData.columnTypeName,
                                ParentTable = tableName,
                                IsPrimaryKey = (metaData.isPrimaryKey == 1)
                            });
                    }

                    sortOrder = (!string.IsNullOrEmpty(sortOrder)) ? sortOrder : "ASC";
                    sortIndex = (!string.IsNullOrEmpty(sortIndex) && dataTable.Columns.Any(x => string.Equals(x.Name, sortIndex, StringComparison.CurrentCultureIgnoreCase))) ? sortIndex : "OSC";

                    var startingRowNum = pageIndex * (pageSize - 1) + 1;
                    var endingRowNum = (pageIndex + 1) * pageSize;

                    var sql = new StringBuilder("");
                    sql.Append("SELECT * FROM ");
                    sql.AppendFormat("(SELECT ROW_NUMBER() OVER (ORDER BY {0} {1}) AS RowNum, * FROM {2} {5}) AS Result WHERE RowNum >= {3} AND RowNum <= {4} ORDER BY {0} {1}", sortIndex, sortOrder, tableName, startingRowNum, endingRowNum, filterQuery);

                    var connection = (SqlConnection)context.Database.Connection;
                    var dataRows = connection.Query(sql.ToString()).ToList();

                    foreach (var row in dataRows)
                    {
                        var dataRow =
                            new DALDataTableRow
                            {
                                Cells = new List<DALDataTableCell>()
                            };

                        var rowId = new StringBuilder(tableName);
                        foreach (var column in dataTable.Columns)
                        {
                            var value = (((IDictionary<string, object>)row)[column.Name]);

                            if (column.IsPrimaryKey && value != null)
                            {
                                var newId = Regex.Replace(value.ToString(), "[^0-9a-zA-Z]+", "");
                                rowId.Append(newId);
                            }

                            dataRow.Cells.Add(
                                new DALDataTableCell
                                {
                                    ColumnName = column.Name,
                                    Value = (value != null) ? value.ToString() : string.Empty
                                });
                        }
                        dataRow.RowId = rowId.ToString();
                        dataTable.Rows.Add(dataRow);
                    }
                }
                else
                {
                    dataTable = null;
                }
            }
            return dataTable;
        }

        public static long GetRowCount(string tableName, string filterQuery, DALISGDatabaseType databaseType)
        {
            long rowCount;
            var db = new DatabaseProviderFactory().Create(Database.GetDABConnectionKey(databaseType));
            using (var cmd = db.GetSqlStringCommand(string.Format("SELECT COUNT(*) FROM {0} {1}", tableName, filterQuery)))
            {
                rowCount = Convert.ToInt64(db.ExecuteScalar(cmd));
            }
            return rowCount;
        }

        public static List<string> GetDistinctValues(string tableName, string columnName, DALISGDatabaseType databaseType)
        {
            List<string> rtrn;

            using (var context = new ISGOutputEntities(Database.GetEFConnectionString(databaseType)))
            {
                var connection = (SqlConnection)context.Database.Connection;
                var queryString = string.Format("SELECT DISTINCT {0} AS DISTINCT_VALUE FROM {1} ORDER BY {0} ASC", columnName, tableName);
                var dataRows = connection.Query(queryString).ToList();

                if (!dataRows.Any())
                {
                    return null;
                }
                rtrn = new List<string>();

                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach(var x in dataRows)
                {
                    if (x.DISTINCT_VALUE != null) { rtrn.Add(Convert.ToString(x.DISTINCT_VALUE)); }
                }
            }
            return rtrn;
        }

        public static void ClearTable(string tableName, DALISGDatabaseType databaseType)
        {
            var db = new DatabaseProviderFactory().Create(Database.GetDABConnectionKey(databaseType));
            using (var cmd = db.GetSqlStringCommand(string.Format("DELETE FROM {0}", tableName)))
            {
                cmd.CommandTimeout = 3000;
                db.ExecuteNonQuery(cmd);
            }
        }

        public static void BulkInsert(DALDataTable dalDataTable, DALISGDatabaseType databaseType)
        {
            var insertTable = new DataTable(dalDataTable.Name);

            foreach (var column in dalDataTable.Columns)
            {
                var dataColumn =
                        new DataColumn(column.Name)
                        {
                            AllowDBNull = column.IsNullable,
                            DataType = Type.GetType(column.TypeName)
                        };

                if (string.Equals(column.TypeName, typeof(string).ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    dataColumn.MaxLength = column.Length;
                }
                insertTable.Columns.Add(dataColumn);
            }

            foreach (var row in dalDataTable.Rows)
            {
                var newRow = insertTable.NewRow();

                foreach (var column in dalDataTable.Columns)
                {
                    var columnName = column.Name;
                    var cell = row.Cells.FirstOrDefault(x => string.Equals(x.ColumnName, columnName, StringComparison.CurrentCultureIgnoreCase));

                    if (cell == null)
                    {
                        continue;
                    }

                    if (!insertTable.Columns[columnName].AllowDBNull && (string.IsNullOrEmpty(cell.Value)))
                    {
                        var type = insertTable.Columns[columnName].DataType;
                        if (type == typeof(string))
                        {
                            newRow[columnName] = string.Empty;
                        }
                        if (type == typeof(DateTime))
                        {
                            newRow[columnName] = new DateTime(1753, 1, 1);
                        }
                        else if (type.IsValueType)
                        {
                            newRow[columnName] = Activator.CreateInstance(type);
                        }
                    }
                    else if (string.IsNullOrEmpty(cell.Value))
                    {
                        newRow[columnName] = DBNull.Value;
                    }
                    else
                    {
                        var columnType = Type.GetType(column.TypeName) ?? typeof(string);
                        if (columnType == typeof(decimal))
                        {
                            newRow[columnName] = decimal.Parse(cell.Value, NumberStyles.Any, null);
                        }
                        else if (columnType == typeof(long))
                        {
                            newRow[columnName] = long.Parse(cell.Value, NumberStyles.Any, null);
                        }
                        else if (columnType == typeof(double))
                        {
                            newRow[columnName] = double.Parse(cell.Value, NumberStyles.Any, null);
                        }
                        else if (columnType == typeof(int))
                        {
                            newRow[columnName] = int.Parse(cell.Value, NumberStyles.Any, null);
                        }
                        else if (columnType == typeof(short))
                        {
                            newRow[columnName] = short.Parse(cell.Value, NumberStyles.Any, null);
                        }
                        else
                        {
                            newRow[columnName] = Convert.ChangeType(cell.Value, columnType);
                        }
                    }
                }
                insertTable.Rows.Add(newRow);
            }

            var connectionString = Database.GetConnectionString(databaseType);
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    foreach (DataColumn column in insertTable.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }

                    var preppedTable = (insertTable.Rows.Cast<DataRow>().Where(row => row.ItemArray.Any(field => !(field is DBNull)))).CopyToDataTable();

                    bulkCopy.DestinationTableName = insertTable.TableName;
                    bulkCopy.WriteToServer(preppedTable);
                }
            }
        }

        public static bool UpdateRow(string tableName, DALDataTableRowMetaData row, DALISGDatabaseType databaseType)
        {
            try
            {
                using (var context = new ISGOutputEntities(Database.GetEFConnectionString(databaseType)))
                {
                    var connection = (SqlConnection)context.Database.Connection;
                    var setQuery = BuildSetQuery(row.Cells.Where(c => !c.IsPrimaryKey));
                    var whereQuery = BuildWhereQuery(row.Cells.Where(c => c.IsPrimaryKey));

                    var query = string.Format("UPDATE {0} SET {1} WHERE {2}", tableName, setQuery, whereQuery);
                    connection.Execute(query);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Unable to update row in table: {0}", tableName), ex.InnerException);
            }
            return true;
        }

        private static string BuildSetQuery(IEnumerable<DALDataTableCellMetaData> cells)
        {
            var query = new StringBuilder();
            var dalDataTableCellMetaDatas = cells as IList<DALDataTableCellMetaData> ?? cells.ToList();
            foreach (var cell in dalDataTableCellMetaDatas)
            {
                var val = cell.Value.Equals(string.Empty) ? "null" : string.Format("'{0}'", cell.Value);
                query.AppendFormat("{0} = {1}", cell.ColumnName, val);
                if (!string.Equals(cell.ColumnName, dalDataTableCellMetaDatas.Last().ColumnName, StringComparison.CurrentCultureIgnoreCase))
                {
                    query.Append(",");
                }
            }
            return query.ToString();
        }

        private static string BuildWhereQuery(IEnumerable<DALDataTableCellMetaData> cells)
        {
            var query = new StringBuilder();
            var dalDataTableCellMetaDatas = cells as IList<DALDataTableCellMetaData> ?? cells.ToList();
            foreach (var cell in dalDataTableCellMetaDatas)
            {
                var val = cell.Value.Equals(string.Empty) ? "null" : string.Format("'{0}'", cell.Value);
                query.AppendFormat("{0} = {1}", cell.ColumnName, val);
                if (!string.Equals(cell.ColumnName, dalDataTableCellMetaDatas.Last().ColumnName, StringComparison.CurrentCultureIgnoreCase))
                {
                    query.Append(" and ");
                }
            }
            return query.ToString();
        }
    }
}
