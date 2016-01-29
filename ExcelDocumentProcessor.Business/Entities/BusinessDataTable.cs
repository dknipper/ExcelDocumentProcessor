using System.Collections.Generic;
using AutoMapper;
using ExcelDocumentProcessor.Business.Enumerations;
using ExcelDocumentProcessor.Data.Entities.Custom;

namespace ExcelDocumentProcessor.Business.Entities
{
    public class BusinessDataTable
    {
        public string Name { get; set; }
        public List<BusinessDataTableRow> Rows { get; set; }
        public List<BusinessDataTableColumn> Columns { get; set; }

        public static long GetRowCount(string tableName, string filter, BusinessISGDatabaseType databaseType)
        {
            var database = Mapper.Map<BusinessISGDatabaseType, Data.Enumerations.DALISGDatabaseType>(databaseType);
            var rtrn = DynamicEntity.GetRowCount(tableName, filter, database);
            return rtrn;
        }

        public static BusinessDataTable GetData(string tableName, string sortIndex, string sortOrder, long pageIndex, long pageSize, string filter, BusinessISGDatabaseType databaseType)
        {
            var database = Mapper.Map<BusinessISGDatabaseType, Data.Enumerations.DALISGDatabaseType>(databaseType);
            var dalTable = DynamicEntity.GetData(tableName, sortIndex, sortOrder, pageIndex, pageSize, filter, database);
            var rtrn = Mapper.Map<DALDataTable, BusinessDataTable>(dalTable);
            return rtrn;
        }

        public static List<string> GetDistinctValues(string tableName, string columnName, BusinessISGDatabaseType databaseType)
        {
            var database = Mapper.Map<BusinessISGDatabaseType, Data.Enumerations.DALISGDatabaseType>(databaseType);
            var rtrn = DynamicEntity.GetDistinctValues(tableName, columnName, database);
            return rtrn;
        }
        
        public static bool UpdateRow(string tableName, BusinessDataTableRowMetaData row, BusinessISGDatabaseType databaseType)
        {
            row.Cells.ForEach(cell => cell.Value = cell.Value.ToString().Replace("'", "''"));

            var database = Mapper.Map<BusinessISGDatabaseType, Data.Enumerations.DALISGDatabaseType>(databaseType);
            var rtrn = DynamicEntity.UpdateRow(tableName, Mapper.Map<BusinessDataTableRowMetaData, DALDataTableRowMetaData>(row), database);
            return rtrn;
        }
    }
}
