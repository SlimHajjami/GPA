using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GPA.BLL;
using GPA.Models;


namespace GPA.Controllers
{
    public class CmbBoxsController : BaseController
    {
        public JsonResult GetReferentielKeys()
        {
            return Json(ReferentielService.GetReferentielKeys(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMarques()
        {
            return Json(ReferentielService.GetReferentiel("MARQUE_VEHICULE"), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetModeles(int codRef)
        {
            return Json(VehiculeModeleService.Read(codRef), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetReferentiel(string keyRef)
        {
            return Json(ReferentielService.GetReferentiel(keyRef), JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetReferentielCarburant(string keyRef)
        //{
        //    ViewBag.ListCarburant = ReferentielService.GetReferentiel(keyRef);
        //    return Json(ViewBag.ListCarburant, JsonRequestBehavior.AllowGet);
        //}
    

        public JsonResult GetDepartements(int client)
        {
            var myUser = new KPIPrincipal();
            DepartementsAccessRightsModel access = null;
            if (myUser.Profile > 2)
            {
                access = myUser.DepartementsAccessRights;
            }
            return Json(VehiculeDepartementService.Read(client, access), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetConducteurs(int client)
        {
            return Json(VehiculeConducteurService.Read(client), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFournisseurs(int client)
        {
            return Json(FournisseurService.Read(client), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFournisseursAcquisition(int client)
        {
            var list = FournisseurService.Read(client).Where(x => x.TypeFournisseur == 1 || x.TypeFournisseur == 2);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFournisseursAssureurs(int client)
        {
            var list = FournisseurService.Read(client).Where(x => x.TypeFournisseur == 3);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFournisseursPieces(int client)
        {
            var list = FournisseurService.Read(client).Where(x => x.TypeFournisseur == 4);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFournisseursEntretiensReparations(int client)
        {
            var list = FournisseurService.Read(client).Where(x => x.TypeFournisseur == 5);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFournisseursCarburant(int client)
        {
            var list = FournisseurService.Read(client).Where(x => x.TypeFournisseur == 6);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVehicules(int client)
        {
            var myUser = new KPIPrincipal();
            DepartementsAccessRightsModel access = null;
            if (myUser.Profile > 2)
            {
                access = myUser.DepartementsAccessRights;
            }
            return Json(VehiculeService.Read(0, client, access), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVehiculesParDep(int client, int departement)
        {
            var myUser = new KPIPrincipal();
            DepartementsAccessRightsModel access = null;
            if (myUser.Profile > 1)
            {
                client = myUser.Client;
            }
            if (myUser.Profile > 2)
            {
                access = myUser.DepartementsAccessRights;
            }
            return Json(VehiculeService.Read(departement, client, access), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRoles(int profile)
        {
            var roles = RoleService.GetAll();
            if (profile > 1)
            {
                roles = roles.Where(x => x.ProfileRole >= profile).ToList();
            }
            return Json(roles, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClients()
        {
            return Json(ClientService.Read(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPieces(int? client)
        {
            int c = 0;
            if (client.HasValue)
            {
                c = client.Value;
            }
            return Json(PieceService.Read(c), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEntretienMaitres(int client)
        {
            var list = EntretienMaitreService.Read(client);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}