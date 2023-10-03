using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using GPA.Models;
using GPA.BLL;
using System.IO;

namespace GPA.Controllers
{
    public class EntretienController : Controller
    {
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int client, int departement, int annee, int mois)
        {
            var myUser = new KPIPrincipal();
            if (myUser.Profile > 1)
            {
                client = myUser.Client;
            }
            DepartementsAccessRightsModel access = null;
            if (myUser.Profile > 2)
            {
                access = myUser.DepartementsAccessRights;
            }
            return Json(EntretienService.Read(client, access, departement, annee, mois).ToDataSourceResult(request));
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, EntretienModel model)
        {
            if (model.ClientId == 0)
            {
                var myUser = new KPIPrincipal();
                model.ClientId = myUser.Client;
            }
            if (ModelState.IsValid)
            {
                model = EntretienService.Create(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, EntretienModel model)
        {
            if (ModelState.IsValid)
            {
                EntretienService.Update(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, EntretienModel model)
        {
            if (ModelState.IsValid)
            {
                EntretienService.Delete(model);
                DepenseService.DeleteDepensesEntretien(model.IdEntretien);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult Import(IEnumerable<HttpPostedFileBase> FichierImport)
        {
            if (FichierImport != null && FichierImport.Count() == 1)
            {
                var file = FichierImport.First();

                string[] formats = new string[] { ".csv" };
                string fileFormat = Path.GetExtension(file.FileName);
                if (formats.Contains(fileFormat))
                {
                    string fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Uploads/Imports"), fileName);
                    file.SaveAs(physicalPath);

                    var myUser = new KPIPrincipal();
                    int importedRows = EntretienService.Import(physicalPath, fileFormat, myUser.Client);
                    return Json(new { message = "1", importedRows = importedRows }, "text/plain");
                }
                else
                {
                    return Json(new { message = "Error file format!", }, "text/plain");
                }
            }
            else
            {
                return Json(new { message = "File not found!", }, "text/plain");
            }
        }
        [HttpPost]
        public ActionResult DeleteAll(string clientId, string annee, string mois)
        {
            var client = int.Parse(clientId);
            var year = int.Parse(annee);
            var month = int.Parse(mois);
            if (client == 0)
            {
                var myUser = new KPIPrincipal();

                client = myUser.Client;
            }
            if (ModelState.IsValid)
            {
                EntretienService.DeleteAll(client,year,month);
            }
            return Json(new { message = "Toutes les données sont supprimées!", }, "text/plain");
        }
    }
}
