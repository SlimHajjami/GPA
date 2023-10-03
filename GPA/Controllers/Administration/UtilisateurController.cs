using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using GPA.Models;
using GPA.BLL;

namespace GPA.Controllers
{
    public class UtilisateurController : Controller
    {
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int client)
        {
            var myUser = new KPIPrincipal();
            if (myUser.Profile > 1)
            {
                client = myUser.Client;
            }
            return Json(UtilisateurService.Read(myUser.Profile, client).ToDataSourceResult(request));
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, UtilisateurModel model)
        {
            var v = UtilisateurService.GetByUsername(model.LoginUtilisateur);
            if (v != null)
            {
                ModelState.AddModelError("LoginUtilisateur", "Login déja utilisé!");
            }
            if(model.ClientId == null || model.ClientId == 0)
            { 
                var myUser = new KPIPrincipal();
                model.ClientId = myUser.Client;
            }
            if (ModelState.IsValid)
            {
                model = UtilisateurService.Create(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, UtilisateurModel model)
        {
            if (ModelState.IsValid)
            {
                UtilisateurService.Update(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, UtilisateurModel model)
        {
            if (ModelState.IsValid)
            {
                UtilisateurService.Delete(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult ReadDepartementsAccessRights([DataSourceRequest]DataSourceRequest request, int? idType, int idUtilisateur)
        {
            Session["_userId"] = idUtilisateur;
            int client = 0;
            var myUser = new KPIPrincipal();
            if (myUser.Profile > 1)
            {
                client = myUser.Client;
            }
            return Json(AccessRightsService.GetDepartementAccessRights(idUtilisateur, client).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateDepartementsAccessRights([DataSourceRequest]DataSourceRequest request, AccessRightDepartementModel model)
        {
            int idUtilisateur = Convert.ToInt32(Session["_userId"]);
            AccessRightsService.DepartementAccessRightsUpdate(idUtilisateur, model);
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}