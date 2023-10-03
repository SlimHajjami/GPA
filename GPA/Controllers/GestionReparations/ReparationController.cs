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
    public class ReparationController : Controller
    {
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int client, int departement, int annee, int mois)
        {
            var myUser = new KPIPrincipal();
            if (myUser.Profile > 1)
            {
                client = myUser.Client;
            }
            DepartementsAccessRightsModel access = null;
            if (myUser.Profile > 2)
            {
                access = myUser.DepartementsAccessRights;
            }
            return Json(ReparationService.Read(client, access, departement, annee, mois).ToDataSourceResult(request));
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, ReparationModel model)
        {
            if (model.ClientId == 0)
            {
                var myUser = new KPIPrincipal();
                model.ClientId = myUser.Client;
            }
            if(model.DateMiseService != null && model.DateReparation != null)
            {
                TimeSpan t = model.DateMiseService - model.DateReparation;
                model.HeuresArret = (decimal)t.TotalHours;
            }
            if (ModelState.IsValid)
            {
                model = ReparationService.Create(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, ReparationModel model)
        {
            if (model.DateMiseService != null && model.DateReparation != null)
            {
                TimeSpan t = model.DateMiseService - model.DateReparation;
                model.HeuresArret = (decimal)t.TotalHours;
            }
            if (ModelState.IsValid)
            {
                ReparationService.Update(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, ReparationModel model)
        {
            if (ModelState.IsValid)
            {
                ReparationService.Delete(model);
                DepenseService.DeleteDepensesReparation(model.IdReparation);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult DeleteAll(string clientId, string annee, string mois)
        {
            var client = int.Parse(clientId);
            var year = int.Parse(annee);
            var month = int.Parse(mois);
            if (client == 0)
            {
                var myUser = new KPIPrincipal();

                client = myUser.Client;
            }
            if (ModelState.IsValid)
            {
                ReparationService.DeleteAll(client, year, month);
            }
            return Json(new { message = "Toutes les données sont supprimées!", }, "text/plain");
        }
    }
}
