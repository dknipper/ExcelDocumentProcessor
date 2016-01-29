using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using ExcelDocumentProcessor.FileServices.Constants;
using ExcelDocumentProcessor.FileServices.DataContracts;

namespace ExcelDocumentProcessor.FileServices.MessageContracts
{
    [MessageContract(WrapperNamespace = NeonFileServiceNamespaces.ClientChangeFileServiceNamespace)]
    public class ClientChangeFileUpload
    {
        [MessageHeader(MustUnderstand = true, Namespace = NeonFileServiceNamespaces.ClientChangeFileServiceNamespace)]
        public List<NeonFSDataColumnBuilder> ExtraColumns;

        [MessageHeader(MustUnderstand = true, Namespace = NeonFileServiceNamespaces.ClientChangeFileServiceNamespace)]
        public NeonFSDataTableMetaData MetaData;

        [MessageHeader(MustUnderstand = true, Namespace = NeonFileServiceNamespaces.ClientChangeFileServiceNamespace)]
        public string UploadCode;

        [MessageHeader(MustUnderstand = true, Namespace = NeonFileServiceNamespaces.ClientChangeFileServiceNamespace)]
        public string SheetName;

        [MessageHeader(MustUnderstand = true, Namespace = NeonFileServiceNamespaces.ClientChangeFileServiceNamespace)]
        public string FileName;

        [MessageBodyMember(Order = 1, Namespace = NeonFileServiceNamespaces.ClientChangeFileServiceNamespace)]
        public Stream ClientChangeFile;
    }
}