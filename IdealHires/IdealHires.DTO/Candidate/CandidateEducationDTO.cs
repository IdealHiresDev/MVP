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
        [DataType(DataType.Text)]
        public string Major { get; set; }

        [Required(ErrorMessage = "Minor is required.")]
        [DataType(DataType.Text)]
        public string Minor { get; set; }
        public string InstituteName { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Text)]
        public string StartAt { get; set; }
        public string EndAt { get; set; }
        public bool IsDegreeOrCertification { get; set; }
        public string TotalDuration { get; set; }
    }
}
