using IdealHires.BAL.DataContext;
using IdealHires.Data;
using IdealHires.DTO;
using IdealHires.DTO.Employer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.BAL.Business
{
    public class PackageTransactionService
    {
        #region Private Member

        bool disposed = false;
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructor
        public PackageTransactionService()
        {
            _unitOfWork = new UnitOfWork(new IdealHiresDbContext());
        }
        #endregion

        #region package transaction details
        public int Insertpackagetransaction(JobCreditDTO jobCreditDTO, int userid)
        {
            int userId = 0;

            try
            {
                if (jobCreditDTO.Id > 0)
                {
                    JobCredit jobCredit = _unitOfWork.jobCreditRepository.Get(se => se.Id == jobCreditDTO.Id).FirstOrDefault();
                    jobCredit.Price = jobCreditDTO.Price;
                    jobCredit.Duration = jobCreditDTO.Duration;
                    jobCredit.JobCredit1 = jobCreditDTO.JobCredit;
                    jobCredit.Description = jobCreditDTO.Description;
                    jobCredit.Discount = jobCreditDTO.Discount;
                    jobCredit.IsActive = true;
                    jobCredit.UpdatedAt = DateTime.Now;
                    jobCredit.IsDeleted = false;
                    jobCredit.UpdatedBy = userid;
                    _unitOfWork.jobCreditRepository.Update(jobCredit);
                    _unitOfWork.Complete();
                }
                else
                {
                    var entity = new JobCredit
                    {
                        Price = jobCreditDTO.Price,
                        Duration = jobCreditDTO.Duration,
                        JobCredit1 = jobCreditDTO.JobCredit,
                        Description = jobCreditDTO.Description,
                        Discount = jobCreditDTO.Discount,
                        IsActive = true,
                        CreatedAt = DateTime.Now,
                        IsDeleted = false,
                        CreatedBy = userid,
                    };
                    _unitOfWork.jobCreditRepository.Add(entity);
                    _unitOfWork.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return userId;
        }
        public List<JobCreditListDTO> Getpackagetransaction(int Id)
        {
            List<JobCreditListDTO> PackageListDTO = new List<JobCreditListDTO>();
            try
            {
                //var userPackageid = _unitOfWork.CompanyPackageDetailRepository.Get(ec => ec.UserId == Id).FirstOrDefault().CompanyId;
                //if (userPackageid > 0)
                //{
                var PackageList = (from jobCredit in _unitOfWork.jobCreditRepository.Query(k => k.IsActive == true && k.IsDeleted == false)
                                   select new JobCreditListDTO()
                                   {
                                       Id = jobCredit.Id,
                                       Price = jobCredit.Price,
                                       Duration = jobCredit.Duration,
                                       JobCredit = jobCredit.JobCredit1,
                                       Description = jobCredit.Description,
                                       Discount = jobCredit.Discount,
                                   });

                PackageListDTO = (from p in PackageList
                                  select new JobCreditListDTO()
                                  {
                                      Id = p.Id,
                                      Price = p.Price,
                                      Duration = p.Duration,
                                      JobCredit = p.JobCredit,
                                      Description = p.Description,
                                      Discount = p.Discount,
                                  }).OrderByDescending(se => se.Id).ToList();
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PackageListDTO;
        }
        public bool Deletepackagetransaction(int Id)
        {
            bool isDeleted = false;
            try
            {
                var jobcredit = _unitOfWork.jobCreditRepository.Get(jc => jc.Id == Id).FirstOrDefault();
                if (jobcredit != null)
                {
                    jobcredit.IsActive = false;
                    jobcredit.IsDeleted = true;
                    _unitOfWork.jobCreditRepository.Update(jobcredit);
                    _unitOfWork.Complete();
                    isDeleted = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isDeleted;
        }
        public JobCreditDTO GetJobCreditDetails(int Id)
        {
            JobCreditDTO jobCreditDTO = new JobCreditDTO();
            try
            {
                var user = _unitOfWork.Users.Get(Id);
                var jobcreditList = _unitOfWork.jobCreditRepository.Get(se => se.Id == Id).FirstOrDefault();
                if (jobcreditList != null)
                {
                    jobCreditDTO.Id = Id;
                    jobCreditDTO.Price = jobcreditList.Price;
                    jobCreditDTO.Duration = jobcreditList.Duration;
                    jobCreditDTO.JobCredit = jobcreditList.JobCredit1;
                    jobCreditDTO.Description = jobcreditList.Description;
                    jobCreditDTO.Discount = jobcreditList.Discount;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobCreditDTO;
        }

        public JobCreditDTO GetjobCredit(int Id)
        {
            JobCreditDTO jobCreditDTO = new JobCreditDTO();
            try
            {
                var jobCredit = _unitOfWork.jobCreditRepository.GetFirstOrDefault(cp => cp.Id == Id);
                if (jobCredit != null)
                {
                    jobCreditDTO.Price = jobCredit.Price;
                    jobCreditDTO.Duration = jobCredit.Duration;
                    jobCreditDTO.JobCredit = jobCredit.JobCredit1;
                    jobCreditDTO.Description = jobCredit.Description;
                    jobCreditDTO.Discount = jobCredit.Discount;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobCreditDTO;
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            disposed = true;
        }
        #endregion
    }
}
