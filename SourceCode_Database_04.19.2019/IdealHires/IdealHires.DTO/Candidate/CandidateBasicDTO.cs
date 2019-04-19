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

        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Desired job title contains only alphabets")]
        [StringLength(255, ErrorMessage = "Desired job title cannot be longer than 255 characters.")]
        public string JobTitle { get; set; }    
        public HttpPostedFileBase ResumeFile { get; set; }
        public string ResumeFilePath { get; set; }    
        public string OrgFileName { get; set; }

        [Required(ErrorMessage = "First name is required.")]        
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "First name contains only alphabets")]
        [StringLength(255, ErrorMessage = "First name cannot be longer than 255 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Last name contains only alphabets")]
        [StringLength(255, ErrorMessage = "Last name cannot be longer than 255 characters.")]
        public string LastName { get; set; }
        public string UserType { get; set; }
    }
}
