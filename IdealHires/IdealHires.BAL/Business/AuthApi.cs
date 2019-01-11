using IdealHires.BAL.DataContext;
using IdealHires.Data;
using IdealHires.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.BAL.Business
{
    public class AuthApi : BaseApi, IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthApi(AppUserInfo appUserInfo) : base(appUserInfo)
        {
            _unitOfWork = new UnitOfWork(new IdealHiresDbContext());
        }

        public AuthApi()
        {
            _unitOfWork = new UnitOfWork(new IdealHiresDbContext());
        }

        public int CreateUser(UserDTO userEntity)
        {
            try
            {
                var userRecord = MapUser(userEntity, false);
                _unitOfWork.Users.Add(userRecord);
                _unitOfWork.Complete();
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int LoginUserIdentity(string UserName, string Password)
        {
            try
            {
               // var userRecord = MapUser(userEntity, false);

                _unitOfWork.Users.Get(1);
                _unitOfWork.Complete();
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetUserPasswordHash(int userId)
        {
            return MapUser(_unitOfWork.Users.GetUser(userId)).Password;
        }

        public UserDTO GetUser(int userId)
        {
            return MapUser(_unitOfWork.Users.GetUser(userId));
        }

        public UserDTO GetUser(string username)
        {
            return MapUser(_unitOfWork.Users.GetUser(username));
        }

        public void SetUserPasswordHash(int userId, string passwordHash)
        {
            var userRecord = _unitOfWork.Users.GetUser(userId);
            userRecord.Password = passwordHash;
            _unitOfWork.Complete();
        }

        public void SetUserPasswordHash(string userId, string passwordHash)
        {
            var userRecord = _unitOfWork.Users.GetUser(userId);
            userRecord.Password = passwordHash;
            _unitOfWork.Complete();
        }
        public string GetUserSecurityStamp(int userId)
        {
            return MapUser(_unitOfWork.Users.GetUser(userId)).SecurityStamp;
        }

        public void SetUserSecurityStamp(int userId, string stamp)
        {
            var userRecord = _unitOfWork.Users.GetUser(userId);
            userRecord.SecurityStamp = stamp;
            _unitOfWork.Complete();
        }

        public string GetUserEmail(int userId)
        {
            return MapUser(_unitOfWork.Users.GetUser(userId)).EmailId;
        }

        public bool GetUserEmailVerified(int userId)
        {
            return MapUser(_unitOfWork.Users.GetUser(userId)).IsEmailConfirm;
        }

        public void SetUserEmail(int userId, string email)
        {
            var userRecord = _unitOfWork.Users.GetUser(userId);
            userRecord.EmailId = email;
            _unitOfWork.Complete();
        }

        public void SetUserEmailVerified(int userId, bool confirmed)
        {
            var userRecord = _unitOfWork.Users.GetUser(userId);
            userRecord.IsEmailConfirm = confirmed;
            _unitOfWork.Complete();
        }

        public void Dispose()
        {

        }

        private User MapUser(UserDTO user, bool mapId = true)
        {
            if (user == null) return null;
            return new User()
            {
                IsEmailConfirm = user.IsEmailConfirm,
                Password = user.Password,
                SecurityStamp = user.SecurityStamp,
                EmailId = user.EmailId,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = DateTime.Now
            };
        }

        private UserDTO MapUser(User user, bool mapId = true)
        {
            if (user == null) return null;
            return new UserDTO()
            {
                Id = user.Id,
                IsEmailConfirm = user.IsEmailConfirm ? user.IsEmailConfirm:false,
                Password = user.Password,
                SecurityStamp = user.SecurityStamp,
                EmailId = user.EmailId,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = DateTime.Now
            };
        }


    }
}
