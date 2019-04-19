using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.DTO
{
    public class ProfileJobDTO
    {
        public int Id { get; set; }
        public int? ProfileId { get; set; }
        public int? JobId { get; set; }
        public int? ActionId { get; set; }
        public int? UserId { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
