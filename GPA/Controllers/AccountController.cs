using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GPA.Models;
using GPA.BLL;
using System.Threading;

namespace GPA.Controllers
{
    public class AccountController : BaseController
    {
        public ActionResult Login()
        {
            if (!IsUserLoggedIn())
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Login(FormCollection form, string ReturnUrl)
        {
            string login = form["LoginUtilisateur"];
            string pwd = form["PasswordUtilisateur"];

            var user = new UtilisateurModel();
            if (UtilisateurService.IsValid(login, pwd, ref user))
            {
                FormsAuthentication.SetAuthCookie(login, false);

                if (Url.IsLocalUrl(ReturnUrl) && ReturnUrl.Length > 1 && ReturnUrl.StartsWith("/")
                    && !ReturnUrl.StartsWith("//") && !ReturnUrl.StartsWith("/\\"))
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
                    HistoriqueConnexionService.UserLogged(user,ip);
                    Session["UTILISATEUR_ID"] = user.IdUtilisateur;
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.Message = "Login ou mot de passe incorrecte !";
            return View("Login");
        }

        public ActionResult LogOut()
        {
            //  this.Session["UTILISATEUR_ID"];
            try
            {
                HistoriqueConnexionService.UserDisconnected((int)this.Session["UTILISATEUR_ID"]);
                HttpContext.User = Thread.CurrentPrincipal = null;
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("Login", "Account");
            }
            catch (Exception)
            {

                HttpContext.User = Thread.CurrentPrincipal = null;
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("Login", "Account");
            }
            
        }
    }
}
