using IdealHires.BAL.DataContext;
using IdealHires.Data;
using IdealHires.DTO;
using IdealHires.DTO.Candidate;
using IdealHires.DTO.Employer;
using IdealHires.DTO.Fields;
using IdealHires.DTO.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.BAL.Business
{
    public class CandidateService : IDisposable
    {
        #region Private Menber
        bool disposed = false;
        private readonly IUnitOfWork _unitOfWork;
        private readonly NotificationService _notificationService;
        #endregion

        #region Constructor
        public CandidateService()
        {
            _unitOfWork = new UnitOfWork(new IdealHiresDbContext());
            _notificationService = new NotificationService();
        }

        #endregion

        #region Candidate Basic

        public CandidateBasicDTO GetBasicDetails(int id)
        {
            CandidateBasicDTO basicDetailDTO = new CandidateBasicDTO();
            try
            {
                User jobTypeList = _unitOfWork.Users.Get(id);
                Profile profile = jobTypeList.Profiles.FirstOrDefault();
                if (profile != null)
                {
                    basicDetailDTO = new CandidateBasicDTO
                    {
                        Id = profile.Id,
                        UserId = jobTypeList.Id,
                        JobTitle = profile.JobTitle,
                        ResumeFilePath = (string.IsNullOrEmpty(profile.ResumeFile)) ? string.Empty : Path.GetFileName(profile.ResumeFile),
                        OrgFileName = (string.IsNullOrEmpty(profile.OrgFileName)) ? string.Empty : profile.OrgFileName,
                        FirstName = jobTypeList.FirstName,
                        LastName = jobTypeList.LastName,
                        UserType = jobTypeList.UserType
                    };
                }
                return basicDetailDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int InsertBasicDetails(CandidateBasicDTO basicCandidate)
        {
            int profileId = 0;
            Profile profile = null;
            try
            {
                var userData = _unitOfWork.Users.Get(basicCandidate.UserId);
                if (userData != null)
                {
                    userData.FirstName = basicCandidate.FirstName;
                    userData.LastName = basicCandidate.LastName;
                    userData.UserType = "Candidate";
                    userData.UpdatedAt = DateTime.Now;
                    userData.UpdatedBy = basicCandidate.UserId;
                    _unitOfWork.Users.Update(userData);

                    var userProfile = userData.Profiles.FirstOrDefault();
                    if (userProfile == null)
                    {
                        profile = new Profile()
                        {
                            UserId = basicCandidate.UserId,
                            ResumeFile = basicCandidate.ResumeFilePath,
                            OrgFileName = basicCandidate.OrgFileName,
                            JobTitle = basicCandidate.JobTitle,
                            CreatedAt = DateTime.Now,
                            CreatedBy = basicCandidate.UserId,
                            IsActive = true,
                            IsDeleted = false
                        };
                        _unitOfWork.ProfileRepository.Add(profile);
                        _unitOfWork.Complete();
                        profileId = profile.Id;
                    }
                    else
                    {
                        userProfile.UserId = basicCandidate.UserId;
                        userProfile.ResumeFile = (string.IsNullOrEmpty(basicCandidate.ResumeFilePath)) ? userProfile.ResumeFile : basicCandidate.ResumeFilePath;
                        userProfile.OrgFileName = basicCandidate.OrgFileName;
                        userProfile.JobTitle = basicCandidate.JobTitle;
                        userProfile.UpdatedAt = DateTime.Now;
                        userProfile.UpdatedBy = basicCandidate.UserId;
                        userProfile.IsActive = true;
                        userProfile.IsDeleted = false;
                        _unitOfWork.ProfileRepository.Update(userProfile);
                        _unitOfWork.Complete();
                        profileId = userProfile.Id;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return profileId;
        }

        #endregion

        #region Candidate Contact

        public CandidateContactDTO GetContactDetails(int id)
        {
            var contactDetailDTO = new CandidateContactDTO();
            try
            {
                User jobTypeList = _unitOfWork.Users.Get(id);
                Profile profile = jobTypeList.Profiles.FirstOrDefault();
                if (profile != null)
                {
                    Address address = profile.Addresses.FirstOrDefault();

                    if (address != null)
                    {
                        contactDetailDTO = new CandidateContactDTO
                        {
                            Id = address.Id,
                            UserId = jobTypeList.Id,
                            StreetAddress1 = address.StreetAddress1,
                            StreetAddress2 = address.StreetAddress2,
                            City = address.City,
                            StateId = address.StateId,
                            CountryId = address.CountryId,
                            ZipCode = address.ZipCode,
                            Phone1 = address.Phone1,
                            Phone2 = address.Phone2,
                            EmailAddress = jobTypeList.EmailId

                        };

                        contactDetailDTO.States = GetStateByCountryId(address.CountryId);
                    }
                }
                contactDetailDTO.EmailAddress = jobTypeList.EmailId;
                contactDetailDTO.Countries = GetCountries();

                return contactDetailDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CandidateContactDTO GetCandidateContactDetails(int id)
        {
            var result = (from user in _unitOfWork.Users.Query(i => i.IsActive == true)
                          join profile in _unitOfWork.ProfileRepository.Query(i => i.IsActive == true && i.IsDeleted == false) on user.Id equals profile.UserId
                          join address in _unitOfWork.AddressRepository.Query(i => i.IsActive == true && i.IsDeleted == false) on profile.Id equals address.ProfileId
                          join country in _unitOfWork.CountryRepository.Query(i => i.IsActive == true && i.IsDeleted == false) on address.CountryId equals country.Id
                          join state in _unitOfWork.StateRepository.Query(i => i.IsActive == true && i.IsDeleted == false) on address.StateId equals state.Id
                          where user.Id == id
                          select new CandidateContactDTO()
                          {
                              Id = user.Id,
                              UserId = user.Id,
                              StreetAddress1 = address.StreetAddress1,
                              StreetAddress2 = address.StreetAddress2,
                              City = address.City,
                              StateName = state.Name,
                              CountryName = country.Name,
                              ZipCode = address.ZipCode,
                              Phone1 = address.Phone1,
                              Phone2 = address.Phone2,
                              EmailAddress = user.EmailId
                          }
                ).ToList();
            return result.FirstOrDefault();
        }

        public int InsertContactDetails(CandidateContactDTO basicCandidate)
        {
            int addressId = 0;
            Address address = null;
            try
            {
                var userProfile = _unitOfWork.ProfileRepository.GetFirstOrDefault(p => p.UserId == basicCandidate.UserId);
                var addresses = userProfile.Addresses.FirstOrDefault();
                if (userProfile != null)
                {
                    if (addresses == null)
                    {
                        address = new Address()
                        {
                            ProfileId = userProfile.Id,
                            StreetAddress1 = basicCandidate.StreetAddress1,
                            StreetAddress2 = basicCandidate.StreetAddress2,
                            City = basicCandidate.City,
                            StateId = basicCandidate.StateId,
                            CountryId = basicCandidate.CountryId,
                            ZipCode = basicCandidate.ZipCode,
                            Phone1 = basicCandidate.Phone1,
                            Phone2 = basicCandidate.Phone2,
                            CreatedAt = DateTime.Now,
                            CreatedBy = basicCandidate.UserId,
                            IsDeleted = false,
                            IsActive = true
                        };
                        _unitOfWork.AddressRepository.Add(address);
                        _unitOfWork.Complete();
                        addressId = address.Id;
                    }
                    else
                    {
                        addresses.ProfileId = userProfile.Id;
                        addresses.StreetAddress1 = basicCandidate.StreetAddress1;
                        addresses.StreetAddress2 = basicCandidate.StreetAddress2;
                        addresses.City = basicCandidate.City;
                        addresses.StateId = basicCandidate.StateId;
                        addresses.CountryId = basicCandidate.CountryId;
                        addresses.ZipCode = basicCandidate.ZipCode;
                        addresses.Phone1 = basicCandidate.Phone1;
                        addresses.Phone2 = basicCandidate.Phone2;
                        addresses.UpdatedAt = DateTime.Now;
                        addresses.UpdatedBy = basicCandidate.UserId;
                        _unitOfWork.AddressRepository.Update(addresses);
                        _unitOfWork.Complete();
                        addressId = addresses.Id;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return addressId;
        }

        #endregion

        #region Candidate Work

        public bool DeleteWork(int workId)
        {
            try
            {
                Work workDetailsByworkId = _unitOfWork.WorkRepository.Get(workId);
                if (workDetailsByworkId.Id > 0)
                {
                    _unitOfWork.WorkRepository.Remove(workDetailsByworkId);
                    _unitOfWork.Complete();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CandidateWorkDTO GetWorkDetailsByWorkId(int workId)
        {
            CandidateWorkDTO workDetailDTO = null;
            try
            {
                Work workDetailsByworkId = _unitOfWork.WorkRepository.Get(workId);
                Profile profile = _unitOfWork.ProfileRepository.Get(workDetailsByworkId.ProfileId);

                if (workDetailsByworkId.Id > 0)
                {
                    workDetailDTO = new CandidateWorkDTO
                    {
                        Id = workDetailsByworkId.Id,
                        UserId = profile.UserId,
                        ProfileId = profile.Id,
                        CompanyName = workDetailsByworkId.CompanyName,
                        PositionName = workDetailsByworkId.PositionName,
                        Description = workDetailsByworkId.Description,
                        Salary = workDetailsByworkId.Salary,
                        CurrencyId = workDetailsByworkId.CurrencyId ?? 0,
                        StartAt = workDetailsByworkId.StartAt.HasValue ? workDetailsByworkId.StartAt.Value : (DateTime?)null,
                        EndAt = workDetailsByworkId.EndAt.HasValue ? workDetailsByworkId.EndAt.Value : (DateTime?)null,
                        IsCurrent = (workDetailsByworkId.EndAt == null) ? true : false,
                        PayPeriodTypeId = Convert.ToInt32(workDetailsByworkId.PayPeriodTypeId)
                    };
                }
                return workDetailDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CandidateWorkDTO> GetWorkDetails(int id)
        {
            List<CandidateWorkDTO> workDetailDTO = new List<CandidateWorkDTO>();
            try
            {
                User jobTypeList = _unitOfWork.Users.Get(id);
                Profile profile = jobTypeList.Profiles.FirstOrDefault();
                if (profile != null)
                {
                    List<Work> work = profile.Works.ToList();
                    if (work.Count > 0)
                    {
                        foreach (var w in work)
                        {
                            var candidateWork = new CandidateWorkDTO
                            {
                                Id = w.Id,
                                UserId = jobTypeList.Id,
                                ProfileId = profile.Id,
                                CompanyName = w.CompanyName,
                                PositionName = w.PositionName,
                                Description = w.Description,
                                Salary = w.Salary,
                                CurrencyId = w.CurrencyId ?? 0,
                                StartAt = w.StartAt.Value.Date,
                                EndAt = w.EndAt.HasValue ? w.EndAt.Value.Date : (DateTime?)null,
                                IsCurrent = (w.EndAt == null) ? true : false,
                                TotalExperience = new DateDifference(w.StartAt ?? DateTime.Now, w.EndAt ?? DateTime.Now).ToString(),
                                PayPeriodTypeId = Convert.ToInt32(w.PayPeriodTypeId)
                            };
                            workDetailDTO.Add(candidateWork);
                        }
                    }
                }
                return workDetailDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int InsertWorkDetails(CandidateWorkDTO workCandidate)
        {
            int workId = 0;
            try
            {
                if (workCandidate != null)
                {
                    User user = _unitOfWork.Users.Get(workCandidate.UserId);
                    Profile profile = user.Profiles.FirstOrDefault();
                    if (workCandidate.Id > 0)
                    {
                        Work workDetails = new Work();
                        workDetails = _unitOfWork.WorkRepository.Get(workCandidate.Id);
                        workDetails.CompanyName = workCandidate.CompanyName;
                        workDetails.PositionName = workCandidate.PositionName;
                        workDetails.Description = workCandidate.Description;
                        workDetails.Salary = workCandidate.Salary;
                        workDetails.CurrencyId = workCandidate.CurrencyId;
                        workDetails.StartAt = workCandidate.StartAt.HasValue ? workCandidate.StartAt.Value : (DateTime?)null;
                        workDetails.EndAt = workCandidate.EndAt.HasValue ? workCandidate.EndAt.Value : (DateTime?)null;//(string.IsNullOrEmpty(workCandidate.EndAt)) ? (DateTime?)null : Convert.ToDateTime(workCandidate.EndAt);
                        workDetails.UpdatedAt = DateTime.Now;
                        workDetails.UpdatedBy = workCandidate.UserId;
                        workDetails.IsActive = true;
                        workDetails.IsDeleted = false;
                        workDetails.PayPeriodTypeId = workCandidate.PayPeriodTypeId;
                        _unitOfWork.WorkRepository.Update(workDetails);
                        _unitOfWork.Complete();
                        workId = workDetails.Id;
                    }
                    else
                    {
                        var work = new Work()
                        {
                            ProfileId = profile.Id,
                            CompanyName = workCandidate.CompanyName,
                            PositionName = workCandidate.PositionName,
                            Description = workCandidate.Description,
                            Salary = workCandidate.Salary,
                            CurrencyId = workCandidate.CurrencyId,
                            StartAt = workCandidate.StartAt.HasValue ? workCandidate.StartAt.Value : (DateTime?)null,
                            EndAt = workCandidate.EndAt.HasValue ? workCandidate.EndAt.Value : (DateTime?)null,
                            IsDeleted = false,
                            IsActive = true,
                            PayPeriodTypeId = workCandidate.PayPeriodTypeId,
                            CreatedAt = DateTime.Now,
                            CreatedBy = workCandidate.UserId

                        };
                        _unitOfWork.WorkRepository.Add(work);
                        _unitOfWork.Complete();
                        workId = work.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return workId;
        }

        #endregion

        #region Candidate Education

        public bool DeleteEducation(int educationId)
        {
            try
            {
                Academic educationDetailsByeducationId = _unitOfWork.AcademicRepository.Get(educationId);
                if (educationDetailsByeducationId.Id > 0)
                {
                    _unitOfWork.AcademicRepository.Remove(educationDetailsByeducationId);
                    _unitOfWork.Complete();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CandidateEducationDTO GeEducationDetailsByEdudationId(int educationId)
        {
            CandidateEducationDTO educationDetailDTO = null;
            try
            {
                Academic educationDetailsById = _unitOfWork.AcademicRepository.Get(educationId);
                Profile profile = _unitOfWork.ProfileRepository.Get(Convert.ToInt32(educationDetailsById.ProfileId));
                if (educationDetailsById.Id > 0)
                {
                    educationDetailDTO = new CandidateEducationDTO
                    {
                        Id = educationDetailsById.Id,
                        UserId = profile.UserId,
                        ProfileId = profile.Id,
                        Major = educationDetailsById.Major,
                        Minor = educationDetailsById.Minor,
                        InstituteName = educationDetailsById.InstituteName,
                        IsDegreeOrCertification = Convert.ToBoolean(educationDetailsById.IsDegreeOrCertification),
                        IsMinorDegree = Convert.ToBoolean(educationDetailsById.IsMinorDegree),
                        StartAt = educationDetailsById.StartAt.HasValue ? educationDetailsById.StartAt.Value : (DateTime?)null,
                        EndAt = educationDetailsById.EndAt.HasValue ? educationDetailsById.EndAt.Value : (DateTime?)null,
                    };
                }
                return educationDetailDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CandidateEducationDTO> GetEducationDetails(int id)
        {
            List<CandidateEducationDTO> educationDetailDTO = new List<CandidateEducationDTO>();
            try
            {
                User jobTypeList = _unitOfWork.Users.Get(id);
                Profile profile = jobTypeList.Profiles.FirstOrDefault();
                if (profile != null)
                {
                    List<Academic> academic = profile.Academics.ToList();
                    if (academic.Count > 0)
                    {
                        foreach (var a in academic)
                        {
                            var candidateWork = new CandidateEducationDTO
                            {
                                Id = a.Id,
                                UserId = jobTypeList.Id,
                                ProfileId = profile.Id,
                                Major = a.Major,
                                Minor = a.Minor,
                                InstituteName = a.InstituteName,
                                IsDegreeOrCertification = Convert.ToBoolean(a.IsDegreeOrCertification),
                                IsMinorDegree = Convert.ToBoolean(a.IsMinorDegree),
                                StartAt = a.StartAt.Value,
                                EndAt = a.EndAt.HasValue ? a.EndAt.Value : (DateTime?)null,
                                TotalDuration = new DateDifference(a.StartAt ?? DateTime.Now, a.EndAt ?? DateTime.Now).ToString()
                            };
                            educationDetailDTO.Add(candidateWork);
                        }
                    }
                }
                return educationDetailDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int InsertEducationDetails(CandidateEducationDTO educationCandidate)
        {
            int educationId = 0;
            try
            {
                if (educationCandidate != null)
                {
                    User user = _unitOfWork.Users.Get(educationCandidate.UserId);
                    Profile profile = user.Profiles.FirstOrDefault();
                    if (educationCandidate.Id > 0)
                    {
                        Academic academicDetails = new Academic();
                        academicDetails = _unitOfWork.AcademicRepository.Get(educationCandidate.Id);
                        academicDetails.Major = educationCandidate.Major;
                        academicDetails.Minor = educationCandidate.Minor;
                        academicDetails.InstituteName = educationCandidate.InstituteName;
                        academicDetails.IsDegreeOrCertification = educationCandidate.IsDegreeOrCertification;
                        academicDetails.IsMinorDegree = educationCandidate.IsMinorDegree;
                        academicDetails.StartAt = educationCandidate.StartAt.HasValue ? educationCandidate.StartAt.Value : (DateTime?)null;
                        academicDetails.EndAt = educationCandidate.EndAt.HasValue ? educationCandidate.EndAt.Value : (DateTime?)null;
                        academicDetails.UpdatedAt = DateTime.Now;
                        academicDetails.UpdatedBy = educationCandidate.UserId;

                        _unitOfWork.AcademicRepository.Update(academicDetails);
                        _unitOfWork.Complete();
                        educationId = academicDetails.Id;
                    }
                    else
                    {
                        var academic = new Academic()
                        {
                            ProfileId = profile.Id,
                            Major = educationCandidate.Major,
                            Minor = educationCandidate.Minor,
                            InstituteName = educationCandidate.InstituteName,
                            StartAt = educationCandidate.StartAt.HasValue ? educationCandidate.StartAt.Value : (DateTime?)null,
                            EndAt = educationCandidate.EndAt.HasValue ? educationCandidate.EndAt.Value : (DateTime?)null,
                            IsDegreeOrCertification = educationCandidate.IsDegreeOrCertification,
                            IsMinorDegree = educationCandidate.IsMinorDegree,
                            CreatedAt = DateTime.Now,
                            CreatedBy = educationCandidate.UserId
                        };
                        _unitOfWork.AcademicRepository.Add(academic);
                        _unitOfWork.Complete();
                        educationId = academic.Id;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return educationId;
        }

        #endregion

        #region Get Preference Details

        public CandidatePreferencesDTO GetPreferencesDetails(int id)
        {
            CandidatePreferencesDTO preferencesDetailDTO = new CandidatePreferencesDTO();
            List<JobTypeProfileDTO> jobTypeProfileDTO = new List<JobTypeProfileDTO>();
            List<JobCategoryProfileDTO> jobCategoryProfileDTO = new List<JobCategoryProfileDTO>();

            List<string> selectedJobTypeProfile = new List<string>();
            List<string> selectedJobCategoryProfile = new List<string>();
            try
            {
                User jobTypeList = _unitOfWork.Users.Get(id);
                Profile profile = jobTypeList.Profiles.FirstOrDefault();
                if (profile != null)
                {
                    List<JobTypeProfile> jobTypeProfile = profile.JobTypeProfiles.ToList();
                    List<JobCategoryProfile> jobCategoryProfile = profile.JobCategoryProfiles.ToList();
                    KeywordsProfile keywordsProfile = profile.KeywordsProfiles.FirstOrDefault();

                    if (jobTypeProfile != null && jobTypeProfile.Count > 0)
                    {
                        foreach (var jT in jobTypeProfile)
                        {
                            var jTPDTO = jT.JobTypeId.ToString();
                            selectedJobTypeProfile.Add(jTPDTO);
                        }
                        preferencesDetailDTO.SelectedJobTypes = selectedJobTypeProfile;
                    }
                    if (jobCategoryProfile != null && jobCategoryProfile.Count > 0)
                    {
                        foreach (var jCP in jobCategoryProfile)
                        {
                            var jCPDTO = jCP.JobCategoryId.ToString();
                            selectedJobCategoryProfile.Add(jCPDTO);
                        }
                        preferencesDetailDTO.SelectedJobCategory = selectedJobCategoryProfile;
                    }
                    if (keywordsProfile != null)
                    {
                        preferencesDetailDTO.Keywords = keywordsProfile.Keywords;
                        preferencesDetailDTO.Id = keywordsProfile.Id;
                    }
                    preferencesDetailDTO.Objective = profile.Objective;
                }
                return preferencesDetailDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Candidate Preview
        public CandidatePreviewDTO GetCandidatePreviewDetails(int id)
        {
            CandidatePreviewDTO candidatePreviewDTO = new CandidatePreviewDTO();
            User jobTypeList = _unitOfWork.Users.Get(id);
            Profile profile = jobTypeList.Profiles.FirstOrDefault();
            if (profile != null)
            {
                List<CandidateEducationDTO> listCandidateEducationDTO = GetEducationDetails(id);
                List<CandidateWorkDTO> listCandidateWorkDTO = GetWorkDetails(id);
                CandidatePreferencesDTO preferencesDetailDTO = GetPreferencesDetails(id);
                CandidateBasicDTO candidateBasicDTO = GetBasicDetails(id);
                CandidateContactDTO candidateContactDTO = GetContactDetails(id);
                candidatePreviewDTO.CandidateBasicPreview = candidateBasicDTO;
                candidatePreviewDTO.CandidateContactPreview = candidateContactDTO;
                candidatePreviewDTO.CandidateEducationPreview = listCandidateEducationDTO;
                candidatePreviewDTO.CandidateWorkPreview = listCandidateWorkDTO;
                candidatePreviewDTO.PreferencesPreview = preferencesDetailDTO;
            }

            return candidatePreviewDTO;
        }
        #endregion

        public CandidatePreviewDTO GetCandidateDetailsForEmployer(int id)
        {
            CandidatePreviewDTO candidatePreviewDTO = new CandidatePreviewDTO();
            User jobTypeList = _unitOfWork.Users.Get(id);
            Profile profile = jobTypeList.Profiles.FirstOrDefault();
            if (profile != null)
            {
                List<CandidateEducationDTO> listCandidateEducationDTO = GetEducationDetails(id);
                List<CandidateWorkDTO> listCandidateWorkDTO = GetWorkDetails(id);
                CandidatePreferencesDTO preferencesDetailDTO = GetPreferencesDetails(id);
                CandidateBasicDTO candidateBasicDTO = GetBasicDetails(id);
                CandidateContactDTO candidateContactDTO = GetCandidateContactDetails(id);
                candidatePreviewDTO.CandidateBasicPreview = candidateBasicDTO;
                candidatePreviewDTO.CandidateContactPreview = candidateContactDTO;
                candidatePreviewDTO.CandidateEducationPreview = listCandidateEducationDTO;
                candidatePreviewDTO.CandidateWorkPreview = listCandidateWorkDTO;
                candidatePreviewDTO.PreferencesPreview = preferencesDetailDTO;
            }

            return candidatePreviewDTO;
        }


        #region Get job bexplore for Candidate
        public List<JobDTO> GetJobExplore()
        {
            var jobDTO = new List<JobDTO>();
            try
            {
                var jobRepository = _unitOfWork.JobRepository.GetAll().Where(i => i.IsActive == true && i.IsDeleted == false);
                var companyRepository = _unitOfWork.CompanyRepository.GetAll().Where(i => i.IsActive == true && i.IsDeleted == false);
                var jobTypeJobs = _unitOfWork.JobTypeJobRepository.GetAll().Where(i => i.IsActive == true && i.IsDeleted == false);
                var jobTypes = _unitOfWork.JobTypeRepository.GetAll().Where(i => i.IsActive == true && i.IsDeleted == false);
                var jobCategoryJob = _unitOfWork.JobCategoryJobRepository.GetAll().Where(i => i.IsActive == true && i.IsDeleted == false);
                var jobCategory = _unitOfWork.JobCategoryRepository.GetAll().Where(i => i.IsActive == true);
                var companyAddressList = _unitOfWork.CompanyAddressRepository.GetAll().Where(i => i.IsActive == true && i.IsDeleted == false);
                var countryList = _unitOfWork.CountryRepository.GetAll().Where(i => i.IsActive == true && i.IsDeleted == false);
                var stateList = _unitOfWork.StateRepository.GetAll().Where(i => i.IsDeleted == false && i.IsActive == true);
                var compLogos = _unitOfWork.CompanyLogoRepository.GetAll().Where(mod => mod.IsActive == true && mod.IsDeleted == false);

                jobDTO = (from jobRepo in jobRepository
                          join compRepo in companyRepository on jobRepo.CompanyId equals compRepo.Id
                          join jobTypeJ in jobTypeJobs on jobRepo.Id equals jobTypeJ.JobId
                          join jobType in jobTypes on jobTypeJ.JobTypeId equals jobType.Id
                          join jobCatj in jobCategoryJob on jobRepo.Id equals jobCatj.JobId
                          join jobCat in jobCategory on jobCatj.JobCategoryId equals jobCat.Id
                          join compAdd in companyAddressList on jobRepo.Location equals compAdd.Id
                          join country in countryList on compAdd.CountryId equals country.Id
                          join state in stateList on compAdd.StateId equals state.Id

                          join logo in compLogos on compRepo.Id equals logo.CompanyId into cLogo
                          from compLogo in cLogo.DefaultIfEmpty()

                          select new { jobRepo, compRepo, jobTypeJ, jobType, jobCatj, jobCat, compAdd, country, state, compLogo } into enty
                          group enty by enty.jobRepo.Id into entity
                          select new JobDTO
                          {
                              Id = entity.FirstOrDefault().jobRepo.Id,
                              CompanyId = entity.FirstOrDefault().jobRepo.CompanyId,
                              CompanyName = entity.FirstOrDefault().compRepo.CompanyName,
                              Title = entity.FirstOrDefault().jobRepo.Title,
                              Description = entity.FirstOrDefault().jobRepo.Description,
                              JobTypeName = string.Join(",", entity.Select(i => i.jobTypeJ.JobType.Name).Distinct()),
                              MinimumSalary = entity.FirstOrDefault().jobRepo.MinimumSalary,
                              MaximumSalary = entity.FirstOrDefault().jobRepo.MaximumSalary,
                              PayPeriodTypeId = entity.FirstOrDefault().jobRepo.PayPeriodTypeId,
                              CurrencyId = entity.FirstOrDefault().jobRepo.CurrencyId,
                              Positions = entity.FirstOrDefault().jobRepo.Positions,
                              LocationCity = entity.FirstOrDefault().compAdd.City,
                              LocationState = entity.FirstOrDefault().state.Name,
                              LocationCountry = entity.FirstOrDefault().country.Name,
                              ExpiredAt = entity.FirstOrDefault().jobRepo.ExpiredAt,
                              CreatedAt = entity.FirstOrDefault().jobRepo.CreatedAt,
                              UpdatedAt = entity.FirstOrDefault().jobRepo.UpdatedAt,
                              CreatedBy = entity.FirstOrDefault().jobRepo.CreatedBy,
                              UpdatedBy = entity.FirstOrDefault().jobRepo.UpdatedBy,
                              Responsibilities = entity.FirstOrDefault().jobRepo.Responsibility,
                              JobCategoryName = string.Join(",", entity.Select(i => i.jobCat.Name).Distinct()),
                              Img = entity.FirstOrDefault().compLogo?.Img ?? null
                          }).Distinct().ToList();

                return jobDTO;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        public List<JobDTO> GetSearchedJobExplore(CandidateJobExploreDTO jobExploreDTO)
        {
            var jobDTO = GetJobExplore();

            if (jobExploreDTO.OrderBy == CandidateFields.OrderByKeyWordASC)
            {
                jobDTO = jobDTO.OrderBy(i => i.Title).ToList();
            }
            else if (jobExploreDTO.OrderBy == CandidateFields.OrderByKeyWordDESC)
            {
                jobDTO = jobDTO.OrderByDescending(i => i.Title).ToList();
            }
            else if (jobExploreDTO.OrderBy == CandidateFields.OrderByCompanyASC)
            {
                jobDTO = jobDTO.OrderBy(i => i.CompanyName).ToList();
            }
            else if (jobExploreDTO.OrderBy == CandidateFields.OrderByCompanyDESC)
            {
                jobDTO = jobDTO.OrderByDescending(i => i.CompanyName).ToList();
            }
            else if (jobExploreDTO.OrderBy == CandidateFields.OrderByRecentASC)
            {
                jobDTO = jobDTO.OrderByDescending(i => i.Id).ToList();
            }

            else
            {
                jobDTO = jobDTO.OrderByDescending(i => i.Id).ToList();
            }

            if (!string.IsNullOrEmpty(jobExploreDTO.JobTitle))
            {
                jobDTO = jobDTO.Where(I => I.Title.ToLower().Contains(jobExploreDTO.Keywords.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(jobExploreDTO.Keywords))
            {
                jobDTO = jobDTO.Where(I => I.Title.ToLower().Contains(jobExploreDTO.Keywords.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(jobExploreDTO.JobType))
            {
                if (jobExploreDTO.JobType.Length > 0)
                {
                    jobDTO = jobDTO.Where(i => i.JobTypeName.Contains(jobExploreDTO.JobType)).ToList();
                }
            }
            if (jobExploreDTO.SelectedJobCategory != null)
            {
                string[] strJobCat = jobExploreDTO.SelectedJobCategory.ToArray();
                if (strJobCat.Length > 0)
                {
                    jobDTO = jobDTO.Where(i => strJobCat.Contains(i.JobCategoryName)).ToList();
                }
            }
            if (!string.IsNullOrEmpty(jobExploreDTO.Description))
            {
                jobDTO = jobDTO.Where(i => i.Description.ToLower().Contains(jobExploreDTO.Description.ToLower())).ToList();
            }
           
            if (!string.IsNullOrEmpty(jobExploreDTO.Company))
            {
                jobDTO = jobDTO.Where(i => i.CompanyName.ToLower().Contains(jobExploreDTO.Company.ToLower())).ToList();
            }

            return jobDTO;
        }

        public IQueryable<JobCategoryDTO> GetJobCategoriesByJobId(int jobId)
        {
            return (from jobcatj in _unitOfWork.JobCategoryJobRepository.Query(i => i.IsActive == true && i.IsDeleted == false)
                    join jobcat in _unitOfWork.JobCategoryRepository.Query(i => i.IsActive == true)
                    on jobcatj.JobCategoryId equals jobcat.Id
                    where jobcatj.JobId == jobId

                    select new JobCategoryDTO()
                    {
                        Id = jobcat.Id,
                        Name = jobcat.Name,
                        IsActive = jobcat.IsActive
                    });
        }

        public JobDTO GetJobDetailsById(int? id)
        {
            var jobDTO = new JobDTO();
            jobDTO = GetJobExplore().Where(i => i.Id == id).FirstOrDefault();

            var profileJobs = (from profileJob in _unitOfWork.ProfileJobRepository.Query(i => i.IsActive == true && i.IsDeleted == false)
                               where profileJob.JobId == id
                               select new ProfileJobDTO()
                               {
                                   Id = profileJob.Id,
                                   ProfileId = profileJob.ProfileId,
                                   JobId = profileJob.JobId,
                                   ActionId = profileJob.ActionId,
                                   UserId = profileJob.UserId,
                                   CreatedBy = profileJob.CreatedBy,
                                   UpdatedBy = profileJob.UpdatedBy,
                                   CreatedAt = profileJob.CreatedAt,
                                   UpdatedAt = profileJob.UpdatedAt,
                               }).ToList();

            jobDTO.ProfileJobs = profileJobs;
            return jobDTO;
        }
        #region Applying job by candidate
        public BaseModel SaveApplyJob(ProfileJobDTO model)
        {
            var response = new BaseModel();
            try
            {
                var profileDtl = _unitOfWork.ProfileRepository.Query(i => i.UserId == model.UserId).FirstOrDefault();
                int? profileId = (profileDtl != null ? profileDtl.Id : 0);
                model.ProfileId = profileId;
                var profileJob = _unitOfWork.ProfileJobRepository.Query(i => i.JobId == model.JobId && i.UserId == model.UserId).FirstOrDefault();

                int id = SaveProfileJob(model);
                var notificationslist = (from notification in _unitOfWork.NotificationTypeJobRepository.Query(x => x.IsActive == true && x.IsDeleted == false)
                                         where notification.JobId == model.JobId
                                         select notification).ToList();

                foreach (var notification in notificationslist)
                {
                    if (notification.NotificationTypeId == 1)
                    {
                        response.IsApp = true;
                    }
                    else if (notification.NotificationTypeId == 2)
                    {
                        response.IsEmailed = true;
                    }

                }
                if (id > 0)
                {
                    if (model.ActionId == 1)
                    {
                       
                        response.Success = true;
                        response.Message = CandidateResource.JobAppliedMsg;

                    }
                    else if (model.ActionId == 3)
                    {

                        response.Success = true;
                        response.Message = CandidateResource.JobSavedMsg;
                    }
                    else if (model.ActionId == 4)
                    {
                        response.Success = true;
                        response.Message = CandidateResource.NotInterestedSavedMsg;
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = CandidateResource.SomeError;
                }

                return response;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region Get profile job by candidate
        public List<JobDTO> GetProfileJob(int userId)
        {
            var jobDTO = new List<JobDTO>();
            try
            {
                var jobs = GetJobExplore();
                var profileJobList = _unitOfWork.ProfileJobRepository.Query(i => i.IsActive == true && i.IsDeleted == false);
                jobDTO = (from jobRepo in jobs
                          join profileJ in profileJobList on jobRepo.Id equals profileJ.JobId
                          where profileJ.UserId == userId
                          select new JobDTO()
                          {
                              Id = jobRepo.Id,
                              CompanyId = jobRepo.CompanyId,
                              CompanyName = jobRepo.CompanyName,
                              Title = jobRepo.Title,
                              Description = jobRepo.Description,
                              JobTypeName = jobRepo.JobTypeName,
                              MinimumSalary = jobRepo.MinimumSalary,
                              MaximumSalary = jobRepo.MaximumSalary,
                              PayPeriodTypeId = jobRepo.PayPeriodTypeId,
                              CurrencyId = jobRepo.CurrencyId,
                              Keywords = jobRepo.Keywords,
                              Positions = jobRepo.Positions,
                              LocationCity = jobRepo.LocationCity,
                              LocationState = jobRepo.LocationState,
                              LocationCountry = jobRepo.LocationCountry,
                              ExpiredAt = jobRepo.ExpiredAt,
                              CreatedAt = jobRepo.CreatedAt,
                              UpdatedAt = jobRepo.UpdatedAt,
                              CreatedBy = jobRepo.CreatedBy,
                              UpdatedBy = jobRepo.UpdatedBy,
                              Responsibilities = jobRepo.Responsibilities,
                              JobCategoryName = jobRepo.JobCategoryName,
                              ActionId = profileJ.ActionId,
                              Img= jobRepo.Img
                          }).ToList();

                return jobDTO;
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region Get Applied job
        public List<JobDTO> GetAppliedJobs(CandidateJobExploreDTO jobExploreDTO)
        {
            var profieJobs = GetProfileJob(jobExploreDTO.UserId);
            if (jobExploreDTO.OrderBy == CandidateFields.OrderByKeyWordASC)
            {
                profieJobs = profieJobs.OrderBy(i => i.Title).ToList();
            }
            if (jobExploreDTO.OrderBy == CandidateFields.OrderByKeyWordDESC)
            {
                profieJobs = profieJobs.OrderByDescending(i => i.Title).ToList();
            }
            else if (jobExploreDTO.OrderBy == CandidateFields.OrderByCompanyASC)
            {
                profieJobs = profieJobs.OrderBy(i => i.CompanyName).ToList();
            }
            else if (jobExploreDTO.OrderBy == CandidateFields.OrderByCompanyDESC)
            {
                profieJobs = profieJobs.OrderByDescending(i => i.CompanyName).ToList();
            }
            else if (jobExploreDTO.OrderBy == CandidateFields.OrderByRecentASC)
            {
                profieJobs = profieJobs.OrderByDescending(i => i.Id).ToList();
            }
            else
            {
                profieJobs = profieJobs.OrderByDescending(i => i.Id).ToList();
            }

            if (!string.IsNullOrEmpty(jobExploreDTO.JobTitle))
            {
                profieJobs = profieJobs.Where(I => I.Title.ToLower().Contains(jobExploreDTO.JobTitle.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(jobExploreDTO.Keywords))
            {
                profieJobs = profieJobs.Where(I => I.Keywords.ToLower().Contains(jobExploreDTO.Keywords.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(jobExploreDTO.JobType))
            {
                if (jobExploreDTO.JobType.Length > 0)
                {
                    profieJobs = profieJobs.Where(i => i.JobTypeName.Contains(jobExploreDTO.JobType)).ToList();
                }
            }

            if (!string.IsNullOrEmpty(jobExploreDTO.JobLocation))
            {
            }
            if (!string.IsNullOrEmpty(jobExploreDTO.Company))
            {
                profieJobs = profieJobs.Where(i => i.CompanyName.ToLower().Contains(jobExploreDTO.Company.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(jobExploreDTO.Description))
            {
                profieJobs = profieJobs.Where(i => i.Description.ToLower().Contains(jobExploreDTO.Description.ToLower())).ToList();
            }
            return profieJobs;
        }
        #endregion

        public int SaveProfileJob(ProfileJobDTO model)
        {
            try
            {
                var entity = _unitOfWork.ProfileJobRepository.Query(i => i.JobId == model.JobId && i.UserId == model.UserId).FirstOrDefault();
                if (entity != null)
                {
                    entity.ActionId = model.ActionId;
                    _unitOfWork.ProfileJobRepository.Update(entity);
                    _unitOfWork.Complete();
                }
                else
                {
                    entity = new ProfileJob()
                    {
                        Id = model.Id,
                        ProfileId = model.ProfileId,
                        JobId = model.JobId,
                        ActionId = model.ActionId,
                        UserId = model.UserId,
                        CreatedBy = model.UserId,
                        UpdatedBy = model.UserId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false
                    };
                    _unitOfWork.ProfileJobRepository.Add(entity);
                    _unitOfWork.Complete();
                }
                return entity.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public BaseModel SaveJobViewedByCandidate(ProfileJobDTO model)
        {
            var response = new BaseModel();
            try
            {
                var profileDtl = _unitOfWork.ProfileRepository.Query(i => i.UserId == model.UserId && i.IsDeleted == false && i.IsActive == true).FirstOrDefault();
                int? profileId = (profileDtl != null ? profileDtl.Id : 0);
                if (profileId != 0)
                {
                    model.ProfileId = profileId;
                    var profileJob = _unitOfWork.ProfileJobRepository.Query(i => i.IsActive == true && i.IsDeleted == false && i.JobId == model.JobId && i.UserId == model.UserId).FirstOrDefault();
                    if (profileJob != null)
                    {
                        if (model.ActionId == 1)
                        {
                            response.Success = false;
                            response.Message = CandidateResource.AlreadyAppliedJob;
                        }
                        else if (model.ActionId == 3)
                        {
                            response.Success = false;
                            response.Message = CandidateResource.AlreadySavedThisJob;
                        }
                        else if (model.ActionId == 4)
                        {
                            response.Success = false;
                            response.Message = CandidateResource.AlreadySavedNotInterested;
                        }
                    }
                    else
                    {
                        int id = SaveProfileJob(model);
                        if (id > 0)
                        {
                            if (model.ActionId == 1)
                            {
                                response.Success = true;
                                response.Message = CandidateResource.JobAppliedMsg;
                            }
                            else if (model.ActionId == 3)
                            {
                                response.Success = true;
                                response.Message = CandidateResource.JobSavedMsg;
                            }
                            else if (model.ActionId == 4)
                            {
                                response.Success = true;
                                response.Message = CandidateResource.NotInterestedSavedMsg;
                            }
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = CandidateResource.SomeError;
                        }
                    }

                }
                else
                {
                    response.Success = false;
                    response.Message = "~/Candidate/Profile";
                }
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<StateDTO> GetStateByCountryId(int? countryId = 0)
        {
            var stateList = new List<StateDTO>();
            var states = _unitOfWork.StateRepository.Query(i => i.IsActive == true && i.IsDeleted == false && i.CID == countryId);
            foreach (var state in states)
            {
                stateList.Add(new StateDTO()
                {
                    Id = state.Id,
                    Name = state.Name,
                    CID = state.CID
                });
            }
            return stateList;
        }

        public List<CountryDTO> GetCountries()
        {
            var countryDto = new List<CountryDTO>();
            var countryList = _unitOfWork.CountryRepository.Query(i => i.IsActive == true && i.IsDeleted == false);
            foreach (var country in countryList)
            {
                countryDto.Add(new CountryDTO()
                {
                    Id = country.Id,
                    Name = country.Name
                });
            }
            return countryDto;
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

        public string GetPhoneFormat(int countryId)
        {
            return _unitOfWork.PhoneFormatRepository.Query(i => i.CountryId == countryId).FirstOrDefault().Format;
        }
        public List<SortListedCandidateDTO> GetSortListedCandidate(int id)
        {
            var jobsFromCompany = new List<SortListedCandidateDTO>();
            var profile = _unitOfWork.ProfileRepository.Query(i => i.UserId == id).FirstOrDefault();
            if (profile != null)
            {
                var profileId = profile.Id;
                jobsFromCompany = (from shorListedCandidate in _unitOfWork.SortListedCandidateRepository.Query(i => i.IsActive == true && i.IsDeleted == false)
                                   join company in _unitOfWork.CompanyRepository.Query(i => i.IsActive == true && i.IsDeleted == false)
                                   on shorListedCandidate.CompanyId equals company.Id
                                   where shorListedCandidate.ProfileId == profileId
                                   select new SortListedCandidateDTO()
                                   {
                                       CompanyId = company.Id,
                                       CompanyName = company.CompanyName,
                                       JobId = shorListedCandidate.JobId,
                                       ProfileId = shorListedCandidate.ProfileId,
                                       SortListedDate = shorListedCandidate.SortListedDate

                                   }).ToList();
            }
            return jobsFromCompany;
        }
        /// <summary>
        /// Dashboard
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Dashboard Dashboard(int id)
        {
            Dashboard dashboard = new Dashboard();
            CandidateJobExploreDTO candidateJobExploreDTO = new CandidateJobExploreDTO();
            candidateJobExploreDTO.UserId = id;
            var jobs = GetAppliedJobs(candidateJobExploreDTO);
            dashboard.TotalAppliedJobs = jobs.Where(i => i.ActionId == 1).Count();
            dashboard.TotalSavedJobs = jobs.Where(i => i.ActionId == 3).Count();
            dashboard.TotalNotInterestedJobs = jobs.Where(i => i.ActionId == 4).Count();
            dashboard.TotalCallForInterviewJobs = GetSortListedCandidate(id).Count();
            dashboard.TotalRecommendedJobs = GetJobExplore().Count();
            return dashboard;
        }
        public List<CompanyProfileDTO> GetCompanyProfile(int id)
        {
            var companyProfiles = new List<CompanyProfileDTO>();
            var companies = _unitOfWork.CompanyRepository.Query(i => i.IsActive == true && i.IsDeleted == false);
            var companyAddresses = _unitOfWork.CompanyAddressRepository.Query(i => i.IsActive == true && i.IsDeleted == false);
            var EmployerCompanies = _unitOfWork.EmployerCompanyRepository.Query(i => i.IsActive == true && i.IsDeleted == false);
            var users = _unitOfWork.Users.Query(i => i.IsActive == true && i.IsDeleted == false);         
            var states = _unitOfWork.StateRepository.Query(i => i.IsActive == true && i.IsDeleted == false);
            var countries = _unitOfWork.CountryRepository.Query(i => i.IsDeleted == false && i.IsActive == true);
            var jobs = _unitOfWork.JobRepository.Query(i => i.IsActive == true && i.IsDeleted == false);
            var profiles = _unitOfWork.ProfileRepository.Query(i => i.IsActive == true && i.IsDeleted == false && i.UserId == id).FirstOrDefault();
            if(profiles!=null)
            {
                var shortlistedCandidates = _unitOfWork.SortListedCandidateRepository.Query(i => i.IsActive == true && i.IsDeleted == false && i.ProfileId == profiles.Id);

                companyProfiles = (from company in companies                                  
                                                          
                                   join candidate in shortlistedCandidates on company.Id equals candidate.CompanyId
                                   join job in jobs on candidate.JobId equals job.Id
                                   join compAddress in companyAddresses on job.Location equals compAddress.Id
                                   join state in states on compAddress.StateId equals state.Id
                                   join country in countries on compAddress.CountryId equals country.Id
                                   select new CompanyProfileDTO() {
                                       
                                       CompanyName =company.CompanyName,
                                       CompanyPhone=company.Phone,
                                       Address=compAddress.Address,
                                       City =compAddress.City,
                                       StateName =state.Name,
                                       CountryName=country.Name,
                                       Zip=compAddress.ZipCode

                                   }).Distinct().ToList();
            }

            return companyProfiles;
        }
        public Dashboard GetJobDetails()
        {
            Dashboard dashboardList= new Dashboard();
           List<JobDTO> listJobDTO = new List<JobDTO>();

            try
            {
                dashboardList.listJobDTO = GetJobExplore();
                return dashboardList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region(GET NOTIFICATION LIST BY CONDIDATE USER ID)

        public List<NotificationDTO> GetNotificationDetailsById(int? id)
        {
            var notificationDTO = new List<NotificationDTO>();


            notificationDTO = (from condidateNotification in _unitOfWork.NotificationRepository.Query(i => i.IsActive == true && i.IsDeleted == false)
                               join eventEntity in _unitOfWork.EntityRepository.Query(j => j.IsActive == true && j.IsDeleted == false)
                               on condidateNotification.EntityId equals eventEntity.Id
                               where condidateNotification.UserId == id
                               orderby condidateNotification.CreatedAt descending
                               select new NotificationDTO()
                               {
                                   Id = condidateNotification.Id,
                                   Title = condidateNotification.Title,
                                   EventEntity = condidateNotification.Entity,
                                   EventOn = condidateNotification.CreatedAt,
                                   status = condidateNotification.Status,
                                   TimeFlag = DateTime.Now
                               }).ToList();

            return notificationDTO;
        }


        #endregion

        #region(GET SHORT NOTIFICATION LIST BY CONDIDATE USER ID)

        public List<NotificationDTO> GetShortNotificationDetailsById(int? id)
        {
            var notificationDTO = new List<NotificationDTO>();


            notificationDTO = (from condidateNotification in _unitOfWork.NotificationRepository.Query(i => i.IsActive == true && i.IsDeleted == false && i.IsRead == false)
                               join eventEntity in _unitOfWork.EntityRepository.Query(j => j.IsActive == true && j.IsDeleted == false)
                               on condidateNotification.EntityId equals eventEntity.Id
                               where condidateNotification.UserId == id
                               orderby condidateNotification.CreatedAt descending
                               select new NotificationDTO()
                               {
                                   Id = condidateNotification.Id,
                                   Title = condidateNotification.Title,
                                   EventEntity = condidateNotification.Entity,
                                   EventOn = condidateNotification.CreatedAt,
                                   status = condidateNotification.Status,
                                   TimeFlag = DateTime.Now,
                                   IsRead = condidateNotification.IsRead,

                               }).Take(10).ToList();

            return notificationDTO;
        }


        #endregion

        #region(UPDATE NOTIFICATION BY ID WHEN USER HAS BEEN READ NOTIFICATION)

        public bool ReadNotificationById(int id)
        {
            try
            {
                Notification notificationById = _unitOfWork.NotificationRepository.Get(id);
                if (id > 0)
                {
                    notificationById.IsRead = true;
                    _unitOfWork.NotificationRepository.Update(notificationById);
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
        #endregion

        public CandidateBasicDTO DownloadResume(int id)
        {
            var profile = _unitOfWork.ProfileRepository.Query(i => i.UserId == id).FirstOrDefault();
            return new CandidateBasicDTO()
            {
                OrgFileName = profile.OrgFileName,
                ResumeFilePath = profile.ResumeFile
            };
        }
        public EnableProfileDTO EnableProfileOption(int id)
        {
            EnableProfileDTO enableProfileDTO = new EnableProfileDTO();
            enableProfileDTO.IsGeneralActive = false;
            enableProfileDTO.IsContactActive = false;
            enableProfileDTO.IsPreferenceActive = false;
            enableProfileDTO.IsPreviewActive = false;
            enableProfileDTO.IsEduActive = false;
            enableProfileDTO.IsWorkExpActive = false;

            var basic = GetBasicDetails(id);
            if (basic.Id != 0)
            {
                enableProfileDTO.IsGeneralActive = true;
            }

            var contacts = GetContactDetails(id);
            if (contacts.Id != 0)
            {
                enableProfileDTO.IsContactActive = true;
            }

            var preferences = GetPreferencesDetails(id);
            if (preferences.SelectedJobTypes!=null && preferences.SelectedJobTypes.Count > 0)
            {
                enableProfileDTO.IsPreferenceActive = true;
            }

            var workExp = GetWorkDetails(id);
            if (workExp.Count > 0)
            {
                enableProfileDTO.IsWorkExpActive = true;
            }
            var eduDetails = GetEducationDetails(id);
            if (eduDetails.Count > 0)
            {
                enableProfileDTO.IsEduActive = true;
            }
            if ((basic.Id != 0) && (contacts.Id != 0) && (preferences.SelectedJobTypes != null && preferences.SelectedJobTypes.Count > 0) && (workExp.Count > 0) && (eduDetails.Count > 0))
            {
                enableProfileDTO.IsPreviewActive = true;
            }
            return enableProfileDTO;
        }
    }
}
