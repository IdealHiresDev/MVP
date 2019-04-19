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
    
    public partial class JobCredit
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public JobCredit()
        {
            this.CompanyPackageDetails = new HashSet<CompanyPackageDetail>();
        }
    
        public int Id { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Duration { get; set; }
        public string JobCredit1 { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<decimal> Discount { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyPackageDetail> CompanyPackageDetails { get; set; }
    }
}