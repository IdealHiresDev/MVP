using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IdealHires.DTO.Employer
{
    public class JobCreditDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 999999999, ErrorMessage = "Please is required price 0.00")]
        public decimal? Price { get; set; }
        [Required(ErrorMessage = "Discount is required.")]
        [Range(0.01, 999999999, ErrorMessage = "Discount is required 0.00")]
        public decimal? Discount { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Duration is required.")]
        public string Duration { get; set; }
        [Required(ErrorMessage = "Job Credit is required.")]
        public string JobCredit { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
