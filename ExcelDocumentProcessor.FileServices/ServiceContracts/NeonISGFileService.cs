using System.Collections.Generic;
using System.ServiceModel;
using AutoMapper;
using ExcelDocumentProcessor.Business.Entities;
using ExcelDocumentProcessor.FileServices.DataContracts;
using ExcelDocumentProcessor.FileServices.MessageContracts;
using ExcelDocumentProcessor.FileServices.ServiceInterfaces;
using ExcelDocumentProcessor.FileServices.Utilities;

namespace ExcelDocumentProcessor.FileServices.ServiceContracts
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class NeonISGFileService : INeonISGFileService
    {
        public void UploadUniverseFile(UniverseFileUpload universeFileUpload)
        {
            MapperSetup.CreateFileServiceBusinessMaps();

            var businessDataColumnBuilders = Mapper.Map<List<NeonFSDataColumnBuilder>, List<BusinessDataColumnBuilder>>(universeFileUpload.ExtraColumns);
            var businessUniverse = Mapper.Map<NeonFSUniverse, BusinessUniverse>(universeFileUpload.Universe);
            var businessMetaData = Mapper.Map<List<NeonFSDataTableMetaData>, List<BusinessDataTableMetaData>>(universeFileUpload.MetaData);
            BusinessUniverse.ProcessUniverseFile(universeFileUpload.UniverseFile, businessDataColumnBuilders, businessUniverse, businessMetaData, universeFileUpload.UploadCode, universeFileUpload.FileName);
        }

        public void UploadClientChangeFile(ClientChangeFileUpload clientChangeFileUpload)
        {
            MapperSetup.CreateFileServiceBusinessMaps();

            var businessDataColumnBuilders = Mapper.Map<List<NeonFSDataColumnBuilder>, List<BusinessDataColumnBuilder>>(clientChangeFileUpload.ExtraColumns);
            var businessMetaData = Mapper.Map<NeonFSDataTableMetaData, BusinessDataTableMetaData>(clientChangeFileUpload.MetaData);
            BusinessClient.ProcessClientChangeFile(clientChangeFileUpload.ClientChangeFile, businessDataColumnBuilders, businessMetaData, clientChangeFileUpload.UploadCode, clientChangeFileUpload.SheetName, clientChangeFileUpload.FileName);
        }
    }
}