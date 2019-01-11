using IdealHires.BAL.Core;
using IdealHires.Data;

namespace IdealHires.BAL.Core
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly IdealHiresDbContext _context;
        public RoleRepository(IdealHiresDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
