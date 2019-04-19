using IdealHires.DTO.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

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

        [Required(ErrorMessage = "Title name is required.")] 
        [DataType(DataType.Text)]
        public string PositionName { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.Text)]
        [Required]
        [RegularExpression(@"^\d*(\.\d{0,2})?$", ErrorMessage ="Salary should be numeric.")]
        public string Salary { get; set; }

        [DataType(DataType.Text)]
        public int CurrencyId { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date)]
        public DateTime? StartAt { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [DataType(DataType.Date)]
        [DateCompare(CompareToPropertyName = "StartAt", OperatorName = DateCompareOperator.GreaterThanOrEqual, ErrorMessage = "Start date must be less than or equal to End date")]
        public DateTime? EndAt { get; set; }

        [DataType(DataType.Text)]
        public bool IsCurrent { get; set; }
        public string TotalExperience { get; set; }
        public string SelectedPayPeriod { get; set; }
        public int PayPeriodTypeId { get; set; }
        public List<SelectListItem> PayPeriodTypes { get; set; }
    }
}
