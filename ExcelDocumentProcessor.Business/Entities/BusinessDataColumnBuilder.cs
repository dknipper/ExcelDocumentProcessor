using System;

namespace ExcelDocumentProcessor.Business.Entities
{
    public class BusinessDataColumnBuilder
    {
        private bool _allowDbNull = true;
        public bool AllowDBNull
        {
            get { return _allowDbNull; }
            set { _allowDbNull = value; }
        }

        public string ColumnName { get; set; }
        public string DataTypeName { get; set; }
        public string DefaultValueString { get; set; }

        public Type DataType
        {
            get { return Type.GetType(DataTypeName); }
        }

        public object DefaultValue
        {
            get
            {
                var convertedValue = Convert.ChangeType(DefaultValueString, Type.GetType(DataType.ToString()) ?? typeof(string));
                return (!string.IsNullOrEmpty(DefaultValueString)) ? convertedValue : null;
            }
        }
    }
}