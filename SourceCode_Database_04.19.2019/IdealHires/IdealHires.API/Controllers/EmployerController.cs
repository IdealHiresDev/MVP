using IdealHires.BAL.Business;
using IdealHires.DTO;
using IdealHires.DTO.Candidate;
using IdealHires.DTO.Employer;
using IdealHires.UserIdentity;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;


namespace IdealHires.API.Controllers
{
    [RoutePrefix("api/employer")]
    public class EmployerController : BaseApiController
    {
        #region Private Member

        private readonly EmployerService _employerService;
        private readonly NotificationService _notificationService;

        #endregion

        #region Company

        public EmployerController()
        {
            _employerService = new EmployerService();
            _notificationService = new NotificationService();
        }

        [Authorize]
        [HttpPost]
        [Route("company")]
        public int AddEmployerCompany(CompanyDTO companyDTO)
        {
            int result = 0;
            try
            {
               
                    if (ModelState.IsValid)
                    {
                        result = _employerService.InsertCompanyDetails(companyDTO);
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
        [Route("company/{id:int}")]
        public CompanyDTO GetEmployerCompany(int id)
        {
            CompanyDTO companyDTO = new CompanyDTO();
            try
            {
                if (ModelState.IsValid)
                {
                    companyDTO = _employerService.GetCompanyDetails(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return companyDTO;
        }


        [HttpGet]
        [Route("employerDetails/{id:int}")]
        public CompanyDTO GetEmployerDetails(int id)
        {
            CompanyDTO companyDTO = new CompanyDTO();
            try
            {
                if (ModelState.IsValid)
                {
                    companyDTO = _employerService.GetEmployerDetails(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return companyDTO;
        }

        #endregion


        [Authorize]
        [HttpPost]
        [Route("general")]

        public int AddGeneral(JobBasicDTO postJobbasicDTO)
        {

            int result = 0;
            try
            {
                if (GetAvailableJobCredit(postJobbasicDTO.UserId).Success)
                {
                    if (ModelState.IsValid)
                    {

                        result = _employerService.InsertJobGeneralDetails(postJobbasicDTO);


                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("preferences")]
        public int AddPreferences(JobPreferencesDTO jobPreferencesDTO)
        {
            int result = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    result = _employerService.InsertJobPreferencesDetails(jobPreferencesDTO);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        [HttpGet]
        [Route("notificationtype")]
        public List<NotificationTypeDTO> GetNotificationType()
        {
            List<NotificationTypeDTO> notificationTypeDTODTO = new List<NotificationTypeDTO>();
            try
            {
                if (ModelState.IsValid)
                {
                    notificationTypeDTODTO = _employerService.GetNotificationType();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return notificationTypeDTODTO;
        }

        [HttpGet]
        [Route("payperiod")]
        public List<PayPeriodTypeDTO> GetPayPeriodType()
        {
            List<PayPeriodTypeDTO> payPeriodTypeDTODTO = new List<PayPeriodTypeDTO>();
            try
            {
                if (ModelState.IsValid)
                {
                    payPeriodTypeDTODTO = _employerService.GetPayPeriodType();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return payPeriodTypeDTODTO;
        }
        [HttpGet]
        [Route("country")]
        public List<CountryDTO> GetCountry()
        {
            List<CountryDTO> countryDTO = new List<CountryDTO>();
            try
            {
                if (ModelState.IsValid)
                {
                    countryDTO = _employerService.GetCountry();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return countryDTO;
        }
        [HttpGet]
        [Route("companycity/{id:int}")]
        public List<CompanyAddressDTO> GeCompanyState( int id)
        {
            List<CompanyAddressDTO> companyAddressDTO = new List<CompanyAddressDTO>();
            try
            {
                if (ModelState.IsValid)
                {
                    companyAddressDTO = _employerService.GeCompanyCity(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return companyAddressDTO;
        }
        [HttpGet]
        [Route("state")]
        public List<StateDTO> GeState()
        {
            List<StateDTO> countryDTO = new List<StateDTO>();
            try
            {
                if (ModelState.IsValid)
                {
                    countryDTO = _employerService.GeState();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return countryDTO;
        }
        [HttpGet]
        [Route("city")]
        public List<CityDTO> GetCity()
        {
            List<CityDTO> cityDTO = new List<CityDTO>();
            try
            {
                if (ModelState.IsValid)
                {
                    cityDTO = _employerService.GetCity();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return cityDTO;
        }
        [HttpGet]
        [Route("stateNameByCityId/{cityId:int}/{userId:int}")]
        public CompanyAddressDTO GetStateName(int cityId, int userId)
        {
            CompanyAddressDTO companyAddressDTO = new CompanyAddressDTO();
            try
            {
                if (ModelState.IsValid)
                {
                    companyAddressDTO = _employerService.GetStateName(cityId, userId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return companyAddressDTO;
        }
     

        [Authorize]
        [HttpGet]
        [Route("getjobbasic/{id:int}")]
        public List<JobBasicDTO> GetJobBasicByCompanyId(int id)
        {
            List<JobBasicDTO> jobBasicDTOList = new List<JobBasicDTO>();
            try
            {
                if (ModelState.IsValid)
                {
                    jobBasicDTOList = _employerService.GetJobBasicByCompanyId(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobBasicDTOList;
        }
        [HttpGet]
        [Route("preview/{id:int}")]
        public EmployerPreviewDTO JobPreview(int id)
        {
            EmployerPreviewDTO employerPreviewList = new EmployerPreviewDTO();

            try
            {
                if (ModelState.IsValid)
                {
                    employerPreviewList = _employerService.GetEmployerPreview(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return employerPreviewList;
        }
        [HttpGet]
        [Route("addressybyid/{addressid:int}")]
        public JobPreferencesDTO GetAddressById(int addressid)
        {
            JobPreferencesDTO jobPreferencesList = new JobPreferencesDTO();

            try
            {
                if (ModelState.IsValid)
                {
                    jobPreferencesList = _employerService.GetAddressById(addressid);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobPreferencesList;
        }
        [HttpGet]
        [Route("searchDescription/{descname}")]
        public List<JobBasicDTO> SearchDescriptionByName(string descname)
        {
            List<JobBasicDTO> jobBasicDTOList = new List<JobBasicDTO>();

            try
            {
                if (ModelState.IsValid)
                {
                    jobBasicDTOList = _employerService.SearchDescriptionByName(descname);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobBasicDTOList;
        }

        [HttpGet]
        [Route("dashboard/{id:int}")]
        public DashboardCalculationDTO GetJobDetailsByCompanyId(int id)
        {
            DashboardCalculationDTO employerDashboardDTOList = new DashboardCalculationDTO();

            try
            {
                if (ModelState.IsValid)
                {
                    employerDashboardDTOList = _employerService.GetJobDetailsByCompanyId(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return employerDashboardDTOList;
        }
        [Authorize]
        [HttpGet]
        [Route("getjob/{id:int}/details")]

        public JobBasicDTO GetJobDetailsById(int id)
        {
            JobBasicDTO jobBasicDTO = new JobBasicDTO();
            try
            {
                if (ModelState.IsValid)
                {
                    jobBasicDTO = _employerService.GetJobDetailsById(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobBasicDTO;
        }

        [Authorize]
        [HttpGet]
        [Route("jobprefrences/{jobid:int}/{id1:int}/details")]

        public JobPreferencesDTO GetJobPreferencesById(int jobid, int id1)
        {
            JobPreferencesDTO jobPreferencesDTO = new JobPreferencesDTO();
            try
            {
                if (ModelState.IsValid)
                {
                    jobPreferencesDTO = _employerService.GetJobPreferencesById(jobid, id1);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobPreferencesDTO;
        }
        [Authorize]
        [HttpGet]
        [Route("jobDetails/{id:int}/remove")]
        public bool DeleteJobdetailsByJobId(int Id)
        {
            bool isWorkDelete = false;
            try
            {
                if (ModelState.IsValid)
                {
                    isWorkDelete = _employerService.DeleteJobdetailsByJobId(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isWorkDelete;
        }
        [Authorize]
        [HttpGet]
        [Route("companyaddress/{id:int}/remove")]
        public bool DeleteAddressItem(int Id)
        {
            bool isWorkDelete = false;
            try
            {
                if (ModelState.IsValid)
                {
                    isWorkDelete = _employerService.DeleteAddressItem(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isWorkDelete;
        }
        [Authorize]
        [HttpGet]
        [Route("getcandidatelist/{id:int}")]
        public List<CandidateDetails> GetCandidateList(int id)
        {
            List<CandidateDetails> candidateBasic = new List<CandidateDetails>();
            try
            {
                if (ModelState.IsValid)
                {
                    candidateBasic = _employerService.GetCandidateList(id);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return candidateBasic;
        }

        [Authorize]
        [HttpPost]
        [Route("getcandidatelist")]
        public List<CandidateDetails> GetCandidateList(CandidateList candidateList)
        {
            List<CandidateDetails> candidateBasic = new List<CandidateDetails>();
            try
            {
                if (ModelState.IsValid)
                {
                    candidateBasic = _employerService.GetSearchedCandidateList(candidateList);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return candidateBasic;
        }

        [Authorize]
        [HttpGet]
        [Route("getjobviewercandidates/{id:int}")]
        public List<CandidateDetails> GetJobViewerCandidates(int id)
        {
            List<CandidateDetails> candidateBasic = new List<CandidateDetails>();
            try
            {
                if (ModelState.IsValid)
                {
                    candidateBasic = _employerService.GetJobViewerCandidateList(id);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return candidateBasic;
        }
        [Authorize]
        [HttpPost]
        [Route("getjobviewercandidates")]
        public List<CandidateDetails> GetJobViewerCandidates(CandidateList candidateList)
        {
            List<CandidateDetails> candidateBasic = new List<CandidateDetails>();
            try
            {
                if (ModelState.IsValid)
                {
                    candidateBasic = _employerService.GetSearchedJobViewerCandidateList(candidateList);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return candidateBasic;
        }

        [Authorize]
        [HttpPost]
        [Route("savesortListedcandidate")]
        public BaseModel SaveSortListedCandidate(SortListedCandidateDTO sortListedCandidateDTO)
        {
            BaseModel response = new BaseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    response = _employerService.SaveSortListedCandidate(sortListedCandidateDTO);
                    if(response.IsApp && response.IsEmailed)
                    {
                        NotificationDTO notificationDTO = _notificationService.ShortListNotificationForJob(sortListedCandidateDTO, 12);
                        if(notificationDTO!=null)
                        {
                            BaseModel baseModel = _notificationService.SaveNotificaton(notificationDTO);
                            SendNotificationByMail(notificationDTO);
                        }
                    }
                    else if(response.IsApp && response.IsEmailed==false)
                    {
                        NotificationDTO notificationDTO = _notificationService.ShortListNotificationForJob(sortListedCandidateDTO, 12);
                        if (notificationDTO != null)
                        {
                            BaseModel baseModel = _notificationService.SaveNotificaton(notificationDTO);     
                        }
                    }
                    else if (response.IsApp == false && response.IsEmailed)
                    {
                        NotificationDTO notificationDTO = _notificationService.ShortListNotificationForJob(sortListedCandidateDTO, 12);
                        SendNotificationByMail(notificationDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
        [Authorize]
        [HttpGet]
        [Route("getsortListedcandidate/{id:int}")]
        public List<CandidateDetails> GetSortListedCandidate(int id)
        {
            List<CandidateDetails> candidateBasicDTOs = new List<CandidateDetails>();
            try
            {
                if (ModelState.IsValid)
                {
                    candidateBasicDTOs = _employerService.GetSortListedCandidate(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return candidateBasicDTOs;
        }
        [Authorize]
        [HttpPost]
        [Route("getsortListedcandidate")]
        public List<CandidateDetails> GetSortListedCandidate(CandidateList candidateList)
        {
            List<CandidateDetails> candidateBasicDTOs = new List<CandidateDetails>();
            try
            {
                if (ModelState.IsValid)
                {
                    candidateBasicDTOs = _employerService.GetSortListedCandidate(candidateList);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return candidateBasicDTOs;
        }
        /// <summary>
        /// Method to save the job credit purchased by employer.
        /// </summary>
        /// <param name="companyPackageDetail"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("savecompanypackagedetail")]
        public BaseModel SaveCompanyPackageDetail(CompanyPackageDetailDTO companyPackageDetail)
        {
            var response = new BaseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    response = _employerService.SavePackageDetail(companyPackageDetail);
                }
                else
                {
                    var message = string.Join(",", ModelState.Values.Where(E => E.Errors.Count > 0).SelectMany(E => E.Errors).Select(E => E.ErrorMessage).ToArray());
                    response.Success = false;
                    response.Message = message;
                    return response;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("BuyJobCredit")]
        public List<JobCreditDTO> BuyJobCredits()
        {
            List<JobCreditDTO> objJobCreditDTO = new List<JobCreditDTO>();
            try
            {
                if (ModelState.IsValid)
                {
                    objJobCreditDTO = _employerService.GetBuyJobCredits();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objJobCreditDTO;
        }

        [Authorize]
        [HttpGet]
        [Route("getavailablejobcredit/{id:int}")]
        public BaseModel GetAvailableJobCredit(int id)
        {
            var response = new BaseModel();
            response = _employerService.GetAvailableJobCredit(id);
            return response;
        }
        [HttpGet]
        [Route("downloadfile/{id:int}")]
        public CandidateBasicDTO Downloadfile(int id)
        {
            return _employerService.Downloadfile(id);
        }


        [HttpGet]
        [Authorize]
        [Route("getcompanyemployeedetails/{id:int}")]
        public CompanyEmployee GetCompanyEmployeeDetails(int id)
        {
            return _employerService.GetEmployeeDetails(id);
        }
        [HttpPost]
        [Authorize]
        [Route("makepayment")]
        public BaseModel MakePayment(TransactionDTO transactionDTO)
        {
            return _employerService.MakePayment(transactionDTO);
        }

        [HttpGet]
        [Authorize]
        [Route("getlatestcandidatelist/{id:int}")]
        public List<CandidateDetails> GetLatestCandidateList()
        {
            return _employerService.GetLatestCandidateList();
        }

        [HttpPost]
        [Authorize]
        [Route("getlatestcandidatelist")]
        public List<CandidateDetails> GetLatestCandidateList(CandidateList candidateList)
        {
            List<CandidateDetails> candidateBasicDTOs = new List<CandidateDetails>();
            try
            {
                if (ModelState.IsValid)
                {
                    candidateBasicDTOs = _employerService.GetSearchedLatestCandidateList(candidateList);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return candidateBasicDTOs;
        }
        [HttpGet]
        [Route("calculatejob/{id:int}")]
        public InternalDashboardDTO PostedNewJob(int id)
        {
            InternalDashboardDTO internalDashboardDTO = new InternalDashboardDTO();
            try
            {
                if (ModelState.IsValid)
                {
                    internalDashboardDTO = _employerService.GetPostedNewJob(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return internalDashboardDTO;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationDTO"></param>
        /// <returns></returns>
        /// 

        public BaseModel SendNotificationByMail(NotificationDTO notificationDTO)
        {
            BaseModel response = new BaseModel();
            string mailBody = "Dear " + notificationDTO.Name + "! <br/><br/>";
            mailBody += "<b>" + notificationDTO.Title + "</b> <br/><br/>";
            mailBody += notificationDTO.EventEntity;
            var message = new Message
            {
                Destination = notificationDTO.ToMail,
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

            return response;
        }

    }

}
