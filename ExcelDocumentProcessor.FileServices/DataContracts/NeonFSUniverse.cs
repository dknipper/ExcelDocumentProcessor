using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ExcelDocumentProcessor.FileServices.DataContracts
{
    [DataContract]
    public class NeonFSUniverse
    {
        [DataMember]
        public string UniverseName { get; set; }

        [DataMember]
        public List<NeonFSUniverseMap> UniverseMaps { get; set; } 
    }
}
