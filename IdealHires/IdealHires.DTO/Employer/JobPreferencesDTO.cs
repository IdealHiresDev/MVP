using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IdealHires.DTO.Employer
{
    public class JobPreferencesDTO
    {
        public JobPreferencesDTO()
        {
            SelectNotificationTypes = new List<SelectListItem>();
            SelectPayPeriodTypes = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int JobId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MinimumSalary { get; set; }
        public string MaximumSalary { get; set; }
        public IList<SelectListItem> SelectNotificationTypes { get; set; }
        public List<SelectListItem> SelectPayPeriodTypes { get; set; }
        public IList<string> SelectedNotificationTypes { get; set; }
        public IList<string> SelectedPayPeriodTypes { get; set; }
        public int CurrencyId { get; set; }
        public string Positions { get; set; }
        public string LocationCity { get; set; }
        public string LocationState { get; set; }
        public string LocationCountry { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public bool Location { get; set; }
    }
}
