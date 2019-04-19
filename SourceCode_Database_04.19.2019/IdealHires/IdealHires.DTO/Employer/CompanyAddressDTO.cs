using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IdealHires.DTO.Employer
{
    public class CompanyAddressDTO
    {
        public CompanyAddressDTO()
        {

            SelectCountries = new List<SelectListItem>();
            SelectStates = new List<SelectListItem>();
            SelectCities = new List<SelectListItem>();
            SelectAddressType = new List<SelectListItem>();
            SelectAddressName = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int AddressTypeId { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessage = "ZipCode must be numeric")]
        public string ZipCode { get; set; }
        public int CountryId { get; set; }
        
        public int StateId { get; set; }
        public string City { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string Address { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public List<SelectListItem> SelectCountries { get; set; }
        public IList<string> SelectedCountries { get; set; }
        public List<SelectListItem> SelectStates { get; set; }
        public IList<string> SelectedStates { get; set; }
        public List<SelectListItem> SelectCities { get; set; }
        public IList<string> SelectedCities { get; set; }
        public List<SelectListItem> SelectAddressType { get; set; }
        public List<SelectListItem> SelectAddressName { get; set; }
    }
}
