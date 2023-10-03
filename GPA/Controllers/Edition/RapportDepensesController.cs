using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GPA.Models;
using GPA.BLL;
using Microsoft.Reporting.WebForms;
using System.IO;

namespace GPA.Controllers
{
    public class RapportDepensesController : BaseController
    {
        public ActionResult Export(string type, int departement, int client, string from, string to)
        {
            try
            {
                DateTime d = new DateTime();
                Nullable<DateTime> start = null;               
                Nullable<DateTime> end = null;

                if (DateTime.TryParse(from, out d))
                {
                    start = d;
                }
                if (DateTime.TryParse(to, out d))
                {
                    end = d;
                }

                if (!IsUserLoggedIn())
                {
                    return RedirectToAction("Login", "Account");
                }
                LocalReport lr = new LocalReport();
                string path = Path.Combine(Server.MapPath("~/Reports/Edition"), "RapportDepenses.rdlc");
                lr.EnableExternalImages = true;
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

                List<RapportDepensesModel> list = RapportDepensesService.Read(client, access, departement, start, end);
                ReportDataSource rd = new ReportDataSource("DepensesDataSet", list);
                lr.DataSources.Add(rd);


                IEnumerable<ReportParameter> ps = Enumerable.Empty<ReportParameter>();
                var dep = VehiculeDepartementService.Get(departement);
                if (dep != null)
                {
                    ps = ps.Concat(new[] { new ReportParameter("departement", "Département: " + dep.NomDepartement) });
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

                if (start.HasValue && end.HasValue)
                {
                    ps = ps.Concat(new[] { new ReportParameter("periode", "Période: " + start.Value.ToShortDateString() + " - " + end.Value.ToShortDateString()) });
                }
                else
                {
                    ps = ps.Concat(new[] { new ReportParameter("periode", "") });
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

                Response.AddHeader("content-disposition", "attachment; filename=rapport_depenses." + fileNameExtension);
                return File(renderedBytes, mimeType);
            }
            catch (Exception ex)
            {
                string exp = ex.InnerException.ToString();
                return null;
            }
        }

        public ActionResult ExportGlobal(string type, int departement, int client, string from, string to)
        {
            try
            {
                DateTime d = new DateTime();
                Nullable<DateTime> start = null;
                Nullable<DateTime> end = null;

                if (DateTime.TryParse(from, out d))
                {
                    start = d;
                }
                if (DateTime.TryParse(to, out d))
                {
                    end = d;
                }

                if (!IsUserLoggedIn())
                {
                    return RedirectToAction("Login", "Account");
                }
                LocalReport lr = new LocalReport();
                string path = Path.Combine(Server.MapPath("~/Reports/Edition"), "RapportDepensesGlobal.rdlc");
                lr.EnableExternalImages = true;
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

                List<RapportDepensesModel> list = RapportDepensesService.Read(client, access, departement, start, end);
                ReportDataSource rd = new ReportDataSource("DepensesDataSet", list);
                lr.DataSources.Add(rd);


                IEnumerable<ReportParameter> ps = Enumerable.Empty<ReportParameter>();
                var dep = VehiculeDepartementService.Get(departement);
                if (dep != null)
                {
                    ps = ps.Concat(new[] { new ReportParameter("departement", "Département: " + dep.NomDepartement) });
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

                if (start.HasValue && end.HasValue)
                {
                    ps = ps.Concat(new[] { new ReportParameter("periode", "Période: " + start.Value.ToShortDateString() + " - " + end.Value.ToShortDateString()) });
                }
                else
                {
                    ps = ps.Concat(new[] { new ReportParameter("periode", "") });
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

                Response.AddHeader("content-disposition", "attachment; filename=rapport_depenses_global." + fileNameExtension);
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
