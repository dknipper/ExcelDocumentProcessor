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
    
    public partial class RoleMapping
    {
        public int InstanceId { get; set; }
        public int RoleId { get; set; }
        public int FunctionId { get; set; }
        public bool Status { get; set; }
    
        public virtual Function Function { get; set; }
    }
}