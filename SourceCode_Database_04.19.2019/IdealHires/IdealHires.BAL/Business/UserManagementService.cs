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
    public class UserManagementService
    {
        #region Private Member
        bool disposed = false;
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructor
        public UserManagementService()
        {
            _unitOfWork = new UnitOfWork(new IdealHiresDbContext());
        }
        #endregion

        #region User Management
        public List<EUserListDTO> GetUserManagementDetails(int id)
        {
            List<EUserListDTO> employerUserList = new List<EUserListDTO>();
            try
            {
                var userCompanyId = _unitOfWork.EmployerCompanyRepository.Get(ec => ec.UserId == id).FirstOrDefault().CompanyId;
                if (userCompanyId > 0)
                {
                    var companyUsers = _unitOfWork.EmployerCompanyRepository.Get(ec => ec.CompanyId == userCompanyId).ToList();
                    var company = _unitOfWork.CompanyRepository.Get(c => c.Id == userCompanyId && c.IsActive == true).FirstOrDefault();
                    if (companyUsers != null)
                    {
                        var userManagementDTOlist = _unitOfWork.Users.GetAll().Where(u => companyUsers.Any(cu => cu.UserId == u.Id) && u.IsActive == true).Select(
                             u => new
                             {
                                 FName = u.FirstName,
                                 LName = u.LastName,
                                 Email = u.EmailId,
                                 UserId = u.Id,
                                 CompanyId = userCompanyId
                             }).ToList();

                        var employerAddresses = _unitOfWork.AddressEmployerRepository.GetAll().Where(u => companyUsers.Any(cu => cu.UserId == u.EUserId) && u.IsActive == true).Select(

                            u => new
                            {

                                Phone = u.Phone1,
                                PhoneNumber = u.Phone2,
                                UserId = u.EUserId
                            }).ToList();
                     
                        employerUserList = (from eUsers in userManagementDTOlist
                                            join ec in employerAddresses on eUsers.UserId equals ec.UserId  into commonUserData
                                           
                                            from finalUserData in commonUserData.DefaultIfEmpty()
                                            select new EUserListDTO()
                                            {
                                                CompanyId = eUsers.CompanyId,
                                                EUserId = eUsers.UserId,
                                                // Name = eUsers?.FName ?? string.Empty + " " + eUsers.LName,
                                                Name = eUsers?.FName + string.Empty + " " + eUsers.LName,
                                                Phone1 = finalUserData?.Phone ?? finalUserData?.PhoneNumber ?? company?.Phone1,
                                               

                                                Email = eUsers.Email,
                                                IsActive=true
                                            }).ToList();
                       
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return employerUserList;
        }

        public EUserDTO GetEmployerUserDetails(int id, int companyId)
        {
            EUserDTO employerUser = new EUserDTO();
            List<UserRole> userRoleDTO = new List<UserRole>();


            List<string> selectedUserRole = new List<string>();
            try
            {
                var user = _unitOfWork.Users.Get(id);
                if (user != null)
                {
                    var addressEmployer = _unitOfWork.AddressEmployerRepository.Get(ae => ae.EUserId == id).FirstOrDefault();
                    var companyDetails = _unitOfWork.CompanyRepository.GetFirstOrDefault(cp=>cp.Id==companyId);
                    UserRole userRole = _unitOfWork.UserRoleRepository.Get().Where(ur => ur.UserId == user.Id).FirstOrDefault();
                    if (addressEmployer != null)
                    {
                        employerUser.Id = user.Id;
                        employerUser.CompanyId = companyId;
                        employerUser.FirstName = user.FirstName;
                        employerUser.LastName = user.LastName;
                        employerUser.Email = user.EmailId;
                        employerUser.Phone1 = addressEmployer.Phone1;
                        employerUser.Phone2 = addressEmployer.Phone2;
                        employerUser.Address1 = addressEmployer.StreetAddress1;
                        employerUser.Address2 = addressEmployer.StreetAddress2;
                        employerUser.City = addressEmployer.City;
                        employerUser.State = addressEmployer.State;
                        employerUser.Country = addressEmployer.Country;
                        employerUser.ZipCode = Convert.ToString(addressEmployer.ZipCode);
                        employerUser.Description = companyDetails.Description;
                    }
                    else if(userRole != null)
                    {
                        employerUser.Id = user.Id;
                        employerUser.CompanyId = companyId;
                        employerUser.FirstName = user.FirstName;
                        employerUser.LastName = user.LastName;
                        employerUser.Email = user.EmailId;
                        employerUser.Phone1 = addressEmployer.Phone1;
                        employerUser.Phone2 = addressEmployer.Phone2;
                        employerUser.Address1 = addressEmployer.StreetAddress1;
                        employerUser.Address2 = addressEmployer.StreetAddress2;
                        employerUser.City = addressEmployer.City;
                        employerUser.Role = Convert.ToInt32(userRole.RoleId);
                        employerUser.ZipCode = Convert.ToString(addressEmployer.ZipCode);
                        employerUser.Description = companyDetails.Description;

                    }
                    
                    else
                    {
                        employerUser.Id = user.Id;
                        employerUser.CompanyId = companyId;
                        employerUser.FirstName = user.FirstName;
                        employerUser.LastName = user.LastName;
                        employerUser.Email = user.EmailId;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return employerUser;
        }

        public int InsertEmployerUser(EUserDTO eUserDTO)
        {
            int userId = 0;
            try
            {

                if (eUserDTO.Id == 0)
                {
                    var userDetails = _unitOfWork.Users.GetFirstOrDefault(u => u.EmailId == eUserDTO.Email && u.IsActive == true);
                    if (userDetails != null)
                    {
                        userDetails.FirstName = eUserDTO.FirstName;
                        userDetails.LastName = eUserDTO.LastName;
                        userDetails.IsEmailConfirm = true;
                        _unitOfWork.Users.Update(userDetails);
                        _unitOfWork.Complete();
                        userId = userDetails.Id;
                        if (userId > 0)
                        {
                            EmployerCompany employerCompany = _unitOfWork.EmployerCompanyRepository.Get(ec => ec.UserId == eUserDTO.EUserId).FirstOrDefault();

                            if (employerCompany != null)
                            {
                                EmployerCompany employerUsers = new EmployerCompany
                                {
                                    UserId = userId,
                                    CompanyId = employerCompany.CompanyId,
                                    IsActive=true,
                                    IsDeleted=false
                                    
                                };
                                _unitOfWork.EmployerCompanyRepository.Add(employerUsers);
                                _unitOfWork.Complete();
                            }
                            AddressEmployer address = new AddressEmployer
                            {
                                EUserId = userId,
                                StreetAddress1 = string.IsNullOrEmpty(eUserDTO.Address1) ? string.Empty : eUserDTO.Address1,
                                StreetAddress2 = string.IsNullOrEmpty(eUserDTO.Address2) ? string.Empty : eUserDTO.Address2,
                                Phone1 = Convert.ToString(eUserDTO.Phone1),
                                Phone2 = Convert.ToString(eUserDTO.Phone2),
                                City = string.IsNullOrEmpty(eUserDTO.City) ? string.Empty : eUserDTO.City,
                                State = Convert.ToString(eUserDTO.StateId),
                                Country = Convert.ToString(eUserDTO.CountryId),
                                ZipCode = Convert.ToString(eUserDTO.ZipCode),
                                IsActive = true,
                                IsDeleted = false,
                                CreatedAt = DateTime.Now,
                                CreatedBy = eUserDTO.EUserId
                            };

                            _unitOfWork.AddressEmployerRepository.Add(address);
                            _unitOfWork.Complete();
                            if (eUserDTO.Role > 0)
                            {
                                var userRoleData = _unitOfWork.UserRoleRepository.Get(r => r.RoleId == eUserDTO.Role && r.UserId == userId).FirstOrDefault();
                                if (userRoleData == null)
                                {
                                    var userrole = new UserRole()
                                    {
                                        UserId = userId,
                                        RoleId = eUserDTO.Role,
                                        CreatedAt = DateTime.Now,
                                        CreatedBy = eUserDTO.EUserId,
                                        IsActive = true,
                                        IsDeleted = false
                                    };
                                    _unitOfWork.UserRoleRepository.Add(userrole);
                                    _unitOfWork.Complete();
                                }
                            }
                        }
                    }
                }
                else
                {
                    User user = _unitOfWork.Users.Get(eUserDTO.Id);
                    if (user != null)
                    {
                        user.FirstName = eUserDTO.FirstName;
                        user.LastName = eUserDTO.LastName;
                        user.EmailId = eUserDTO.Email;
                        user.UpdatedAt = DateTime.Now;
                        user.UpdatedBy = eUserDTO.EUserId;

                        _unitOfWork.Users.Update(user);
                        _unitOfWork.Complete();
                        userId = user.Id;
                        if (userId > 0)
                        {
                            EmployerCompany employerCompany = _unitOfWork.EmployerCompanyRepository.Get(ec => ec.UserId == eUserDTO.Id).FirstOrDefault();
                            if (employerCompany == null)
                            {
                                EmployerCompany employerUsers = new EmployerCompany
                                {
                                    UserId = userId,
                                    CompanyId = employerCompany.CompanyId,
                                    IsActive = true,
                                    IsDeleted = false
                                };
                                _unitOfWork.EmployerCompanyRepository.Add(employerUsers);
                                _unitOfWork.Complete();
                            }
                            AddressEmployer address = _unitOfWork.AddressEmployerRepository.Get(ae => ae.EUserId == userId).FirstOrDefault();
                            if (address != null)
                            {
                                address.EUserId = userId;
                                address.StreetAddress1 = string.IsNullOrEmpty(eUserDTO.Address1) ? string.Empty : eUserDTO.Address1;
                                address.StreetAddress2 = string.IsNullOrEmpty(eUserDTO.Address2) ? string.Empty : eUserDTO.Address2;
                                address.Phone1 = Convert.ToString(eUserDTO.Phone1);
                                address.Phone2 = Convert.ToString(eUserDTO.Phone2);
                                address.City = string.IsNullOrEmpty(eUserDTO.City) ? string.Empty : eUserDTO.City;
                                address.State = Convert.ToString(eUserDTO.StateId);
                                address.Country = Convert.ToString(eUserDTO.CountryId);
                                address.ZipCode = Convert.ToString(eUserDTO.ZipCode);
                                address.UpdatedAt = DateTime.Now;
                                address.UpdatedBy = eUserDTO.EUserId;

                                _unitOfWork.AddressEmployerRepository.Update(address);
                                _unitOfWork.Complete();
                            }
                            else
                            {
                                AddressEmployer addressNew = new AddressEmployer
                                {
                                    EUserId = userId,
                                    StreetAddress1 = string.IsNullOrEmpty(eUserDTO.Address1) ? string.Empty : eUserDTO.Address1,
                                    StreetAddress2 = string.IsNullOrEmpty(eUserDTO.Address2) ? string.Empty : eUserDTO.Address2,
                                    Phone1 = Convert.ToString(eUserDTO.Phone1),
                                    Phone2 = Convert.ToString(eUserDTO.Phone2),
                                    City = string.IsNullOrEmpty(eUserDTO.City) ? string.Empty : eUserDTO.City,
                                    State = Convert.ToString(eUserDTO.StateId),
                                    Country = Convert.ToString(eUserDTO.CountryId),
                                ZipCode = Convert.ToString(eUserDTO.ZipCode),
                                    IsActive = true,
                                    IsDeleted = false,
                                    CreatedAt = DateTime.Now,
                                    CreatedBy = eUserDTO.EUserId
                                };
                                _unitOfWork.AddressEmployerRepository.Add(addressNew);
                                _unitOfWork.Complete();
                            }
                            if (eUserDTO.Role > 0)
                            {
                                var userRoleData = _unitOfWork.UserRoleRepository.Get(r => r.RoleId == eUserDTO.Role && r.UserId == userId).FirstOrDefault();
                                if (userRoleData == null)
                                {
                                    var userrole = new UserRole()
                                    {
                                        UserId = userId,
                                        RoleId = eUserDTO.Role,
                                        CreatedAt = DateTime.Now,
                                        CreatedBy = eUserDTO.EUserId,
                                        IsActive = true,
                                        IsDeleted = false
                                    };
                                    _unitOfWork.UserRoleRepository.Add(userrole);
                                    _unitOfWork.Complete();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return userId;
        }
        public List<RoleDTO> GetRole(int id)
        {
           
            List<RoleDTO> roleDTOList = new List<RoleDTO>();
            try
            {
                User user = _unitOfWork.Users.Get(id);
                if (user.UserType != null)
                {
                    List<Role> roleList = _unitOfWork.Roles.Query().Where(e=>e.UserType== user.UserType).ToList();
                    foreach (var role in roleList)
                    {
                        RoleDTO roleDTO = new RoleDTO()
                        {
                            Id = role.Id,
                            RoleName = role.RoleName
                        };
                        roleDTOList.Add(roleDTO);
                    }

                }
                return roleDTOList;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public bool DeleteEmployerUserDetails(int id, int companyId)
        {
            bool isDeleted = false;
            try
            {
                var user = _unitOfWork.Users.Get(id);
                if (user != null)
                {
                    user.IsActive = false;
                    _unitOfWork.Users.Update(user);
                    _unitOfWork.Complete();

                    var addressEmployer = _unitOfWork.AddressEmployerRepository.Get(ae => ae.EUserId == id).FirstOrDefault();
                    if (addressEmployer != null)
                    {
                        addressEmployer.IsActive = false;
                        addressEmployer.IsDeleted = false;
                        _unitOfWork.AddressEmployerRepository.Update(addressEmployer);
                        _unitOfWork.Complete();

                        isDeleted = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return isDeleted;
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

        private List<UserRole> MapUserRoleData(EUserDTO eUserDTO,int userId)
        {
            List<UserRole> userroleList = new List<UserRole>();
            for (int i = 0; i < eUserDTO.SelectedRole.Count; i++)
            {
                var userroleConvert = new UserRole()
                {
                    UserId = userId,
                    RoleId = int.Parse(eUserDTO.SelectedRole[i]),
                    CreatedAt = DateTime.Now,
                    CreatedBy = eUserDTO.EUserId,
                    IsActive = true,
                    IsDeleted = false
                };
                userroleList.Add(userroleConvert);
            }
            return userroleList;
        }
    }
}
