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
    public class ModeleC100KMController : Controller
    {
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int client)
        {
            var myUser = new KPIPrincipal();
            if (myUser.Profile > 1)
            {
                client = myUser.Client;
            }
            return Json( ModeleC100KMService.Read(client).ToDataSourceResult(request));
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request,  ModeleC100KMModel model)
        {
            if (model.ClientId == 0)
            {
                var myUser = new KPIPrincipal();
                model.ClientId = myUser.Client;
            }
            if (ModelState.IsValid)
            {
                model =  ModeleC100KMService.Create(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request,  ModeleC100KMModel model)
        {
            if (ModelState.IsValid)
            {
                 ModeleC100KMService.Update(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request,  ModeleC100KMModel model)
        {
            if (ModelState.IsValid)
            {
                 ModeleC100KMService.Delete(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        } 
    }
}
