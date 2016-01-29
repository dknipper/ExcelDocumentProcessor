using System;
using System.Web.Mvc;

namespace ExcelDocumentProcessor.Web.ApplicationLogic.Security
{
    /// <summary>
    /// Derrived Authorize Attribute for controlling authorization
    /// </summary>
    public class NeonAuthorize : AuthorizeAttribute
    {
        private int? _functionId;

        public NeonAuthorize(int FunctionId)
        {
            _functionId = FunctionId;
        }
        public NeonAuthorize() { }
        /// <summary>
        /// FunctionId for a controller method or class
        /// </summary>
        
        public int FunctionId
        {
            get { return _functionId.Value; }
            set { _functionId = value; }
        }
        /// <summary>
        /// Authorize the user against the functionid
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                //1 check for Neon account authentication
                if (base.AuthorizeCore(filterContext.HttpContext) || ((NeonUserIdentity)filterContext.HttpContext.User.Identity).Login())
                {
                    //only check for function if it exists
                    if (this._functionId.HasValue)
                    {
                        //user is a valid user; check user's function id access
                        if (((NeonPrincipal)filterContext.HttpContext.User).HasFunctionId(this._functionId.Value))
                        {
                            //function security check passed!
                            return;
                        }
                        //user is unauthorized for this function
                        HandleUnauthorizedRequest(filterContext);
                    }
                    //user authentication passed!
                    return;
                }
                //user is not authenticated in the system
                HandleUnAuthenticatedRequest(filterContext);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Handle an unauthorized request
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/SecurityIssues/Unauthorized");
        }
        /// <summary>
        /// Handle an unauthenticated user
        /// </summary>
        /// <param name="filterContext"></param>
        protected void HandleUnAuthenticatedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/SecurityIssues/Unauthenticated");
        }
    }
}
