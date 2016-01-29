using System;
using System.Collections.Generic;
using System.Linq;
using ExcelDocumentProcessor.Data.Entities.SystemGenerated;
using ExcelDocumentProcessor.Data.Enumerations;
using ExcelDocumentProcessor.Data.Utilities;

namespace ExcelDocumentProcessor.Data.Entities.Custom
{
    public class DALDataTableMetaData
    {
        public string Name { get; set; }
        public List<DALDataTableColumn> Columns { get; set; }
        public DALISGDatabaseType DatabaseType { get; set; }

        public static List<DALDataTableMetaData> GetAllMetaData()
        {
            var rtrn = new List<DALDataTableMetaData>();

            var databaseTypes =
                new List<DALISGDatabaseType>
                    {
                        DALISGDatabaseType.ISGInput,
                        DALISGDatabaseType.ISGTransient,
                        DALISGDatabaseType.ISGOutput
                    };

            foreach (var databaseType in databaseTypes)
            {
                using (var context = new ISGOutputEntities(Database.GetEFConnectionString(databaseType)))
                {
                    var metaDatas = context.TableMetaData.ToList().OrderBy(x => x.tableName);

                    var dalDataTable = new DALDataTableMetaData();
                    foreach (var metaData in metaDatas)
                    {
                        if (!string.Equals(metaData.tableName, dalDataTable.Name, StringComparison.CurrentCultureIgnoreCase))
                        {
                            dalDataTable =
                                new DALDataTableMetaData
                                {
                                    Name = metaData.tableName,
                                    Columns = new List<DALDataTableColumn>(),
                                    DatabaseType = databaseType
                                };
                            rtrn.Add(dalDataTable);
                        }

                        dalDataTable.Columns.Add(
                            new DALDataTableColumn
                            {
                                Name = metaData.columnName,
                                IsNullable = (metaData.columnIsNullable.HasValue && metaData.columnIsNullable == 1),
                                Length = metaData.columnLength,
                                Precision = metaData.columnPrecision,
                                ColumnOrdinal = metaData.columnOrdinal,
                                Scale = metaData.columnScale,
                                TypeName = metaData.columnTypeName,
                                ParentTable = metaData.tableName,
                                IsPrimaryKey = (metaData.isPrimaryKey == 1),
                                IsReadOnlyIdentity = (metaData.isReadonlyIdentity == 1)
                            });
                    }
                }
            }

            return rtrn;
        }
    }
}
