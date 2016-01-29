using System;
using System.Linq;
using System.Text;
using ExcelDocumentProcessor.Web.ApplicationLogic.AppConfig;
using ExcelDocumentProcessor.Web.ApplicationLogic.Enumerations;
using ExcelDocumentProcessor.Web.ApplicationLogic.Proxies.DataService;
using ExcelDocumentProcessor.Web.ApplicationLogic.UserInterfaceHelpers;

namespace ExcelDocumentProcessor.Web.ApplicationLogic.Entities.Custom
{
    public class WebDataTableColumn
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public WebISGDatabaseType DatabaseType { get; set; }
        public short? Precision { get; set; }
        public int? Scale { get; set; }
        public short Length { get; set; }
        public bool IsNullable { get; set; }
        public string ParentTable { get; set; }
        public string ParentSourceTable { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsReadonlyIdentity { get; set; }

        public Type StrongType
        {
            get
            {
                Type rtrn;
                switch (TypeName)
                {
                    case ("bigint"): { rtrn = typeof(long); break; }
                    case ("binary"): { rtrn = typeof(byte[]); break; }
                    case ("bit"): { rtrn = typeof(bool); break; }
                    case ("char"): { rtrn = typeof(string); break; }
                    case ("date"): { rtrn = typeof(DateTime); break; }
                    case ("datetime"): { rtrn = typeof(DateTime); break; }
                    case ("datetime2"): { rtrn = typeof(DateTime); break; }
                    case ("datetimeoffset"): { rtrn = typeof(DateTimeOffset); break; }
                    case ("decimal"): { rtrn = typeof(decimal); break; }
                    case ("float"): { rtrn = typeof(double); break; }
                    case ("image"): { rtrn = typeof(byte[]); break; }
                    case ("int"): { rtrn = typeof(int); break; }
                    case ("money"): { rtrn = typeof(decimal); break; }
                    case ("nchar"): { rtrn = typeof(string); break; }
                    case ("ntext"): { rtrn = typeof(string); break; }
                    case ("numeric"): { rtrn = typeof(decimal); break; }
                    case ("nvarchar"): { rtrn = typeof(string); break; }
                    case ("real"): { rtrn = typeof(float); break; }
                    case ("rowversion"): { rtrn = typeof(byte[]); break; }
                    case ("smalldatetime"): { rtrn = typeof(DateTime); break; }
                    case ("smallint"): { rtrn = typeof(short); break; }
                    case ("smallmoney"): { rtrn = typeof(decimal); break; }
                    case ("text"): { rtrn = typeof(string); break; }
                    case ("time"): { rtrn = typeof(TimeSpan); break; }
                    case ("timestamp"): { rtrn = typeof(byte[]); break; }
                    case ("tinyint"): { rtrn = typeof(byte); break; }
                    case ("uniqueidentifier"): { rtrn = typeof(Guid); break; }
                    case ("varbinary"): { rtrn = typeof(byte[]); break; }
                    case ("varchar"): { rtrn = typeof(string); break; }
                    default: { rtrn = typeof(object); break; }
                }
                return rtrn;
            }
        }

        public string ExcelColumn
        {
            get
            {
                var rtrn = string.Empty;
                var mapping = WebCache.IsgColumnMappings.FirstOrDefault(x => string.Equals(x.TableName, ParentTable, StringComparison.CurrentCultureIgnoreCase) && string.Equals(x.TableColumnName, Name, StringComparison.CurrentCultureIgnoreCase));
                if (mapping != null)
                {
                    rtrn = mapping.ExcelColumnName;
                }
                return rtrn;
            }
        }

        private GridColumnMapping _configSettings;
        private GridColumnMapping ConfigSettings
        {
            get
            {
                _configSettings = _configSettings ?? WebCache.GridColumnConfiguration.FirstOrDefault(x => string.Equals(x.DBTableColumnName, Name, StringComparison.CurrentCultureIgnoreCase) && string.Equals(x.DBTableName, ParentTable, StringComparison.CurrentCultureIgnoreCase));
                return _configSettings;
            }
        }

        public long Order
        {
            get
            {
                return (ConfigSettings == null || ConfigSettings.Order == 0) ? int.MaxValue : ConfigSettings.Order;
            }
        }

        public long? Width
        {
            get { return (ConfigSettings == null) ? null : ConfigSettings.Width; }
        }

        public string FriendlyName
        {
            get
            {
                return
                    (ConfigSettings == null || string.IsNullOrEmpty(ConfigSettings.FriendlyName))
                        ? Name
                        : ConfigSettings.FriendlyName;
            }
        }

        public string Required
        {
            get
            {
                return (!IsNullable).ToString().ToLower();
            }
        }

        public string Hidden
        {
            get
            {
                return (ConfigSettings != null && ConfigSettings.Hidden).ToString().ToLower();
            }
        }

        public string Frozen
        {
            get
            {
                return (ConfigSettings != null && ConfigSettings.Frozen).ToString().ToLower();
            }
        }

        public string Sortable
        {
            get
            {
                return (ConfigSettings == null || ConfigSettings.Sortable).ToString().ToLower();
            }
        }

        public string SortType
        {
            get
            {
                var rtrn = "text";
                if (StrongType == typeof(short) || StrongType == typeof(int) || StrongType == typeof(long))
                {
                    rtrn = "int";
                }
                else if (StrongType == typeof(decimal) || StrongType == typeof(double))
                {
                    rtrn = "number";
                }
                else if (StrongType == typeof(DateTime))
                {
                    rtrn = "date";
                }

                return rtrn;
            }
        }

        public string Editable
        {
            get
            {
                return (ConfigSettings == null || ConfigSettings.Editable).ToString().ToLower();
            }
        }

        public string EditOptions
        {
            get
            {
                var rtrn = new StringBuilder("{");
                if (StrongType == typeof(short) || StrongType == typeof(int) || StrongType == typeof(long))
                {
                    // any special options?
                }
                else if (StrongType == typeof(decimal) || StrongType == typeof(double))
                {
                    // any special options?
                }
                else if (StrongType == typeof(DateTime))
                {
                    rtrn.Append("dataInit: function (elem) {");
                    rtrn.Append("   $(elem).datepicker()");
                    rtrn.Append("   .attr('readonly', 'true')");
                    rtrn.Append("   .attr('background', 'white')");
                    rtrn.Append("   .keydown(function(e) {");
                    rtrn.Append("       e.preventDefault();");
                    rtrn.Append("       })");
                    rtrn.Append("   .keyup(function(e) {");
                    rtrn.Append("       e.preventDefault();");
                    rtrn.Append("       if(e.keyCode == 8 || e.keyCode == 46) {");
                    rtrn.Append("           $.datepicker._clearDate(this);");
                    rtrn.Append("           $(this).blur();");
                    rtrn.Append("       }");
                    rtrn.Append("   });");
                    rtrn.Append("}");
                }
                else   // text
                {
                    rtrn.Append("dataInit: function (elem) {");
                    rtrn.AppendFormat("   $(elem).attr('maxlength', '{0}');", (Length != -1 ? Length : 500));
                    rtrn.Append("}");
                }

                rtrn.Append("}");
                return rtrn.ToString();
            }
        }
        
        public string EditRules
        {
            get
            {
                var rtrn = new StringBuilder("{");
                if (string.Equals(Hidden, true.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    rtrn.AppendFormat(" \"edithidden\": {0},", false.ToString().ToLower());
                }
                rtrn.AppendFormat(" \"required\": {0},", Required);


                if (StrongType == typeof(short) || StrongType == typeof(int) || StrongType == typeof(long))
                {
                    rtrn.Append(" \"integer\": true");
                }
                else if (StrongType == typeof(decimal) || StrongType == typeof(double))
                {
                    rtrn.Append(" \"number\": true");
                }

                rtrn.Append("}");
                return rtrn.ToString();
            }
        }

        public string EditType
        {
            get
            {
                return (Length != -1) ? "text" : "textarea";
            }
        }

        public string Formatter
        {
            get
            {
                var rtrn = "";
                if (StrongType == typeof(short) || StrongType == typeof(int) || StrongType == typeof(long))
                {
                    rtrn = "integer";
                }
                else if (StrongType == typeof(decimal) || StrongType == typeof(double))
                {
                    rtrn = "number";
                }
                else if (StrongType == typeof(DateTime))
                {
                    rtrn = "date";
                }
                return rtrn;
            }
        }

        public string FormatOptions
        {
            get
            {
                var rtrn = string.Empty;
                if (Formatter.Equals("date"))
                {
                    rtrn = "{ \"srcformat\": \"m/d/Y\", \"newformat\": \"m/d/Y\"}";
                }
                return rtrn;
            }
        }

        public string Search
        {
            get { return (SearchType != Constants.SearchType.None).ToString().ToLower(); }
        }

        public string SearchType
        {
            get
            {
                return
                    (ConfigSettings == null || string.IsNullOrEmpty(ConfigSettings.SearchType))
                        ? Constants.SearchType.None
                        : ConfigSettings.SearchType;
            }
        }

        public string SearchOptions
        {
            get
            {
                var rtrn = string.Empty;
                if (SearchType.Equals(Constants.SearchType.None))
                {
                    return rtrn;
                }
                var searchOps = new StringBuilder("\"searchoptions\":{");

                var sopt = SearchType.Equals(Constants.SearchType.Select) ? "[\"eq\"]," : "[\"cn\"]";
                searchOps.AppendFormat("\"sopt\": {0}", sopt);

                if (SearchType.Equals(Constants.SearchType.Select))
                {
                    searchOps.AppendFormat("\"value\": {0}, ", UniqueColumnValues);

                    const string attr = "{\"multiple\": \"multiple\", \"class\": \"searchDropDown\"}";
                    searchOps.AppendFormat("\"attr\": {0},", attr);
                }
                rtrn = searchOps.ToString().TrimEnd(',') + "},";
                return rtrn;
            }
        }

        private string _uniqueColumnValues;
        public string UniqueColumnValues
        {
            get
            {
                if (_uniqueColumnValues != null)
                {
                    return _uniqueColumnValues;
                }
                using (new NeonISGDataServiceClient(Configuration.ActiveNeonDataServiceEndpoint))
                {
                    var data = new StringBuilder();
                    data.Append("{");
                    data.Append("}");

                    _uniqueColumnValues = data.ToString();
                }
                return _uniqueColumnValues;
            }
        }

        public string Align
        {
            get
            {
                return
                    (StrongType == typeof(short) || StrongType == typeof(int) || StrongType == typeof(long))
                        ? "right"
                        : "left";
            }
        }

        public string JsonColumn
        {
            get
            {
                var rtrn = new StringBuilder();
                rtrn.Append("{");
                rtrn.AppendFormat(" \"name\": \"{0}\",", Name);
                rtrn.AppendFormat(" \"label\": \"{0}\",", FriendlyName);
                rtrn.AppendFormat(" \"index\": \"{0}\",", Name);
                rtrn.AppendFormat(" \"hidden\": {0},", Hidden);
                rtrn.AppendFormat(" \"frozen\": {0},", Frozen);
                rtrn.AppendFormat(" \"search\": {0},", Search);
                rtrn.AppendFormat(" \"stype\": \"{0}\",", SearchType);
                rtrn.AppendFormat(" \"sortable\": {0},", Sortable);

                if (!string.Equals(Name, "act", StringComparison.CurrentCultureIgnoreCase))
                {
                    rtrn.Append(SearchOptions);
                }

                if (Width.HasValue)
                {
                    rtrn.AppendFormat(" \"width\": \"{0}\",", Width);
                }
                if (Convert.ToBoolean(Sortable))
                {
                    rtrn.AppendFormat(" \"sorttype\": \"{0}\",", SortType);
                }
                if (string.Equals(Hidden, false.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    rtrn.AppendFormat(" \"formatter\": \"{0}\",", Formatter);
                    if (Formatter.Equals("date"))
                    {
                        rtrn.AppendFormat(" \"formatoptions\": {0},", FormatOptions);
                    }
                }
                if (Convert.ToBoolean(Editable))
                {
                    rtrn.AppendFormat(" \"edittype\": \"{0}\",", EditType);
                    rtrn.AppendFormat(" \"editable\": {0},", Editable);
                    rtrn.AppendFormat(" \"editoptions\": {0},", EditOptions);
                    rtrn.AppendFormat(" \"editrules\": {0},", EditRules);
                }
                rtrn.AppendFormat(" \"align\": \"{0}\"", Align);
                rtrn.Append("}");
                return rtrn.ToString();
            }
        }

        public WebDataTableColumn(string name)
        {
            Name = name;
        }
    }
}