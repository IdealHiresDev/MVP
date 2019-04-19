using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IdealHires.DTO.Candidate
{
    public class JobDTO
    {
        public int Id { get; set; }
        public int? CompanyId { get; set; }
        public byte[] Img { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyName { get; set; }
        public int? JobTypeId { get; set; }
        public string JobTypeName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Currency)]
        public decimal? MinimumSalary { get; set; }
        public decimal? MaximumSalary { get; set; }
        public int? PayPeriodTypeId { get; set; }
        public int? CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string Positions { get; set; }
        public int? Location { get; set; }
        public string LocationCity { get; set; }
        public string LocationState { get; set; }
        public string LocationCountry { get; set; }     
        public string JobCategoryName { get; set; }
        public string Keywords { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? ActionId { get; set; }
        public string Responsibilities { get; set; }
        public List<ProfileJobDTO> ProfileJobs { get; set; }   
    }
}
