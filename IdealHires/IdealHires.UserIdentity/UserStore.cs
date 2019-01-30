using IdealHires.BAL.Business;
using IdealHires.DTO;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.UserIdentity
{
    public class UserStore : IUserStore<UserDTO, int>, IUserLoginStore<UserDTO, int>,
        IUserRoleStore<UserDTO, int>, IUserPasswordStore<UserDTO, int>,
        IUserSecurityStampStore<UserDTO, int>, IUserEmailStore<UserDTO, int>,
        IUserLockoutStore<UserDTO, int>, IUserTwoFactorStore<UserDTO, int>,
        IClaimsIdentityFactory<UserDTO, int>
    {
        private readonly AuthApi _userService;

        public UserStore() //AppUserInfo appUserInfo
        {
            _userService = new AuthApi();
        }

        #region IUserStore
        public Task CreateAsync(UserDTO user)
        {
            try
            {
                var userEntity = new UserDTO()
                {

                    UserName = user.EmailId,
                    EmailId = user.EmailId,
                    Password = user.Password,
                    IsActive = user.IsActive,
                    SecurityStamp = user.SecurityStamp,
                    IsEmailConfirm = false,
                    UserType= user.UserType,
                    CreatedAt = DateTime.Now                    
                };

                _userService.CreateUser(userEntity);
            }
            catch (Exception)
            {
                throw;
            }
            return Task.FromResult(0);
        }
        public Task DeleteAsync(UserDTO user)
        {
            //User should not delete itself
            return Task.FromResult(0);
        }

        public Task<UserDTO> FindByIdAsync(int userId)
        {
            if (userId == 0)
            {
                return Task.FromResult(default(UserDTO));
            }

            var id = userId;
            var userRequest = _userService.GetUser(id);
            return Task.FromResult(userRequest != null ? MappUser(userRequest) : default(UserDTO));

        }

        public Task<UserDTO> FindByNameAsync(string userName)
        {
            var userRequest = _userService.GetUser(userName);
            return Task.FromResult(userRequest != null ? MappUser(userRequest) : default(UserDTO));
        }

        public Task UpdateAsync(UserDTO user)
        {
            //Update database
            return Task.FromResult(0);
        }

        public void Dispose()
        {
        }
        #endregion

        //NOT REQUIRED AT THIS TIME
        #region IUserLoginStore
        public Task AddLoginAsync(UserDTO user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(UserDTO user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IUserRoleStore
        public Task AddToRoleAsync(UserDTO user, string roleName)
        {            
            return Task.FromResult(0);
        }

        public Task<IList<string>> GetRolesAsync(UserDTO user)
        {
            List<string> list = new List<string>();
            list.Add("Admin");
            list.Add("Candidate");
            list.Add("Employer");
            return Task.FromResult((IList<string>)list);
        }

        public Task<bool> IsInRoleAsync(UserDTO user, string roleName)
        {
            var roleExists = false;// user.Roles.Any(r => r.Equals(roleName, StringComparison.CurrentCultureIgnoreCase));
            return Task.FromResult(roleExists);
        }

        public Task RemoveFromRoleAsync(UserDTO user, string roleName)
        {           
            return Task.FromResult(0);
        }
        #endregion

        #region IUserPasswordStore
        public Task<string> GetPasswordHashAsync(UserDTO user)
        {
            if (user.Id == 0) return Task.FromResult(user.Password);

            var id = int.Parse(user.Id.ToString());
            var passwordHash = _userService.GetUserPasswordHash(id);
            return Task.FromResult(passwordHash);

        }

        public Task<bool> HasPasswordAsync(UserDTO user)
        {
            if (user.Id == 0) return Task.FromResult(!string.IsNullOrWhiteSpace(user.Password));

            var id = int.Parse(user.Id.ToString());
            var passwordHash = _userService.GetUserPasswordHash(id);
            return Task.FromResult(!string.IsNullOrWhiteSpace(passwordHash));

        }

        public Task SetPasswordHashAsync(UserDTO user, string passwordHash)
        {
            if (user.Id > 0)
            {
                var id = int.Parse(user.Id.ToString());
                _userService.SetUserPasswordHash(user.UserName, passwordHash);
            }
            user.Password = passwordHash;
            return Task.FromResult(0);
        }
        #endregion

        #region IUserSecurityStampStore
        public Task<string> GetSecurityStampAsync(UserDTO user)
        {
            if (user.Id > 0) return Task.FromResult(user.SecurityStamp);

            var id = int.Parse(user.Id.ToString());
            var securityStamp = _userService.GetUserSecurityStamp(id);
            return Task.FromResult(securityStamp);
        }


        public Task SetSecurityStampAsync(UserDTO user, string stamp)
        {
            if (user.Id > 0)
            {
                var id = int.Parse(user.Id.ToString());
                _userService.SetUserSecurityStamp(id, stamp);
            }

            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }
        #endregion

        #region IUserEmailStore
        public Task<UserDTO> FindByEmailAsync(string email)
        {
            var userRequest = _userService.GetUser(email);
            return Task.FromResult(userRequest != null ? MappUser(userRequest) : default(UserDTO));

        }

        public Task<string> GetEmailAsync(UserDTO user)
        {
            if (user.Id == 0) return Task.FromResult(user.UserName);

            var id = int.Parse(user.Id.ToString());
            var email = _userService.GetUserEmail(id);
            return Task.FromResult(email);

        }

        public Task<bool> GetEmailConfirmedAsync(UserDTO user)
        {
            if (user.Id == 0) return Task.FromResult(user.IsEmailConfirm);
            var id = int.Parse(user.Id.ToString());
            var emailVerified = _userService.GetUserEmailVerified(id);
            return Task.FromResult(emailVerified);
        }

        public Task SetEmailAsync(UserDTO user, string email)
        {
            if (user.Id > 0)
            {
                var id = int.Parse(user.Id.ToString());
                _userService.SetUserEmail(id, email);
            }

            user.UserName = email;
            return Task.FromResult(0);
        }

        public Task SetEmailConfirmedAsync(UserDTO user, bool confirmed)
        {
            if (user.Id > 0)
            {
                var id = int.Parse(user.Id.ToString());
                _userService.SetUserEmailVerified(id, confirmed);
            }

            user.IsEmailConfirm = confirmed;
            return Task.FromResult(0);
        }
        #endregion

        //NOT REQUIRED AT THIS TIME
        #region IUserLockoutStore
        public Task<int> GetAccessFailedCountAsync(UserDTO user)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(UserDTO user)
        {
            return Task.FromResult(false);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEnabledAsync(UserDTO user, bool enabled)
        {
            throw new NotImplementedException();
            //return Task.FromResult(0);
        }

        public Task SetLockoutEndDateAsync(UserDTO user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }
        #endregion

        //NOT REQUIRED AT THIS TIME
        #region IUserTwoFactorStore
        public Task<bool> GetTwoFactorEnabledAsync(UserDTO user)
        {
            return Task.FromResult(false);
        }

        public Task SetTwoFactorEnabledAsync(UserDTO user, bool enabled)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IClaimsIdentityFactory
        public Task<ClaimsIdentity> CreateAsync(UserManager<UserDTO, int> manager, UserDTO user, string authenticationType)
        {
            var claimIdentity = new ClaimsIdentity(authenticationType, ClaimTypes.Name, ClaimTypes.Role);          
           
            claimIdentity.AddClaim(new Claim(CustomClaims.Firstname, user.FirstName, ClaimValueTypes.String));
            claimIdentity.AddClaim(new Claim(CustomClaims.Firstname, user.LastName, ClaimValueTypes.String));

            return Task.FromResult(claimIdentity);
        }
        #endregion

        #region IUserPhoneNumberStore
        //public Task<string> GetPhoneNumberAsync(User user)
        //{
        //    if (string.IsNullOrWhiteSpace(user.Id)) return Task.FromResult(user.Phone);

        //    var id = int.Parse(user.Id);
        //    var userPhoneNumber = _userService.GetUserPhone(id);
        //    return Task.FromResult(userPhoneNumber);
        //}

        //public Task<bool> GetPhoneNumberConfirmedAsync(User user)
        //{
        //    if (string.IsNullOrWhiteSpace(user.Id)) return Task.FromResult(user.PhoneVerified);

        //    var id = int.Parse(user.Id);
        //    var phoneVerified = _userService.GetUserPhoneVerified(id);
        //    return Task.FromResult(phoneVerified);
        //}

        //public Task SetPhoneNumberAsync(User user, string phoneNumber)
        //{
        //    if (!string.IsNullOrWhiteSpace(user.Id))
        //    {
        //        var id = int.Parse(user.Id);
        //        _userService.SetUserPhone(id, phoneNumber);
        //    }

        //    user.Phone = phoneNumber;
        //    return Task.FromResult(0);
        //}

        //public Task SetPhoneNumberConfirmedAsync(MyUser user, bool confirmed)
        //{
        //    if (!string.IsNullOrWhiteSpace(user.Id))
        //    {
        //        var id = int.Parse(user.Id);
        //        _userService.SetUserPhoneVerified(id, confirmed);
        //    }

        //    user.PhoneVerified = confirmed;
        //    return Task.FromResult(0);
        //}
        #endregion

        public static UserDTO MappUser(UserDTO record)
        {
            var result = new UserDTO()
            {
                Id = record.Id,
                UserName = record.EmailId,
                FirstName=string.IsNullOrEmpty(record.FirstName)?string.Empty: record.FirstName,
                LastName= string.IsNullOrEmpty(record.LastName) ? string.Empty : record.LastName,
                UserType = string.IsNullOrEmpty(record.UserType) ? string.Empty : record.UserType,
                EmailId = record.EmailId,
                Password = record.Password,
                IsActive = record.IsActive,
                SecurityStamp = record.SecurityStamp,
                IsEmailConfirm = record.IsEmailConfirm,
                CreatedAt = record.CreatedAt
            };

            return result;
        }

    }
}
