using IdealHires.BAL.Core;
using IdealHires.Data;

namespace IdealHires.BAL.Core
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUser(int userId);
        User GetUser(string username);     
    }
}
