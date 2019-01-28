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

        [Required(ErrorMessage = "Company name is required.")]
        [DataType(DataType.Text)]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Position name is required.")]
        [DataType(DataType.Text)]
        public string PositionName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.Text)]
        public string Salary { get; set; }

        //[Required(ErrorMessage = "Currency is required.")]
        [DataType(DataType.Text)]
        public int CurrencyId { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Text)]
        public string StartAt { get; set; }

        [DataType(DataType.Text)]
        public string EndAt { get; set; }

        [DataType(DataType.Text)]
        public bool IsCurrent { get; set; }

        public string TotalExperience { get; set; }

    }
}
