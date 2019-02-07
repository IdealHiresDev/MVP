using IdealHires.DTO.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.DTO.Employer
{
    public class CompanyDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage ="Employer phone is required.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Employer email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Company location is required.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Company website is required.")]
        public string Website { get; set; }
        
        public string Description { get; set; }

        [Required(ErrorMessage = "Company name is required.")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Employer first name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Employer last name is required.")]
        public string LastName { get; set; }
        public string Title { get; set; }
        public string ContactEmail { get; set; }

        [CheckBoxRequired(ErrorMessage = "Please accept the terms and condition.")]
        public bool IsAgree { get; set; }
    }
}
