using IdealHires.DTO.Employer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.DTO.Candidate
{
    public class Dashboard
    {
        public int TotalSavedJobs { get; set; }
        public int TotalAppliedJobs { get; set; }
        public int TotalNotInterestedJobs { get; set; }
        public int TotalCallForInterviewJobs { get; set; }
        public int TotalRecommendedJobs { get; set; }
        public List<JobDTO> listJobDTO { get; set; }
    }
}
