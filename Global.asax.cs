using Amazon.EC2.Model;
using System.Web.Mvc;
using System.Web.Optimization;
using Url_Shortener.App_Start;

namespace Url_Shortener
{
    public class Global
    {
        protected void Application_Start()
        {
            // ... другие настройки ...

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

    }
}
