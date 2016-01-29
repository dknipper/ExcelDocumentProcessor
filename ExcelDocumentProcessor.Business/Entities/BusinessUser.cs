using System;
using System.Linq;

namespace ExcelDocumentProcessor.Business.Entities
{
    public class BusinessUser
    {
        public static int? Login(string userName)
        {
            var dalUser = Data.Entities.Custom.DALUser.GetUser(userName);

            if (dalUser.InstanceId == 0 || 
                dalUser.StartDate > DateTime.Now || 
                dalUser.EndDate <= DateTime.Now ||
                !dalUser.Status)
            {
                return null;
            }
            return dalUser.InstanceId;
        }

        public static bool UserHasFunction(int userId, int functionId)
        {
            var dalRAs = Data.Entities.Custom.DALRoleAssignment.GetUserRoles(userId);
            var dalRMs = Data.Entities.Custom.DALRoleMapping.GetRoleMappings(functionId);

            var userHasFunction = (from ra in dalRAs
                                   join rm in dalRMs on ra.RoleId equals rm.RoleId
                                   where rm.Status && ra.Status
                                   select ra.InstanceId).Any();
            return userHasFunction;
        }
    }
}
