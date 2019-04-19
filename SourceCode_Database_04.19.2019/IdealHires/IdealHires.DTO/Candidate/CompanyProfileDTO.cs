using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.DTO.Candidate
{
    public class CompanyProfileDTO
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyPhone { get; set; }
        public string Profle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public string Zip { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }
    }
}
