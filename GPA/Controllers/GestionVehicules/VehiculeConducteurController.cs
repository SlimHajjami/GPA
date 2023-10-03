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
    public class VehiculeConducteurController : Controller
    {
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int client)
        {
            var myUser = new KPIPrincipal();
            if (myUser.Profile > 1)
            {
                client = myUser.Client;
            }
            return Json(VehiculeConducteurService.Read(client).ToDataSourceResult(request));
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, VehiculeConducteurModel model)
        {
            if (model.ClientId == 0)
            {
                var myUser = new KPIPrincipal();
                model.ClientId = myUser.Client;
            }
            if (ModelState.IsValid)
            {
                model = VehiculeConducteurService.Create(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, VehiculeConducteurModel model)
        {
            if (ModelState.IsValid)
            {
                VehiculeConducteurService.Update(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, VehiculeConducteurModel model)
        {
            if (ModelState.IsValid)
            {
                VehiculeConducteurService.Delete(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        } 
    }
}
