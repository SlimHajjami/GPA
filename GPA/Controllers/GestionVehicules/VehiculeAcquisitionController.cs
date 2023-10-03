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
    public class VehiculeAcquisitionController : Controller
    {
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int departement, int client)
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
            return Json(VehiculeAcquisitionService.Read(departement, client, access).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, VehiculeAcquisitionModel model)
        {
            if (ModelState.IsValid)
            {
                model = VehiculeAcquisitionService.Create(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, VehiculeAcquisitionModel model)
        {
            if (ModelState.IsValid)
            {
                VehiculeAcquisitionService.Update(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, VehiculeAcquisitionModel model)
        {
            if (ModelState.IsValid)
            {
                VehiculeAcquisitionService.Delete(model);
                VehiculeAcquisitionPaiementService.Delete(model.IdAcquisition);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        } 
    }
}
