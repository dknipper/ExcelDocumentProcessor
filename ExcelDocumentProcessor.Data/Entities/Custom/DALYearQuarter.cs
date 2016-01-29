using System.Collections.Generic;
using ExcelDocumentProcessor.Data.Entities.SystemGenerated;

namespace ExcelDocumentProcessor.Data.Entities.Custom
{
    public class DALYearQuarter
    {
        public int Id { get; set; }
        public string Quarter { get; set; }
        public string LastMonth { get; set; }
        public string EndDate { get; set; }

        public static List<DALYearQuarter> GetYearQuarters()
        {
            List<DALYearQuarter> rtrn = null;
            using (var context = new ISGAdminEntities())
            {
                var yearQuarters = context.YearQuaters;

                foreach (var yearQuarter in yearQuarters)
                {
                    rtrn = rtrn ?? new List<DALYearQuarter>();
                    rtrn.Add(
                        new DALYearQuarter
                            {
                                Id = yearQuarter.Id,
                                Quarter = yearQuarter.Quater,
                                LastMonth = yearQuarter.LastMonth,
                                EndDate = yearQuarter.EndDate
                            });
                }
            }
            return rtrn;
        }
    }
}
