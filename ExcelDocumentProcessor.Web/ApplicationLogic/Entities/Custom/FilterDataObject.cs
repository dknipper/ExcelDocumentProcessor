using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelDocumentProcessor.Web.ApplicationLogic.Entities.Custom
{
    public class FilterDataObject
    {
        private string _filterQuery;

        public string GroupOp { private get; set; }
        public List<Dictionary<string, string>> Rules { private get; set; }

        // ReSharper disable once EmptyConstructor
        public FilterDataObject() { }

        public string FilterQuery
        {
            get
            {
                if (_filterQuery != null)
                {
                    return _filterQuery;
                }
                var queryString = new StringBuilder();

                if (Rules != null && Rules.Any())
                {
                    queryString.AppendFormat("WHERE ");

                    Rules.ForEach(rule =>
                    {
                        var field = rule["field"];
                        var data = rule["data"].Replace("'", "''");

                        // handle TEXT filter
                        if (rule["op"].Contains("cn"))
                        {
                            queryString.AppendFormat("{0} LIKE \'%{1}%\'", field, data);
                        }
                        // handle DROPDOWN filter
                        else if (rule["op"].Contains("eq"))
                        {
                            queryString.Append("(");
                            var selected = rule["data"].Split(new []{"C711291D"}, StringSplitOptions.None);
                            foreach (var option in selected)
                            {
                                queryString.AppendFormat("{0} = \'{1}\'", field, option.Replace("'", "''"));
                                if (!selected.Last().Equals(option))
                                    queryString.AppendFormat(" OR ");
                            }
                            queryString.Append(")");
                        }

                        if (!Rules.Last()["field"].Equals(rule["field"]))
                            queryString.AppendFormat(" {0} ", GroupOp);
                    });
                }
                _filterQuery = queryString.ToString();
                return _filterQuery;
            }
        }
    }
}