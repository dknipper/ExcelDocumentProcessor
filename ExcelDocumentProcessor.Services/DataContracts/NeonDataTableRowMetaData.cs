using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ExcelDocumentProcessor.Services.DataContracts
{
    [DataContract]
    public class NeonDataTableRowMetaData
    {
        [DataMember]
        public List<NeonDataTableCellMetaData> Cells { get; set; }
    }
}
