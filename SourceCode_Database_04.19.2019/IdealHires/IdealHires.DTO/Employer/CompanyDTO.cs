using IdealHires.DTO.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IdealHires.DTO.Employer
{
    public class CompanyDTO
    {
        public CompanyDTO()
        {

            SelectCountries = new List<SelectListItem>();
            SelectStates = new List<SelectListItem>();
            SelectCities = new List<SelectListItem>();
            SelectAddressTypes= new List<SelectListItem>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "Employer phone is required.")]
     
        public string Phone { get; set; }
        [Required(ErrorMessage = "Employer phone2 is required.")]
        public string Phone1 { get; set; }
        [Required(ErrorMessage = "Company website is required.")]
        [RegularExpression(@"^([a-zA-Z0-9]+(\.[a-zA-Z0-9]+)+.*)$", ErrorMessage = "Please input valid website")]
        public string Website { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Company name is required.")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Employer first name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Employer last name is required.")]
        public string LastName { get; set; }
        public string Title { get; set; }

        [CheckBoxRequired(ErrorMessage = "Please accept the terms and condition.")]
        public bool IsAgree { get; set; }
        public bool IsChecked { get; set; }
        public string ZipCode { get; set; }
        public string AddreessType { get; set; }
        public List<SelectListItem> SelectCountries { get; set; }
        public IList<string> SelectedCountries { get; set; }
        public List<SelectListItem> SelectStates { get; set; }
        public IList<string> SelectedStates { get; set; }
        public List<SelectListItem> SelectCities { get; set; }
        public IList<string> SelectedCities { get; set; }
        public CompanyAddressDTO companyAddressDTO { get; set; }
        public List<CompanyAddressDTO> companyAddressDTOList { get; set; }
        public List<SelectListItem> SelectAddressTypes { get; set; }
        public string PhotoURL { get; set; }
        public byte[] Img { get; set; }
        public string avatarCropped { get; set; }
    }
}
