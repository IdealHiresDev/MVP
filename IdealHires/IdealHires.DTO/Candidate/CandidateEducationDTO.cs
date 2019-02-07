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
        [DataType(DataType.Text)]
        public string Major { get; set; }

        [Required(ErrorMessage = "Minor is required.")]
        [DataType(DataType.Text)]
        public string Minor { get; set; }

        public string InstituteName { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date)]
        public DateTime? StartAt { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [DataType(DataType.Date)]
        [DateCompare(CompareToPropertyName = "StartAt", OperatorName = DateCompareOperator.GreaterThanOrEqual, ErrorMessage = "Start date must be less than or equal to End date")]
        public DateTime? EndAt { get; set; }

        [Required(ErrorMessage = "Major Degree/Certificate is required.")]
        public bool IsDegreeOrCertification { get; set; }

        [Required(ErrorMessage = "Minor Degree/Certificate is required.")]
        public bool IsMinorDegree { get; set; }

        public string TotalDuration { get; set; }
        public string EduOption { get; set; }
    }
}
