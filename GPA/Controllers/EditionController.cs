using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GPA.BLL;
using GPA.Models;
using GPA.Filters;

namespace GPA.Controllers
{
    [AuthorizePackage(Enumerators.AccessRights.MODULES_EDITIONS)]
    public class EditionController : BaseController
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

        [AuthorizeModule(Enumerators.AccessRights.RAPPORT_DEPENSES)]
        public ActionResult RapportDepenses()
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


        [AuthorizeModule(Enumerators.AccessRights.RAPPORT_DEPENSES_GLOBAL)]
        public ActionResult RapportDepensesGlobal()
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

        
        [AuthorizeModule(Enumerators.AccessRights.RAPPORT_CONSOMMATION_CARBURANT_MENSUEL)]
        public ActionResult RapportConsommationCarburantMensuel()
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

        [AuthorizeModule(Enumerators.AccessRights.RAPPORT_ENTRETIEN_REPARATION)]
        public ActionResult RapportEntretienReparation()
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

        [AuthorizeModule(Enumerators.AccessRights.RAPPORT_ENTRETIEN_REPARATION_MENSUEL)]
        public ActionResult RapportEntretienReparationMensuel()
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

     

        [AuthorizeModule(Enumerators.AccessRights.RAPPORT_GLOBAL)]
        public ActionResult RapportGlobal()
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

        [AuthorizeModule(Enumerators.AccessRights.RAPPORT_CONSOMMATION_CARBURANT_SEMESTRIELLE)]
        public ActionResult RapportConsommationCarburantSemestrielle()
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

        [AuthorizeModule(Enumerators.AccessRights.RAPPORT_CONSOMMATION_CARBURANT_SEMESTRIELLE)]
        public ActionResult RapportConsommationCarburantParMarqueSemestrielle()
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

        [AuthorizeModule(Enumerators.AccessRights.RAPPORT_DEPENSES_DETAILLEES)]
        public ActionResult RapportDepensesDetaillees()
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

    }
}
