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
    public class ReferentielController : Controller
    {
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, string key_ref)
        {
            if (string.IsNullOrEmpty(key_ref))
                return null;

            return Json(ReferentielService.Read(key_ref).ToDataSourceResult(request));
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, ReferentielViewModel model, string key_ref)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(key_ref))
            {
                model = ReferentielService.Create(model, key_ref);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, ReferentielViewModel model)
        {
            if (ModelState.IsValid)
            {
                ReferentielService.Update(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, ReferentielViewModel model)
        {
            if (ModelState.IsValid)
            {
                ReferentielService.Delete(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
    }
}
