using System.Collections.Generic;
using System.Linq;
using ExcelDocumentProcessor.Data.Entities.SystemGenerated;

namespace ExcelDocumentProcessor.Data.Entities.Custom
{
    public class DALRoleAssignment
    {
        public int InstanceId { get; set; }
        public int RoleId { get; set; }
        public bool Status { get; set; }

        public static List<DALRoleAssignment> GetUserRoles(int userId)
        {
            var retVal = new List<DALRoleAssignment>();

            using (var context = new ISGAdminEntities())
            {
                var roleAssignments = context.RoleAssignments.Where(f => f.UserId == userId).ToList();

                if (!roleAssignments.Any())
                {
                    return retVal;
                }

                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var ra in roleAssignments)
                {
                    retVal.Add(new DALRoleAssignment
                    {
                        InstanceId = ra.InstanceId,
                        RoleId = ra.RoleId,
                        Status = ra.Status
                    });
                }
            }
            return retVal;
        }
    }
}
