using System.Runtime.Serialization;

namespace ExcelDocumentProcessor.Services.DataContracts
{
    [DataContract]
    public class NeonExcelValidationRecord
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string ExcelSheet { get; set; }

        [DataMember]
        public string ExcelColumn { get; set; }

        [DataMember]
        public int? MaxStringLength { get; set; }

        [DataMember]
        public int? ExcelRow { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string BadValue { get; set; }

        [DataMember]
        public NeonExcelValidationDataType ExcelValidationDataType { get; set; }

        [DataMember]
        public NeonExcelValidationResultType ExcelValidationResultType { get; set; }
    }
}
