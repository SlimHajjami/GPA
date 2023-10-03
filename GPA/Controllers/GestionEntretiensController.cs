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
    [AuthorizePackage(Enumerators.AccessRights.MODULES_ENTRETIENS)]
    public class GestionEntretiensController : BaseController
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

        [AuthorizeModule(Enumerators.AccessRights.ENTRETIENS)]
        public ActionResult Entretiens()
        {
            if (IsUserLoggedIn())
            {
                var myUser = new KPIPrincipal();
                int client = 0;
                if (myUser.Profile > 1)
                {
                    client = myUser.Client;
                }
                ViewBag.ListVehicules = VehiculeService.Read(0, client, null);
                ViewBag.ListClients = ClientService.Read();
                ViewBag.ListFournisseurs = FournisseurService.Read(client);
                ViewBag.ListTypesDepenses = ReferentielService.GetReferentiel("TYPE_DEPENSE");
                ViewBag.ListPieces = PieceService.Read(client);
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [AuthorizeModule(Enumerators.AccessRights.ENTRETIENS_MAITRES)]
        public ActionResult EntretiensMaitres()
        {
            if (IsUserLoggedIn())
            {
                ViewBag.ListClients = ClientService.Read();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [AuthorizeModule(Enumerators.AccessRights.PROGRAMME_ENTRETIEN)]
        public ActionResult ProgrammeEntretien()
        {
            if (IsUserLoggedIn())
            {
                var myUser = new KPIPrincipal();
                int client = 0;
                if (myUser.Profile > 1)
                {
                    client = myUser.Client;
                }
                ViewBag.ListVehicules = VehiculeService.Read(0, client, null);
                ViewBag.ListClients = ClientService.Read();
                ViewBag.ListEntretiens = EntretienMaitreService.Read(client);
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
