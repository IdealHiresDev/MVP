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
    
    public partial class ProfileJob
    {
        public int Id { get; set; }
        public Nullable<int> ProfileId { get; set; }
        public Nullable<int> JobId { get; set; }
        public Nullable<int> ActionId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
    
        public virtual Action Action { get; set; }
        public virtual Job Job { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual User User { get; set; }
    }
}
