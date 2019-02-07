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
    public class EmployerService
    {
        #region Private Menber
        bool disposed = false;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Constructor
        public EmployerService()
        {
            _unitOfWork = new UnitOfWork(new IdealHiresDbContext());
        }
        #endregion

        #region Company Information

        public int InsertCompanyDetails(CompanyDTO companyCandidate)
        {
            int companyId = 0;
            try
            {
                if (companyCandidate != null)
                {
                    User user = _unitOfWork.Users.Get(companyCandidate.UserId);
                    EmployerCompany employerCompanyDetails = new EmployerCompany();
                    Company companyDetails = null;
                    if (user.Id > 0)
                    {
                        employerCompanyDetails = _unitOfWork.EmployerCompanyRepository.SingleOrDefault(ec => ec.UserId == user.Id);
                        if (employerCompanyDetails != null)
                        {
                            companyDetails = _unitOfWork.CompanyRepository.Get(employerCompanyDetails.CompanyId);
                        }
                        user.FirstName = companyCandidate.FirstName;
                        user.LastName = companyCandidate.LastName;
                        user.UserType = "Employer";
                        user.UpdatedAt = DateTime.Now;
                        user.UpdatedBy = companyCandidate.UserId;

                        _unitOfWork.Users.Update(user);
                        _unitOfWork.Complete();

                        if (companyDetails != null)
                        {
                            companyDetails.CompanyName = companyCandidate.CompanyName;
                            companyDetails.Phone = companyCandidate.Phone;
                            companyDetails.Email = companyCandidate.Email;
                            companyDetails.Location = companyCandidate.Location;
                            companyDetails.Website = companyCandidate.Website;
                            companyDetails.Description = companyCandidate.Description;
                            companyDetails.UpdatedAt = DateTime.Now;
                            companyDetails.UpdatedBy = companyCandidate.UserId;
                            companyDetails.ContactEmail = companyCandidate.ContactEmail;

                            _unitOfWork.CompanyRepository.Update(companyDetails);
                            _unitOfWork.Complete();
                            companyId = companyDetails.Id;
                        }
                        else
                        {
                            var company = new Company
                            {
                                CompanyName = companyCandidate.CompanyName,
                                Phone = companyCandidate.Phone,
                                Email = companyCandidate.Email,
                                Location = companyCandidate.Location,
                                Website = companyCandidate.Website,
                                Description = companyCandidate.Description,
                                ContactEmail= companyCandidate.ContactEmail,
                                CreatedAt = DateTime.Now,
                                CreatedBy = companyCandidate.UserId,
                            };
                            _unitOfWork.CompanyRepository.Add(company);
                            _unitOfWork.Complete();
                            companyId = company.Id;
                        }
                        if (employerCompanyDetails == null)
                        {
                            EmployerCompany employerCompany = new EmployerCompany()
                            {
                                UserId = user.Id,
                                CompanyId = companyId
                            };
                            _unitOfWork.EmployerCompanyRepository.Add(employerCompany);
                            _unitOfWork.Complete();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return companyId;
        }

        public CompanyDTO GetCompanyDetails(int id)
        {
            CompanyDTO companyDTO = new CompanyDTO();
            try
            {
                User userData = _unitOfWork.Users.Get(id);
                Company company = new Company();
                if (userData.Id > 0)
                {
                    EmployerCompany employerCompany = _unitOfWork.EmployerCompanyRepository.Get(ec => ec.UserId == userData.Id).FirstOrDefault();
                    company = _unitOfWork.CompanyRepository.Get(employerCompany.CompanyId);
                    companyDTO = new CompanyDTO
                    {
                        Id = company.Id,
                        UserId = userData.Id,
                        CompanyName = company.CompanyName,
                        Phone = company.Phone,
                        Email = company.Email,
                        Location = company.Location,
                        Website = company.Website,
                        Description = company.Description,
                        FirstName = userData.FirstName,
                        LastName = userData.LastName,
                        ContactEmail= company.ContactEmail
                    };
                }
                return companyDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public int InsertJobGeneralDetails(JobBasicDTO postJobbasicDTO)
        {
            int jobId = 0;
            KeywordsJob keywordsJob = new KeywordsJob();
            try
            {
                if (postJobbasicDTO != null)
                {
                    User user = _unitOfWork.Users.Get(postJobbasicDTO.UserId);
                    EmployerCompany employerCompany = _unitOfWork.EmployerCompanyRepository.Get(ec => ec.UserId == user.Id).FirstOrDefault();
                    Company company = _unitOfWork.CompanyRepository.Get(employerCompany.CompanyId);
                    if (user.Id > 0)
                    {
                        Job job = new Job()
                        {
                            CompanyId = company.Id,
                            Title = postJobbasicDTO.Keywords,
                            Description = postJobbasicDTO.Description
                        };
                        _unitOfWork.JobRepository.Add(job);
                        _unitOfWork.Complete();
                        jobId = job.Id;

                        var jobTypeJobList = MapJobTypeJobData(postJobbasicDTO, jobId);
                        _unitOfWork.JobTypeJobRepository.AddRange(jobTypeJobList);
                        _unitOfWork.Complete();

                        var jobCategoryJobList = MapJobCategoryJobData(postJobbasicDTO, jobId);
                        _unitOfWork.JobCategoryJobRepository.AddRange(jobCategoryJobList);
                        _unitOfWork.Complete();

                        KeywordsJob keywordJob = new KeywordsJob()
                        {
                            JobId = jobId,
                            Keywords = postJobbasicDTO.Keywords,
                            CreatedAt = DateTime.Now,
                            CreatedBy = postJobbasicDTO.UserId
                        };
                        _unitOfWork.KeywordsJobRepository.Add(keywordJob);
                        _unitOfWork.Complete();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return jobId;
        }

        public int InsertJobPreferencesDetails(JobPreferencesDTO jobPreferencesDTO)
        {
            int jobId = 0;
            try
            {
                if (jobPreferencesDTO != null)
                {
                    User user = _unitOfWork.Users.Get(jobPreferencesDTO.UserId);
                    Job jobDetails = _unitOfWork.JobRepository.Get(jobPreferencesDTO.JobId);

                    if (user.Id > 0)
                    {
                        if (jobDetails != null && jobDetails.Id > 0)
                        {
                            jobDetails.MinimumSalary = jobPreferencesDTO.MinimumSalary;
                            jobDetails.MaximumSalary = jobPreferencesDTO.MaximumSalary;
                            jobDetails.CurrencyId = jobPreferencesDTO.CurrencyId;
                            jobDetails.Positions = jobPreferencesDTO.Positions;
                            jobDetails.LocationCity = jobPreferencesDTO.LocationCity;
                            jobDetails.LocationState = jobPreferencesDTO.LocationState;
                            jobDetails.LocationCountry = jobPreferencesDTO.LocationCountry;
                            jobDetails.ExpiredAt = jobPreferencesDTO.ExpiredAt;
                            jobDetails.PayPeriodTypeId = Convert.ToInt32(jobPreferencesDTO.SelectedPayPeriodTypes[0]);

                            _unitOfWork.JobRepository.Update(jobDetails);
                            _unitOfWork.Complete();
                            jobId = jobDetails.Id;
                        }

                        var notificationTypeList = MapNotificationTypeJobData(jobPreferencesDTO);
                        _unitOfWork.NotificationTypeJobRepository.AddRange(notificationTypeList);
                        _unitOfWork.Complete();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return jobId;
        }



        public List<NotificationTypeDTO> GetNotificationType()
        {
            List<NotificationTypeDTO> notificationTypeDTOList = new List<NotificationTypeDTO>();
            try
            {
                List<NotificationType> notificationTypeList = _unitOfWork.NotificationTypeRepository.GetAll().ToList();
                foreach (var notificationType in notificationTypeList)
                {
                    NotificationTypeDTO notificationTypeDTO = new NotificationTypeDTO()
                    {
                        Id = notificationType.Id,
                        Name = notificationType.Name
                    };
                    notificationTypeDTOList.Add(notificationTypeDTO);
                }

                return notificationTypeDTOList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PayPeriodTypeDTO> GetPayPeriodType()
        {
            List<PayPeriodTypeDTO> payPeriodTypeDTOList = new List<PayPeriodTypeDTO>();
            try
            {
                List<PayPeriodType> payPeriodTypeList = _unitOfWork.PayPeriodTypeRepository.GetAll().ToList();
                foreach (var payPeriodType in payPeriodTypeList)
                {
                    PayPeriodTypeDTO payPeriodTypeDTO = new PayPeriodTypeDTO()
                    {
                        Id = payPeriodType.Id,
                        Name = payPeriodType.Name
                    };
                    payPeriodTypeDTOList.Add(payPeriodTypeDTO);
                }
                return payPeriodTypeDTOList;
            }
            catch (Exception)
            {
                throw;
            }
        }

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

        #region Private
        private List<JobTypeJob> MapJobTypeJobData(JobBasicDTO employerBasic, int Id)
        {
            List<JobTypeJob> jobTypeJobList = new List<JobTypeJob>();
            for (int i = 0; i < employerBasic.SelectedJobTypes.Count; i++)
            {
                var jobTypeJobConvert = new JobTypeJob()
                {
                    JobId = Id,
                    JobTypeId = int.Parse(employerBasic.SelectedJobTypes[i]),
                    CreatedAt = DateTime.Now,
                    CreatedBy = employerBasic.UserId
                };
                jobTypeJobList.Add(jobTypeJobConvert);
            }
            return jobTypeJobList;
        }

        private List<NotificationTypeJob> MapNotificationTypeJobData(JobPreferencesDTO jobPreferences)
        {
            List<NotificationTypeJob> notificationTypeJobList = new List<NotificationTypeJob>();
            for (int i = 0; i < jobPreferences.SelectedPayPeriodTypes.Count; i++)
            {
                var notificationTypeJobConvert = new NotificationTypeJob()
                {
                    JobId = jobPreferences.JobId,
                    NotificationTypeId = int.Parse(jobPreferences.SelectedPayPeriodTypes[i]),
                    CreatedAt = DateTime.Now,
                    CreatedBy = jobPreferences.UserId
                };
                notificationTypeJobList.Add(notificationTypeJobConvert);
            }
            return notificationTypeJobList;
        }

        private List<JobCategoryJob> MapJobCategoryJobData(JobBasicDTO employerBasic, int Id)
        {
            List<JobCategoryJob> jobCategoryJobList = new List<JobCategoryJob>();
            for (int i = 0; i < employerBasic.SelectedJobCategory.Count; i++)
            {
                var jobCategoryJobConvert = new JobCategoryJob()
                {
                    JobId = Id,
                    JobCategoryId = int.Parse(employerBasic.SelectedJobCategory[i]),
                    CreatedAt = DateTime.Now,
                    CreatedBy = employerBasic.UserId
                };
                jobCategoryJobList.Add(jobCategoryJobConvert);
            }
            return jobCategoryJobList;
        }
        #endregion
    }
}
