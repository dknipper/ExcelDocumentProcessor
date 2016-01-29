using System.Collections.Generic;
using AutoMapper;
using ExcelDocumentProcessor.Business.Enumerations;
using ExcelDocumentProcessor.Data.Entities.Custom;

namespace ExcelDocumentProcessor.Business.Entities
{
    public class BusinessDataTableMetaData
    {
        public string Name { get; set; }
        public List<BusinessDataTableColumn> Columns { get; set; }
        public BusinessISGDatabaseType DatabaseType { get; set; }

        public static List<BusinessDataTableMetaData> GetAllMetaData()
        {
            var dalTableMetaData = DALDataTableMetaData.GetAllMetaData();
            var rtrn = Mapper.Map<List<DALDataTableMetaData>, List<BusinessDataTableMetaData>>(dalTableMetaData);
            return rtrn;
        }
    }
}
