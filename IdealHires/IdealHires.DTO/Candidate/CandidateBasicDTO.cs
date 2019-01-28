using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IdealHires.DTO.Candidate
{
    public class CandidateBasicDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "Desired job title is required.")]
        [DataType(DataType.Text)]        
        public string JobTitle { get; set; }  
        
        public HttpPostedFileBase ResumeFile { get; set; }
        public string ResumeFilePath { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        public string UserType { get; set; }
    }
}
