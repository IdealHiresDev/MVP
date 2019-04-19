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
    public class NotificationService : IDisposable
    {
        #region (PRIVATE MEMBER)
        bool disposed = false;
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region (CONSTRUCTOR)
        public NotificationService()
        {

            _unitOfWork = new UnitOfWork(new IdealHiresDbContext());

        }

        #endregion

        #region(GET NOTIFICATION LIST BY CONDIDATE USER ID)

        public List<NotificationDTO> GetNotificationDetailsById(int? id)
        {
            var notificationDTO = new List<NotificationDTO>();



            var UserCompanyBy = _unitOfWork.EmployerCompanyRepository.Query(i => i.UserId == id).FirstOrDefault();
            int? CompanyId = (UserCompanyBy != null ? UserCompanyBy.CompanyId : 0);
            if (CompanyId > 0)
            {
                notificationDTO = (from condidateNotification in _unitOfWork.NotificationRepository.Query(i => i.IsActive == true && i.IsDeleted == false)
                                   join eventEntity in _unitOfWork.EntityRepository.Query(j => j.IsActive == true && j.IsDeleted == false)
                                   on condidateNotification.EntityId equals eventEntity.Id
                                   where (condidateNotification.UserId == id || condidateNotification.CompanyId == CompanyId)
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


            }
            else
            {
                notificationDTO = (from condidateNotification in _unitOfWork.NotificationRepository.Query(i => i.IsActive == true && i.IsDeleted == false)
                                   join eventEntity in _unitOfWork.EntityRepository.Query(j => j.IsActive == true && j.IsDeleted == false)
                                   on condidateNotification.EntityId equals eventEntity.Id
                                   where (condidateNotification.UserId == id || condidateNotification.EntityId == 5 || condidateNotification.EntityId == 8
                                    || condidateNotification.EntityId == 10 || condidateNotification.EntityId == 12)
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


            }
            return notificationDTO;
        }


        #endregion

        #region(GET SHORT NOTIFICATION LIST BY CONDIDATE USER ID)

        public List<NotificationDTO> GetShortNotificationDetailsById(int? id)
        {
            var notificationDTO = new List<NotificationDTO>();

            var UserCompanyBy = _unitOfWork.EmployerCompanyRepository.Query(i => i.UserId == id).FirstOrDefault();
            int? CompanyId = (UserCompanyBy != null ? UserCompanyBy.CompanyId : 0);
            if (CompanyId > 0)
            {
                notificationDTO = (from condidateNotification in _unitOfWork.NotificationRepository.Query(i => i.IsActive == true && i.IsDeleted == false && i.IsRead == false)
                                   join eventEntity in _unitOfWork.EntityRepository.Query(j => j.IsActive == true && j.IsDeleted == false)
                                   on condidateNotification.EntityId equals eventEntity.Id
                                   where condidateNotification.CompanyId == CompanyId
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

                                   }).ToList();
            }
            else
            {
                notificationDTO = (from condidateNotification in _unitOfWork.NotificationRepository.Query(i => i.IsActive == true && i.IsDeleted == false && i.IsRead == false)
                                   join eventEntity in _unitOfWork.EntityRepository.Query(j => j.IsActive == true && j.IsDeleted == false)
                                   on condidateNotification.EntityId equals eventEntity.Id
                                   where (condidateNotification.UserId == id || condidateNotification.EntityId == 5 || condidateNotification.EntityId == 8
                                     || condidateNotification.EntityId == 10 || condidateNotification.EntityId == 13)
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

                                   }).ToList();
            }



            return notificationDTO;
        }


        #endregion

        #region(UPDATE NOTIFICATION BY ID WHEN USER HAS BEEN READ NOTIFICATION)

        public bool ReadNotificationById(int id)
        {
            try
            {
                var UserCompanyBy = _unitOfWork.EmployerCompanyRepository.Query(i => i.UserId == id).FirstOrDefault();
                int? CompanyId = (UserCompanyBy != null ? UserCompanyBy.CompanyId : 0);
                if (CompanyId > 0)
                {
                    var notificationslist = (from notification in _unitOfWork.NotificationRepository.Query(x => x.CompanyId == CompanyId && x.IsActive == true && x.IsDeleted == false) select notification).ToList();
                    if (id > 0)
                    {
                        foreach (var notification in notificationslist)
                        {
                            notification.IsRead = true;
                            notification.UpdatedBy = id;
                            notification.UpdatedAt = GetCurrentDate();
                            _unitOfWork.NotificationRepository.Update(notification);
                            _unitOfWork.Complete();

                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    var notificationslist = (from notification in _unitOfWork.NotificationRepository.Query(x => x.UserId == id && x.IsActive == true && x.IsDeleted == false) select notification).ToList();
                    if (id > 0)
                    {
                        foreach (var notification in notificationslist)
                        {
                            notification.IsRead = true;
                            notification.UpdatedBy = id;
                            notification.UpdatedAt = GetCurrentDate();
                            _unitOfWork.NotificationRepository.Update(notification);
                            _unitOfWork.Complete();

                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region(SET NOTIFICATION ALERT ACCORDING TO ACTIVITY BY CONDIDATE SIDE FOR JOB APPLY, SAVE, AND NOT INTERESTED)
        public NotificationDTO SetNotificationAlert(ProfileJobDTO model)
        {
            List<string> emailAddress = new List<string>();

            NotificationDTO notificationDTO = new NotificationDTO();
            var response = new BaseModel();
            if (model.ActionId == 1)
            {
                var jobCreatedBy = _unitOfWork.JobRepository.Query(i => i.Id == model.JobId).FirstOrDefault();
                int? EmployerId = (jobCreatedBy != null ? jobCreatedBy.CreatedBy : 0);

                var company = _unitOfWork.EmployerCompanyRepository.Query(i => i.UserId == EmployerId).FirstOrDefault();
                int? CompanyId = (company != null ? company.CompanyId : 0);

                var userlist = (from user in _unitOfWork.Users.Query(u => u.IsActive == true)
                                join useremail in _unitOfWork.EmployerCompanyRepository.Query(x => x.IsActive == true)
                                on user.Id equals useremail.UserId
                                where useremail.CompanyId == CompanyId
                                select user).ToList();

                foreach (var user in userlist)
                {
                    emailAddress.Add(user.EmailId);

                }


                notificationDTO = (from userDetails in _unitOfWork.Users.Query(i => i.Id == model.UserId)
                                   join profile in _unitOfWork.ProfileJobRepository.Query(p => p.IsActive == true)
                                   on userDetails.Id equals profile.UserId
                                   join empComp in _unitOfWork.EmployerCompanyRepository.Query(ec => ec.IsActive == true && ec.IsDeleted == false)
                                   on EmployerId equals empComp.UserId
                                   join data in _unitOfWork.JobRepository.Query(j => j.IsActive == true)
                                   on profile.JobId equals data.Id
                                   join companyDetails in _unitOfWork.CompanyRepository.Query(c => c.IsActive == true && c.IsDeleted == false)
                                   on data.CompanyId equals companyDetails.Id
                                   join notificatioTypeJOb in _unitOfWork.NotificationTypeJobRepository.Query(n => n.IsActive == true && n.IsDeleted == false)
                                   on model.JobId equals notificatioTypeJOb.JobId
                                   where (profile.JobId == model.JobId && profile.UserId == model.UserId && empComp.UserId == EmployerId)

                                   select new NotificationDTO()
                                   {
                                       Title = userDetails.FirstName + " " + userDetails.LastName + " applied  job for " + data.Title + " profile.",
                                       EventEntity =data.Description,
                                       entityId = 1,
                                       createdBy = model.UserId,
                                       userId = EmployerId,
                                       CompanyId = empComp.CompanyId,
                                       NotificationTypeId = notificatioTypeJOb.NotificationTypeId,
                                       EmailAddress = emailAddress,
                                       Name=companyDetails.CompanyName,
                                       Subject = "New Job Enquiry"

                                   }).FirstOrDefault();
                return notificationDTO;
            }
            else if (model.ActionId == 3)
            {
                var jobCreatedBy = _unitOfWork.JobRepository.Query(i => i.Id == model.JobId).FirstOrDefault();
                int? EmployerId = (jobCreatedBy != null ? jobCreatedBy.CreatedBy : 0);

                notificationDTO = (from userDetails in _unitOfWork.Users.Query(i => i.Id == model.UserId)
                                   join profile in _unitOfWork.ProfileJobRepository.Query(p => p.IsActive == true)
                                   on userDetails.Id equals profile.UserId
                                   join empComp in _unitOfWork.EmployerCompanyRepository.Query(ec => ec.IsActive == true && ec.IsDeleted == false)
                                   on EmployerId equals empComp.UserId
                                   join data in _unitOfWork.JobRepository.Query(j => j.IsActive == true)
                                   on profile.JobId equals data.Id
                                   join companyDetails in _unitOfWork.CompanyRepository.Query(c => c.IsActive == true && c.IsDeleted == false)
                                   on data.CompanyId equals companyDetails.Id
                                   join notificatioTypeJOb in _unitOfWork.NotificationTypeJobRepository.Query(n => n.IsActive == true && n.IsDeleted == false)
                                   on model.JobId equals notificatioTypeJOb.JobId
                                   where (profile.JobId == model.JobId && profile.UserId == model.UserId && empComp.UserId == EmployerId)

                                   select new NotificationDTO()
                                   {
                                       Title = userDetails.FirstName + " " + userDetails.LastName + " is  saved  job for " + data.Title + " profile.",
                                       EventEntity =data.Description,
                                       entityId = 3,
                                       createdBy = model.UserId,
                                       userId = EmployerId,
                                       CompanyId = empComp.CompanyId,
                                       NotificationTypeId = notificatioTypeJOb.NotificationTypeId,
                                       EmailAddress = emailAddress,
                                       Subject = "New Job Saved",
                                        Name = userDetails.FirstName + " " + userDetails.LastName,
                                   }).FirstOrDefault();
                return notificationDTO;
            }
            else if (model.ActionId == 4)
            {
                var jobCreatedBy = _unitOfWork.JobRepository.Query(i => i.Id == model.JobId).FirstOrDefault();
                int? EmployerId = (jobCreatedBy != null ? jobCreatedBy.CreatedBy : 0);

                notificationDTO = (from userDetails in _unitOfWork.Users.Query(i => i.Id == model.UserId)
                                   join profile in _unitOfWork.ProfileJobRepository.Query(p => p.IsActive == true)
                                   on userDetails.Id equals profile.UserId
                                   join empComp in _unitOfWork.EmployerCompanyRepository.Query(ec => ec.IsActive == true && ec.IsDeleted == false)
                                   on EmployerId equals empComp.UserId
                                   join data in _unitOfWork.JobRepository.Query(j => j.IsActive == true)
                                   on profile.JobId equals data.Id
                                   join companyDetails in _unitOfWork.CompanyRepository.Query(c => c.IsActive == true && c.IsDeleted == false)
                                   on data.CompanyId equals companyDetails.Id
                                   join notificatioTypeJOb in _unitOfWork.NotificationTypeJobRepository.Query(n => n.IsActive == true && n.IsDeleted == false)
                                   on model.JobId equals notificatioTypeJOb.JobId
                                   where (profile.JobId == model.JobId && profile.UserId == model.UserId && empComp.UserId == EmployerId)

                                   select new NotificationDTO()
                                   {
                                       Title = userDetails.FirstName + " " + userDetails.LastName + " is not interested for " + data.Title + " profile.",
                                       EventEntity =data.Description,
                                       entityId = 4,
                                       createdBy = model.UserId,
                                       userId = EmployerId,
                                       CompanyId = empComp.CompanyId,
                                       NotificationTypeId = notificatioTypeJOb.NotificationTypeId,
                                       EmailAddress = emailAddress,
                                       Subject = "Condidate Isn't intersted",
                                       Name = userDetails.FirstName + " " + userDetails.LastName,
                                   }).FirstOrDefault();
                return notificationDTO;
            }
            else
            {
                return notificationDTO;
            }

        }

        #endregion

        #region(SET NOTIFICATION ALERT SHORT LIST CONDIDATE FOR JOB BY EMPLOYEER)
        public NotificationDTO ShortListNotificationForJob(SortListedCandidateDTO sortListedCandidateDTO, int entityId)
        {
            BaseModel response = new BaseModel();
            EmployerCompany employerCompany = _unitOfWork.EmployerCompanyRepository.Get(ec => ec.UserId == sortListedCandidateDTO.CreatedBy).FirstOrDefault();
            var jobId = _unitOfWork.ProfileJobRepository.Get(i => i.ProfileId == sortListedCandidateDTO.ProfileId && i.IsActive == true).LastOrDefault();
            sortListedCandidateDTO.JobId = jobId.JobId.Value;
            sortListedCandidateDTO.CompanyId = employerCompany.CompanyId;
            var notificationDTO = new NotificationDTO();
            if (entityId == 12)
            {

                notificationDTO = (from data in _unitOfWork.JobRepository.Query(i => i.Id == sortListedCandidateDTO.JobId)
                                   join companyDetails in _unitOfWork.CompanyRepository.Query(c => c.IsActive == true && c.IsDeleted == false)
                                   on data.CompanyId equals companyDetails.Id
                                   join profile in _unitOfWork.ProfileRepository.Query(p => p.IsActive == true && p.IsDeleted == false)
                                   on sortListedCandidateDTO.ProfileId equals profile.Id
                                   join user in _unitOfWork.Users.Query(u => u.IsActive == true)
                                   on profile.UserId equals user.Id
                                   join notificatioTypeJOb in _unitOfWork.NotificationTypeJobRepository.Query(n => n.IsActive == true && n.IsDeleted == false)
                                   on sortListedCandidateDTO.JobId equals notificatioTypeJOb.JobId
                                   select new NotificationDTO()
                                   {
                                       Title = "Your resume has been shortlisted for the role of  " + data.Title + " in " + companyDetails.CompanyName,
                                       EventEntity = data.Description,
                                       entityId = 12,
                                       createdBy = sortListedCandidateDTO.CreatedBy,
                                       userId = profile.UserId,
                                       NotificationTypeId = notificatioTypeJOb.NotificationTypeId,
                                       ToMail = user.EmailId,
                                       Name = user.FirstName + " " + user.LastName,
                                       Subject = "Resume has been sortlisted by " + companyDetails.CompanyName
                                   }).FirstOrDefault();

            }

            return notificationDTO;
        }
        #endregion

        #region(ADD NOTIFICATION DETAILS ACCORDING TO ENTITY EVENT ID)

        /// <summary>
        /// This methoda used for save the all types of notification according to entity id
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="userId"></param>   
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public BaseModel SaveNotificaton(NotificationDTO notificationDTO)
        {
            var response = new BaseModel();
            Notification notification = new Notification();

            try
            {
                notification.Title = notificationDTO.Title;
                notification.Entity = notificationDTO.EventEntity;
                notification.EntityId = notificationDTO.entityId;
                notification.UserId = notificationDTO.userId;
                notification.CompanyId = notificationDTO.CompanyId;
                notification.Status = true;
                notification.CreatedAt = GetCurrentDate();
                notification.CreatedBy = notificationDTO.createdBy;
                notification.IsActive = true;
                notification.IsDeleted = false;
                notification.IsRead = false;
                _unitOfWork.NotificationRepository.Add(notification);
                int iRows = _unitOfWork.Complete();
                if (iRows > 0)
                {
                    response.Success = true;
                    response.Message = CandidateResource.NotificatioAddSucess;
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

        public static DateTime GetCurrentDate()
        {
            using (IdealHiresDbContext db = new IdealHiresDbContext())
            {
                return db.Database.SqlQuery<DateTime>("select dbo.uf_UTCToEST(@UTC_Date,@WithDaylight) ", new System.Data.SqlClient.SqlParameter("@UTC_Date", DateTime.UtcNow), new System.Data.SqlClient.SqlParameter("@WithDaylight", 1)).Single();
            }
        }

        #endregion



        #region (DISPOSE)
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
