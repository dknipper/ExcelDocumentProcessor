using System.Collections.Generic;
using System.ServiceModel;
using ExcelDocumentProcessor.Services.DataContracts;

namespace ExcelDocumentProcessor.Services.ServiceInterfaces
{
    [ServiceContract]
    interface INeonISGDataService
    {
        [OperationContract]
        long GetRowCount(string tableName, string filterData, NeonISGDatabaseType databaseType);

        [OperationContract]
        NeonDataTable GetData(string tableName, string sortIndex, string sortOrder, long pageIndex, long pageSize, string filters, NeonISGDatabaseType databaseType);

        [OperationContract]
        List<string> GetDistinctValues(string tableName, string columnName, NeonISGDatabaseType databaseType);

        [OperationContract]
        int? Login(string userName);

        [OperationContract]
        bool UserHasFunction(int userId, int functionId);

        [OperationContract]
        bool SaveRow(string tableName, NeonDataTableRowMetaData row, NeonISGDatabaseType databaseType);

        [OperationContract]
        List<NeonYearQuarter> GetYearQuarters();

        [OperationContract]
        List<NeonClient> GetActiveClients();

        [OperationContract]
        List<NeonDataTableMetaData> GetAllMetaData();

        [OperationContract]
        List<NeonExcelValidationRecord> GetExcelValidationRecordsByCode(string recordCode);

        [OperationContract]
        List<NeonExcelValidationRecord> GetExcelValidationRecordsByFileName(string fileName);

        [OperationContract]
        List<NeonExcelValidationRecord> GetAllExcelValidationRecords();
    }
}
