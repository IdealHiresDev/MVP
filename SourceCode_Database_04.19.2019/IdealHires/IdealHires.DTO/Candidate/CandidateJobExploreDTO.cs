using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IdealHires.DTO.Candidate
{
    public class CandidateJobExploreDTO
    {
        public CandidateJobExploreDTO()
        {
            SelectJobTypes = new List<SelectListItem>();
        }
        public int JobId { get; set; }
        public int  UserId { get; set; }
        public string Keywords { get; set; }
        public string JobTitle { get; set; }
        public string Description { get; set; }
        public string JobCategory { get; set; }
        public string JobType { get; set; }
        public string JobLocation { get; set; }
        public string Company { get; set; }
        public int? ActionId { get; set; }
        public IList<SelectListItem> SelectJobTypes { get; set; }
        public List<SelectListItem> SelectJobCategories { get; set; }
        public List<string> SelectedJobCategory { get; set; }
        public JobDTO PropJobDTO { get; set; }
        public List<JobDTO> AvailableJobs { get; set; }
        public string OrderBy { get; set; }
        public string Message { get; set; }
        public byte[] Img { get; set; }
        public string CompanyLogo { get; set; }
    }
}
