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
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [DataType(DataType.Text)]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [DataType(DataType.Text)]
        public string State { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [DataType(DataType.Text)]
        public string Country { get; set; }

        [Required(ErrorMessage = "Zip code is required.")]
        [RegularExpression("^[0-9]{5}(?:-[0-9]{4})?$", ErrorMessage = "Invalid ZIP code.")]
        [MaxLength(5)]
        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        [RegularExpression(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}", ErrorMessage = "Invalid phone number.")]
        [MaxLength(10)]
        [DataType(DataType.PhoneNumber)]        
        public string Phone1 { get; set; }

        [RegularExpression(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}", ErrorMessage = "Invalid phone number 2.")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(10)]
        public string Phone2 { get; set; }
        public string EmailAddress { get; set; }
    }
}
