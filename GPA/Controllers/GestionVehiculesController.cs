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
    [AuthorizePackage(Enumerators.AccessRights.MODULES_GESTION_VEHICULES)]
    public class GestionVehiculesController : BaseController
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

        [AuthorizeModule(Enumerators.AccessRights.CONDUCTEURS)]
        public ActionResult Conducteurs()
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

        [AuthorizeModule(Enumerators.AccessRights.DEPARTEMENTS)]
        public ActionResult Departements()
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

        [AuthorizeModule(Enumerators.AccessRights.VEHICULES)]
        public ActionResult Vehicules()
        {
            if (IsUserLoggedIn())
            {
                var myUser = new KPIPrincipal();
                int client = 0;
                if (myUser.Profile > 1)
                {
                    client = myUser.Client;
                }
                ViewBag.ListCarburants = ReferentielService.GetReferentiel("TYPE_CARBURANT");
                ViewBag.ListTypes = ReferentielService.GetReferentiel("TYPE_VEHICULE");
                ViewBag.ListDepartements = VehiculeDepartementService.Read(client, null);
                ViewBag.ListConducteurs = VehiculeConducteurService.Read(client);
                ViewBag.ListClients = ClientService.Read();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [AuthorizeModule(Enumerators.AccessRights.KILOMETRAGES)]
        public ActionResult Kilometrages()
        {
            if (IsUserLoggedIn())
            {
                var myUser = new KPIPrincipal();
                int client = 0;
                if (myUser.Profile > 1)
                {
                    client = myUser.Client;
                }
                ViewBag.ListVehicules = VehiculeService.Read(0,client,null);              
                ViewBag.ListDepartements = VehiculeDepartementService.Read(client, null);
                ViewBag.ListClients = ClientService.Read();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        public ActionResult KilometragesMensuels()
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
                ViewBag.ListDepartements = VehiculeDepartementService.Read(client, null);
                ViewBag.ListClients = ClientService.Read();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        [AuthorizeModule(Enumerators.AccessRights.ACQUISITIONS)]
        public ActionResult Acquisitions()
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
                ViewBag.ListFournisseurs = FournisseurService.Read(client);
                ViewBag.ListClients = ClientService.Read();

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [AuthorizeModule(Enumerators.AccessRights.AUTRES_DEPENSES)]
        public ActionResult AutresDepenses()
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
                ViewBag.ListDepenses = ParametresDepenseService.Read(client);
                ViewBag.ListClients = ClientService.Read();

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [AuthorizeModule(Enumerators.AccessRights.DOCUMENTS)]
        public ActionResult Documents()
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
                ViewBag.ListFournisseurs = FournisseurService.Read(client);
                ViewBag.ListClients = ClientService.Read();

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
