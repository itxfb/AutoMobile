using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CarSystem
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute();
            // test route
            routes.MapRoute(
               "WeBuyCar",                                           // Route name
               "WeBuyCar/{state}/{action}/{id}",                            // URL with parameters
               new { controller = "Home", action = "WeBuyCar", state="{StateName}", id = UrlParameter.Optional }  // Parameter defaults
           );

            routes.MapRoute(
              "CarByCity",                                           // Route name
              "CarByCity/{City}/{action}/{id}",                            // URL with parameters
              new { controller = "Home", action = "CarByCityDetail", City = "{CityName}", id = UrlParameter.Optional }  // Parameter defaults
          );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
