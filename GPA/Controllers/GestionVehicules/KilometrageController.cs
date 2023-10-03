using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using GPA.Models;
using GPA.BLL;
using Microsoft.Reporting.WebForms;
using System.IO;
namespace GPA.Controllers
{
    public class KilometrageController : Controller
    {
        #region Kilometrage journalier
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int departement, int client, int annee, int mois, int vehicule)
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
            return Json(LectureKilometrageService.Read(departement, client, annee, mois, vehicule, access).ToList().OrderByDescending(x=>x.DateLectureKilometrage).ToDataSourceResult(request));
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, LectureKilometrageModel model)
        {
            if (model.ClientId == 0)
            {
                var myUser = new KPIPrincipal();
                model.ClientId = myUser.Client;
            }
            if (ModelState.IsValid)
            {
                model.Note = "Lors d'une mise à jour manuelle";
                model = LectureKilometrageService.Create(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult GetKilomteragePrecedentByIdVehicule(int idVehicule, DateTime date)
        {
            return Json(LectureKilometrageService.GetKilomteragePrecedentByIdVehicule(idVehicule, date), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, LectureKilometrageModel model)
        {
            if (ModelState.IsValid)
            {
                LectureKilometrageService.Update(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, LectureKilometrageModel model)
        {
            if (ModelState.IsValid)
            {
                LectureKilometrageService.Delete(model);    
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region Kilometrage Mensuel
        public ActionResult ReadMensuel([DataSourceRequest] DataSourceRequest request, int departement, int client, int annee, int mois, int vehicule)
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
            return Json(LectureKilometrageService.ReadMensuel(departement, client, annee, mois, vehicule, access).ToList().OrderByDescending(x => x.DateKilometrageMensuel).ToDataSourceResult(request));
        }

        public ActionResult CreateMensuel([DataSourceRequest]DataSourceRequest request, KilometrageMensuelModel model)
        {
            if (model.ClientId == 0)
            {
                var myUser = new KPIPrincipal();
                model.ClientId = myUser.Client;
            }
            if (ModelState.IsValid)
            {
               // model.Note = "Lors d'une mise à jour manuelle";
                model = LectureKilometrageService.CreateMensuel(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        
        public ActionResult UpdateMensuel([DataSourceRequest]DataSourceRequest request, KilometrageMensuelModel model)
        {
            if (ModelState.IsValid)
            {
                LectureKilometrageService.UpdateMensuel(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult DeleteMensuel([DataSourceRequest]DataSourceRequest request, KilometrageMensuelModel model)
        {
            if (ModelState.IsValid)
            {
                LectureKilometrageService.DeleteMensuel(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult DeleteAllMensuel(string clientId)
        {
            var client = int.Parse(clientId);
            if (client == 0)
            {
                var myUser = new KPIPrincipal();

                client = myUser.Client;
            }
            if (ModelState.IsValid)
            {
                LectureKilometrageService.DeleteAllMensuel(client);
            }
            return Json(new { message = "Toutes les données sont supprimées!", }, "text/plain");
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
                    int importedRows = LectureKilometrageService.Import(physicalPath, fileFormat, myUser.Client);
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

        #endregion

    }
}
