using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.DTO
{
    public class SortListedCandidateDTO
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public int ProfileId { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public bool IsSortListed { get; set; }
        public DateTime SortListedDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
