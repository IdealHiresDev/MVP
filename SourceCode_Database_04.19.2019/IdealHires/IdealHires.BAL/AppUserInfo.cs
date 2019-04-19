using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.BAL
{
    public class AppUserInfo
    {
        public static AppUserInfo DefaultUser = new AppUserInfo(0, "System", "System User", true, null);

        public int Id { get; }
        public string Username { get; }
        public string Fullname { get; }
        public bool Active { get; }
        public string ProfilePic { get; }

        public AppUserInfo(int id, string username, string fullname, bool active, string profilePic)
        {
            Id = id;
            Username = username;
            Fullname = fullname;
            Active = active;
            ProfilePic = profilePic;
        }

    }
}
