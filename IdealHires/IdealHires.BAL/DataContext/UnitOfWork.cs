using IdealHires.BAL.Core;
using IdealHires.Data;


namespace IdealHires.BAL.DataContext
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Private variables
        private readonly IdealHiresDbContext _context;

        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private IRepository<Profile> _profileRepository;
        private IRepository<Address> _addressRepository;
        private IRepository<Work> _workRepository;
        private IRepository<Academic> _academicRepository;
        private IRepository<JobType> _jobTypeRepository;
        private IRepository<JobCategory> _jobCategoryRepository;
        private IRepository<KeywordsProfile> _keywordsProfileRepository;
        private IRepository<JobTypeProfile> _jobTypeProfileRepository;
        private IRepository<JobCategoryProfile> _jobCategoryProfileRepository;
        private IRepository<Company> _companyRepository;
        private IRepository<EmployerCompany> _employerCompanyRepository;
        private IRepository<Job> _jobRepository;
        private IRepository<JobTypeJob> _jobTypeJobRepository;
        private IRepository<JobCategoryJob> _jobCategoryJobRepository;
        private IRepository<KeywordsJob> _keywordsJobRepository;
        private IRepository<NotificationType> _notificationTypeRepository;
        private IRepository<PayPeriodType> _payPeriodTypeRepository;
        private IRepository<NotificationTypeJob> _notificationTypeJobRepository;

        #endregion

        #region Constructor and Repository Instance
        public UnitOfWork(IdealHiresDbContext context)
        {
            _context = context;
        }

        public IRepository<NotificationTypeJob> NotificationTypeJobRepository
        {
            get { return _notificationTypeJobRepository ?? (_notificationTypeJobRepository = new Repository<NotificationTypeJob>(_context)); }
        }

        public IRepository<PayPeriodType> PayPeriodTypeRepository
        {
            get { return _payPeriodTypeRepository ?? (_payPeriodTypeRepository = new Repository<PayPeriodType>(_context)); }
        }

        public IRepository<NotificationType> NotificationTypeRepository
        {
            get { return _notificationTypeRepository ?? (_notificationTypeRepository = new Repository<NotificationType>(_context)); }
        }

        public IRepository<KeywordsJob> KeywordsJobRepository
        {
            get { return _keywordsJobRepository ?? (_keywordsJobRepository = new Repository<KeywordsJob>(_context)); }
        }
        public IRepository<JobTypeJob> JobTypeJobRepository
        {
            get { return _jobTypeJobRepository ?? (_jobTypeJobRepository = new Repository<JobTypeJob>(_context)); }
        }
        public IRepository<JobCategoryJob> JobCategoryJobRepository
        {
            get { return _jobCategoryJobRepository ?? (_jobCategoryJobRepository = new Repository<JobCategoryJob>(_context)); }
        }
        public IRepository<Job> JobRepository
        {
            get { return _jobRepository ?? (_jobRepository = new Repository<Job>(_context)); }
        }

        public IRepository<JobTypeProfile> JobTypeProfileRepository
        {
            get { return _jobTypeProfileRepository ?? (_jobTypeProfileRepository = new Repository<JobTypeProfile>(_context)); }
        }

        public IRepository<JobCategoryProfile> JobCategoryProfileRepository
        {
            get { return _jobCategoryProfileRepository ?? (_jobCategoryProfileRepository = new Repository<JobCategoryProfile>(_context)); }
        }

        public IRepository<JobType> JobTypeRepository
        {
            get { return _jobTypeRepository ?? (_jobTypeRepository = new Repository<JobType>(_context)); }
        }

        public IRepository<JobCategory> JobCategoryRepository
        {
            get { return _jobCategoryRepository ?? (_jobCategoryRepository = new Repository<JobCategory>(_context)); }
        }

        public IRepository<KeywordsProfile> KeywordsProfileRepository
        {
            get { return _keywordsProfileRepository ?? (_keywordsProfileRepository = new Repository<KeywordsProfile>(_context)); }
        }

        public IRepository<EmployerCompany> EmployerCompanyRepository
        {
            get { return _employerCompanyRepository ?? (_employerCompanyRepository = new Repository<EmployerCompany>(_context)); }
        }

        public IRepository<Company> CompanyRepository
        {
            get { return _companyRepository ?? (_companyRepository = new Repository<Company>(_context)); }
        }        

        public IUserRepository Users
        {
            get { return _userRepository ?? (_userRepository = new UserRepository(_context)); }
        }

        public IRoleRepository Roles
        {
            get { return _roleRepository ?? (_roleRepository = new RoleRepository(_context)); }
        }

        public IRepository<Profile> ProfileRepository
        {
            get { return _profileRepository ?? (_profileRepository = new Repository<Profile>(_context)); }
        }

        public IRepository<Address> AddressRepository
        {
            get { return _addressRepository ?? (_addressRepository = new Repository<Address>(_context)); }
        }


        public IRepository<Work> WorkRepository
        {
            get { return _workRepository ?? (_workRepository = new Repository<Work>(_context)); }
        }

        public IRepository<Academic> AcademicRepository
        {
            get { return _academicRepository ?? (_academicRepository = new Repository<Academic>(_context)); }
        }
        #endregion

        #region Save data and disposing
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        #endregion
    }
}
