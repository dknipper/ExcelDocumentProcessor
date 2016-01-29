using System;
using System.Security.Principal;
using ExcelDocumentProcessor.Web.ApplicationLogic.Security.Interfaces;

namespace ExcelDocumentProcessor.Web.ApplicationLogic.Security
{
    public class NeonPrincipal : IPrincipal, ISecurity
    {
        private NeonUserIdentity _identity;

        /// <summary>
        /// Store the users identity
        /// </summary>
        public IIdentity Identity
        {
            get { return _identity; }
        }

        public NeonPrincipal(IIdentity identity)
        {
            _identity = new NeonUserIdentity(identity);
        }

        #region NotImplemented Functionality
        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// Make a database call to check if the user has access to the passed in function
        /// </summary>
        /// <param name="functionId"></param>
        /// <returns></returns>
        public bool HasFunctionId(int functionId)
        {
            try
            {
                //check user for functionid using userId
                SecurityModel security = new SecurityModel();
                if (security.UserHasFunction(((NeonUserIdentity)Identity).UserId.Value, functionId))
                {
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                throw new Exception("functionId;"+functionId.ToString(),ex);
            }
        }
    }
}
