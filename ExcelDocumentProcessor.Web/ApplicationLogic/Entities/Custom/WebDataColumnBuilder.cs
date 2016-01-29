
using System;

namespace ExcelDocumentProcessor.Web.ApplicationLogic.Entities.Custom
{
    public class WebDataColumnBuilder
    {
        private bool _allowDbNull = true;
        public bool AllowDBNull
        {
            get { return _allowDbNull; }
            set { _allowDbNull = value; }
        }

        public string ColumnName { get; set; }
        public string DataTypeName { get; private set; }
        public string DefaultValueString{ get; set; }

        public WebDataColumnBuilder(bool allowDbNull, string columnName, Type dataType, string defaultValueString)
        {
            AllowDBNull = allowDbNull;
            ColumnName = columnName;
            DataTypeName = dataType.ToString();
            DefaultValueString = defaultValueString;
        }
    }
}