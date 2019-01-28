using IdealHires.BAL.Business;
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
    }
}
