
using System;

namespace ExcelDocumentProcessor.Business.Entities
{
    public class BusinessDataTableColumn
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public string StrongTypeName { get; set; }
        public short? Precision { get; set; }
        public short? ColumnOrdinal { get; set; }
        public int? Scale { get; set; }
        public short Length { get; set; }
        public bool IsNullable { get; set; }
        public string ParentTable { get; set; }
        public string ParentSourceTable { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsReadonlyIdentity { get; set; }
        public string ExcelColumn { get; set; }
        public string FriendlyName { get; set; }

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
    }
}
