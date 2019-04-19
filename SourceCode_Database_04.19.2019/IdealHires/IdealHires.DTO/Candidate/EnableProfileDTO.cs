using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.DTO.Candidate
{
    public class EnableProfileDTO
    {
        public bool IsGeneralActive { get; set; }
        public bool IsContactActive { get; set; }
        public bool IsWorkExpActive { get; set; }
        public bool IsEduActive { get; set; }
        public bool IsPreferenceActive { get; set; }
        public bool IsPreviewActive { get; set; }
    }
}
