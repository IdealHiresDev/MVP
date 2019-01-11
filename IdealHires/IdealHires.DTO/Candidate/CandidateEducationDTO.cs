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

        [Required]
        [DataType(DataType.Text)]
        public string Major { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Minor { get; set; }
        public string InstituteName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public bool IsDegreeOrCertification { get; set; }
    }
}
