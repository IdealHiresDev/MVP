using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.DTO.Candidate
{
    public class CandidateContactDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "Street address is required.")]
        [DataType(DataType.Text)]
        [StringLength(255, ErrorMessage = "Street address cannot be longer than 255 characters.")]
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }

        [Required(ErrorMessage = "City is required.")]  
        [RegularExpression(@"^[a-zA-Z ]*$",ErrorMessage ="City contains only alphabets")]
        [StringLength(255, ErrorMessage = "City cannot be longer than 255 characters.")]
        public string City { get; set; }

    
        public string StateName { get; set; }
        [Required(ErrorMessage = "State is required.")]
        public int? StateId { get; set; }

       
        [DataType(DataType.Text)]   
        public string CountryName { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        public int? CountryId { get; set; }

        [Required(ErrorMessage = "Zip code is required.")]
        [RegularExpression("^[0-9]{5}(?:-[0-9]{4})?$", ErrorMessage = "Invalid ZIP code.")]
        [MaxLength(5)]
        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "Phone is required.")]
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        public List<CountryDTO> Countries { get; set; }
        public List<StateDTO> States { get; set; }
    }
}
