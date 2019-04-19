using IdealHires.API.Utility;
using IdealHires.BAL.Business;
using IdealHires.DTO;
using IdealHires.DTO.Employer;
using IdealHires.UserIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IdealHires.API.Controllers
{
    [RoutePrefix("api/PackageTransaction")]
    public class PackageTransactionController : BaseApiController
    {
        // GET: PackageTransaction
        #region Private Member
        private readonly PackageTransactionService _TransactionPackageService;
        #endregion

        #region Transaction Package
        public PackageTransactionController()
        {
            _TransactionPackageService = new PackageTransactionService();
        }
        #endregion

        #region transactionpackage

        [Authorize]
        [HttpPost]
        [Route("package/{userid:int}")]
        public int packagetransaction(JobCreditDTO jobCreditDTO, int userid)
        {
            int result = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    result = _TransactionPackageService.Insertpackagetransaction(jobCreditDTO, userid);
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
        [Route("PackageDetails/{id:int}")]
        public List<JobCreditListDTO> GetPackageDetails(int id)
        {
            List<JobCreditListDTO> packageListDTO = new List<JobCreditListDTO>();
            try
            {
                if (ModelState.IsValid)
                {
                    packageListDTO = _TransactionPackageService.Getpackagetransaction(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return packageListDTO;
        }

        [Authorize]
        [HttpGet]
        [Route("deletepackagetransaction/{Id:int}")]
        public bool DeletePackage(int Id)
        {
            bool transaction = false;
            try
            {
                if (ModelState.IsValid)
                {
                    transaction = _TransactionPackageService.Deletepackagetransaction(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return transaction;
        }

        [Authorize]
        [HttpGet]
        [Route("jobcreditdetails/{Id:int}")]
        public JobCreditDTO GetjobCreditdetails(int Id)
        {
            JobCreditDTO jobcreditDTO = new JobCreditDTO();
            try
            {
                if (ModelState.IsValid)
                {
                    jobcreditDTO = _TransactionPackageService.GetJobCreditDetails(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobcreditDTO;
        }

        [Authorize]
        [HttpGet]
        [Route("details/{Id:int}")]
        public JobCreditDTO GetjobCredit(int Id)
        {
            JobCreditDTO jobcredit = new JobCreditDTO();
            try
            {
                if (ModelState.IsValid)
                {
                    jobcredit = _TransactionPackageService.GetjobCredit(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobcredit;
        }
        #endregion
    }
}