
using IdealHires.BAL.DataContext;
using IdealHires.Data;
using IdealHires.DTO;
using IdealHires.DTO.Candidate;
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
        #endregion

        #region Constructor
        public CandidateService()
        {
            _unitOfWork = new UnitOfWork(new IdealHiresDbContext());
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
                            JobTitle = basicCandidate.JobTitle,
                            CreatedAt = DateTime.Now,
                            CreatedBy = basicCandidate.UserId
                        };
                        _unitOfWork.ProfileRepository.Add(profile);
                        _unitOfWork.Complete();
                        profileId = profile.Id;
                    }
                    else
                    {
                        userProfile.UserId = basicCandidate.UserId;
                        userProfile.ResumeFile = (string.IsNullOrEmpty(basicCandidate.ResumeFilePath)) ? userProfile.ResumeFile : basicCandidate.ResumeFilePath;
                        userProfile.JobTitle = basicCandidate.JobTitle;
                        userProfile.UpdatedAt = DateTime.Now;
                        userProfile.UpdatedBy = basicCandidate.UserId;

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
            CandidateContactDTO contactDetailDTO = new CandidateContactDTO();
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
                            State = address.State,
                            Country = address.Country,
                            ZipCode = address.ZipCode,
                            Phone1 = address.Phone1,
                            Phone2 = address.Phone2
                        };
                    }
                }
                return contactDetailDTO;
            }
            catch (Exception)
            {
                throw;
            }
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
                            State = basicCandidate.State,
                            Country = basicCandidate.Country,
                            ZipCode = basicCandidate.ZipCode,
                            Phone1 = basicCandidate.Phone1,
                            Phone2 = basicCandidate.Phone2,
                            CreatedAt = DateTime.Now,
                            CreatedBy = basicCandidate.UserId
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
                        addresses.State = basicCandidate.State;
                        addresses.Country = basicCandidate.Country;
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
                        //StartAt = String.Format("{0:MM-dd-yyyy}", workDetailsByworkId.StartAt),
                        //EndAt = String.Format("{0:MM-dd-yyyy}", workDetailsByworkId.EndAt),
                        StartAt = workDetailsByworkId.StartAt.HasValue ? workDetailsByworkId.StartAt.Value : (DateTime?)null,
                        EndAt = workDetailsByworkId.EndAt.HasValue ? workDetailsByworkId.EndAt.Value : (DateTime?)null,
                        IsCurrent = (workDetailsByworkId.EndAt == null) ? true : false,
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
                                TotalExperience = new DateDifference(w.StartAt ?? DateTime.Now, w.EndAt ?? DateTime.Now).ToString()
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
                            StartAt = workCandidate.StartAt.HasValue ? workCandidate.StartAt.Value : (DateTime?)null,//Convert.ToDateTime(workCandidate.StartAt),
                            EndAt = workCandidate.EndAt.HasValue ? workCandidate.EndAt.Value : (DateTime?)null,// (string.IsNullOrEmpty(workCandidate.EndAt)) ? (DateTime?)null : Convert.ToDateTime(workCandidate.EndAt),
                            CreatedAt = DateTime.Now,
                            CreatedBy = workCandidate.UserId
                        };
                        _unitOfWork.WorkRepository.Add(work);
                        _unitOfWork.Complete();
                        workId = work.Id;
                    }
                }
            }
            catch (Exception)
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
