using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.DTO.Candidate
{
    public class CandidateBasicDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "Desired Job Title is required.")]
        [DataType(DataType.Text)]        
        public string JobTitle { get; set; }  
        
        public string ResumeFile { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }
    }
}
