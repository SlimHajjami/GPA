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
    public class CarburantCoutController : Controller
    {
        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {

            return Json(CarburantCoutService.Read().ToDataSourceResult(request));
        }  
        
        public ActionResult GetPrixUnitaireByIdVehicule(int idVehicule , DateTime dateCarburant)
        {
            return Json(CarburantCoutService.GetPrixUnitaireByIdVehicule(idVehicule, dateCarburant), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, CarburantCoutUnitaireModel model)
        {

            if (ModelState.IsValid)
            {
                model = CarburantCoutService.Create(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, CarburantCoutUnitaireModel model)
        {
            if (ModelState.IsValid)
            {
                CarburantCoutService.Update(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, CarburantCoutUnitaireModel model)
        {
            if (ModelState.IsValid)
            {
                CarburantCoutService.Delete(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

    }
}
