using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GPA.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Data;
using GPA.BLL;
using GPA.Filters;

namespace GPA.Controllers
{
    [AuthorizePackage(Enumerators.AccessRights.MODULES_ADMINISTRATION)]
    public class AdministrationController : BaseController
    {
        public ActionResult Index()
        {
            if (IsUserLoggedIn())
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [AuthorizeModule(Enumerators.AccessRights.CLIENTS)]
        public ActionResult Clients()
        {
            if (IsUserLoggedIn())
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [AuthorizeModule(Enumerators.AccessRights.USERS)]
        public ActionResult Utilisateurs()
        {
            if (IsUserLoggedIn())
            {
                var myUser = new KPIPrincipal();
                var roles = RoleService.GetAll();
                if (myUser.Profile > 1)
                {
                    int p = myUser.Profile;
                    roles = roles.Where(x => x.ProfileRole >= p).ToList();
                }
                
                ViewBag.ListRoles = roles;
                ViewBag.ListClients = ClientService.Read();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [AuthorizeModule(Enumerators.AccessRights.HISTORIQUE)]
        public ActionResult Historique()
        {
            if (IsUserLoggedIn())
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [AuthorizeModule(Enumerators.AccessRights.ROLES)]
        public ActionResult Roles()
        {
            if (IsUserLoggedIn())
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [AuthorizeModule(Enumerators.AccessRights.ROLES)]
        public ActionResult RoleEdit(int id)
        {
            if (IsUserLoggedIn())
            {
                ViewBag.AllAccessRights = AccessRightsService.AllAccessRights;
                var myRole = RoleService.GetById(id);
                return View(myRole);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [AuthorizeModule(Enumerators.AccessRights.ROLES)]
        public ActionResult RoleNew()
        {
            if (IsUserLoggedIn())
            {
                ViewBag.AllAccessRights = AccessRightsService.AllAccessRights;
                var myRole = RoleService.GetNew();
                return View(myRole);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }           
        }

        [AuthorizeModule(Enumerators.AccessRights.ROLES)]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult RoleEdit(FormCollection form)
        {
            string roleID = form["IdRole"];
            string roleName = form["role_name"];
            string roleDescription = form["role_description"];
            string rights = form["rights"];

            string smessage = RoleService.Update(roleID, roleName, roleDescription, rights);
            if (string.IsNullOrEmpty(smessage))
            {
                return RedirectToAction("Roles");
            }
            else
            {
                return RedirectToAction("RoleEdit", new { id = roleID });
            }
        }

        [AuthorizeModule(Enumerators.AccessRights.ROLES)]
        [HttpPost]
        public ActionResult RoleNew(FormCollection form)
        {
            string roleName = form["role_name"];
            string roleDescription = form["role_description"];
            string rights = form["rights"];

            TempData["msg_success"] = string.Empty;
            TempData["msg_error"] = string.Empty;

            string smessage = RoleService.Create(roleName, roleDescription, rights);
            if (!string.IsNullOrEmpty(smessage))
            {
                return RedirectToAction("Roles");
            }
            else
            {
                return RedirectToAction("RoleNew");
            }
        }
    }
}
