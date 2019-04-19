using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.DTO.Employer
{
    public class EmployerDashboardDTO
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public int UserId { get; set; }
        public string JobTitle { get; set; }
        public int? ActionId { get; set; }
        public string Action { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Status { get; set; }
        

    }
}
