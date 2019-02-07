using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IdealHires.DTO.Candidate
{
    public class CandidatePreferencesDTO
    {
        public CandidatePreferencesDTO()
        {
            SelectJobTypes = new List<SelectListItem>();
            SelectJobCategories = new List<SelectListItem>();           
        }
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage ="Keywords is required.")]
        public string Keywords { get; set; }       
        public string Objective { get; set; }
        public IList<SelectListItem> SelectJobTypes { get; set; }
        public List<SelectListItem> SelectJobCategories { get; set; }      
        public IList<string> SelectedJobTypes { get; set; }
        public IList<string> SelectedJobCategory { get; set; }
        public string PreferenceOption { get; set; }
    }
}
