using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IdealHires.DTO.Employer
{
    public class EUserDTO
    {
        public EUserDTO()
        {
            SelectRoles = new List<SelectListItem>();
            SelectCountries = new List<SelectListItem>();
            SelectStates = new List<SelectListItem>();
        }
        public int Id { get; set; }

        public int EUserId { get; set; }
        public int CompanyId { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

 
        [DataType(DataType.PhoneNumber)]       
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid number.")]
        public string Phone1 { get; set; }

        public string Phone2 { get; set; }
      
        public int? Role { get; set; }

      
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public List<SelectListItem> SelectRoles { get; set; }
        public IList<string> SelectedRole { get; set; }
        public string City { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Description { get; set; }

        public List<SelectListItem> SelectCountries { get; set; }
        public IList<string> SelectedCountry { get; set; }
        public List<StateDTO> States { get; set; }
        public List<CountryDTO> Countries { get; set; }
        public List<SelectListItem> SelectStates{ get; set; }
        public IList<string> SelectedStates { get; set; }
    }
}
