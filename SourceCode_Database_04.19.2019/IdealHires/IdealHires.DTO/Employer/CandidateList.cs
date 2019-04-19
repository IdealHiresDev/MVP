using IdealHires.DTO.Candidate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IdealHires.DTO.Employer
{
    public class CandidateList
    {
        public string JobTitle { get; set; }
        public string Objective { get; set; }
        public string JobType { get; set; }
        public string OrderBy { get; set; }
        public int UserId { get; set; }
        public IList<SelectListItem> SelectJobTypes { get; set; }
        public List<CandidateDetails> CandidateDetailList { get; set; }
    }
}
