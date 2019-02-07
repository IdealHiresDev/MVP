using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IdealHires.DTO.Employer
{
    public class JobBasicDTO
    {
        public JobBasicDTO()
        {
            SelectJobTypes = new List<SelectListItem>();
            SelectJobCategories = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public string JobTitle { get; set; }
        public string Keywords { get; set; }
       
        public IList<SelectListItem> SelectJobTypes { get; set; }
        public List<SelectListItem> SelectJobCategories { get; set; }

        public IList<string> SelectedJobTypes { get; set; }
        public IList<string> SelectedJobCategory { get; set; }


        public string Description { get; set; }
    }
}

