using AutoMapper;
using ExcelDocumentProcessor.Business.Entities;
using ExcelDocumentProcessor.Business.Enumerations;
using ExcelDocumentProcessor.FileServices.DataContracts;

namespace ExcelDocumentProcessor.FileServices.Utilities
{
    public static class MapperSetup
    {
        public static void CreateFileServiceBusinessMaps()
        {
            Mapper.CreateMap<BusinessDataTableColumn, NeonFSDataTableColumn>();
            Mapper.CreateMap<BusinessDataTableMetaData, NeonFSDataTableMetaData>();
            Mapper.CreateMap<BusinessISGDatabaseType, NeonFSISGDatabaseType>();
            Mapper.CreateMap<BusinessDataColumnBuilder, NeonFSDataColumnBuilder>();
            Mapper.CreateMap<BusinessUniverse, NeonFSUniverse>();
            Mapper.CreateMap<BusinessUniverseMap, NeonFSUniverseMap>();

            Mapper.CreateMap<NeonFSDataTableColumn, BusinessDataTableColumn>();
            Mapper.CreateMap<NeonFSDataTableMetaData, BusinessDataTableMetaData>();
            Mapper.CreateMap<NeonFSISGDatabaseType, BusinessISGDatabaseType>();
            Mapper.CreateMap<NeonFSDataColumnBuilder, BusinessDataColumnBuilder>();
            Mapper.CreateMap<NeonFSUniverse, BusinessUniverse>();
            Mapper.CreateMap<NeonFSUniverseMap, BusinessUniverseMap>();

            Business.Utilities.MapperSetup.CreateBusinessDALMaps();
        }
    }
}
