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
        [Required]
        [DataType(DataType.Text)]
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string City { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string State { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Country { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string ZipCode { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
    }
}
