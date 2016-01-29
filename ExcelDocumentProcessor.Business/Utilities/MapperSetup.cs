using AutoMapper;
using ExcelDocumentProcessor.Business.Entities;
using ExcelDocumentProcessor.Business.Enumerations;
using ExcelDocumentProcessor.Data.Entities.Custom;
using ExcelDocumentProcessor.Data.Enumerations;

namespace ExcelDocumentProcessor.Business.Utilities
{
    public static class MapperSetup
    {
        public static void CreateBusinessDALMaps()
        {
            Mapper.CreateMap<DALClient, BusinessClient>();
            Mapper.CreateMap<DALDataTable, BusinessDataTable>();
            Mapper.CreateMap<DALDataTableCell, BusinessDataTableCell>();
            Mapper.CreateMap<DALDataTableColumn, BusinessDataTableColumn>();
            Mapper.CreateMap<DALDataTableRow, BusinessDataTableRow>();
            Mapper.CreateMap<DALDataTableMetaData, BusinessDataTableMetaData>();
            Mapper.CreateMap<DALYearQuarter, BusinessYearQuarter>();
            Mapper.CreateMap<DALDataTableRowMetaData, BusinessDataTableRowMetaData>();
            Mapper.CreateMap<DALDataTableCellMetaData, BusinessDataTableCellMetaData>();
            Mapper.CreateMap<DALISGDatabaseType, BusinessISGDatabaseType>();
            Mapper.CreateMap<DALExcelValidationDataType, BusinessExcelValidationDataType>();
            Mapper.CreateMap<DALExcelValidationResultType, BusinessExcelValidationResultType>();
            Mapper.CreateMap<DALExcelValidationRecord, BusinessExcelValidationRecord>();

            Mapper.CreateMap<BusinessClient, DALClient>();
            Mapper.CreateMap<BusinessDataTable, DALDataTable>();
            Mapper.CreateMap<BusinessDataTableCell, DALDataTableCell>();
            Mapper.CreateMap<BusinessDataTableColumn, DALDataTableColumn>();
            Mapper.CreateMap<BusinessDataTableRow, DALDataTableRow>();
            Mapper.CreateMap<BusinessDataTableMetaData, DALDataTableMetaData>();
            Mapper.CreateMap<BusinessYearQuarter, DALYearQuarter>();
            Mapper.CreateMap<BusinessDataTableRowMetaData, DALDataTableRowMetaData>();
            Mapper.CreateMap<BusinessDataTableCellMetaData, DALDataTableCellMetaData>();
            Mapper.CreateMap<BusinessISGDatabaseType, DALISGDatabaseType>();
            Mapper.CreateMap<BusinessExcelValidationDataType, DALExcelValidationDataType>();
            Mapper.CreateMap<BusinessExcelValidationResultType, DALExcelValidationResultType>();
            Mapper.CreateMap<BusinessExcelValidationRecord, DALExcelValidationRecord>();
        }
    }
}
