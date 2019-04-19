using IdealHires.BAL.Business;
using IdealHires.DTO;
using IdealHires.DTO.Candidate;
using IdealHires.DTO.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using IdealHires.UserIdentity;

namespace IdealHires.API.Controllers
{
    /// <summary>
    /// that controller handle the condidate activity logic 
    /// </summary>
    [RoutePrefix("api/candidate")]
    public class CandidateController : BaseApiController
    {
        #region Private Member

        private readonly CandidateService _candidateService;
        private readonly PreferencesService _preferencesService;
        private readonly NotificationService _notificationService;

        #endregion

        #region Candidate Basic

        /// <summary>
        /// this constructor intialize the CandidateService object and PreferencesService.
        /// </summary>
        public CandidateController()
        {
            _candidateService = new CandidateService();
            _preferencesService = new PreferencesService();
            _notificationService = new NotificationService();
        }


        /// <summary>
        /// This method  used for add the condidate basic details.
        /// </summary>
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

        /// <summary>
        /// this method is get  condidate basic details  by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// this method is use to  add the condidate details.
        /// </summary>
        /// <param name="candidateContact"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This method is used for get the condidate address by parameter id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// this method is used for add condidate  work details.
        /// </summary>
        /// <param name="candidateWork"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("work")]
        public int AddCandidateWork(CandidateWorkDTO candidateWork)
        {
            int result = 0;
            try
            {
                if (ModelState.ContainsKey("candidateWork.EndAt"))
                    ModelState["candidateWork.EndAt"].Errors.Clear();

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

        /// <summary>
        /// This method is used for get the condidate work by parameter id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This method is used for get the condidate work by parameter id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This method  used for delete the work by work id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// This method is used for get the condidate details by parameter id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This method is used for add the Education work details.
        /// </summary>
        /// <param name="candidateEducation"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This method used for get the education details by condidtae details.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This method used for  delete the education by educationId
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This methiod is used for get the Candidate Preference by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This method used for add the preferences 
        /// </summary>
        /// <param name="candidatePreferences"></param>
        /// <returns></returns>
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
                else
                {
                    var message = string.Join(",", ModelState.Values.Where(E => E.Errors.Count > 0).SelectMany(E => E.Errors).Select(E => E.ErrorMessage).ToArray());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// This method used for get job list by getjob type.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// This method used for get  getjob type by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This method used for get Job Category type by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        ///This method used for get Job Category.
        /// </summary>
        /// <returns></returns>
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

        #region Candidate Preview

        /// <summary>
        /// This method is used for get the  Candidate Preview by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("preview/{id:int}")]
        public CandidatePreviewDTO GetCandidatePreview(int id)
        {
            CandidatePreviewDTO previewDetailDTO = new CandidatePreviewDTO();
            try
            {
                if (ModelState.IsValid)
                {
                    previewDetailDTO = _candidateService.GetCandidatePreviewDetails(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return previewDetailDTO;
        }

        /// <summary>
        /// This method is used for get the condidate details by condidate used id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getcandidatedetails/{id:int}")]
        public CandidatePreviewDTO GetCandidateDetails(int id)
        {
            CandidatePreviewDTO previewDetailDTO = new CandidatePreviewDTO();
            try
            {
                if (ModelState.IsValid)
                {
                    previewDetailDTO = _candidateService.GetCandidateDetailsForEmployer(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return previewDetailDTO;
        }
        #endregion 

        #region Find Job for Candidate

        /// <summary>
        /// This methood is used for Find Job for Candidate by Condidate Id.
        /// </summary>
        /// <param name="jobExploreDTO"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("getjobexplore")]

        public CandidateJobExploreDTO GetJobExplore(CandidateJobExploreDTO jobExploreDTO)
        {
            List<JobDTO> jobDTO = new List<JobDTO>();
            try
            {
                jobDTO = _candidateService.GetSearchedJobExplore(jobExploreDTO);
                jobExploreDTO.AvailableJobs = jobDTO;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return jobExploreDTO;
        }

        #endregion

        #region Get job details for candidate

        /// <summary>
        /// This method is used to Get job details for candidate by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getjobdetailsbyid/{id:int}")]
        public JobDTO GetJobDetailsById(int? id)
        {
            JobDTO jobDTO = new JobDTO();
            try
            {
                jobDTO = _candidateService.GetJobDetailsById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return jobDTO;
        }
        #endregion

        #region Jobs for Candidate

        /// <summary>
        /// This method is used to Searched Job Explore for condidate.
        /// </summary>
        /// <param name="jobExploreDTO"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("jobs")]
        public CandidateJobExploreDTO Jobs(CandidateJobExploreDTO jobExploreDTO)
        {
            List<JobDTO> jobDTO = new List<JobDTO>();
            try
            {
                jobDTO = _candidateService.GetSearchedJobExplore(jobExploreDTO);
                if (jobDTO.Count() == 0)
                {
                    if (!string.IsNullOrEmpty(jobExploreDTO.JobTitle) && !string.IsNullOrEmpty(jobExploreDTO.Company))
                    {
                        jobExploreDTO.Message = CandidateResource.SimilarJobMsg;
                        jobExploreDTO.Company = string.Empty;
                        jobDTO = _candidateService.GetSearchedJobExplore(jobExploreDTO);
                    }
                }
                jobExploreDTO.AvailableJobs = jobDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobExploreDTO;
        }

        #endregion

        #region Job applied by candidated
        /// <summary>
        /// This method is used for apply the job by condidate profile.
        /// </summary>
        /// <param name="profileJobDTO"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("applyjob")]
        public BaseModel ApplyJob(ProfileJobDTO profileJobDTO)
        {
            var response = new BaseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    response = _candidateService.SaveApplyJob(profileJobDTO);
                    if (response.IsApp && response.IsEmailed)
                    {
                        NotificationDTO notificationDTO = _notificationService.SetNotificationAlert(profileJobDTO);
                        if (notificationDTO != null)
                        {
                            BaseModel baseModel = _notificationService.SaveNotificaton(notificationDTO);
                            if(profileJobDTO.ActionId==1 || profileJobDTO.ActionId==3)
                            {
                                SendNotificationByMail(notificationDTO);
                            }
                            
                        }
                    }
                    else if (response.IsApp==true && response.IsEmailed == false)
                    {
                        NotificationDTO notificationDTO = _notificationService.SetNotificationAlert(profileJobDTO);
                        if (notificationDTO != null)
                        {
                            BaseModel baseModel = _notificationService.SaveNotificaton(notificationDTO);
                        }
                    }
                    else if (response.IsApp == false && response.IsEmailed)
                    {
                        NotificationDTO notificationDTO = _notificationService.SetNotificationAlert(profileJobDTO);
                        if (profileJobDTO.ActionId == 1 || profileJobDTO.ActionId == 3)
                        {
                            SendNotificationByMail(notificationDTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return response;
        }
        #endregion

        #region get applied jobs by Candidate
        /// <summary>
        /// This method is used for get the all applied job by condidate.
        /// </summary>
        /// <param name="jobExploreDTO"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("getappliedjobs")]

        public CandidateJobExploreDTO GetAppliedJobs(CandidateJobExploreDTO jobExploreDTO)
        {
            List<JobDTO> jobDTO = new List<JobDTO>();
            try
            {
                jobDTO = _candidateService.GetAppliedJobs(jobExploreDTO).Where(i=>i.ActionId==1).ToList();//ActionId 1 is for Applied job 
                jobExploreDTO.AvailableJobs = jobDTO;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return jobExploreDTO;
        }
        #endregion

        #region get saved jobs by Candidate

        /// <summary>
        /// This method is used for saved the job by condidate.
        /// </summary>
        /// <param name="jobExploreDTO"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("getsavedjobs")]
        public CandidateJobExploreDTO GetSavedJobs(CandidateJobExploreDTO jobExploreDTO)
        {
            List<JobDTO> jobDTO = new List<JobDTO>();
            try
            {
                jobDTO = _candidateService.GetAppliedJobs(jobExploreDTO).Where(i => i.ActionId == 3).ToList();//ActionId 3 is for Save this Job 
                jobExploreDTO.AvailableJobs = jobDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobExploreDTO;
        }

        #endregion

        #region get saved jobs by Candidate
        /// <summary>
        /// This method is used to get the all saved job by condidate profile.
        /// </summary>
        /// <param name="jobExploreDTO"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("getnotinterestedjobs")]
        public CandidateJobExploreDTO GetNotInterestedJobs(CandidateJobExploreDTO jobExploreDTO)
        {
            List<JobDTO> jobDTO = new List<JobDTO>();
            try
            {
                jobDTO = _candidateService.GetAppliedJobs(jobExploreDTO).Where(i => i.ActionId == 4).ToList();//ActionId 4 is for Not Interested
                jobExploreDTO.AvailableJobs = jobDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobExploreDTO;
        }

        #endregion

        #region Save view details of the job by candidate
        /// <summary>
        /// This is method used for Save view details of the job by candidate
        /// </summary>
        /// <param name="profileJobDTO"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("saveviewdetails")]
        public BaseModel SaveViewDetails(ProfileJobDTO profileJobDTO)
        {
            var response = new BaseModel();
            response= _candidateService.SaveJobViewedByCandidate(profileJobDTO);
            return response;
        }

        /// <summary>
        /// This method used for get the phone formate by country id.
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("getstatebycountryid/{countryId:int}")]
        public List<StateDTO> GetStateByCountryId(int countryId)
        {
            return _candidateService.GetStateByCountryId(countryId);
        }

        /// <summary>
        /// This method used for get the phone formate by country id.
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("getphoneformat/{countryId:int}")]
        public string GetPhoneFormat(int countryId)
        {
            return _candidateService.GetPhoneFormat(countryId);
        }

        /// <summary>
        /// This method used for get the all dashboard details by condidate user id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("dashboard/{id:int}")]
        public Dashboard Dashboard(int id)
        {
            return _candidateService.Dashboard(id);
        }
        [HttpGet]
        [Authorize]
        [Route("callforinterview/{id:int}")]

        /// <summary>
        /// This is method used  for get the company profile by condidate userid.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<CompanyProfileDTO> GetCompanyProfile(int id)
        {
            return _candidateService.GetCompanyProfile(id);
        }

        /// <summary>
        /// This is method used for get the all job list for condidate.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("allJobs")]
        public Dashboard GetAllJobs()
        {
            Dashboard dashboardList = new Dashboard();

            try
            {
                if (ModelState.IsValid)
                {
                    dashboardList = _candidateService.GetJobDetails();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dashboardList;
        }


        #endregion

        #region(GET CONDIDATE NOTIFICATION LIST)
        /// <summary>
        /// This method used for get the notification By user id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getnotificationbyId/{id:int}")]
        public List<NotificationDTO> GetNotificationById(int? Id)
        {
            List<NotificationDTO> notificationDTO = new List<NotificationDTO>();
            try
            {
                notificationDTO = _notificationService.GetNotificationDetailsById(Id);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return notificationDTO;
        }
        #endregion

        #region(GET CONDIDATE SHORT NOTIFICATION LIST)
        /// <summary>
        /// This method used for get the unread notification By user id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getshortnotificationbyId/{id:int}")]
        public List<NotificationDTO> GetShortNotificationById(int? Id)
        {
            List<NotificationDTO> notificationDTO = new List<NotificationDTO>();
            try
            {
                notificationDTO = _notificationService.GetShortNotificationDetailsById(Id);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return notificationDTO;
        }
        #endregion

        #region(UPDATE NOTIFICATION BY ID WHEN USER HAS BEEN READ NOTIFICATION)
        [Authorize]
        [HttpGet]
        [Route("work/{Id:int}/notificationread")]
        public bool ReadNotificationById(int Id)
        {
            bool isRead = false;
            try
            {
                if (ModelState.IsValid)
                {
                    isRead = _notificationService.ReadNotificationById(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isRead;
        }
        [HttpGet]
        [Route("downloadresume/{id:int}")]
        public CandidateBasicDTO DownloadResume(int id)
        {
            return _candidateService.DownloadResume(id);
        }
        #endregion

        #region(ADD NEW NOTIFICATION ACCORDING TO ENTITY ID)
        /// <summary>
        /// This method is used to save notification according entity id
        /// </summary>
        /// <param name="notificationDTO"></param>
        /// <returns></returns>
        public BaseModel AddNotification(NotificationDTO notificationDTO)
        {
            var response = new BaseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    response = _notificationService.SaveNotificaton(notificationDTO);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return response;
        }

        [HttpGet]
        [Route("enableprofileoption/{id:int}")]
        public EnableProfileDTO EnableProfileOption(int id)
        {
            return _candidateService.EnableProfileOption(id);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationDTO"></param>
        /// <returns></returns>
        public BaseModel SendNotificationByMail(NotificationDTO notificationDTO)
        {
            BaseModel response = new BaseModel();
            string mailBody = "Dear " + notificationDTO.Name+ "!  <br/><br/>";
            mailBody += "<b>" + notificationDTO.Title + "</b> <br/><br/>";
            mailBody += notificationDTO.EventEntity;
            foreach(var email in notificationDTO.EmailAddress)
            {
                var message = new Message
                {
                    Destination = email,    
                    Body = mailBody,
                    Subject = notificationDTO.Subject
                };
                var emailData = UserManager.EmailService.SendAsync(message);
                if (emailData.Exception == null)
                {
                    response.Success = true;
                    response.Message = "sucess";
                }
                else
                {
                    response.Success = false;
                    response.Message = "failure";
                }
            }
            

            return response;
        }

    }
}
