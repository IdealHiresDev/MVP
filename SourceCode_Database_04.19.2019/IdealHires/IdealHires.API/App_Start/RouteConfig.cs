using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace IdealHires.API
{
    /// <summary>
    /// URL Routing or URL Rewriting is a technique 
    /// using that we can define user friendly URL which is useful for Search Engine Optimization (SEO).
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Route. Route defines the URL pattern and handler information.
        /// All the configured routes of an application stored in RouteTable and will be used by 
        /// Routing engine to determine appropriate handler class or file for an incoming request.
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
