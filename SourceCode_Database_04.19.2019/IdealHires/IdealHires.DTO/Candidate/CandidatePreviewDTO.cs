using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.DTO.Candidate
{
    public class CandidatePreviewDTO
    {
        public List<CandidateEducationDTO> CandidateEducationPreview { get; set; }
        public List<CandidateWorkDTO> CandidateWorkPreview { get; set; }
        public CandidatePreferencesDTO PreferencesPreview { get; set; }
        public CandidateBasicDTO CandidateBasicPreview { get; set; }
        public CandidateContactDTO CandidateContactPreview { get; set; }
    }
}
