using IdealHires.BAL.DataContext;
using IdealHires.Data;
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
                            companyDetails.Location = companyCandidate.Email;
                            companyDetails.Website = companyCandidate.Website;
                            companyDetails.Description = companyCandidate.Description;
                            companyDetails.UpdatedAt = DateTime.Now;
                            companyDetails.UpdatedBy = companyCandidate.UserId;

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
                                Location = companyCandidate.Email,
                                Website = companyCandidate.Website,
                                Description = companyCandidate.Description,
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
            catch (Exception ex)
            {
                throw ex;
            }
            return companyId;
        }

        public CompanyDTO GetCompanyDetails(int id)
        {
            CompanyDTO companyDTO = new CompanyDTO();
            try
            {
                User userData = _unitOfWork.Users.Get(id);
                Company company = null;
                if (company.Id > 0 && userData.Id > 0)
                {
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
                    };
                }
                return companyDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
