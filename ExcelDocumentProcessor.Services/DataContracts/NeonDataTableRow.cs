using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ExcelDocumentProcessor.Services.DataContracts
{
    [DataContract]
    public class NeonDataTableRow
    {
        [DataMember]
        public string RowId { get; set; }

        [DataMember]
        public List<NeonDataTableCell> Cells { get; set; }
    }
}
