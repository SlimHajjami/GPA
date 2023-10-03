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
using Microsoft.Reporting.WebForms;
using System.Globalization;

namespace GPA.Controllers
{
    public class CarburantController : Controller
    {
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int client, int departement, int annee, int mois, int vehicule)
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
            return Json(CarburantService.Read(client, access, departement, annee, mois, vehicule).ToList().OrderByDescending(x=>x.DateCarburant).ToDataSourceResult(request));
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, CarburantModel model)
        {
            if (model.ClientId == 0)
            {
                var myUser = new KPIPrincipal();
                model.ClientId = myUser.Client;
            }
            bool entryexist = CarburantService.EntreeExiste(model);
            
            if (ModelState.IsValid)
            {
                if (CarburantService.EntreeExiste(model))
                {
                    ModelState.AddModelError("Error", "Une entrée avec la même combinaison (Véhicule, Date) existe déjà dans la base de données!");
                }
                else
                {
                    model = CarburantService.Create(model,false);
                }
                
            }           
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, CarburantModel model)
        {
            if (ModelState.IsValid)
            {
                CarburantService.Update(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, CarburantModel model)
        {
            if (ModelState.IsValid)
            {
                CarburantService.Delete(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult DeleteAll(string clientId)
        {
            var client = int.Parse(clientId);
            if (client == 0)
            {
                var myUser = new KPIPrincipal();

                client = myUser.Client;
            }
            if (ModelState.IsValid)
            {
                CarburantService.DeleteAll(client);
            }
            return Json(new { message = "Toutes les données sont supprimées!", }, "text/plain");
        }

        public ActionResult GetKilomterageDebutByIdVehicule(int idVehicule, DateTime dateCarburant)
        {
            return Json(CarburantService.GetKilomterageDebutByIdVehicule(idVehicule, dateCarburant), JsonRequestBehavior.AllowGet);
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
        public ActionResult Import(string file, CarburantColumns columns)
        {
            string fileFormat = Path.GetExtension(file);
            string fileName = Path.GetFileName(file);
            var physicalPath = Path.Combine(Server.MapPath("~/Uploads/Imports"), fileName);

            var myUser = new KPIPrincipal();
            int importedArticles = CarburantService.Import(physicalPath, fileFormat, columns, myUser.Client);

            return Content(importedArticles.ToString());
        }

        public ActionResult RapportConsommationCarburantMensuel(int annee, int mois, int departement, int client, string type)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = Path.Combine(Server.MapPath("~/Reports/Carburant"), "RapportConsommationCarburantMensuel2.rdlc");
                if (System.IO.File.Exists(path))
                {
                    lr.ReportPath = path;
                }
                else
                {
                    return Content("");
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
                List<RapportMensuelCarburantModel> list = CarburantService.Report(client, access, departement, annee, mois);

                ReportDataSource rd = new ReportDataSource("ConsommationDataSet", list);

                lr.DataSources.Add(rd);


                IEnumerable<ReportParameter> ps = Enumerable.Empty<ReportParameter>();
                var dep = VehiculeDepartementService.Get(departement);
                if (dep != null)
                {
                    ps = ps.Concat(new[] { new ReportParameter("Departement", "Département: " + dep.NomDepartement) });
                }
                else
                {
                    ps = ps.Concat(new[] { new ReportParameter("Departement", "") });
                }

                if (myUser.Profile == 1)
                {
                    var c = ClientService.Get(client);
                    if (c != null)
                    {
                        ps = ps.Concat(new[] { new ReportParameter("Client", "Client: " + c.NomClient) });
                    }
                    else
                    {
                        ps = ps.Concat(new[] { new ReportParameter("Client", "") });
                    }
                }
                else
                {
                    ps = ps.Concat(new[] { new ReportParameter("Client", "") });
                }

                string fullMonthName = new DateTime(annee, mois, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("fr"));
                ps = ps.Concat(new[] { new ReportParameter("Periode", "Mois: " + fullMonthName + "  " + annee) });

                lr.SetParameters(ps);

                string reportType = type;
                string mimeType;
                string encoding;
                string fileNameExtension;

                string deviceInfo =
                "<DeviceInfo>" +
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
    }
}
