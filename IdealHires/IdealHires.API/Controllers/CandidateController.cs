using IdealHires.BAL.Business;
using IdealHires.DTO;
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
        private readonly PreferencesService _preferencesService;

        #endregion

        #region Candidate Basic

        public CandidateController()
        {
            _candidateService = new CandidateService();
            _preferencesService = new PreferencesService();
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

        [Authorize]
        [HttpGet]
        [Route("basic/{id:int}")]
        public CandidateBasicDTO GetCandidateBasic(int id)
        {
            CandidateBasicDTO basicDetailDTO = new CandidateBasicDTO();
            try
            {
                if (ModelState.IsValid)
                {
                    basicDetailDTO = _candidateService.GetBasicDetails(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return basicDetailDTO;
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

        [Authorize]
        [HttpGet]
        [Route("contact/{id:int}")]
        public CandidateContactDTO GetCandidateAddress(int id)
        {
            CandidateContactDTO contactDetailDTO = new CandidateContactDTO();
            try
            {
                if (ModelState.IsValid)
                {
                    contactDetailDTO = _candidateService.GetContactDetails(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return contactDetailDTO;
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

        [Authorize]
        [HttpGet]
        [Route("work/{id:int}")]
        public List<CandidateWorkDTO> GetCandidateWork(int id)
        {
            List<CandidateWorkDTO> workDetailDTO = new List<CandidateWorkDTO>();
            try
            {
                if (ModelState.IsValid)
                {
                    workDetailDTO = _candidateService.GetWorkDetails(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return workDetailDTO;
        }

        [Authorize]
        [HttpGet]
        [Route("work/{Id:int}/details")]
        public CandidateWorkDTO GetWorkByWorkId(int Id)
        {
            CandidateWorkDTO workDetailDTO = new CandidateWorkDTO();
            try
            {
                if (ModelState.IsValid)
                {
                    workDetailDTO = _candidateService.GetWorkDetailsByWorkId(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return workDetailDTO;
        }

        [Authorize]
        [HttpGet]
        [Route("work/{id:int}/remove")]
        public bool DeleteWorkByWorkId(int Id)
        {
            bool isWorkDelete = false;
            try
            {
                if (ModelState.IsValid)
                {
                    isWorkDelete = _candidateService.DeleteWork(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isWorkDelete;
        }

        #endregion

        #region Candidate Education
        [Authorize]
        [HttpGet]
        [Route("education/{id:int}")]
        public List<CandidateEducationDTO> GetCandidateEducation(int id)
        {
            List<CandidateEducationDTO> educationDetailDTO = new List<CandidateEducationDTO>();
            try
            {
                if (ModelState.IsValid)
                {
                    educationDetailDTO = _candidateService.GetEducationDetails(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return educationDetailDTO;
        }

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

        [Authorize]
        [HttpGet]
        [Route("education/{Id:int}/details")]
        public CandidateEducationDTO GetEducationById(int Id)
        {
            CandidateEducationDTO educationDetailDTO = new CandidateEducationDTO();
            try
            {
                if (ModelState.IsValid)
                {
                    educationDetailDTO = _candidateService.GeEducationDetailsByEdudationId(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return educationDetailDTO;
        }

        [Authorize]
        [HttpGet]
        [Route("education/{id:int}/remove")]
        public bool DeleteEducationById(int Id)
        {
            bool isEducationDelete = false;
            try
            {
                if (ModelState.IsValid)
                {
                    isEducationDelete = _candidateService.DeleteEducation(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isEducationDelete;
        }

        #endregion

        #region Candidate Preference

        [Authorize]
        [HttpGet]
        [Route("preferences/{id:int}")]
        public CandidatePreferencesDTO GetPreferencesEducation(int id)
        {
            CandidatePreferencesDTO preferencesDetailDTO = new CandidatePreferencesDTO();
            try
            {
                if (ModelState.IsValid)
                {
                    preferencesDetailDTO = _candidateService.GetPreferencesDetails(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return preferencesDetailDTO;
        }
        [Authorize]
        [HttpPost]
        [Route("preferences")]
        public int AddPreferences(CandidatePreferencesDTO candidatePreferences)
        {
            int result = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    result = _preferencesService.InsertPreferencesDetails(candidatePreferences);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        
        [HttpGet]
        [Route("jobtype")]
        public List<JobTypeDTO> GetJobType()
        {
            List<JobTypeDTO> jobTypeDTO = new List<JobTypeDTO>();
            try
            {
                if (ModelState.IsValid)
                {
                    jobTypeDTO = _preferencesService.GetJobTypeList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobTypeDTO;
        }
        
        [HttpGet]
        [Route("jobtype/{id:int}")]
        public JobTypeDTO GetJobType(int id)
        {
            JobTypeDTO jobTypeDTO = new JobTypeDTO();
            try
            {
                if (ModelState.IsValid)
                {
                    jobTypeDTO = _preferencesService.GetJobType(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobTypeDTO;
        }
       
        [HttpGet]
        [Route("category/{id:int}")]
        public JobCategoryDTO GetJobCategory(int id)
        {
            JobCategoryDTO jobCategoryDTO = new JobCategoryDTO();
            try
            {
                if (ModelState.IsValid)
                {
                    jobCategoryDTO = _preferencesService.GetJobCategory(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobCategoryDTO;
        }
        
        [HttpGet]
        [Route("category")]
        public List<JobCategoryDTO> GetJobCategory()
        {
            List<JobCategoryDTO> jobCategoryDTO = new List<JobCategoryDTO>();
            try
            {
                if (ModelState.IsValid)
                {
                    jobCategoryDTO = _preferencesService.GetJobCategoryList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobCategoryDTO;
        }
        #endregion
    }
}
