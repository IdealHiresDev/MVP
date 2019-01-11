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

        #endregion

        #region Constructor and Repository Instance
        public UnitOfWork(IdealHiresDbContext context)
        {
            _context = context;            
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
