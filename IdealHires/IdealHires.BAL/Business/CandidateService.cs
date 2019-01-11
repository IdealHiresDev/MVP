using IdealHires.BAL.DataContext;
using IdealHires.Data;
using IdealHires.DTO.Candidate;
using System;
using System.Collections.Generic;
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

        public int InsertBasicDetails(CandidateBasicDTO basicCandidate)
        {
            int profileId = 0;
            try
            {
                var userData = _unitOfWork.Users.Get(basicCandidate.UserId);
                if (userData != null)
                {
                    userData.FirstName = basicCandidate.FirstName;
                    userData.LastName = basicCandidate.LastName;
                    userData.UpdatedAt = DateTime.Now;
                    userData.UpdatedBy = basicCandidate.UserId;
                    _unitOfWork.Users.Update(userData);
                    //_unitOfWork.Complete();

                    var profile = new Profile()
                    {
                        UserId = basicCandidate.UserId,
                        ResumeFile = basicCandidate.ResumeFile,
                        JobTitle = basicCandidate.JobTitle,
                        CreatedAt=DateTime.Now,
                        CreatedBy= basicCandidate.UserId
                    };
                    _unitOfWork.ProfileRepository.Add(profile);
                    _unitOfWork.Complete();
                    profileId = profile.Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return profileId;
        }

        #endregion

        #region Candidate Contact

        public int InsertContactDetails(CandidateContactDTO basicCandidate)
        {
            int addressId = 0;
            try
            {
                var userProfile = _unitOfWork.ProfileRepository.GetFirstOrDefault(p => p.UserId == basicCandidate.UserId);
                if (userProfile != null)
                {
                    var address = new Address()
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return addressId;
        }

        #endregion

        #region Candidate Work

        public int InsertWorkDetails(CandidateWorkDTO workCandidate)
        {
            int workId = 0;
            try
            {
                if (workCandidate != null)
                {
                    var work = new Work()
                    {
                        ProfileId = workCandidate.ProfileId,
                        CompanyName = workCandidate.CompanyName,
                        PositionName = workCandidate.PositionName,
                        Description = workCandidate.Description,
                        Salary = workCandidate.Salary,
                        CurrencyId = workCandidate.CurrencyId,
                        StartAt = workCandidate.StartAt,
                        EndAt = workCandidate.EndAt,                        
                        CreatedAt = DateTime.Now,
                        CreatedBy = workCandidate.UserId
                    };
                    _unitOfWork.WorkRepository.Add(work);
                    _unitOfWork.Complete();
                    workId = work.Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return workId;
        }

        #endregion

        #region Candidate Education

        public int InsertEducationDetails(CandidateEducationDTO educationCandidate)
        {
            int educationId = 0;
            try
            {
                if (educationCandidate != null)
                {
                    var academic = new Academic()
                    {
                        ProfileId = educationCandidate.ProfileId,
                        Major = educationCandidate.Major,
                        Minor = educationCandidate.Minor,
                        InstituteName = educationCandidate.InstituteName,
                        StartAt = educationCandidate.StartAt,
                        EndAt = educationCandidate.EndAt,
                        IsDegreeOrCertification = educationCandidate.IsDegreeOrCertification,
                        CreatedAt = DateTime.Now,
                        CreatedBy = educationCandidate.UserId
                    };
                    _unitOfWork.AcademicRepository.Add(academic);
                    _unitOfWork.Complete();
                    educationId = academic.Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return educationId;
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
