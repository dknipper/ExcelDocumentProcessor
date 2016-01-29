using System;
using System.Linq;
using System.Security.Principal;

namespace ExcelDocumentProcessor.Web.ApplicationLogic.Security
{
    public class NeonUserIdentity : IIdentity
    {
        private readonly string _name;

        #region Properties
        /// <summary>
        /// Store the userId from the database
        /// </summary>
        public int? UserId { get; set; }

        public string AuthenticationType
        {
            get { return "AD Neon Hybrid"; }
        }
        /// <summary>
        /// The user's active directory account
        /// </summary>
        public string Name
        {
            get { return _name; }
        }
        /// <summary>
        /// Check that the user has been authenticated
        /// </summary>
        public bool IsAuthenticated
        {
            get { return UserId.HasValue && UserId.GetValueOrDefault() > 0; }
        }
        /// <summary>
        /// Used for storing the user name and id in the forms cookie
        /// </summary>
        public string UserNameID
        {
            get { return _name + "|" + UserId; }
        }
        #endregion

        public NeonUserIdentity(IIdentity identity) 
        {
            if (string.IsNullOrEmpty(identity.Name))
            {
                //if empty get the AD user name
                var windowsIdentity = WindowsIdentity.GetCurrent();
                if (windowsIdentity != null)
                {
                    _name = windowsIdentity.Name;
                }
            }
            else
            {
                if (identity.Name.Contains('|'))
                {
                    //read the name from the forms cookie
                    _name = identity.Name.Split('|')[0];
                    UserId = int.Parse(identity.Name.Split('|')[1]);
                }
                else
                {
                    //read the name from the object
                    _name = identity.Name;
                }
            }

            var userIdentity = identity as NeonUserIdentity;
            if (userIdentity != null)
            {
                UserId = userIdentity.UserId;
            }
        }

        /// <summary>
        /// Login the user to the system by checking that they are a named user in the system; 
        /// Authentication is accomplished via AD
        /// </summary>
        /// <returns></returns>
        public bool Login()
        {
            //if the user is already authenticated continue
            if (IsAuthenticated)
            {
                return true;
            }

            try
            {
                //check that the logged in user is an authenticated user in the system
                var security = new SecurityModel();
                UserId = security.Login(Name);
                return IsAuthenticated;
            }
            catch (Exception ex)
            {
                // ReSharper disable once PossibleIntendedRethrow
                throw ex;
            }
        }
    }
}
