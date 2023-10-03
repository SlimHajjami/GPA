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
    public class EntretienProgrammeController : Controller
    {
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int client, int departement, int statut, int annee, int mois)
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
            return Json(EntretienProgrammeService.Read(client, access, departement, statut, annee, mois).ToDataSourceResult(request));
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, EntretienProgrammeModel model)
        {
            if (model.ClientId == 0)
            {
                var myUser = new KPIPrincipal();
                model.ClientId = myUser.Client;
            }
            if (ModelState.IsValid)
            {
                model = EntretienProgrammeService.Create(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, EntretienProgrammeModel model)
        {
            if (ModelState.IsValid)
            {
                EntretienProgrammeService.Update(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, EntretienProgrammeModel model)
        {
            if (ModelState.IsValid)
            {
                EntretienProgrammeService.Delete(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Execute(int idEntretienProgramme, EntretienModel entretien)
        {
            int res = 0;

            EntretienService.Create(entretien);
            int kilo = 0;
            if (entretien.KilometrageEntretien.HasValue)
            {
                kilo = entretien.KilometrageEntretien.Value;
            }

            bool result = EntretienProgrammeService.UpdateAfterExecute(idEntretienProgramme, entretien.DateEntretien, kilo);
            if (result)
            {
                res = 1;
            }

            return Content(res.ToString());
        }
    }
}
