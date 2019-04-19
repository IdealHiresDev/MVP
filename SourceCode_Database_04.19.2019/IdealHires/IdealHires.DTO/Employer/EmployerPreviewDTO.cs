using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.DTO.Employer
{
    public  class EmployerPreviewDTO
       
    {
        public int Id { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int? JobTypeId { get; set; }
        public string JobTypeName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
       public string Keywords { get; set; }
        public decimal? MinimumSalary { get; set; }
        public decimal? MaximumSalary { get; set; }
        public int? PayPeriodTypeId { get; set; }
        public int? CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string Positions { get; set; }
        public string LocationCity { get; set; }
        public string LocationState { get; set; }
        public string LocationCountry { get; set; }
        public string city { get; set; }
        public string countryName { get; set; }
        public string JobCategoryName { get; set; }
        public string PayPeriodTypeName { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public byte[] Img { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }


    }
}
