using IdealHires.DTO.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IdealHires.DTO.Employer
{
    public class JobPreferencesDTO
    {
        public JobPreferencesDTO()
        {
            SelectNotificationTypes = new List<SelectListItem>();
            SelectPayPeriodTypes = new List<SelectListItem>();
            SelectJobCategories = new List<SelectListItem>();
            SelectJobCategories = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int JobId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string PayPeriodType { get; set; }
        //public string AddressType { get; set; }
       // [Required(ErrorMessage = "Select Address  is required.")]
        public int? AddressNameId { get; set; }
        public string Description { get; set; }
      
        //[Range(0.01, 999999999, ErrorMessage = "Minimum Salary must be greater than 0.00")]
        //[DisplayName("Price ($)")]
        public decimal? MinimumSalary { get; set; }
        public decimal? MaximumSalary { get; set; }
        public IList<SelectListItem> SelectNotificationTypes { get; set; }

        public IList<string> SelectedNotificationTypes { get; set; }
        public IList<SelectListItem> SelectAddressTpe { get; set; }

        public IList<string> SelectedAddressTpe { get; set; }
        public IList<SelectListItem> SelectAddressName { get; set; }
        public IList<string> SelectedAddressName { get; set; }
        public IList<SelectListItem> SelectCompanyCity { get; set; }
        [Required(ErrorMessage = "Select city  is required.")]
        public IList<string> SelectedCompanyCityName { get; set; }
        //public IList<SelectListItem> SelectAddressTypeName { get; set; }
        //[Required(ErrorMessage = "Select Address  is required.")]
        //public IList<string> SelectedAddressTypeName { get; set; }
        public List<SelectListItem> SelectPayPeriodTypes { get; set; }
        [Required(ErrorMessage = "Pay Period is required.")]
        public IList<string> SelectedPayPeriodTypes { get; set; }
        public IList<string> SelectedJobCategory { get; set; }
        public List<SelectListItem> SelectJobCategories { get; set; }
        public int CurrencyId { get; set; }
        [Required(ErrorMessage = "Position  is required.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Positions must be numeric")]
        public string Positions { get; set; }
        public int LocationCity { get; set; }
        public string LocationState { get; set; }
        public string LocationCountry { get; set; }
        public IList<SelectListItem> SelectCountry { get; set; }

        public IList<string> SelectedCountry { get; set; }
        public IList<SelectListItem> SelectState { get; set; }

        public IList<string> SelectedState { get; set; }
        public IList<SelectListItem> SelectCity { get; set; }

        public IList<string> SelectedCity { get; set; }
        [Required(ErrorMessage = "Expiration Date is required.")]
        [ExpiryDate(ErrorMessage = "Back date entry not allowed.")]
        [DataType(DataType.Date)]
        public DateTime? ExpiredAt { get; set; }
        public string Location { get; set; }
        public string Assumption { get; set; }
      
        public bool UnAssumption { get; set; }
        public List<StateDTO> stateDtoList { get; set; }
        public CompanyAddressDTO companyAddressDTO { get; set; }
        public List<CompanyAddressDTO> companyAddressList { get; set; }
        public JobPreferencesDTO jobPreferencesDTO { get; set; }
    }
}
