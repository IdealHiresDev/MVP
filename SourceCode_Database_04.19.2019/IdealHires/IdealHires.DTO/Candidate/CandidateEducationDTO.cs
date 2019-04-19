using IdealHires.DTO.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.DTO.Candidate
{
    public class CandidateEducationDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProfileId { get; set; }

        [Required(ErrorMessage = "Major is required.")]
        [StringLength(255, ErrorMessage = "Major cannot be longer than 255 characters.")]
        [DataType(DataType.Text)]
        public string Major { get; set; }

        [DataType(DataType.Text)]
        [StringLength(255, ErrorMessage = "Minor cannot be longer than 255 characters.")]
        public string Minor { get; set; }
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Institute name contains only alphabets")]
        [StringLength(255, ErrorMessage = "Institute name cannot be longer than 255 characters.")]
        public string InstituteName { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date)]
        public DateTime? StartAt { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [DataType(DataType.Date)]
        public DateTime? EndAt { get; set; }

        [Required(ErrorMessage = "Major Degree/Certificate is required.")]
        public bool IsDegreeOrCertification { get; set; }
        
        public bool IsMinorDegree { get; set; }

        public string TotalDuration { get; set; }        
    }
}
