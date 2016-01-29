using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ExcelDocumentProcessor.Data.Entities.Custom;

namespace ExcelDocumentProcessor.Business.Entities
{
    public class BusinessYearQuarter
    {
        public int Id { get; set; }
        public string Quarter { get; set; }
        public string LastMonth { get; set; }
        public string EndDate { get; set; }

        public static List<BusinessYearQuarter> GetYearQuarters()
        {
            var dalYearQuarters = DALYearQuarter.GetYearQuarters();
            var allYearQuarters = Mapper.Map<List<DALYearQuarter>, List<BusinessYearQuarter>>(dalYearQuarters);
            return allYearQuarters.OrderByDescending(x => x.Id).ToList();
        }
    }
}
