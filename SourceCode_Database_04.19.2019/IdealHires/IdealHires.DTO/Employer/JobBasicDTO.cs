using IdealHires.DTO.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage = "Job Title is required.")]
        public string JobTitle { get; set; }
        public string Keywords { get; set; }
        public string JobType { get; set; }
        public string JobCategory { get; set; }

        public IList<SelectListItem> SelectJobTypes { get; set; }
        
        public IList<SelectListItem> SelectJobCategories { get; set; }
        
        public IList<string> SelectedJobTypes { get; set; }
        
        public IList<string> SelectedJobCategory { get; set; }

        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime? ExpiredAt { get; set; }
        [UIHint("tinymce_full")]
        [Display(Name = "Responsibilities")]
        public string Responsibilities { get; set; }
    }
}

