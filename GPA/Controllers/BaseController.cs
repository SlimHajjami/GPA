using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GPA.BLL;
using System.Web.Routing;


namespace GPA.Controllers
{
    public class BaseController : Controller
    {
        protected int userId { get { return (int)(Session["UTILISATEUR_ID"] ?? 0); } }

        protected bool IsUserLoggedIn()
        {
            bool loggedIn = false;
            if (userId > 0)
            {
                loggedIn = true;
            }
            return loggedIn;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            string exception = filterContext.Exception.InnerException.ToString();
        }
    }
}