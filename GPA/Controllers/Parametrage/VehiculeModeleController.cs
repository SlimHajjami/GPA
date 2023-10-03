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
    public class VehiculeModeleController : Controller
    {
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int codRef)
        {
        
            return Json(VehiculeModeleService.Read(codRef).ToDataSourceResult(request));
        }

        

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, VehiculeModeleModel model, int codRef)
        {

            if (ModelState.IsValid)
            {
                model = VehiculeModeleService.Create(model, codRef);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, VehiculeModeleModel model)
        {
            if (ModelState.IsValid)
            {
                VehiculeModeleService.Update(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, VehiculeModeleModel model)
        {
            if (ModelState.IsValid)
            {
                VehiculeModeleService.Delete(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

    }
}
