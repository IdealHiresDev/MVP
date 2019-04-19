using IdealHires.BAL.DataContext;
using IdealHires.Data;
using IdealHires.DTO;
using IdealHires.DTO.Candidate;
using IdealHires.DTO.Employer;
using IdealHires.DTO.Fields;
using IdealHires.DTO.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdealHires.BAL.Business
{
    public class EmployerService
    {
        #region Private Menber
        bool disposed = false;
        private readonly IUnitOfWork _unitOfWork;
        private readonly NotificationService _notificationService;
        #endregion

        #region Constructor
        public EmployerService()
        {
            _unitOfWork = new UnitOfWork(new IdealHiresDbContext());
            _notificationService = new NotificationService();
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
                    CompanyAddress companyAddressDetalis = new CompanyAddress();
                    List<CompanyAddress> companAddressList = new List<CompanyAddress>();

                    if (user.Id > 0)
                    {

                        employerCompanyDetails = _unitOfWork.EmployerCompanyRepository.GetFirstOrDefault(ec => ec.UserId == user.Id);
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
                            companyDetails.Phone1 = companyCandidate.Phone1;
                            companyDetails.PhoneChecked = companyCandidate.IsChecked;
                            companyDetails.Website = companyCandidate.Website;
                            companyDetails.Description = companyCandidate.Description;
                            companyDetails.UpdatedAt = DateTime.Now;
                            companyDetails.UpdatedBy = companyCandidate.UserId;
                            companyDetails.IsAgree = companyCandidate.IsAgree;
                            companyDetails.IsActive = true;
                            companyDetails.IsDeleted = false;
                            _unitOfWork.CompanyRepository.Update(companyDetails);
                            _unitOfWork.Complete();
                            companyId = companyDetails.Id;
                            SaveCompanyLogo(companyCandidate, companyId);
                        }
                        else
                        {
                            var company = new Company
                            {
                                CompanyName = companyCandidate.CompanyName,
                                Phone = companyCandidate.Phone,
                                Phone1 = companyCandidate.Phone1,
                                PhoneChecked = companyCandidate.IsChecked,
                                Website = companyCandidate.Website,
                                Description = companyCandidate.Description,
                                IsAgree = companyCandidate.IsAgree,
                                CreatedAt = DateTime.Now,
                                CreatedBy = companyCandidate.UserId,
                                IsActive = true,
                                IsDeleted = false
                            };
                            _unitOfWork.CompanyRepository.Add(company);
                            _unitOfWork.Complete();
                            companyId = company.Id;
                            SaveCompanyLogo(companyCandidate, companyId);
                        }
                        if (employerCompanyDetails == null)
                        {
                            EmployerCompany employerCompany = new EmployerCompany()
                            {
                                UserId = user.Id,
                                CompanyId = companyId,
                                IsActive = true,
                                IsDeleted = false

                            };
                            _unitOfWork.EmployerCompanyRepository.Add(employerCompany);
                            _unitOfWork.Complete();
                        }

                        companAddressList = _unitOfWork.CompanyAddressRepository.Find(x => x.CompanyId == companyId).ToList();
                        if (companAddressList.Count >= 0)
                        {
                            if (companAddressList.Count > 0)
                            {
                                foreach (var res in companyCandidate.companyAddressDTOList)
                                {
                                    var currentCompanyAddress = companAddressList.Where(ca => ca.Id == res.Id && ca.IsActive == true && ca.IsDeleted == false).FirstOrDefault();
                                    if (currentCompanyAddress != null)
                                    {
                                        currentCompanyAddress.CompanyId = companyId;
                                        currentCompanyAddress.AddressTypeId = res.AddressTypeId;
                                        currentCompanyAddress.ZipCode = res.ZipCode;
                                        currentCompanyAddress.CountryId = res.CountryId;
                                        currentCompanyAddress.StateId = res.StateId;
                                        currentCompanyAddress.City = res.City;
                                        currentCompanyAddress.Address = res.Address;
                                        currentCompanyAddress.UpdatedAt = DateTime.Now;
                                        currentCompanyAddress.UpdateBy = companyCandidate.UserId;
                                        currentCompanyAddress.IsActive = true;
                                        currentCompanyAddress.IsDeleted = false;
                                        _unitOfWork.CompanyAddressRepository.Update(currentCompanyAddress);
                                        _unitOfWork.Complete();
                                    }
                                    else
                                    {
                                        companyAddressDetalis = new CompanyAddress()
                                        {
                                            CompanyId = companyId,
                                            AddressTypeId = res.AddressTypeId,
                                            ZipCode = res.ZipCode,
                                            CountryId = res.CountryId,
                                            StateId = res.StateId,
                                            City = res.City,
                                            Address = res.Address,
                                            CreatedAt = DateTime.Now,
                                            CreatedBy = companyCandidate.UserId,
                                            UpdateBy = companyCandidate.UserId,
                                            UpdatedAt = DateTime.Now,
                                            IsActive = true,
                                            IsDeleted = false
                                        };
                                        _unitOfWork.CompanyAddressRepository.Add(companyAddressDetalis);
                                        _unitOfWork.Complete();
                                    }
                                }
                            }
                            else
                            {
                                foreach (var res in companyCandidate.companyAddressDTOList)
                                {
                                    companyAddressDetalis = new CompanyAddress()
                                    {
                                        CompanyId = companyId,
                                        AddressTypeId = res.AddressTypeId,
                                        ZipCode = res.ZipCode,
                                        CountryId = res.CountryId,
                                        StateId = res.StateId,
                                        City = res.City,
                                        Address = res.Address,
                                        CreatedAt = DateTime.Now,
                                        CreatedBy = companyCandidate.UserId,
                                        UpdateBy = companyCandidate.UserId,
                                        UpdatedAt = DateTime.Now,
                                        IsActive = true,
                                        IsDeleted = false

                                    };
                                    _unitOfWork.CompanyAddressRepository.Add(companyAddressDetalis);
                                    _unitOfWork.Complete();
                                }
                            }
                        }
                        else
                        {
                            foreach (var res in companyCandidate.companyAddressDTOList)
                            {
                                companyAddressDetalis = new CompanyAddress()
                                {
                                    CompanyId = companyId,
                                    AddressTypeId = res.AddressTypeId,
                                    ZipCode = res.ZipCode,
                                    CountryId = res.CountryId,
                                    StateId = res.StateId,
                                    City = res.City,
                                    Address = res.Address,
                                    CreatedAt = DateTime.Now,
                                    CreatedBy = companyCandidate.UserId,
                                    UpdateBy = companyCandidate.UserId,
                                    UpdatedAt = DateTime.Now,
                                    IsActive = true,
                                    IsDeleted = false
                                };
                                _unitOfWork.CompanyAddressRepository.Add(companyAddressDetalis);
                                _unitOfWork.Complete();
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return companyId;
        }

        public BaseModel SaveCompanyLogo(CompanyDTO companyDTO, int companyId)
        {
            var baseModel = new BaseModel();
            var compLogo = _unitOfWork.CompanyLogoRepository.Query(mod => mod.IsActive == true && mod.IsDeleted == false && mod.CompanyId == companyId).FirstOrDefault();
            if (compLogo != null)
            {
                compLogo.Img = companyDTO.Img;
                compLogo.CompanyId = companyId;
                compLogo.UpdatedBy = companyDTO.UserId;
                compLogo.UpdatedAt = DateTime.Now;
                compLogo.IsActive = true;
                compLogo.IsDeleted = false;
                _unitOfWork.CompanyLogoRepository.Update(compLogo);
                _unitOfWork.Complete();

                if (compLogo.Id > 0)
                {
                    baseModel.Success = true;
                    baseModel.Message = EmployerResource.SavedSuccessfully;
                }
                else
                {
                    baseModel.Success = false;
                    baseModel.Message = EmployerResource.SomeErrorOccured;
                }
            }
            else
            {
                compLogo = new CompanyLogo()
                {
                    Img = companyDTO.Img,
                    CompanyId = companyId,
                    CreatedBy = companyDTO.UserId,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false
                };
                _unitOfWork.CompanyLogoRepository.Add(compLogo);
                _unitOfWork.Complete();

                if (compLogo.Id > 0)
                {
                    baseModel.Success = true;
                    baseModel.Message = EmployerResource.SavedSuccessfully;
                }
                else
                {
                    baseModel.Success = false;
                    baseModel.Message = EmployerResource.SomeErrorOccured;
                }
            }
            return baseModel;
        }
        public CompanyDTO GetCompanyDetails(int id)
        {
            CompanyDTO companyDTO = new CompanyDTO();
            List<CountryDTO> countryDTO = new List<CountryDTO>();
            List<CompanyAddressDTO> companyAddressDTO = new List<CompanyAddressDTO>();
            List<CompanyAddress> companyaddressList = new List<CompanyAddress>();
            try
            {
                User userData = _unitOfWork.Users.Get(id);
                Company company = null;

                if (userData.Id > 0)
                {
                    EmployerCompany employerCompany = _unitOfWork.EmployerCompanyRepository.Get(ec => ec.UserId == userData.Id).FirstOrDefault();
                    if (employerCompany != null)
                    {
                        company = _unitOfWork.CompanyRepository.Get(employerCompany.CompanyId);
                        companyaddressList = _unitOfWork.CompanyAddressRepository.Get(ec => ec.CompanyId == employerCompany.CompanyId && ec.IsActive == true && ec.IsDeleted == false);
                        companyDTO = new CompanyDTO
                        {
                            Id = company.Id,
                            UserId = userData.Id,
                            CompanyName = company.CompanyName,
                            Phone = company.Phone,
                            Phone1 = company.Phone1,
                            IsChecked = Convert.ToBoolean(company.PhoneChecked),
                            Website = company.Website,
                            Description = company.Description,
                            FirstName = userData.FirstName,
                            LastName = userData.LastName,
                            IsAgree = company.IsAgree

                        };

                        var compLog = _unitOfWork.CompanyLogoRepository.Query(mod => mod.CompanyId == companyDTO.Id && mod.IsActive == true && mod.IsDeleted == false).FirstOrDefault();
                        if (compLog != null)
                        {
                            companyDTO.Img = compLog.Img;
                        }
                        else
                        {
                            companyDTO.Img = new byte[0];
                        }

                        if (companyaddressList != null)
                        {
                            foreach (var a in companyaddressList)
                            {
                                var companyaddress = new CompanyAddressDTO
                                {
                                    Id = a.Id,
                                    CompanyId = Convert.ToInt32(a.CompanyId),
                                    ZipCode = a.ZipCode,
                                    AddressTypeId = Convert.ToInt32(a.AddressTypeId),
                                    CountryId = Convert.ToInt32(a.CountryId),
                                    StateId = Convert.ToInt32(a.StateId),
                                    City = a.City,
                                    Address = a.Address
                                };
                                companyAddressDTO.Add(companyaddress);
                            }
                            companyDTO.companyAddressDTOList = companyAddressDTO;
                        }
                    }
                }
                return companyDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Post Job

        public JobBasicDTO GetJobDetailsById(int id)
        {
            JobBasicDTO jobBasicDTO = new JobBasicDTO();
            List<JobTypeJobDTO> jobTypeJobDTO = new List<JobTypeJobDTO>();
            List<JobCategoryJobDTO> jobCategoryJobDTO = new List<JobCategoryJobDTO>();

            List<string> selectedJobTypeJob = new List<string>();
            List<string> selectedJobCategoryJob = new List<string>();

            try
            {
                Job jobGeneralDetails = _unitOfWork.JobRepository.GetFirstOrDefault(ec => ec.Id == id);
                if (jobGeneralDetails != null)
                {
                    List<JobCategoryJob> jobCategoryJob = jobGeneralDetails.JobCategoryJobs.ToList();
                    List<JobTypeJob> jobTypeJob = jobGeneralDetails.JobTypeJobs.ToList();
                    if (jobCategoryJob != null && jobCategoryJob.Count > 0)
                    {
                        foreach (var jCJ in jobCategoryJob)
                        {
                            var jCPDTO = jCJ.JobCategoryId.ToString();
                            selectedJobCategoryJob.Add(jCPDTO);
                        }
                        jobBasicDTO.SelectedJobCategory = selectedJobCategoryJob.ToArray();
                    }

                    if (jobTypeJob != null && jobTypeJob.Count > 0)
                    {
                        foreach (var jT in jobTypeJob)
                        {
                            var jTPDTO = jT.JobTypeId.ToString();
                            selectedJobTypeJob.Add(jTPDTO);
                        }
                        jobBasicDTO.SelectedJobTypes = selectedJobTypeJob;
                    }

                    if (jobGeneralDetails.Id > 0)
                    {

                        jobBasicDTO.Id = jobGeneralDetails.Id;
                        jobBasicDTO.JobTitle = jobGeneralDetails.Title;
                        jobBasicDTO.ExpiredAt = jobGeneralDetails.ExpiredAt;
                        jobBasicDTO.Status = jobGeneralDetails.Status;
                        jobBasicDTO.Description = jobGeneralDetails.Description;


                    }
                }
                return jobBasicDTO;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertJobGeneralDetails(JobBasicDTO postJobbasicDTO)
        {
            int jobId = 0;
            User user = _unitOfWork.Users.Get(postJobbasicDTO.UserId);
            EmployerCompany employerCompany = _unitOfWork.EmployerCompanyRepository.Get(ec => ec.UserId == user.Id).FirstOrDefault();
            Company company = _unitOfWork.CompanyRepository.Get(employerCompany.CompanyId);
            try
            {

                if (user.Id > 0)
                {
                    var jobDetails = _unitOfWork.JobRepository.GetFirstOrDefault(ec => ec.Id == postJobbasicDTO.Id);
                    if (postJobbasicDTO.Id > 0)
                    {
                        List<JobCategoryJob> jobCategoryJob = jobDetails.JobCategoryJobs.ToList();
                        List<JobTypeJob> jobTypeJob = jobDetails.JobTypeJobs.ToList();
                        postJobbasicDTO.Id = jobDetails.Id;
                        jobDetails.CompanyId = company.Id;
                        jobDetails.Title = postJobbasicDTO.JobTitle;
                        jobDetails.Description = postJobbasicDTO.Description;
                        jobDetails.UpdatedAt = DateTime.Now;
                        jobDetails.UpdatedBy = user.Id;
                        _unitOfWork.JobRepository.Update(jobDetails);
                        jobId = jobDetails.Id;
                        if (jobCategoryJob.Count > 0)
                        {
                            var jobCategoryJobList = MapJobCategoryJobData(postJobbasicDTO, postJobbasicDTO.Id);
                            _unitOfWork.JobCategoryJobRepository.RemoveRange(jobCategoryJob);
                            _unitOfWork.JobCategoryJobRepository.AddRange(jobCategoryJobList);
                        }
                        else
                        {

                            var jobCategoryJobList = MapJobCategoryJobData(postJobbasicDTO, jobId);
                            _unitOfWork.JobCategoryJobRepository.AddRange(jobCategoryJobList);
                        }

                        if (jobTypeJob.Count > 0)
                        {
                            var jobTypeJobList = MapJobTypeJobData(postJobbasicDTO, postJobbasicDTO.Id);
                            _unitOfWork.JobTypeJobRepository.RemoveRange(jobTypeJob);
                            _unitOfWork.JobTypeJobRepository.AddRange(jobTypeJobList);
                        }
                        else
                        {

                            var jobTypeJobList = MapJobTypeJobData(postJobbasicDTO, jobId);
                            _unitOfWork.JobTypeJobRepository.AddRange(jobTypeJobList);
                        }
                        _unitOfWork.Complete();

                    }

                    else
                    {
                        Job job = new Job()
                        {
                            CompanyId = company.Id,
                            Title = postJobbasicDTO.JobTitle,
                            Description = postJobbasicDTO.Description,
                            CreatedAt = DateTime.Now,
                            CreatedBy = user.Id,
                            IsActive = true,
                            IsDeleted = false,
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
                        UpdateCompanyJobCreditDetails(company.Id, user.Id);
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
                    Job jobDetails = _unitOfWork.JobRepository.GetFirstOrDefault(x => x.Id == jobPreferencesDTO.JobId);
                    List<NotificationTypeJob> notificationTypeJob = jobDetails.NotificationTypeJobs.ToList();
                    List<CompanyAddress> companyaddresslist = _unitOfWork.CompanyAddressRepository.Find(ca => ca.CompanyId == jobDetails.CompanyId).ToList();
                    if (user.Id > 0)
                    {
                        if (jobDetails != null && jobDetails.Id > 0)
                        {
                            bool assumpt = false;
                            if (jobPreferencesDTO.Assumption.ToUpper() == "EXEMPT")
                            {
                                assumpt = true;
                            }
                            jobDetails.MinimumSalary = jobPreferencesDTO.MinimumSalary;
                            jobDetails.MaximumSalary = jobPreferencesDTO.MaximumSalary;
                            jobDetails.CurrencyId = jobPreferencesDTO.CurrencyId;
                            jobDetails.Positions = jobPreferencesDTO.Positions;
                            jobDetails.Assumption = assumpt;
                            jobDetails.UnAssumption = (!assumpt);
                            jobDetails.ExpiredAt = jobPreferencesDTO.ExpiredAt;
                            jobDetails.PayPeriodTypeId = Convert.ToInt32(jobPreferencesDTO.SelectedPayPeriodTypes[0]);
                            jobDetails.Location = Convert.ToInt32(jobPreferencesDTO.AddressNameId);
                            jobDetails.UpdatedAt = DateTime.Now;
                            jobDetails.UpdatedBy = user.Id;
                            jobDetails.CreatedAt = DateTime.Now;
                            jobDetails.CreatedBy = user.Id;
                            _unitOfWork.JobRepository.Update(jobDetails);
                            _unitOfWork.Complete();
                            jobId = jobDetails.Id;

                        }

                        if (notificationTypeJob.Count > 0)
                        {
                            var notificationTypeList = MapNotificationTypeJobData(jobPreferencesDTO);
                            _unitOfWork.NotificationTypeJobRepository.RemoveRange(notificationTypeJob);
                            _unitOfWork.NotificationTypeJobRepository.AddRange(notificationTypeList);
                            _unitOfWork.Complete();
                        }


                        else
                        {

                            var notificationTypeList = MapNotificationTypeJobData(jobPreferencesDTO);

                            _unitOfWork.NotificationTypeJobRepository.AddRange(notificationTypeList);
                            _unitOfWork.Complete();
                        }
                       


                    }

                }
            }

            catch (Exception)
            {
                throw;
            }
            return jobId;
        }
        
        public JobPreferencesDTO GetJobPreferencesById(int jobid, int userid)
        {
            JobPreferencesDTO jobPreferencesDTO = new JobPreferencesDTO();
            List<CompanyAddressDTO> companyaddressList = new List<CompanyAddressDTO>();
            CompanyAddressDTO companyAddressDTO = new CompanyAddressDTO();
            List<NotificationTypeJob> jobTypeJobDTO = new List<NotificationTypeJob>();
            List<string> selectedNotificationTypeJob = new List<string>();
            try
            {
                Job jobpreferencesDetails = _unitOfWork.JobRepository.GetFirstOrDefault(e => e.Id == jobid);
                List<CompanyAddress> companyAddresses = _unitOfWork.CompanyAddressRepository.GetAll().Where(ca => ca.CompanyId == jobpreferencesDetails.CompanyId && ca.IsActive == true && ca.IsDeleted == false).ToList();
                var countries = _unitOfWork.CountryRepository.Query(i => i.IsActive == true && i.IsDeleted == false);
                var states = _unitOfWork.StateRepository.Query(i => i.IsActive == true && i.IsDeleted == false);
                var compAddressDetails = (from compAddress in companyAddresses
                                          join country in countries on compAddress.CountryId equals country.Id
                                          join state in states on compAddress.StateId equals state.Id
                                          select new CompanyAddressDTO()
                                          {
                                              Id = compAddress.Id,
                                              CompanyId = Convert.ToInt32(compAddress.CompanyId),
                                              ZipCode = compAddress.ZipCode,
                                              Address = compAddress.Address,
                                              AddressTypeId = Convert.ToInt32(compAddress.AddressTypeId),
                                              CountryId = Convert.ToInt32(compAddress.CountryId),
                                              StateId = Convert.ToInt32(compAddress.StateId),
                                              CountryName = country.Name,
                                              StateName = state.Name,
                                              City = compAddress.City
                                          }).FirstOrDefault();

                jobPreferencesDTO.companyAddressDTO = compAddressDetails;



                if (jobpreferencesDetails != null)
                {
                    List<NotificationTypeJob> notificationTypeJob = jobpreferencesDetails.NotificationTypeJobs.ToList();

                    if (notificationTypeJob != null && notificationTypeJob.Count > 0)
                    {
                        foreach (var jNJ in notificationTypeJob)
                        {
                            var jNPDTO = jNJ.NotificationTypeId.ToString();
                            selectedNotificationTypeJob.Add(jNPDTO);
                        }
                        jobPreferencesDTO.SelectedNotificationTypes = selectedNotificationTypeJob;
                    }
                    if (jobpreferencesDetails.Location > 0 && jobpreferencesDetails.Location != null)
                    {
                        CompanyAddress companyaddresss = _unitOfWork.CompanyAddressRepository.GetFirstOrDefault(ca => ca.Id == jobpreferencesDetails.Location && ca.IsActive == true && ca.IsDeleted == false);
                        //jobPreferencesDTO.AddressType = Convert.ToString(companyaddresss.AddressTypeId);
                        int StateId = Convert.ToInt32(companyaddresss.Id);
                        jobPreferencesDTO.companyAddressDTO = GetStateName(StateId, userid);

                    }
                    if (jobpreferencesDetails.Id > 0)
                    {

                        jobPreferencesDTO.JobId = jobpreferencesDetails.Id;
                        jobPreferencesDTO.MinimumSalary = Convert.ToDecimal(jobpreferencesDetails.MinimumSalary);
                        jobPreferencesDTO.MaximumSalary = Convert.ToDecimal(jobpreferencesDetails.MaximumSalary);
                        jobPreferencesDTO.Positions = jobpreferencesDetails.Positions;
                        jobPreferencesDTO.PayPeriodType = Convert.ToString(jobpreferencesDetails.PayPeriodTypeId);
                        jobPreferencesDTO.LocationCity = Convert.ToInt32(jobpreferencesDetails.Location);

                        if (jobpreferencesDetails.Assumption == true)
                        {
                            jobPreferencesDTO.Assumption = "Exempt";
                        }
                        else
                        {
                            jobPreferencesDTO.Assumption = "Non-Exempt";
                        }
                        jobPreferencesDTO.UnAssumption = Convert.ToBoolean(jobpreferencesDetails.UnAssumption);
                        jobPreferencesDTO.ExpiredAt = jobpreferencesDetails.ExpiredAt;
                    }


                }
                return jobPreferencesDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EmployerPreviewDTO GetEmployerPreview(int id)
        {
            EmployerPreviewDTO employerPreviewDTO = new EmployerPreviewDTO();

            var jobRepository = _unitOfWork.JobRepository.GetAll();
            var companyRepository = _unitOfWork.CompanyRepository.GetAll();
            var jobCatJobList = _unitOfWork.JobCategoryJobRepository.GetAll();
            var jobCatList = _unitOfWork.JobCategoryRepository.GetAll();
            var jobTypeJobList = _unitOfWork.JobTypeJobRepository.GetAll();
            var jobTypes = _unitOfWork.JobTypeRepository.GetAll();
            var payPeriodTypes = _unitOfWork.PayPeriodTypeRepository.GetAll();
            var companyAddress = _unitOfWork.CompanyAddressRepository.GetAll();
            var country = _unitOfWork.CountryRepository.GetAll();

            var companyLogo = _unitOfWork.CompanyLogoRepository.GetAll();
            var city = _unitOfWork.CityRepository.GetAll();

            employerPreviewDTO = (from jobRepo in jobRepository
                                  join jobCatJob in jobCatJobList on jobRepo.Id equals jobCatJob.JobId
                                  join jobCat in jobCatList on jobCatJob.JobCategoryId equals jobCat.Id
                                  join jobTypeJob in jobTypeJobList on jobRepo.Id equals jobTypeJob.JobId
                                  join jobType in jobTypes on jobTypeJob.JobTypeId equals jobType.Id
                                  join payPeriodType in payPeriodTypes on jobRepo.PayPeriodTypeId equals payPeriodType.Id
                                  join compAdd in companyAddress on jobRepo.CompanyId equals compAdd.CompanyId
                                  join contry in country on compAdd.CountryId equals Convert.ToInt32(contry.Id)
                                  join compLogo in companyLogo on jobRepo.CompanyId equals compLogo.CompanyId
                                  where (jobRepo.Id == id && jobRepo.IsActive == true && jobRepo.IsDeleted == false)
                                  select new EmployerPreviewDTO()
                                  {
                                      Id = jobRepo.Id,
                                      CompanyId = jobRepo.CompanyId,
                                      Title = jobRepo.Title,
                                      Description = jobRepo.Description,
                                      MinimumSalary = Convert.ToDecimal(jobRepo.MinimumSalary),
                                      MaximumSalary = Convert.ToDecimal(jobRepo.MaximumSalary),
                                      JobCategoryName = jobCat.Name,
                                      JobTypeName = jobType.Name,
                                      PayPeriodTypeName = payPeriodType.Name,
                                      ExpiredAt = jobRepo.ExpiredAt,
                                      city = compAdd.City,
                                      countryName = contry.Name,
                                      Img = compLogo.Img,
                                      CreatedAt = jobRepo.CreatedAt,
                                      UpdatedAt = jobRepo.UpdatedAt,
                                      CreatedBy = jobRepo.CreatedBy,
                                      UpdatedBy = jobRepo.UpdatedBy
                                  }).FirstOrDefault();

            return employerPreviewDTO;

        }
        #endregion
        #region Bind Dropdown

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
        public List<CountryDTO> GetCountry()
        {
            List<CountryDTO> countryDTOList = new List<CountryDTO>();
            try
            {
                List<Country> countryList = _unitOfWork.CountryRepository.Query().ToList();
                foreach (var country in countryList)
                {
                    CountryDTO countryDTO = new CountryDTO()
                    {
                        Id = country.Id,
                        Name = country.Name
                    };
                    countryDTOList.Add(countryDTO);
                }
                return countryDTOList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public CompanyAddressDTO GetStateName(int cityid, int userid)
        {
            EmployerCompany employerCompany = _unitOfWork.EmployerCompanyRepository.Query(e => e.UserId == userid).FirstOrDefault();
            CompanyAddressDTO companyAddressDTO = new CompanyAddressDTO();
            try
            {

                // var addressListRepository = _unitOfWork.CompanyAddressRepository.GetFirstOrDefault(e => e.Id == cityid && e.CompanyId == employerCompany.CompanyId && e.IsActive == true && e.IsDeleted == false);
                var addressListRepository = _unitOfWork.CompanyAddressRepository.GetAll();
                var stateRepository = _unitOfWork.StateRepository.GetAll();
                companyAddressDTO = (from addressRepo in addressListRepository
                                      join stateRepo in stateRepository on addressRepo.StateId  equals stateRepo.Id

                                     where (addressRepo.Id == cityid && addressRepo.IsActive == true && addressRepo.IsDeleted == false)
               select new CompanyAddressDTO()
                {
                    Id = addressRepo.Id,
                    StateId = Convert.ToInt32(addressRepo.StateId),
                    StateName=stateRepo.Name
                }).FirstOrDefault();
                return companyAddressDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<CompanyAddressDTO> GeCompanyCity(int userid)
        {
            List<CompanyAddressDTO> companyAddressDTOList = new List<CompanyAddressDTO>();
            EmployerCompany employerCompany = _unitOfWork.EmployerCompanyRepository.Query(e => e.UserId == userid).FirstOrDefault();
            try
            {
                var companyAddressRepository = _unitOfWork.CompanyAddressRepository.GetAll().Where(i => i.CompanyId == employerCompany.CompanyId && i.IsActive == true && i.IsDeleted == false);
                
                companyAddressDTOList = (from companyAddrRepo in companyAddressRepository
                                         

                                         select new { companyAddrRepo } into enty
                                         group enty by enty.companyAddrRepo.StateId into entity
                                         select new CompanyAddressDTO
                                         {
                                             Id = entity.FirstOrDefault().companyAddrRepo.Id,
                                             City= entity.FirstOrDefault().companyAddrRepo.City
                                             // StateName = string.Join(",", entity.Select(i => i.stateRepo.Name).Distinct())
                                         }).Distinct().ToList();

                return companyAddressDTOList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<JobBasicDTO> SearchDescriptionByName(string descname)
        {
            var jobBasicDTO = new List<JobBasicDTO>();
            try
            {
                var descriptions = _unitOfWork.JobRepository.Query().Where(e => e.IsActive == true && e.IsDeleted == false).ToList();
                if (!string.IsNullOrEmpty(descname))
                {
                    descriptions = descriptions.Where(i => i.Description == descname).ToList();
                }

                foreach (var desc in descriptions)
                {
                    jobBasicDTO.Add(new JobBasicDTO() { Description = desc.Description });
                }

                return jobBasicDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<CompanyAddressDTO> GeCompanyState(int userid)
        {
            List<CompanyAddressDTO> companyAddressDTOList = new List<CompanyAddressDTO>();
            EmployerCompany employerCompany = _unitOfWork.EmployerCompanyRepository.Query(e => e.UserId == userid).FirstOrDefault();
            try
            {
                var companyAddressRepository = _unitOfWork.CompanyAddressRepository.GetAll().Where(i => i.CompanyId == employerCompany.CompanyId && i.IsActive == true && i.IsDeleted == false);
                var stateRepository = _unitOfWork.StateRepository.GetAll().Where(i => i.IsActive == true && i.IsDeleted == false);
                companyAddressDTOList = (from companyAddrRepo in companyAddressRepository
                                         join stateRepo in stateRepository on companyAddrRepo.StateId equals stateRepo.Id

                                         select new { companyAddrRepo, stateRepo } into enty
                                         group enty by enty.companyAddrRepo.StateId into entity
                                         select new CompanyAddressDTO
                                         {
                                             Id = entity.FirstOrDefault().companyAddrRepo.Id,
                                             StateId = Convert.ToInt32(entity.FirstOrDefault().companyAddrRepo.StateId),
                                             StateName = entity.FirstOrDefault().stateRepo.Name,
                                             // StateName = string.Join(",", entity.Select(i => i.stateRepo.Name).Distinct())
                                         }).Distinct().ToList();

                return companyAddressDTOList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<StateDTO> GeState()
        {
            List<StateDTO> stateDTOList = new List<StateDTO>();
            try
            {
                List<State> stateList = _unitOfWork.StateRepository.GetAll().ToList();
                foreach (var state in stateList)
                {
                    StateDTO stateDTO = new StateDTO()
                    {
                        Id = state.Id,
                        CID = Convert.ToInt32(state.CID),
                        Name = state.Name
                    };
                    stateDTOList.Add(stateDTO);
                }
                return stateDTOList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<CityDTO> GetCity()
        {
            List<CityDTO> cityDTOList = new List<CityDTO>();
            try
            {
                List<City> cityList = _unitOfWork.CityRepository.GetAll().ToList();
                foreach (var city in cityList)
                {
                    CityDTO cityDTO = new CityDTO()
                    {
                        Id = city.Id,
                        SId = Convert.ToInt32(city.SID),
                        Name = city.Name
                    };
                    cityDTOList.Add(cityDTO);
                }
                return cityDTOList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
        public bool DeleteJobdetailsByJobId(int jobId)
        {

            try
            {
                Job jobDetailsByjobId = _unitOfWork.JobRepository.Get(jobId);

                if (jobId > 0)
                {
                    jobDetailsByjobId.IsActive = false;
                    jobDetailsByjobId.IsDeleted = true;
                    _unitOfWork.JobRepository.Update(jobDetailsByjobId);
                    _unitOfWork.Complete();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public bool DeleteAddressItem(int addressId)
        {
            try
            {
                CompanyAddress companyAddressById = _unitOfWork.CompanyAddressRepository.Get(addressId);

                if (addressId > 0)
                {
                    companyAddressById.IsActive = false;
                    companyAddressById.IsDeleted = true;
                    _unitOfWork.CompanyAddressRepository.Update(companyAddressById);
                    _unitOfWork.Complete();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #region Dashboard
        public List<JobBasicDTO> GetJobBasicByCompanyId(int id)
        {
            List<JobBasicDTO> jobBasicDTOList = new List<JobBasicDTO>();
            try
            {
                List<Job> jobList = new List<Job>();
                List<JobCategoryJob> jobCategoryJobList = new List<JobCategoryJob>();
                List<JobTypeJob> jobTypeJobList = new List<JobTypeJob>();
                List<string> selectedjobCategoryJob = new List<string>();
                List<string> selectedjobTypeJob = new List<string>();
                User user = _unitOfWork.Users.Get(id);
                EmployerCompany employerCompany = _unitOfWork.EmployerCompanyRepository.Get(ec => ec.UserId == user.Id).FirstOrDefault();
                Company company = _unitOfWork.CompanyRepository.Get(employerCompany.CompanyId);
                jobList = _unitOfWork.JobRepository.GetAll().Where(a => a.CompanyId == company.Id).ToList();
                jobCategoryJobList = _unitOfWork.JobCategoryJobRepository.Get();
                jobTypeJobList = _unitOfWork.JobTypeJobRepository.Get();

                var resultantList = jobList.Where(item1 =>
                          jobCategoryJobList.Any(item2 => item1.Id == item2.JobId
                                  ));

                foreach (var job in jobList)
                {
                    JobBasicDTO jobBasicDTO = new JobBasicDTO()
                    {
                        CompanyId = Convert.ToInt32(job.CompanyId),
                        Id = job.Id,
                        JobTitle = job.Title,
                        Keywords = job.Description,

                    };
                    jobBasicDTOList.Add(jobBasicDTO);
                }

                return jobBasicDTOList;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public DashboardCalculationDTO GetJobDetailsByCompanyId(int id)
        {
            DashboardCalculationDTO dashboardCalculationDTO = new DashboardCalculationDTO();
            List<EmployerDashboardDTO> listEmployerDashboardDTO = new List<EmployerDashboardDTO>();

            try
            {
                EmployerCompany employerCompany = _unitOfWork.EmployerCompanyRepository.Query(ec => ec.UserId == id).FirstOrDefault();
                if (employerCompany != null)
                {
                    int companyId = employerCompany.CompanyId;
                    var jobRepository = _unitOfWork.JobRepository.Query(j => j.CompanyId == companyId && j.IsActive == true && j.IsDeleted == false).ToList();
                    var profileJobList = _unitOfWork.ProfileJobRepository.GetAll().Where(
                        j => jobRepository.Any(pj => pj.Id == j.JobId && j.ActionId == 1)
                        ).ToList();
                    if (profileJobList.Count > 0)
                    {
                        var appliedCountProfileJob = profileJobList.GroupBy(n => n.JobId).Select(n => new ProfileJob
                        {
                            JobId = n.Key,
                            ActionId = n.Count()
                        }).ToList();

                        listEmployerDashboardDTO = (from jobRepo in jobRepository
                                                    join profileJob in appliedCountProfileJob
                                                    on jobRepo.Id equals profileJob.JobId into profileLst
                                                    from profileJ in profileLst.DefaultIfEmpty()
                                                    where jobRepo.IsActive == true && jobRepo.IsDeleted == false
                                                    select new EmployerDashboardDTO()
                                                    {
                                                        JobId = jobRepo.Id,
                                                        UserId = jobRepo.CreatedBy.HasValue ? jobRepo.CreatedBy.Value : 0,
                                                        JobTitle = jobRepo.Title,
                                                        ExpiryDate = jobRepo.ExpiredAt,
                                                        ActionId = profileJ?.ActionId ?? 0
                                                    }).ToList();
                    }
                    else
                    {
                        listEmployerDashboardDTO = (from jobRepo in jobRepository
                                                    select new EmployerDashboardDTO()
                                                    {
                                                        JobId = jobRepo.Id,
                                                        UserId = jobRepo.CreatedBy.HasValue ? jobRepo.CreatedBy.Value : 0,
                                                        JobTitle = jobRepo.Title,
                                                        ExpiryDate = jobRepo.ExpiredAt,
                                                        ActionId = 0
                                                    }).ToList();
                    }


                    dashboardCalculationDTO.emloyerDashboardList = listEmployerDashboardDTO;
                    var jobCount = jobRepository.ToList().Count();

                    dashboardCalculationDTO.TotalJob = jobCount;
                    var applicatiocount = profileJobList.ToList().Count;
                    var totalJobViewed = (from profileJob in _unitOfWork.ProfileJobRepository.Query()
                                          join jobs in _unitOfWork.JobRepository.Query() on profileJob.JobId equals jobs.Id
                                          join user in _unitOfWork.Users.Query() on profileJob.UserId equals user.Id
                                          where jobs.CompanyId == companyId && profileJob.ActionId == 5
                                          select new
                                          {
                                              Id = user.Id,
                                              FirstName = user.FirstName
                                          }
                                        ).Distinct().ToList().Count();

                    dashboardCalculationDTO.ApplicationSubmit = GetCandidateList(id).Count();
                    dashboardCalculationDTO.TotalCallForInterview = GetSortListedCandidate(id).Count();
                    dashboardCalculationDTO.TotalJobViewed = totalJobViewed;
                }
                return dashboardCalculationDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public CompanyDTO GetEmployerDetails(int id)
        {
            CompanyDTO companyDTO = new CompanyDTO();
            List<CompanyAddressDTO> companyaddressList = new List<CompanyAddressDTO>();
            Company company = _unitOfWork.CompanyRepository.Get(id);
            if (company.Id > 0)
            {
                companyDTO.CompanyName = company.CompanyName;
                companyDTO.Phone = company.Phone;
                companyDTO.Description = company.Description;
            }
            if (company.CompanyAddresses.Count > 0)
            {
                foreach (var a in company.CompanyAddresses)
                {
                    var companyaddress = new CompanyAddressDTO
                    {
                        Id = a.Id,
                        CompanyId = Convert.ToInt32(a.CompanyId),
                        ZipCode = a.ZipCode,
                        AddressTypeId = Convert.ToInt32(a.AddressTypeId),
                        CountryId = Convert.ToInt32(a.CountryId),
                        StateId = Convert.ToInt32(a.StateId),
                        City = a.City
                    };
                    companyaddressList.Add(companyaddress);
                }
                companyDTO.companyAddressDTOList = companyaddressList;
            }
            return companyDTO;

        }
        #endregion

        public List<CandidateDetails> GetCandidateList(int id)
        {
            EmployerCompany employerCompany = _unitOfWork.EmployerCompanyRepository.Get(ec => ec.UserId == id).FirstOrDefault();
            var profileJobs = _unitOfWork.ProfileJobRepository.Query();
            var jobs = _unitOfWork.JobRepository.Query();
            var users = _unitOfWork.Users.Query();
            var shorlistedCandidate = _unitOfWork.SortListedCandidateRepository.Query(i => i.CompanyId == employerCompany.CompanyId);
            var profiles = _unitOfWork.ProfileRepository.Query();

            var sortlistedCandidateList = (from sortcand in shorlistedCandidate
                                           join prof in profiles on sortcand.ProfileId equals prof.Id
                                           join user in users on prof.UserId equals user.Id
                                           join jobR in jobs on sortcand.JobId equals jobR.Id
                                           where jobR.CompanyId == employerCompany.CompanyId
                                           select new CandidateDetails()
                                           {
                                               Id = user.Id,
                                               FirstName = user.FirstName,
                                               LastName = user.LastName,
                                               UserType = user.UserType,
                                               JobTitle = prof.JobTitle,
                                               Objective = prof.Objective
                                           }
                                         ).Distinct().ToList();


            var applicantCandidate = (from profileJ in profileJobs
                                      join jobR in jobs on profileJ.JobId equals jobR.Id
                                      join user in users on profileJ.UserId equals user.Id
                                      join prof in profiles on user.Id equals prof.UserId
                                      where jobR.CompanyId == employerCompany.CompanyId && profileJ.ActionId == 1
                                      select new CandidateDetails()
                                      {
                                          Id = user.Id,
                                          FirstName = user.FirstName,
                                          LastName = user.LastName,
                                          UserType = user.UserType,
                                          JobTitle = prof.JobTitle,
                                          Objective = prof.Objective
                                      }

                  ).Distinct().ToList();

            var result = applicantCandidate.Where(p => !sortlistedCandidateList.Any(p2 => p2.Id == p.Id)).ToList();

            return result;
        }

        public List<CandidateDetails> GetSearchedCandidateList(CandidateList candidateList)
        {
            var candidates = GetCandidateList(candidateList.UserId);
            if (candidateList.OrderBy == EmployerFields.OrderByJobTitleASC)
            {
                candidates = candidates.OrderBy(i => i.JobTitle).ToList();
            }

            else if (candidateList.OrderBy == EmployerFields.OrderByJobTitleDESC)
            {
                candidates = candidates.OrderByDescending(i => i.JobTitle).ToList();
            }

            else if (candidateList.OrderBy == EmployerFields.OrderByDescriptionASC)
            {
                candidates = candidates.OrderBy(i => i.Objective).ToList();
            }

            else if (candidateList.OrderBy == EmployerFields.OrderByDescriptionDESC)
            {
                candidates = candidates.OrderByDescending(i => i.Objective).ToList();
            }
            else
            {
                candidates = candidates.OrderByDescending(i => i.FirstName).ToList();
            }
            if (!string.IsNullOrEmpty(candidateList.JobTitle))
            {
                candidates = candidates.Where(i => i.JobTitle.Trim().ToLower().Contains(candidateList.JobTitle.Trim().ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(candidateList.Objective))
            {
                if (candidateList.Objective != "Availability")
                {
                    candidates = candidates.Where(i => i.Objective.Trim().ToLower().Contains(candidateList.Objective.Trim().ToLower())).ToList();
                }
            }
            return candidates;
        }

        public List<CandidateDetails> GetJobViewerCandidateList(int id)
        {
            EmployerCompany employerCompany = _unitOfWork.EmployerCompanyRepository.Get(ec => ec.UserId == id).FirstOrDefault();
            var profileJobs = _unitOfWork.ProfileJobRepository.Query(i => i.IsActive == true);
            var jobs = _unitOfWork.JobRepository.Query(i => i.IsActive == true);
            var users = _unitOfWork.Users.Query(i => i.IsActive == true);
            var profiles = _unitOfWork.ProfileRepository.Query(i => i.IsActive == true);
            return (from profileJ in profileJobs
                    join jobR in jobs on profileJ.JobId equals jobR.Id
                    join user in users on profileJ.UserId equals user.Id
                    join profile in profiles on user.Id equals profile.UserId
                    where jobR.CompanyId == employerCompany.CompanyId && profileJ.ActionId == 5
                    select new CandidateDetails()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserType = user.UserType,
                        JobTitle = profile.JobTitle,
                        Objective = profile.Objective
                    }

                  ).Distinct().ToList();
        }

        public List<CandidateDetails> GetSearchedJobViewerCandidateList(CandidateList candidateList)
        {
            var candidates = GetJobViewerCandidateList(candidateList.UserId);
            if (candidateList.OrderBy == EmployerFields.OrderByJobTitleASC)
            {
                candidates = candidates.OrderBy(i => i.JobTitle).ToList();
            }

            else if (candidateList.OrderBy == EmployerFields.OrderByJobTitleDESC)
            {
                candidates = candidates.OrderByDescending(i => i.JobTitle).ToList();
            }

            else if (candidateList.OrderBy == EmployerFields.OrderByDescriptionASC)
            {
                candidates = candidates.OrderBy(i => i.Objective).ToList();
            }

            else if (candidateList.OrderBy == EmployerFields.OrderByDescriptionDESC)
            {
                candidates = candidates.OrderByDescending(i => i.Objective).ToList();
            }
            else
            {
                candidates = candidates.OrderByDescending(i => i.FirstName).ToList();
            }

            if (!string.IsNullOrEmpty(candidateList.JobTitle))
            {
                candidates = candidates.Where(i => i.JobTitle.Trim().ToLower().Contains(candidateList.JobTitle.Trim().ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(candidateList.Objective))
            {
                if (candidateList.Objective != "Availability")
                {
                    candidates = candidates.Where(i => i.Objective.Trim().ToLower().Contains(candidateList.Objective.Trim().ToLower())).ToList();
                }
            }
            return candidates;
        }
        public BaseModel SaveSortListedCandidate(SortListedCandidateDTO sortListedCandidateDTO)
        {

            var baseModel = new BaseModel();
            EmployerCompany employerCompany = _unitOfWork.EmployerCompanyRepository.Get(ec => ec.UserId == sortListedCandidateDTO.CreatedBy).FirstOrDefault();
            var jobId = _unitOfWork.ProfileJobRepository.Get(i => i.ProfileId == sortListedCandidateDTO.ProfileId && i.IsActive == true).LastOrDefault();
            if (jobId != null && jobId.Id > 0)
            {
                var entity = new SortListedCandidate()
                {
                    JobId = jobId.JobId.Value,
                    ProfileId = sortListedCandidateDTO.ProfileId,
                    CompanyId = employerCompany.CompanyId,
                    IsSortListed = true,
                    SortListedDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = sortListedCandidateDTO.CreatedBy,
                    CreatedAt = DateTime.Now
                };
                _unitOfWork.SortListedCandidateRepository.Add(entity);
                _unitOfWork.Complete();
                if(entity.Id>0)
                {
                    var notificationslist = (from notification in _unitOfWork.NotificationTypeJobRepository.Query(x =>  x.IsActive == true && x.IsDeleted == false)
                                             where notification.JobId== jobId.JobId.Value
                                             select notification).ToList();

                    foreach (var notification in notificationslist)
                    {
                       if(notification.NotificationTypeId==1)
                        {
                            baseModel.IsApp = true;
                        }
                       else if(notification.NotificationTypeId==2)
                        {
                            baseModel.IsEmailed = true;
                        }

                    }
                    baseModel.Success = true;
                    baseModel.Message = "Saved successfully.";

                }
                else
                {
                    baseModel.Success = false;
                    baseModel.Message = "Some error occured.";
                }
            }
            return baseModel;
        }
        public List<CandidateDetails> GetSortListedCandidate(int id)
        {
            EmployerCompany employerCompany = _unitOfWork.EmployerCompanyRepository.Get(ec => ec.UserId == id).FirstOrDefault();
            var profileJobs = _unitOfWork.ProfileJobRepository.Query();
            var jobs = _unitOfWork.JobRepository.Query();
            var users = _unitOfWork.Users.Query();
            var shorlistedCandidate = _unitOfWork.SortListedCandidateRepository.Query(i => i.CompanyId == employerCompany.CompanyId);
            var profiles = _unitOfWork.ProfileRepository.Query();
            var sortlistedCandidateList = (from sortcand in shorlistedCandidate
                                           join prof in profiles on sortcand.ProfileId equals prof.Id
                                           join user in users on prof.UserId equals user.Id
                                           join jobR in jobs on sortcand.JobId equals jobR.Id
                                           where jobR.CompanyId == employerCompany.CompanyId
                                           select new CandidateDetails()
                                           {
                                               Id = user.Id,
                                               FirstName = user.FirstName,
                                               LastName = user.LastName,
                                               UserType = user.UserType,
                                               JobTitle = prof.JobTitle,
                                               Objective = prof.Objective
                                           }
                                        ).Distinct().ToList();
            return sortlistedCandidateList;
        }

        public List<CandidateDetails> GetSortListedCandidate(CandidateList candidateList)
        {
            var candidates = GetSortListedCandidate(candidateList.UserId);
            if (candidateList.OrderBy == EmployerFields.OrderByJobTitleASC)
            {
                candidates = candidates.OrderBy(i => i.JobTitle).ToList();
            }

            else if (candidateList.OrderBy == EmployerFields.OrderByJobTitleDESC)
            {
                candidates = candidates.OrderByDescending(i => i.JobTitle).ToList();
            }

            else if (candidateList.OrderBy == EmployerFields.OrderByDescriptionASC)
            {
                candidates = candidates.OrderBy(i => i.Objective).ToList();
            }

            else if (candidateList.OrderBy == EmployerFields.OrderByDescriptionDESC)
            {
                candidates = candidates.OrderByDescending(i => i.Objective).ToList();
            }
            else
            {
                candidates = candidates.OrderByDescending(i => i.FirstName).ToList();
            }
            if (!string.IsNullOrEmpty(candidateList.JobTitle))
            {
                candidates = candidates.Where(i => i.JobTitle.Trim().ToLower().Contains(candidateList.JobTitle.Trim().ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(candidateList.Objective))
            {
                if (candidateList.Objective != "Availability")
                {
                    candidates = candidates.Where(i => i.Objective.Trim().ToLower().Contains(candidateList.Objective.Trim().ToLower())).ToList();
                }
            }
            return candidates;
        }

        public JobPreferencesDTO GetAddressById(int addressid)
        {
            JobPreferencesDTO jobPreferencesDTO = new JobPreferencesDTO();
            CompanyAddressDTO companyAddressdto = new CompanyAddressDTO();
            CompanyAddress companyaddress = new CompanyAddress();
            try
            {
                if (addressid != 0)
                {
                    companyaddress = _unitOfWork.CompanyAddressRepository.Get(ec => ec.Id == addressid && ec.IsActive == true && ec.IsDeleted == false).FirstOrDefault();
                }

                if (companyaddress != null)
                {
                    companyAddressdto = new CompanyAddressDTO
                    {
                        Id = companyaddress.Id,
                        City = companyaddress.City,
                        ZipCode = companyaddress.ZipCode,
                        StateId = Convert.ToInt32(companyaddress.StateId),
                        CountryId = Convert.ToInt32(companyaddress.CountryId),
                        AddressTypeId = Convert.ToInt32(companyaddress.AddressTypeId)
                    };
                    jobPreferencesDTO.companyAddressDTO = companyAddressdto;
                }
                return jobPreferencesDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public InternalDashboardDTO GetPostedNewJob(int id)
        {
            InternalDashboardDTO internalDashboardDTO = new InternalDashboardDTO();
            //List<EmployerDashboardDTO> listEmployerDashboardDTO = new List<EmployerDashboardDTO>();

            try
            {
                using (IdealHiresDbContext db = new IdealHiresDbContext())
                {
                    var newJobCalculate = db.Usp_Get_IHDashboard_Result();
                    foreach (var nJC in newJobCalculate)
                    {
                        internalDashboardDTO.TodayJob = Convert.ToInt32(nJC.TodayJobPost);
                        internalDashboardDTO.WeekJob = Convert.ToInt32(nJC.PreviousWeekJob);
                        internalDashboardDTO.MonthJob = Convert.ToInt32(nJC.PreviousMonthJob);
                        internalDashboardDTO.YearJob = Convert.ToInt32(nJC.PreviousYearJob);
                        internalDashboardDTO.TodayExpiredJob = 0;
                        internalDashboardDTO.WeekExpiredJob = Convert.ToInt32(nJC.PreviousWeekExpiredJob);
                        internalDashboardDTO.MonthExpiredJob = Convert.ToInt32(nJC.PreviousMonthExpiredJob);
                        internalDashboardDTO.YearExpiredJob = Convert.ToInt32(nJC.PreviousYearJobExpired);
                        internalDashboardDTO.Total = Convert.ToInt32(nJC.TotalJob);
                        internalDashboardDTO.TotalExpiredJob = Convert.ToInt32(nJC.TotalExpiredJob);
                        internalDashboardDTO.TotalTodayJob = Convert.ToInt32(nJC.TotalToday);
                        internalDashboardDTO.TotalWeekJob = Convert.ToInt32(nJC.TotalWeek);
                        internalDashboardDTO.TotalMonthJob = Convert.ToInt32(nJC.TotalMonth);
                        internalDashboardDTO.TotalYearJob = Convert.ToInt32(nJC.TotalYear);
                        internalDashboardDTO.AllColumnTotal = Convert.ToInt32(nJC.AllTotal);
                        internalDashboardDTO.TodayModifiedJobPost = Convert.ToInt32(nJC.TodayModifiedJobPost);
                        internalDashboardDTO.WeekModifiedJobPost = Convert.ToInt32(nJC.WeekModifiedJobPost);
                        internalDashboardDTO.MonthModifiedJobPost = Convert.ToInt32(nJC.MonthModifiedJob);
                        internalDashboardDTO.ModifiedYearJobPost = Convert.ToInt32(nJC.ModifiedYearJobPost);
                        internalDashboardDTO.TotalModifiedJobPost = Convert.ToInt32(nJC.TotalModifiedJobPost);
                        internalDashboardDTO.ModifiedJobTotalToday = Convert.ToInt32(nJC.MTotalToday);
                        internalDashboardDTO.ModifiedJobTotalWeek = Convert.ToInt32(nJC.MTotalWeek);
                        internalDashboardDTO.ModifiedJobTotalMonth = Convert.ToInt32(nJC.MTotalMonth);
                        internalDashboardDTO.ModifiedJobTotalYear = Convert.ToInt32(nJC.MTotalYear);
                        internalDashboardDTO.AllModifiedJobTotal = Convert.ToInt32(nJC.MAllTotal);
                        internalDashboardDTO.EmpTProfile = Convert.ToInt32(nJC.TodayProfile);
                        internalDashboardDTO.EmpWProfile = Convert.ToInt32(nJC.PreviousWeekprofile);

                        internalDashboardDTO.EmpMProfile = Convert.ToInt32(nJC.PreviousMonthProfile);
                        internalDashboardDTO.EmpYProfile = Convert.ToInt32(nJC.PreviousYearProfile);
                        internalDashboardDTO.NewEmpTotalProfile = Convert.ToInt32(nJC.TotalProfile);
                        internalDashboardDTO.EmpTProfileNot = Convert.ToInt32(nJC.TodayProfileNot);
                        internalDashboardDTO.EmpWProfileNot = Convert.ToInt32(nJC.PreviousWeekProfileNot);
                        internalDashboardDTO.EmpMProfileNot = Convert.ToInt32(nJC.PreviousMonthProfileNot);
                        internalDashboardDTO.EmpYProfileNot = Convert.ToInt32(nJC.PreviousYearProfileNot);
                        internalDashboardDTO.TotalEmpProfileNot = Convert.ToInt32(nJC.TotalProfileNot);
                        internalDashboardDTO.TodayTotalProfileRow = Convert.ToInt32(nJC.TotalTProfileRow);
                        internalDashboardDTO.WeekTotalProfileRow = Convert.ToInt32(nJC.TotalWRow);
                        internalDashboardDTO.MonthTotalProfileRow = Convert.ToInt32(nJC.TotalMRow);
                        internalDashboardDTO.YearTotalProfileRow = Convert.ToInt32(nJC.TotalYRow);
                        internalDashboardDTO.AllTotalProfileColumn = Convert.ToInt32(nJC.AllTotalcolumn);





                    }
                }

                return internalDashboardDTO;
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
                    CreatedBy = employerBasic.UserId,
                    IsActive = true,
                    IsDeleted = false
                };
                jobTypeJobList.Add(jobTypeJobConvert);
            }
            return jobTypeJobList;
        }
        private List<CompanyAddress> MapComapnyAddress(CompanyDTO company, int Id)
        {
            List<CompanyAddress> companyaddressList = new List<CompanyAddress>();
            for (int i = 0; i < company.SelectedCountries.Count; i++)
            {
                var companyaddressConvert = new CompanyAddress()
                {
                    CompanyId = Id,
                    CountryId = Convert.ToInt32(company.SelectedCountries[i]),
                    IsActive = true,
                    IsDeleted = false,

                };
                companyaddressList.Add(companyaddressConvert);
            }
            return companyaddressList;
        }

        private List<NotificationTypeJob> MapNotificationTypeJobData(JobPreferencesDTO jobPreferences)
        {
            List<NotificationTypeJob> notificationTypeJobList = new List<NotificationTypeJob>();
            for (int i = 0; i < jobPreferences.SelectedNotificationTypes.Count; i++)
            {
                var notificationTypeJobConvert = new NotificationTypeJob()
                {
                    JobId = jobPreferences.JobId,
                    NotificationTypeId = int.Parse(jobPreferences.SelectedNotificationTypes[i]),
                    CreatedAt = DateTime.Now,
                    CreatedBy = jobPreferences.UserId,
                    IsActive = true,
                    IsDeleted = false
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
                    CreatedBy = employerBasic.UserId,
                    IsActive = true,
                    IsDeleted = false

                };
                jobCategoryJobList.Add(jobCategoryJobConvert);
            }
            return jobCategoryJobList;
        }

        public List<JobCreditDTO> GetBuyJobCredits()
        {
            List<JobCreditDTO> JobCreditDTOs = new List<JobCreditDTO>();
            JobCreditDTOs = _unitOfWork.jobCreditRepository.GetAll().Where(i=>i.IsActive==true && i.IsDeleted==false).Select(x => new JobCreditDTO()
            {
                Id = x.Id,
                Description = x.Description,
                Discount = x.Discount,
                Duration = x.Duration,
                IsActive = x.IsActive,
                IsDeleted = x.IsDeleted,
                JobCredit = x.JobCredit1,
                Price = x.Price
            }).ToList();
            return JobCreditDTOs;
        }
        private List<CompanyAddress> MapCompanyAddressData(JobPreferencesDTO jobPreferences)
        {
            List<CompanyAddress> companyaddresslist = new List<CompanyAddress>();
            foreach (var item in jobPreferences.companyAddressList)
            {
                var companyaddressConvert = new CompanyAddress()
                {
                    CompanyId = item.CompanyId,
                    CountryId = Convert.ToInt32(item.CountryId),
                    StateId = Convert.ToInt32(item.StateId),
                    City = Convert.ToString(item.City),
                    ZipCode = Convert.ToString(item.ZipCode),
                    AddressTypeId = Convert.ToInt32(item.AddressTypeId),
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    CreatedBy = jobPreferences.UserId,
                    UpdatedAt = DateTime.Now,
                    UpdateBy = jobPreferences.UserId
                };
                companyaddresslist.Add(companyaddressConvert);
            }
            return companyaddresslist;
        }
        public BaseModel SaveCompanyPackageDetail(CompanyPackageDetailDTO companyPackageDetailDTO)
        {
            var companyId = _unitOfWork.EmployerCompanyRepository.GetFirstOrDefault(ec => ec.UserId == companyPackageDetailDTO.UserId).CompanyId;
            var response = new BaseModel();

            var entity = new CompanyPackageDetail()
            {
                CompanyId = companyId,
                UserId = companyPackageDetailDTO.UserId,
                JobCreditId = companyPackageDetailDTO.JobCreditId,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = companyPackageDetailDTO.UserId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UpdatedBy = companyPackageDetailDTO.UserId
            };
            _unitOfWork.CompanyPackageDetailRepository.Add(entity);
            _unitOfWork.Complete();
            int id = entity.Id;
            var companyJobCredit = _unitOfWork.CompanyJobCreditDetailRepository.Query(i => i.CompanyId == companyId).FirstOrDefault();
            if (companyJobCredit != null)
            {
                int availableCredit = (companyPackageDetailDTO.JobCredit + Convert.ToInt32(companyJobCredit.AvailableCredit));

                companyJobCredit.AvailableCredit = availableCredit;
                _unitOfWork.CompanyJobCreditDetailRepository.Update(companyJobCredit);
                _unitOfWork.Complete();
                SaveCompanyJobCreditDetailHistory(companyId, availableCredit, Convert.ToInt32(companyJobCredit.UsedCredit), companyPackageDetailDTO.UserId);
            }
            else
            {
                var companyJobCreditDetail = new CompanyJobCreditDetail()
                {
                    CompanyId = companyId,
                    CompanyPackageDetailsId = id,
                    AvailableCredit = companyPackageDetailDTO.JobCredit,
                    UsedCredit = 0
                };
                _unitOfWork.CompanyJobCreditDetailRepository.Add(companyJobCreditDetail);
                _unitOfWork.Complete();
                SaveCompanyJobCreditDetailHistory(companyId, companyPackageDetailDTO.JobCredit, 0, companyPackageDetailDTO.UserId);
                if (entity.Id > 0)
                {
                    response.Success = true;
                    response.Message = EmployerResource.SavedSuccessfully;
                }
                else
                {
                    response.Success = false;
                    response.Message = EmployerResource.SomeErrorOccured;
                }
            }
            return response;
        }

        public void SaveCompanyJobCreditDetailHistory(int companyId, int availableCredit, int usedCredit, int userId)
        {
            var companyJobCreditDetailHistory = new CompanyJobCreditDetailHistory()
            {
                CompanyId = companyId,
                AvailableCredit = availableCredit,
                UsedCredit = usedCredit,
                UsedBy = userId
            };
            _unitOfWork.CompanyJobCreditDetailHistoryRepository.Add(companyJobCreditDetailHistory);
            _unitOfWork.Complete();
        }

        public BaseModel GetAvailableJobCredit(int id)
        {
            var response = new BaseModel();
            var companyId = _unitOfWork.EmployerCompanyRepository.GetFirstOrDefault(ec => ec.UserId == id).CompanyId;
            var jobCreditDetails = _unitOfWork.CompanyJobCreditDetailRepository.Query(i => i.CompanyId == companyId).FirstOrDefault();
            if (jobCreditDetails != null)
            {
                var availableJobCredit = jobCreditDetails.AvailableCredit;

                if (availableJobCredit > 0)
                {
                    response.Success = true;
                    response.Message = EmployerResource.SufficientJobCredit + " " + availableJobCredit;
                }
                else
                {
                    response.Success = false;
                    response.Message = EmployerResource.LessJobCredit;
                }
            }
            else
            {
                response.Success = false;
                response.Message = EmployerResource.LessJobCredit;
            }
            return response;
        }
        public CandidateBasicDTO Downloadfile(int id)
        {
            var profile = _unitOfWork.ProfileRepository.Query(i => i.Id == id).FirstOrDefault();
            return new CandidateBasicDTO()
            {
                OrgFileName = profile.OrgFileName,
                ResumeFilePath = profile.ResumeFile
            };
        }


        /// <summary>
        /// Save package details purchased by company.
        /// </summary>
        /// <param name="companyPackageDetailDTO"></param>
        /// <returns></returns>
        public BaseModel SavePackageDetail(CompanyPackageDetailDTO companyPackageDetailDTO)
        {

            var response = new BaseModel();
            int id = SaveCompanyPackageDetails(companyPackageDetailDTO);
            var companyJobCredit = _unitOfWork.CompanyJobCreditDetailRepository.Query(i => i.CompanyId == companyPackageDetailDTO.CompanyId).FirstOrDefault();
            if (companyJobCredit != null)
            {
                int availableCredit = (companyPackageDetailDTO.JobCredit + Convert.ToInt32(companyJobCredit.AvailableCredit));
                companyJobCredit.AvailableCredit = availableCredit;
                _unitOfWork.CompanyJobCreditDetailRepository.Update(companyJobCredit);
                _unitOfWork.Complete();
                response.Success = true;
                response.Message = EmployerResource.TransactionSuccessful;
            }
            else
            {
                SaveCompanyJobCreditDetail(companyPackageDetailDTO.CompanyId, id, companyPackageDetailDTO.JobCredit);
                if (id > 0)
                {
                    response.Success = true;
                    response.Message = EmployerResource.SavedSuccessfully;
                }
                else
                {
                    response.Success = false;
                    response.Message = EmployerResource.SomeErrorOccured;
                }
            }
            return response;
        }

        public int SaveCompanyPackageDetails(CompanyPackageDetailDTO companyPackageDetailDTO)
        {
            var entity = new CompanyPackageDetail()
            {
                CompanyId = companyPackageDetailDTO.CompanyId,
                UserId = companyPackageDetailDTO.UserId,
                JobCreditId = companyPackageDetailDTO.JobCreditId,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = companyPackageDetailDTO.UserId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UpdatedBy = companyPackageDetailDTO.UserId
            };
            _unitOfWork.CompanyPackageDetailRepository.Add(entity);
            _unitOfWork.Complete();
            return entity.Id;
        }

        public int SaveCompanyJobCreditDetail(int companyId, int companyPackageDetailsId, int jobCredit)
        {
            var companyJobCreditDetail = new CompanyJobCreditDetail()
            {
                CompanyId = companyId,
                CompanyPackageDetailsId = companyPackageDetailsId,
                AvailableCredit = jobCredit,
                UsedCredit = 0
            };
            _unitOfWork.CompanyJobCreditDetailRepository.Add(companyJobCreditDetail);
            _unitOfWork.Complete();
            return companyJobCreditDetail.Id;
        }

        public int UpdateCompanyJobCreditDetails(int companyId, int userId)
        {
            var companyJobCreditDetail = _unitOfWork.CompanyJobCreditDetailRepository.Query(i => i.CompanyId == companyId).FirstOrDefault();
            if (companyJobCreditDetail != null)
            {
                var availableJobCredit = companyJobCreditDetail.AvailableCredit - 1;
                var usedJobCredit = companyJobCreditDetail.UsedCredit + 1;

                companyJobCreditDetail.AvailableCredit = availableJobCredit;
                companyJobCreditDetail.UsedCredit = usedJobCredit;
                companyJobCreditDetail.LastUsedBy = userId;
                companyJobCreditDetail.LastUsedDate = DateTime.Now;
                _unitOfWork.CompanyJobCreditDetailRepository.Update(companyJobCreditDetail);
                _unitOfWork.Complete();
            }
            return companyJobCreditDetail.Id;
        }

        public int SaveTransactionDetails(TransactionDTO transactionDetailDTO)
        {
            var entity = new TransactionDetail()
            {
                CompanyId = transactionDetailDTO.CompanyId,
                UserId = transactionDetailDTO.UserId,
                Amount = transactionDetailDTO.TotalAmount,
                TransactionId = transactionDetailDTO.TransId,
                TransactionDate = transactionDetailDTO.TransactionDateTime,
                AccountType = transactionDetailDTO.AccountType,
                Authorization = transactionDetailDTO.Authorization,
                ErrorCode = transactionDetailDTO.ErrorCode,
                ErrorMessage = transactionDetailDTO.ErrorMessage,
                Message = transactionDetailDTO.Message
            };
            _unitOfWork.TransactionDetailRepository.Add(entity);
            return entity.Id;
        }

        public CompanyEmployee GetEmployeeDetails(int id)
        {
            var users = _unitOfWork.Users.Query(i => i.IsActive == true);
            var employerCompanies = _unitOfWork.EmployerCompanyRepository.Query(i => i.IsActive == true);
            var companies = _unitOfWork.CompanyRepository.Query(i => i.IsActive == true);
            var companyAddresses = _unitOfWork.CompanyAddressRepository.Query(i => i.IsActive == true);
            var states = _unitOfWork.StateRepository.Query(i => i.IsActive == true);
            var countries = _unitOfWork.CountryRepository.Query(i => i.IsActive == true);
            return (from user in users
                    join employerCompany in employerCompanies on user.Id equals employerCompany.UserId
                    join company in companies on employerCompany.CompanyId equals company.Id
                    join compAddress in companyAddresses on company.Id equals compAddress.CompanyId
                    join state in states on compAddress.StateId equals state.Id
                    join country in countries on compAddress.CountryId equals country.Id
                    where user.Id == id
                    select new CompanyEmployee()
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        EmailAddress = user.EmailId,
                        CompanyName = company.CompanyName,
                        Address = compAddress.Address,
                        City = compAddress.City,
                        StateCode = state.Name,
                        Country = country.Name,
                        Phone= (company.Phone!=null?company.Phone:company.Phone1),
                        Zip= (compAddress.ZipCode!=null) ? compAddress.ZipCode:string.Empty,
                        //Zip=compAddress.ZipCode!=null?Convert.ToUInt32(compAddress.ZipCode)?0
                    }).FirstOrDefault();
        }
        public BaseModel MakePayment(TransactionDTO transactionDTO)
        {
            var response = new BaseModel();
            var companyId = _unitOfWork.EmployerCompanyRepository.GetFirstOrDefault(ec => ec.UserId == transactionDTO.UserId).CompanyId;
            transactionDTO.CompanyId = companyId;
            int transactionDetialsId = SaveTransactionDetails(transactionDTO);
            CompanyPackageDetailDTO companyPackageDetailDTO = new CompanyPackageDetailDTO();
            companyPackageDetailDTO.UserId = transactionDTO.UserId;
            companyPackageDetailDTO.JobCreditId = transactionDTO.JobCreditId;
            companyPackageDetailDTO.JobCredit = transactionDTO.JobCredit;
            companyPackageDetailDTO.Price = transactionDTO.TotalAmount;
            companyPackageDetailDTO.CompanyId = companyId;
            response = SavePackageDetail(companyPackageDetailDTO);
            return response;
        }

        /// <summary>
        /// Get latest candidate list for employer to search appropriate candidate.
        /// </summary>
        /// <returns></returns>
        public List<CandidateDetails> GetLatestCandidateList()
        {
            var users = _unitOfWork.Users.Query();
            var profiles = _unitOfWork.ProfileRepository.Query();

            var applicantCandidate = (from user in users
                                      join profile in profiles on user.Id equals profile.UserId
                                      select new CandidateDetails()
                                      {
                                          Id = user.Id,
                                          FirstName = user.FirstName,
                                          LastName = user.LastName,
                                          UserType = user.UserType,
                                          JobTitle = (profile.JobTitle != null ? profile.JobTitle : string.Empty),
                                          Objective = (!string.IsNullOrEmpty(profile.Objective) ? profile.Objective : string.Empty)
                                      }

                  ).Distinct().ToList();


            return applicantCandidate;
        }

        public List<CandidateDetails> GetSearchedLatestCandidateList(CandidateList candidateList)
        {
            var candidates = GetLatestCandidateList();
            if (candidateList.OrderBy == EmployerFields.OrderByJobTitleASC)
            {
                candidates = candidates.OrderBy(i => i.JobTitle).ToList();
            }

            else if (candidateList.OrderBy == EmployerFields.OrderByJobTitleDESC)
            {
                candidates = candidates.OrderByDescending(i => i.JobTitle).ToList();
            }

            else if (candidateList.OrderBy == EmployerFields.OrderByDescriptionASC)
            {
                candidates = candidates.OrderBy(i => i.Objective).ToList();
            }

            else if (candidateList.OrderBy == EmployerFields.OrderByDescriptionDESC)
            {
                candidates = candidates.OrderByDescending(i => i.Objective).ToList();
            }
            else
            {
                candidates = candidates.OrderByDescending(i => i.FirstName).ToList();
            }
            if (!string.IsNullOrEmpty(candidateList.JobTitle))
            {
                candidates = candidates.Where(i => i.JobTitle.Trim().ToLower().Contains(candidateList.JobTitle.Trim().ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(candidateList.Objective))
            {
                if (candidateList.Objective != "Availability")
                    candidates = candidates.Where(i => i.Objective.Trim().ToLower().Contains(candidateList.Objective.Trim().ToLower())).ToList();
            }
            return candidates;
        }
        #endregion
    }
}

