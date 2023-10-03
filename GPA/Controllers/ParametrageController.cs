using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GPA.Models;
using GPA.BLL;
using GPA.Models;
using GPA.Filters;

namespace GPA.Controllers
{
    [AuthorizePackage(Enumerators.AccessRights.MODULES_PARAMETRAGE)]
    public class ParametrageController : BaseController
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

        [AuthorizeModule(Enumerators.AccessRights.REFERENTIELS)]
        public ActionResult Referentiels()
        {
            if (IsUserLoggedIn())
            {
                ViewBag.ReferentielKeys = ReferentielService.GetReferentielKeys();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [AuthorizeModule(Enumerators.AccessRights.REFERENTIELS)]
        public ActionResult VehiculeModele()
        {
            if (IsUserLoggedIn())
            {
                ViewBag.ListMarques = ReferentielService.GetReferentiel("MARQUE_VEHICULE");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [AuthorizeModule(Enumerators.AccessRights.FOURNISSEURS)]
        public ActionResult Fournisseurs()
        {
            if (IsUserLoggedIn())
            {
                ViewBag.ListClients = ClientService.Read();
                ViewBag.ListTypes = ReferentielService.GetReferentiel("TYPE_FOURNISSEUR");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [AuthorizeModule(Enumerators.AccessRights.REFERENTIELS)]
        public ActionResult CarburantCout()
        {
            if (IsUserLoggedIn())
            {
                ViewBag.ListClients = ClientService.Read();
                ViewBag.ListCarburant = CarburantCoutService.Read();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [AuthorizeModule(Enumerators.AccessRights.DEPENSES)]
        public ActionResult Depenses()
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

        [AuthorizeModule(Enumerators.AccessRights.PIECES)]
        public ActionResult Pieces()
        {
            if (IsUserLoggedIn())
            {
                var myUser = new KPIPrincipal();
                int client = 0;
                if (myUser.Profile > 1)
                {
                    client = myUser.Client;
                }
                ViewBag.ListClients = ClientService.Read();
                ViewBag.ListFournisseurs = FournisseurService.Read(client);
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [AuthorizeModule(Enumerators.AccessRights.ALERTE_EMAIL)]
        public ActionResult AlertesEmail()
        {
            if (IsUserLoggedIn())
            {
                ViewBag.ListClients = ClientService.Read();
                ViewBag.ListTypes = ReferentielService.GetReferentiel("TYPE_ALERTE");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [AuthorizeModule(Enumerators.AccessRights.MODELE_C100KM)]
        public ActionResult ModeleC100KM()
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

        [AuthorizeModule(Enumerators.AccessRights.GIS_GPA)]
        public ActionResult GISGPA()
        {
         
            if (IsUserLoggedIn())
            {
                var myUser = new KPIPrincipal();
                int client = 0;
                if (myUser.Profile > 1)
                {
                    client = myUser.Client;
                }

                ViewBag.ListVehiculesGIS = GISGPAService.ReadFromGis(client);
                ViewBag.ListVehicules = VehiculeService.Read(0, client, null);
                ViewBag.ListClients = ClientService.Read();
                ViewBag.ListDepartements = VehiculeDepartementService.Read(client, null);
               // ViewBag.VehiculesGIS = new dynamic[] { /*new { id = "0", Value = "" },*/ new { id = "1", VehiculeGISMat = "veh1" , isSelected = false}, new { id = "2", VehiculeGISMat = "veh2", isSelected = false }, new { id = "3", VehiculeGISMat = "veh3", isSelected = false } };
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
