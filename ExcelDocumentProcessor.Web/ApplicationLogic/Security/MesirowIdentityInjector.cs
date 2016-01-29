using System.Web.Mvc;
using System.Web.Security;

namespace ExcelDocumentProcessor.Web.ApplicationLogic.Security
{
    public class NeonIdentityInjector :ActionFilterAttribute,IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var identity = filterContext.HttpContext.User.Identity;

            var principal = new NeonPrincipal(identity);

            // set the custom principal
            filterContext.HttpContext.User = principal;

            if (principal.Identity.IsAuthenticated)
            {
                FormsAuthentication.SetAuthCookie(((NeonUserIdentity)principal.Identity).UserNameID, false);
            }
        }
    }
}
