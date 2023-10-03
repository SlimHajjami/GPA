using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GPA.BLL;
using GPA.Models;

namespace GPA.Filters
{
    public class AuthorizePackage : AuthorizeAttribute
    {
        private Enumerators.AccessRights _package;

        public AuthorizePackage(Enumerators.AccessRights package)
        {
            _package = package;
        }

        private KPIPrincipal CurrentUser
        {
            get { return new KPIPrincipal(); }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                if (!CurrentUser.IsInRights(_package))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "AccessDenied", area = "" }));
                }
            }
        }
    }
}