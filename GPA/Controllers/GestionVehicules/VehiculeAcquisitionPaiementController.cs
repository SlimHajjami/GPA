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
    public class VehiculeAcquisitionPaiementController : Controller
    {
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int acquisition)
        {
            return Json(VehiculeAcquisitionPaiementService.Read(acquisition).ToDataSourceResult(request));
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, VehiculeAcquisitionPaiementModel model)
        {
            if (ModelState.IsValid)
            {
                model = VehiculeAcquisitionPaiementService.Create(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, VehiculeAcquisitionPaiementModel model)
        {
            if (ModelState.IsValid)
            {
                VehiculeAcquisitionPaiementService.Update(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, VehiculeAcquisitionPaiementModel model)
        {
            if (ModelState.IsValid)
            {
                VehiculeAcquisitionPaiementService.Delete(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        } 
    }
}
