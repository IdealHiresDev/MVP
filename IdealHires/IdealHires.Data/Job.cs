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
    
    public partial class Job
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Job()
        {
            this.JobCategoryJobs = new HashSet<JobCategoryJob>();
            this.JobTypeJobs = new HashSet<JobTypeJob>();
            this.KeywordsJobs = new HashSet<KeywordsJob>();
            this.ProfileJobs = new HashSet<ProfileJob>();
        }
    
        public int Id { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MinimumSalary { get; set; }
        public string MaximumSalary { get; set; }
        public string PayPeriod { get; set; }
        public Nullable<int> CurrencyId { get; set; }
        public string Positions { get; set; }
        public string LocationCity { get; set; }
        public string LocationState { get; set; }
        public string LocationCountry { get; set; }
        public Nullable<System.DateTime> ExpiredAt { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual Currency Currency { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobCategoryJob> JobCategoryJobs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobTypeJob> JobTypeJobs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KeywordsJob> KeywordsJobs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProfileJob> ProfileJobs { get; set; }
    }
}