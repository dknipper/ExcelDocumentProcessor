using System.Collections.Generic;
using AutoMapper;
using ExcelDocumentProcessor.Business.Entities;
using ExcelDocumentProcessor.Business.Enumerations;
using ExcelDocumentProcessor.Services.DataContracts;
using ExcelDocumentProcessor.Services.ServiceBehaviors;
using ExcelDocumentProcessor.Services.ServiceInterfaces;

namespace ExcelDocumentProcessor.Services.ServiceContracts
{
    [AutomapServiceBehavior]
    public class NeonISGDataService : INeonISGDataService
    {
        public long GetRowCount(string tableName, string filterData, NeonISGDatabaseType databaseType)
        {
            var businessDatabaseType = Mapper.Map<NeonISGDatabaseType, BusinessISGDatabaseType>(databaseType);
            var rtrn = BusinessDataTable.GetRowCount(tableName, filterData, businessDatabaseType);
            return rtrn;
        }

        public NeonDataTable GetData(string tableName, string sortIndex, string sortOrder, long pageIndex, long pageSize, string filters, NeonISGDatabaseType databaseType)
        {
            var businessDatabaseType = Mapper.Map<NeonISGDatabaseType, BusinessISGDatabaseType>(databaseType);
            var businessDataTable = BusinessDataTable.GetData(tableName, sortIndex, sortOrder, pageIndex, pageSize, filters, businessDatabaseType);
            var rtrn = Mapper.Map<BusinessDataTable, NeonDataTable>(businessDataTable);
            return rtrn;
        }

        public List<string> GetDistinctValues(string tableName, string columnName, NeonISGDatabaseType databaseType)
        {
            var businessDatabaseType = Mapper.Map<NeonISGDatabaseType, BusinessISGDatabaseType>(databaseType);
            var rtrn = BusinessDataTable.GetDistinctValues(tableName, columnName, businessDatabaseType);
            return rtrn;
        }

        public int? Login(string userName)
        {
            var rtrn = BusinessUser.Login(userName);
            return rtrn;
        }

        public bool UserHasFunction(int userId, int functionId)
        {
            var rtrn = BusinessUser.UserHasFunction(userId, functionId);
            return rtrn;
        }

        public bool SaveRow(string tableName, NeonDataTableRowMetaData row, NeonISGDatabaseType databaseType)
        {
            var businessDatabaseType = Mapper.Map<NeonISGDatabaseType, BusinessISGDatabaseType>(databaseType);
            var rtrn = BusinessDataTable.UpdateRow(tableName, Mapper.Map<NeonDataTableRowMetaData, BusinessDataTableRowMetaData>(row), businessDatabaseType);
            return rtrn;
        }

        public List<NeonYearQuarter> GetYearQuarters()
        {
            var businessYearQuarters = BusinessYearQuarter.GetYearQuarters();
            var rtrn = Mapper.Map<List<BusinessYearQuarter>, List<NeonYearQuarter>>(businessYearQuarters);
            return rtrn;
        }

        public List<NeonClient> GetActiveClients()
        {
            var businessClients = BusinessClient.GetActiveClients();
            var rtrn = Mapper.Map<List<BusinessClient>, List<NeonClient>>(businessClients);
            return rtrn;
        }

        public List<NeonDataTableMetaData> GetAllMetaData()
        {
            var businessTableMetaData = BusinessDataTableMetaData.GetAllMetaData();
            var rtrn = Mapper.Map<List<BusinessDataTableMetaData>, List<NeonDataTableMetaData>>(businessTableMetaData);
            return rtrn;
        }

        public List<NeonExcelValidationRecord> GetExcelValidationRecordsByCode(string recordCode)
        {
            var businessRecords = BusinessExcelValidationRecord.GetExcelValidationRecordsByCode(recordCode);
            var neonRecords = Mapper.Map<List<BusinessExcelValidationRecord>, List<NeonExcelValidationRecord>>(businessRecords);
            return neonRecords;
        }

        public List<NeonExcelValidationRecord> GetExcelValidationRecordsByFileName(string fileName)
        {
            var businessRecords = BusinessExcelValidationRecord.GetExcelValidationRecordsByFileName(fileName);
            var neonRecords = Mapper.Map<List<BusinessExcelValidationRecord>, List<NeonExcelValidationRecord>>(businessRecords);
            return neonRecords;
        }

        public List<NeonExcelValidationRecord> GetAllExcelValidationRecords()
        {
            var businessRecords = BusinessExcelValidationRecord.GetAllExcelValidationRecords();
            var neonRecords = Mapper.Map<List<BusinessExcelValidationRecord>, List<NeonExcelValidationRecord>>(businessRecords);
            return neonRecords;
        }
    }
}