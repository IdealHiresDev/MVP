using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.DTO.Employer
{
    public class JobCreditListDTO
    {
        public int? Id { get; set; }
        public int? CompanyId { get; set; }
        public int? UserId { get; set; }
        public decimal? Price { get; set; }
        public string Duration { get; set; }
        public string JobCredit { get; set; }
        public string Description { get; set; }
        public decimal? Discount { get; set; }
    }
}
