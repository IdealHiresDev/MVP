using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.DTO.Candidate
{
    public class CandidateWorkDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProfileId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string CompanyName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string PositionName { get; set; }
        public string Description { get; set; }
        public string Salary { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public int CurrencyId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
    }
}
