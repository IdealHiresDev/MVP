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

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Website { get; set; }
        
        public string Description { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        public string Title { get; set; }
        public string ContactEmail { get; set; }
        public bool IsAgree { get; set; }
    }
}
