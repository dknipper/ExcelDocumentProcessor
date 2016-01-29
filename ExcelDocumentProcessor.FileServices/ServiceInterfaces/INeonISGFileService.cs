using System.ServiceModel;
using ExcelDocumentProcessor.FileServices.MessageContracts;

namespace ExcelDocumentProcessor.FileServices.ServiceInterfaces
{
    [ServiceContract]
    public interface INeonISGFileService
    {
        [OperationContract(IsOneWay = true)]
        void UploadUniverseFile(UniverseFileUpload universeFileUpload);

        [OperationContract(IsOneWay = true)]
        void UploadClientChangeFile(ClientChangeFileUpload clientChangeFileUpload);
    }
}
