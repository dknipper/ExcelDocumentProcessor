using AutoMapper;
using ExcelDocumentProcessor.Business.Entities;
using ExcelDocumentProcessor.Business.Enumerations;
using ExcelDocumentProcessor.Services.DataContracts;

namespace ExcelDocumentProcessor.Services.ServiceBehaviors
{
    public class AutomapBootstrap
    {
        public static void InitializeMap()
        {
            Mapper.CreateMap<BusinessClient, NeonClient>();
            Mapper.CreateMap<BusinessDataTable, NeonDataTable>();
            Mapper.CreateMap<BusinessDataTableCell, NeonDataTableCell>();
            Mapper.CreateMap<BusinessDataTableColumn, NeonDataTableColumn>();
            Mapper.CreateMap<BusinessDataTableRow, NeonDataTableRow>();
            Mapper.CreateMap<BusinessDataTableMetaData, NeonDataTableMetaData>();
            Mapper.CreateMap<BusinessYearQuarter, NeonYearQuarter>();
            Mapper.CreateMap<BusinessDataTableCellMetaData, NeonDataTableCellMetaData>();
            Mapper.CreateMap<BusinessDataTableRowMetaData, NeonDataTableRowMetaData>();
            Mapper.CreateMap<BusinessISGDatabaseType, NeonISGDatabaseType>();
            Mapper.CreateMap<BusinessExcelValidationDataType, NeonExcelValidationDataType>();
            Mapper.CreateMap<BusinessExcelValidationResultType, NeonExcelValidationResultType>();
            Mapper.CreateMap<BusinessExcelValidationRecord, NeonExcelValidationRecord>();

            Mapper.CreateMap<NeonClient, BusinessClient>();
            Mapper.CreateMap<NeonDataTable, BusinessDataTable>();
            Mapper.CreateMap<NeonDataTableCell, BusinessDataTableCell>();
            Mapper.CreateMap<NeonDataTableColumn, BusinessDataTableColumn>();
            Mapper.CreateMap<NeonDataTableRow, BusinessDataTableRow>();
            Mapper.CreateMap<NeonDataTableMetaData, BusinessDataTableMetaData>();
            Mapper.CreateMap<NeonYearQuarter, BusinessYearQuarter>();
            Mapper.CreateMap<NeonDataTableCellMetaData, BusinessDataTableCellMetaData>();
            Mapper.CreateMap<NeonDataTableRowMetaData, BusinessDataTableRowMetaData>();
            Mapper.CreateMap<NeonISGDatabaseType, BusinessISGDatabaseType>();
            Mapper.CreateMap<NeonExcelValidationDataType, BusinessExcelValidationDataType>();
            Mapper.CreateMap<NeonExcelValidationResultType, BusinessExcelValidationResultType>();
            Mapper.CreateMap<NeonExcelValidationRecord, BusinessExcelValidationRecord>();

            Business.Utilities.MapperSetup.CreateBusinessDALMaps();
        }
    }
}