using IdealHires.BAL.Core;
using IdealHires.Data;
using System;


namespace IdealHires.BAL.DataContext
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }        
        IRepository<Profile> ProfileRepository { get; }
        IRepository<Address> AddressRepository { get; }
        IRepository<Work> WorkRepository { get; }
        IRepository<Academic> AcademicRepository { get; }
        IRepository<JobType> JobTypeRepository { get; }
        IRepository<JobCategory> JobCategoryRepository { get; }
        IRepository<KeywordsProfile> KeywordsProfileRepository { get; }
        IRepository<JobTypeProfile> JobTypeProfileRepository { get; }
        IRepository<JobCategoryProfile> JobCategoryProfileRepository { get; }
        IRepository<Company> CompanyRepository { get; }
        IRepository<EmployerCompany> EmployerCompanyRepository { get; }
        IRepository<Job> JobRepository { get; }
        IRepository<JobTypeJob> JobTypeJobRepository { get; }
        IRepository<JobCategoryJob> JobCategoryJobRepository { get; }
        IRepository<KeywordsJob> KeywordsJobRepository { get; }
        IRepository<NotificationType> NotificationTypeRepository { get; }
        IRepository<PayPeriodType> PayPeriodTypeRepository { get; }
        IRepository<NotificationTypeJob> NotificationTypeJobRepository { get; }     
        IRepository<ProfileJob> ProfileJobRepository { get; }
        IRepository<Country> CountryRepository { get; }
        IRepository<State> StateRepository { get; }
        IRepository<City> CityRepository { get; }        
        IRepository<CompanyAddress> CompanyAddressRepository { get; }
        IRepository<SortListedCandidate> SortListedCandidateRepository { get; }        
        IRepository<JobCredit> jobCreditRepository { get; }
        IRepository<AddressEmployer> AddressEmployerRepository { get; }
        IRepository<CompanyPackageDetail> CompanyPackageDetailRepository { get; }
        IRepository<CompanyJobCreditDetail> CompanyJobCreditDetailRepository { get; }
        IRepository<CompanyJobCreditDetailHistory> CompanyJobCreditDetailHistoryRepository { get; }
        IRepository<PhoneFormat> PhoneFormatRepository { get; }
        IRepository<UserRole> UserRoleRepository { get; }
        IRepository<TransactionDetail> TransactionDetailRepository { get; }
        IRepository<CompanyLogo> CompanyLogoRepository { get; }
        IRepository<Notification> NotificationRepository { get; }
        IRepository<Entity> EntityRepository { get; }
        int Complete();
    }
}
