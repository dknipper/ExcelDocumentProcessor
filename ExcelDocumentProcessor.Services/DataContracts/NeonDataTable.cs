using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ExcelDocumentProcessor.Services.DataContracts
{
    [DataContract]
    public class NeonDataTable
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<NeonDataTableRow> Rows { get; set; }

        [DataMember]
        public List<NeonDataTableColumn> Columns { get; set; }
    }
}
