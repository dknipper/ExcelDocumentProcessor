
namespace ExcelDocumentProcessor.Web.ApplicationLogic.Entities.Custom
{
    public class WebClient
    {
        public int ClientKey { get; set; }
        public string ClientName { get; set; }
        public string FriendlyClientName { get; set; }
        public string RecordStatus { get; set; }
        public string InputTable { get; set; }
        public string OutputTable { get; set; }
        public string TransientTable { get; set; }
    }
}
