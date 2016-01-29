using System.Web.Mvc;
using ExcelDocumentProcessor.Web.ApplicationLogic.Security;

namespace ExcelDocumentProcessor.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new NeonIdentityInjector());
            filters.Add(new HandleErrorAttribute());
        }
    }
}