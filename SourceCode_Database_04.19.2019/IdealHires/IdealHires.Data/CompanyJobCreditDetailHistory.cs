//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IdealHires.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class CompanyJobCreditDetailHistory
    {
        public int Id { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> AvailableCredit { get; set; }
        public Nullable<int> UsedCredit { get; set; }
        public Nullable<int> UsedBy { get; set; }
        public Nullable<System.DateTime> UsedDate { get; set; }
    
        public virtual Company Company { get; set; }
    }
}