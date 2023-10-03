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
    public class ParametresDepenseController : Controller
    {
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int client)
        {
            var myUser = new KPIPrincipal();
            if (myUser.Profile > 1)
            {
                client = myUser.Client;
            }
            return Json(ParametresDepenseService.Read(client).ToDataSourceResult(request));
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, ParametresDepenseModel model)
        {
            if (model.ClientId == 0)
            {
                var myUser = new KPIPrincipal();
                model.ClientId = myUser.Client;
            }
            if (ModelState.IsValid)
            {
                model = ParametresDepenseService.Create(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }


        public ActionResult Update([DataSourceRequest]DataSourceRequest request, ParametresDepenseModel model)
        {
            if (ModelState.IsValid)
            {
                ParametresDepenseService.Update(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, ParametresDepenseModel model)
        {
            if (ModelState.IsValid)
            {
                ParametresDepenseService.Delete(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

    }
}
