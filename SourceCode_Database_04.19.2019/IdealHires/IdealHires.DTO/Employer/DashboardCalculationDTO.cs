using System.Collections.Generic;

namespace IdealHires.DTO.Employer
{
    public class DashboardCalculationDTO
    {
        public int TotalJob { get; set; }
        public int ApplicationSubmit { get; set; }
        public int TotalCallForInterview { get; set; }
        public int TotalJobViewed { get; set; }
        public string DasboardType { get; set; }
        public List<EmployerDashboardDTO> emloyerDashboardList { get; set; }

       
    }
}