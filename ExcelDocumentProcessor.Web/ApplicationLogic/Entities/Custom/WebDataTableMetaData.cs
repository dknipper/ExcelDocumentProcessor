using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using ExcelDocumentProcessor.Web.ApplicationLogic.Enumerations;
using ExcelDocumentProcessor.Web.ApplicationLogic.Proxies.DataService;

namespace ExcelDocumentProcessor.Web.ApplicationLogic.Entities.Custom
{
    public class WebDataTableMetaData
    {
        public string Name { get; set; }
        public string SourceName { get; set; }
        public List<WebDataTableColumn> Columns { get; set; }
        public WebISGDatabaseType DatabaseType { get; set; }

        public WebDataTableMetaData(NeonDataTableMetaData serviceMetadata)
        {
            Name = serviceMetadata.Name;
            DatabaseType = Mapper.Map<NeonISGDatabaseType, WebISGDatabaseType>(serviceMetadata.DatabaseType);

            // The "Actions" column will be the same across every table
            Columns =
                new List<WebDataTableColumn>
                    {
                        new WebDataTableColumn("act")
                            {
                                ParentTable = serviceMetadata.Name,
                                IsNullable = true,
                                DatabaseType = DatabaseType
                            }
                    };

            foreach (var metaData in serviceMetadata.Columns)
            {
                Columns.Add(
                    new WebDataTableColumn(metaData.Name)
                        {
                            IsNullable = metaData.IsNullable,
                            Length = metaData.Length,
                            Precision = metaData.Precision,
                            Scale = metaData.Scale,
                            TypeName = metaData.TypeName,
                            ParentTable = serviceMetadata.Name,
                            ParentSourceTable = metaData.ParentSourceTable,
                            IsPrimaryKey = metaData.IsPrimaryKey,
                            IsReadonlyIdentity = metaData.IsReadonlyIdentity,
                            DatabaseType = DatabaseType
                        });
            }
            Columns = Columns.OrderBy(x => x.Name).ToList();

            var firstColumn = Columns.FirstOrDefault(c => !string.Equals(c.FriendlyName, "Actions", StringComparison.CurrentCultureIgnoreCase));
            if (firstColumn != null)
            {
                SourceName = firstColumn.ParentSourceTable;
            }
        }

        public string JqGridColumns
        {
            get
            {
                var rtrn = new StringBuilder("[");
                foreach (var column in Columns.OrderBy(x => x.Order).ThenBy(x => x.FriendlyName))
                {
                    rtrn.AppendFormat("{0},", column.JsonColumn);
                }
                rtrn = new StringBuilder(rtrn.ToString().TrimEnd(",".ToCharArray()));
                rtrn.Append("]");
                return rtrn.ToString();
            }
        }

        public string JqGridColumnNames
        {
            get
            {
                var rtrn = new StringBuilder("[");
                foreach (var column in Columns.OrderBy(x => x.Order).ThenBy(x => x.FriendlyName))
                {
                    rtrn.AppendFormat("\"{0}\",", column.FriendlyName);
                }
                rtrn = new StringBuilder(rtrn.ToString().TrimEnd(",".ToCharArray()));
                rtrn.Append("]");
                return rtrn.ToString();
            }
        }

        public string JqGridNonEditableColumns
        {
            get
            {
                var rtrn = new StringBuilder("[");
                foreach (var column in Columns.Where(c => c.Editable.Equals("false") && 
                    c.Hidden.Equals("false") &&
                    !c.Name.Equals("act")))
                {
                    rtrn.AppendFormat("\"{0}\",", column.Name);
                }
                rtrn = new StringBuilder(rtrn.ToString().TrimEnd(",".ToCharArray()));
                rtrn.Append("]");
                return rtrn.ToString();
            }
        }
    }
}