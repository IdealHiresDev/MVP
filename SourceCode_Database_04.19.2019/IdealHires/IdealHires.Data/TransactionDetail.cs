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
    
    public partial class TransactionDetail
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string TransactionId { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public string AccountType { get; set; }
        public string Authorization { get; set; }
        public Nullable<int> ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Message { get; set; }
    }
}
