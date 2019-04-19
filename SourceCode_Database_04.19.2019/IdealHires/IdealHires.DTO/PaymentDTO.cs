using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.DTO
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public string StateName { get; set; }
        public int? StateId { get; set; }
        public int? Zip { get; set; }
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public int? ExpirationMonth { get; set; }
        public int? ExpirationYear { get; set; }
        public int? CVVNumber { get; set; }
        public string Description { get; set; }
        public double? Amount { get; set; }
    }
}
