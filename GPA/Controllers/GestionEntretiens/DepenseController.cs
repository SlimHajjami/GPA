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
    public class DepenseController : Controller
    {
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int entretien, int reparation)
        {
            return Json(DepenseService.Read(entretien, reparation).ToDataSourceResult(request));
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, DepenseModel model)
        {
            if (ModelState.IsValid)
            {
                model = DepenseService.Create(model);
                if (model.EntretienId != null && model.EntretienId > 0)
                {
                    EntretienService.UpdateCout(model.EntretienId.Value);
                }
                else if (model.ReparationId != null && model.ReparationId > 0)
                {
                    ReparationService.UpdateCout(model.ReparationId.Value);
                }
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, DepenseModel model)
        {
            if (ModelState.IsValid)
            {
                DepenseService.Update(model);
                if (model.EntretienId != null && model.EntretienId > 0)
                {
                    EntretienService.UpdateCout(model.EntretienId.Value);
                }
                else if (model.ReparationId != null && model.ReparationId > 0)
                {
                    ReparationService.UpdateCout(model.ReparationId.Value);
                }
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, DepenseModel model)
        {
            if (ModelState.IsValid)
            {
                DepenseService.Delete(model);
                if (model.EntretienId != null && model.EntretienId > 0)
                {
                    EntretienService.UpdateCout(model.EntretienId.Value);
                }
                else if (model.ReparationId != null && model.ReparationId > 0)
                {
                    ReparationService.UpdateCout(model.ReparationId.Value);
                }
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        } 
    }
}
