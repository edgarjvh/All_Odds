using AllOdds.SqlDependencies;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AllOdds
{
    public class MvcApplication : System.Web.HttpApplication
    {
        string strConn = ConfigurationManager.ConnectionStrings["defaultConn"].ConnectionString;
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SqlDependency.Start(strConn);

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            UpdateComponent uc = new UpdateComponent();
            uc.BookmakersUpdated();
            uc.CategoriesUpdated();
            uc.EventsUpdated();
            uc.MatchesUpdated();
            uc.PlayersUpdated();
            uc.TeamsUpdated();
        }

        protected void Application_Stop()
        {
            SqlDependency.Stop(strConn);
        }
    }
}
