//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExcelDocumentProcessor.Data.Entities.SystemGenerated
{
    using System;
    using System.Collections.Generic;
    
    public partial class TableMetaData
    {
        public System.Guid Id { get; set; }
        public string tableName { get; set; }
        public string tableDescription { get; set; }
        public string columnName { get; set; }
        public string columnDescription { get; set; }
        public string columnTypeName { get; set; }
        public Nullable<short> columnPrecision { get; set; }
        public Nullable<int> columnScale { get; set; }
        public short columnLength { get; set; }
        public Nullable<int> columnIsNullable { get; set; }
        public Nullable<short> columnOrdinal { get; set; }
        public Nullable<int> isPrimaryKey { get; set; }
        public int isReadonlyIdentity { get; set; }
        public string sourceTableName { get; set; }
    }
}
