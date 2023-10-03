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
    public class EntretienMaitreController : Controller
    {
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int client)
        {
            var myUser = new KPIPrincipal();
            if (myUser.Profile > 1)
            {
                client = myUser.Client;
            }
            return Json(EntretienMaitreService.Read(client).ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult Get([DataSourceRequest] DataSourceRequest request, int id)
        {
            return Json(EntretienMaitreService.Get(id));
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, EntretienMaitreModel model)
        {
            if (model.ClientId == 0)
            {
                var myUser = new KPIPrincipal();
                model.ClientId = myUser.Client;
            }
            if (ModelState.IsValid)
            {
                model = EntretienMaitreService.Create(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, EntretienMaitreModel model)
        {
            if (ModelState.IsValid)
            {
                EntretienMaitreService.Update(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, EntretienMaitreModel model)
        {
            if (ModelState.IsValid)
            {
                EntretienMaitreService.Delete(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        } 
    }
}
