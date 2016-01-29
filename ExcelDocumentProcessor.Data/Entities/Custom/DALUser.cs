using System;
using System.Linq;
using ExcelDocumentProcessor.Data.Entities.SystemGenerated;

namespace ExcelDocumentProcessor.Data.Entities.Custom
{
    public class DALUser
    {
        public int InstanceId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Status { get; set; }

        public static DALUser GetUser(string userName)
        {
            var retVal = new DALUser();

            using (var context = new ISGAdminEntities())
            {
                var user = context.Users.FirstOrDefault(f => string.Equals(f.Name, userName, StringComparison.CurrentCultureIgnoreCase));

                if (user != null)
                {
                    retVal = new DALUser
                    {
                        InstanceId = user.InstanceId,
                        Name = user.Name,
                        Password = user.Password,
                        StartDate = user.StartDate,
                        EndDate = user.EndDate,
                        Status = user.Status
                    };
                }
            }
            return retVal;
        }
    }
}
