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
    public class ClientController : Controller
    {
        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(ClientService.Read().ToDataSourceResult(request));
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, ClientModel model)
        {
            if (ModelState.IsValid)
            {
                model = ClientService.Create(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, ClientModel model)
        {
            if (ModelState.IsValid)
            {
                ClientService.Update(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, ClientModel model)
        {
            if (ModelState.IsValid)
            {
                ClientService.Delete(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        } 
    }
}
