using IdealHires.BAL.Business;
using IdealHires.DTO.Candidate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace IdealHires.API.Controllers
{
    [RoutePrefix("api/candidate")]
    public class CandidateController : BaseApiController
    {
        #region Private Member

        private readonly CandidateService _candidateService;

        #endregion

        #region Candidate Basic

        public CandidateController()
        {
            _candidateService = new CandidateService();
        }

        [Authorize]
        [HttpPost]
        [Route("basic")]
        public int AddCandidateBasic(CandidateBasicDTO candidateBasic)
        {
            int result = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    result = _candidateService.InsertBasicDetails(candidateBasic);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        #region Candidate Address

        [Authorize]
        [HttpPost]
        [Route("contact")]
        public int AddCandidateAddress(CandidateContactDTO candidateContact)
        {
            int result = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    result = _candidateService.InsertContactDetails(candidateContact);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        #endregion

        #region Candidate Work

        [Authorize]
        [HttpPost]
        [Route("work")]
        public int AddCandidateWork(CandidateWorkDTO candidateWork)
        {
            int result = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    result = _candidateService.InsertWorkDetails(candidateWork);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        #endregion

        #region Candidate Work

        [Authorize]
        [HttpPost]
        [Route("education")]
        public int AddEducationWork(CandidateEducationDTO candidateEducation)
        {
            int result = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    result = _candidateService.InsertEducationDetails(candidateEducation);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        #endregion
    }
}
