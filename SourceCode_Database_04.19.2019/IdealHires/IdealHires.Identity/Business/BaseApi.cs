using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.BAL.Business
{
    public abstract class BaseApi
    {
        protected readonly AppUserInfo AppUserInfo;
        protected BaseApi(AppUserInfo appUserInfo)
        {
            AppUserInfo = appUserInfo;
        }
    }
}
