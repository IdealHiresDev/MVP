using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IdealHires.DTO.Employer
{
    public class CandidateDetails
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string JobTitle { get; set; }
        public HttpPostedFileBase ResumeFile { get; set; }
        public string ResumeFilePath { get; set; }
        public string OrgFileName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserType { get; set; }
        public string Objective { get; set; }
    }
}
