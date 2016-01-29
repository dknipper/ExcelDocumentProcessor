namespace ExcelDocumentProcessor.Web.ApplicationLogic.Security.Interfaces
{
    public interface ISecurity
    {
        bool HasFunctionId(int functionId);
    }

    public interface ISecureApprovable
    {
        int ApproveFunctionId { get; }
        bool CanDelete { get; }
    }
}
