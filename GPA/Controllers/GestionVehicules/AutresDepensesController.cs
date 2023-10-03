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
    public class AutresDepensesController : Controller
    {
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int departement, int client,int annee, int mois, int vehicule)
        {
            var myUser = new KPIPrincipal();
            if (myUser.Profile > 1)
            {
                client = myUser.Client;
            }
            return Json(AutresDepenseService.Read(departement, client, annee, mois, vehicule).ToDataSourceResult(request));
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, AutresDepenseModel model)
        {
            
            if (ModelState.IsValid)
            {
                model = AutresDepenseService.Create(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }


        public ActionResult Update([DataSourceRequest]DataSourceRequest request, AutresDepenseModel model)
        {
            if (ModelState.IsValid)
            {
                AutresDepenseService.Update(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, AutresDepenseModel model)
        {
            if (ModelState.IsValid)
            {
                AutresDepenseService.Delete(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

    }
}
