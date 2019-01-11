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
        int Complete();
    }
}
