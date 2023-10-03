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
    public class VehiculeController : BaseController
    {
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int departement, int client)
        {
            var myUser = new KPIPrincipal();
            if (myUser.Profile > 1)
            {
                client = myUser.Client;
            }
            DepartementsAccessRightsModel access = null;
            if(myUser.Profile > 2)
            {
                access = myUser.DepartementsAccessRights;
            }
            return Json(VehiculeService.Read(departement, client, access).ToDataSourceResult(request));
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, VehiculeModel model)
        {
            var v = VehiculeService.Exist(model.MatriculeVehicule);
            if (v != null)
            {
                ModelState.AddModelError("MatriculeVehicule", "Matricule qui existe déja!");
            }
            if (model.ClientId == 0)
            {
                var myUser = new KPIPrincipal();
                model.ClientId = myUser.Client;
            }
            if (ModelState.IsValid)
            {
                model = VehiculeService.Create(model);
                VehiculeAcquisitionService.Create(model.IdVehicule);
                VehiculeDocumentsService.Create(model.IdVehicule);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, VehiculeModel model)
        {
            if (ModelState.IsValid)
            {
                VehiculeService.Update(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, VehiculeModel model)
        {
            if (ModelState.IsValid)
            {
                VehiculeService.Delete(model);
                VehiculeAcquisitionService.Delete(model.IdVehicule);
                VehiculeDocumentsService.Delete(model.IdVehicule);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Export(string type, int departement, int client)
        {
            try
            {
                if (!IsUserLoggedIn())
                {
                    return RedirectToAction("Login", "Account");
                }
                LocalReport lr = new LocalReport();
                string path = Path.Combine(Server.MapPath("~/Reports/GestionVehicules"), "Vehicules.rdlc");
                lr.EnableExternalImages = true;
                if (System.IO.File.Exists(path))
                {
                    lr.ReportPath = path;
                }
                else
                {
                    return View("Read");
                }

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

                List<VehiculeModel> list = VehiculeService.Read(departement, client, access);
                ReportDataSource rd = new ReportDataSource("VehiculesDataSet", list);
                lr.DataSources.Add(rd);


                IEnumerable<ReportParameter> ps = Enumerable.Empty<ReportParameter>();
                
                var d = VehiculeDepartementService.Get(departement);
                if (d != null)
                {
                    ps = ps.Concat(new[] { new ReportParameter("departement", "Département: " + d.NomDepartement) });
                }
                else
                {
                    ps = ps.Concat(new[] { new ReportParameter("departement", "") });
                }

                if (myUser.Profile == 1)
                {
                    var c = ClientService.Get(client);
                    if (c != null)
                    {
                        ps = ps.Concat(new[] { new ReportParameter("client", "Client: " + c.NomClient) });
                    }
                    else
                    {
                        ps = ps.Concat(new[] { new ReportParameter("client", "") });
                    }
                }
                else
                {
                    ps = ps.Concat(new[] { new ReportParameter("client", "") });
                }

                lr.SetParameters(ps);
                lr.Refresh();
                string reportType = type;
                string mimeType;
                string encoding;
                string fileNameExtension;

                string deviceInfo =
                "<DeviceInfo>" +
                "  <OutputFormat>" + reportType + "</OutputFormat>" +
                "  <PageWidth>11.6in</PageWidth>" +
                "  <PageHeight>8.2in</PageHeight>" +
                "  <MarginTop>0.5in</MarginTop>" +
                "  <MarginLeft>0in</MarginLeft>" +
                "  <MarginRight>0in</MarginRight>" +
                "  <MarginBottom>0.5in</MarginBottom>" +
                "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                return File(renderedBytes, mimeType);
            }
            catch (Exception ex)
            {
                string exp = ex.InnerException.ToString();
                return null;
            }
        }

        [HttpPost]
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> FichierImport)
        {
            if (FichierImport != null && FichierImport.Count() == 1)
            {
                var file = FichierImport.First();

                string[] formats = new string[] { ".xls", ".xlsx" };
                string fileFormat = Path.GetExtension(file.FileName);
                if (formats.Contains(fileFormat))
                {
                    string fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Uploads/Imports"), fileName);
                    file.SaveAs(physicalPath);

                    List<string> columns = VehiculeService.getImportedFileColumns(physicalPath, fileFormat);
                    return Json(new { message = "1", file = fileName, columns = columns }, "text/plain");
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
        public ActionResult Import(string file, VehiculeColumns columns)
        {
            string fileFormat = Path.GetExtension(file);
            string fileName = Path.GetFileName(file);
            var physicalPath = Path.Combine(Server.MapPath("~/Uploads/Imports"), fileName);

            var myUser = new KPIPrincipal();
            int importedArticles = VehiculeService.Import(physicalPath, fileFormat, columns, myUser.Client);

            return Content(importedArticles.ToString());
        }
    }
}
