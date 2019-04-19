using System.Web;
using System.Web.Mvc;

namespace IdealHires.API
{
    /// <summary>
    ///  write custom logic to execute before or after an action method executes.
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// This is static method for excecuter before action method is execute.
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
