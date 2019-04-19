using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.DTO.Candidate
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public int entityId { get; set; }
        public int ? createdBy { get; set; }
        public int? userId { get; set; }
        public string Title { get; set; }
        public string EventEntity { get; set; }
        public bool? status { get; set; }
        public DateTime? EventOn { get; set; }
        public DateTime? TimeFlag { get; set; }
        public bool? IsRead { get; set; }
        public int? TotalUnread { get; set; }
        public int? CompanyId { get; set; }
        public int? NotificationTypeId { get; set; }
        public string ToMail { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public List<string> EmailAddress { get; set; }


    }
}
