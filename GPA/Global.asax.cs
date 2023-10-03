using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GPA.BLL;
namespace GPA
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Session_Start()
        {
        } 
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public void Application_Error(Object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Server.ClearError();

            var routeData = new RouteData();
            routeData.Values.Add("area", "");
            routeData.Values.Add("controller", "SystemAdministration");

            if (exception.GetType() == typeof(HttpException))
            {
                var code = ((HttpException)exception).GetHttpCode();
                if (code == 404)
                {
                    routeData.Values.Add("action", "NotFound");
                }
                else if (code == 403 || code == 401)
                {
                    routeData.Values.Add("action", "NoAuthorization");
                }
                else
                {
                    routeData.Values.Add("action", "Index");
                    routeData.Values.Add("statusCode", code);
                }
            }
            else
            {
                routeData.Values.Add("action", "Index");
                routeData.Values.Add("statusCode", 500);
            }
            routeData.Values.Add("exception", exception);

            Response.TrySkipIisCustomErrors = true;


        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            CultureInfo ci;
            try
            {

                if (Session["CurrentUICulture"] == null)
                {
                    ci = new CultureInfo("fr-FR");
                }
                else if (Session["CurrentUICulture"].ToString() == "fr-FR")
                {
                    ci = new CultureInfo("fr-FR");
                }
                else
                {
                    ci = new CultureInfo("fr-FR");
                }

            }
            catch
            {
                ci = new CultureInfo("fr-FR");
            }


            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
        }



        protected void Session_End()
        {
            if (this.Session["UTILISATEUR_ID"] != null) { 
                HistoriqueConnexionService.UserDisconnected((int)this.Session["UTILISATEUR_ID"]);
              }
            //  HttpContext.Current.Session.Abandon();

            this.Session.Clear();
            this.Session.Abandon();
            //System.Web.HttpContext.Current.User = null;
            //Session.Abandon();

            //FormsAuthentication.SignOut();
        }

    }
}