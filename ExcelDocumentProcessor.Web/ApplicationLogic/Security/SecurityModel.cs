using System;
using ExcelDocumentProcessor.Web.ApplicationLogic.AppConfig;
using ExcelDocumentProcessor.Web.ApplicationLogic.Proxies.DataService;

namespace ExcelDocumentProcessor.Web.ApplicationLogic.Security
{
    public class SecurityModel
    {
        #region Security Functions
        /// <summary>
        /// Check that the user has access
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="functionId"></param>
        /// <returns></returns>
        public bool UserHasFunction(int userId, int functionId)
        {
            try
            {
                bool rtrn;
                using (var serviceClient = new NeonISGDataServiceClient(Configuration.ActiveNeonDataServiceEndpoint))
                {
                    rtrn = serviceClient.UserHasFunction(userId, functionId);
                }
                return rtrn;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error in SecurityVM.UserHasFunction() userId:{0} functionId:{1}", userId, functionId), ex);
            }
        }
        /// <summary>
        /// Attempt to login the user by matching their AD account to a registered user in the system
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int? Login(string userName)
        {
            try
            {
                int? rtrn;
                using (var serviceClient = new NeonISGDataServiceClient(Configuration.ActiveNeonDataServiceEndpoint))
                {
                    rtrn = serviceClient.Login(userName);
                }
                return rtrn;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error in SecurityVM.Login() userName:{0}", userName), ex);
            }
        }
        #endregion
    }
}
