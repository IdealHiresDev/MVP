using IdealHires.BAL.Business;
using IdealHires.DTO;
using IdealHires.DTO.Employer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IdealHires.API.Controllers
{
    [RoutePrefix("api/employer")]
    public class EmployerController : BaseApiController
    {
        #region Private Member

        private readonly EmployerService _employerService;
       
        #endregion

        #region Company

        public EmployerController()
        {
            _employerService = new EmployerService();            
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

        #endregion


        [Authorize]
        [HttpPost]
        [Route("general")]
        public int AddGeneral(JobBasicDTO postJobbasicDTO)
        {
            int result = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    result = _employerService.InsertJobGeneralDetails(postJobbasicDTO);
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
    }
}
