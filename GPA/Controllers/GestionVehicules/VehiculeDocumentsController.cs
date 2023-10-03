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
    public class VehiculeDocumentsController : BaseController
    {
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int departement, int client, int type, int annee, int mois)
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
            return Json(VehiculeDocumentsService.Read(departement, client, access, type,annee, mois).ToDataSourceResult(request));
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, VehiculeDocumentsModel model)
        {
            if (ModelState.IsValid)
            {
                model = VehiculeDocumentsService.Create(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, VehiculeDocumentsModel model)
        {
            if (ModelState.IsValid)
            {
                VehiculeDocumentsService.Update(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, VehiculeDocumentsModel model)
        {
            if (ModelState.IsValid)
            {
                VehiculeDocumentsService.Delete(model);
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
                string path = Path.Combine(Server.MapPath("~/Reports/GestionVehicules"), "Documents.rdlc");
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

                List<VehiculeDocumentsModel> list = VehiculeDocumentsService.Read(departement, client, access, 0, 0, 0);
                ReportDataSource rd = new ReportDataSource("DocumentsDataSet", list);
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
    }
}
