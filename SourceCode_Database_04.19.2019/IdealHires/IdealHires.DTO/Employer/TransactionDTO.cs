using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.DTO.Employer
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public decimal? TotalAmount { get; set; }
        public string TransId { get; set; }
        public DateTime? TransactionDateTime { get; set; }
        public string AccountType { get; set; }
        public string Authorization { get; set; }
        public string ResponseCode { get; set; }
        public int? ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Message { get; set; }
        public int JobCreditId { get; set; }
        public int JobCredit { get; set; }
    }
}
