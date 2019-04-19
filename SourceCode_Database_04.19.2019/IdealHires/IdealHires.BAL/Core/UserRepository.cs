using IdealHires.Data;
using System.Linq;

namespace IdealHires.BAL.Core
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IdealHiresDbContext _context;
        public UserRepository(IdealHiresDbContext context) : base(context)
        {
            _context = context;
        }

        public User GetUser(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId && u.IsActive == true);
        }

        public User GetUser(string username)
        {
            return _context.Users.FirstOrDefault(u => u.EmailId == username && u.IsActive == true);
        }
    }
}
