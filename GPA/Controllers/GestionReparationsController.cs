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
    [AuthorizePackage(Enumerators.AccessRights.MODULES_REPARATIONS)]
    public class GestionReparationsController : BaseController
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

        [AuthorizeModule(Enumerators.AccessRights.REPARATIONS)]
        public ActionResult Reparations()
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
    }
}
