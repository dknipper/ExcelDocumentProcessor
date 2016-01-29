using System.Collections.Generic;
using System.Linq;
using ExcelDocumentProcessor.Data.Entities.SystemGenerated;

namespace ExcelDocumentProcessor.Data.Entities.Custom
{
    public class DALRoleMapping
    {
        public int InstanceId { get; set; }
        public int RoleId { get; set; }
        public int FunctionId { get; set; }
        public bool Status { get; set; }

        public static List<DALRoleMapping> GetRoleMappings(int functionId)
        {
            var retVal = new List<DALRoleMapping>();

            using (var context = new ISGAdminEntities())
            {
                var user = context.RoleMappings.Where(f => f.FunctionId == functionId).ToList();

                if (!user.Any())
                {
                    return retVal;
                }

                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var u in user)
                {
                    retVal.Add(new DALRoleMapping
                    {
                        InstanceId = u.InstanceId,
                        RoleId = u.RoleId,
                        FunctionId = u.FunctionId,
                        Status = u.Status
                    });
                }
            }
            return retVal;
        }
    }
}
