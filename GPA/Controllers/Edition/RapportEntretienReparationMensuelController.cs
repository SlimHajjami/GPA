using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GPA.Models;
using GPA.BLL;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Globalization;

namespace GPA.Controllers
{
    public class RapportEntretienReparationMensuelController : BaseController
    {
        public ActionResult Export(string type, int departement, int client, int annee, int mois)
        {
            try
            {
                if (!IsUserLoggedIn())
                {
                    return RedirectToAction("Login", "Account");
                }
                LocalReport lr = new LocalReport();
                string path = Path.Combine(Server.MapPath("~/Reports/Edition"), "RapportEntretienReparationMensuel.rdlc");
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
                bool mensuelImported = false;
                if (myUser.Profile > 2)
                {
                    access = myUser.DepartementsAccessRights;
                }
                if (myUser.Profile > 1)
                {
                    mensuelImported = myUser.Rights.Contains(Enumerators.AccessRights.IMPORT_KILOMETRAGE_MENSUEL);
                }

                List<RapportEntretienReparationMensuelModel> list = RapportEntretienReparationMensuelService.Read(client, access, mensuelImported, departement, annee, mois);
                ReportDataSource rd = new ReportDataSource("ConsommationDataSet", list);
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

                string fullMonthName = new DateTime(annee, mois, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("fr"));
                ps = ps.Concat(new[] { new ReportParameter("periode", "Mois: " + fullMonthName + "  " + annee) });

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

                Response.AddHeader("content-disposition", "attachment; filename=entretien_reparation_mensuel." + fileNameExtension);
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
