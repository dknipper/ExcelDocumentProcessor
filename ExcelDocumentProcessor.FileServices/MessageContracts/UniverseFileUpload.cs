using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using ExcelDocumentProcessor.FileServices.Constants;
using ExcelDocumentProcessor.FileServices.DataContracts;

namespace ExcelDocumentProcessor.FileServices.MessageContracts
{
    [MessageContract(WrapperNamespace = NeonFileServiceNamespaces.UniverseFileServiceNamespace)]
    public class UniverseFileUpload
    {
        [MessageHeader(MustUnderstand = true, Namespace = NeonFileServiceNamespaces.UniverseFileServiceNamespace)]
        public List<NeonFSDataColumnBuilder> ExtraColumns;

        [MessageHeader(MustUnderstand = true, Namespace = NeonFileServiceNamespaces.UniverseFileServiceNamespace)]
        public NeonFSUniverse Universe;

        [MessageHeader(MustUnderstand = true, Namespace = NeonFileServiceNamespaces.UniverseFileServiceNamespace)]
        public List<NeonFSDataTableMetaData> MetaData;

        [MessageHeader(MustUnderstand = true, Namespace = NeonFileServiceNamespaces.UniverseFileServiceNamespace)]
        public string UploadCode;

        [MessageHeader(MustUnderstand = true, Namespace = NeonFileServiceNamespaces.UniverseFileServiceNamespace)]
        public string FileName;

        [MessageBodyMember(Order = 1, Namespace = NeonFileServiceNamespaces.UniverseFileServiceNamespace)]
        public Stream UniverseFile;
    }
}