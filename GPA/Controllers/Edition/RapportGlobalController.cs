﻿using System;
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
    public class RapportGlobalController : BaseController
    {
        public ActionResult Export(string type, int client, int annee, int mois)
        {
            try
            {
                if (!IsUserLoggedIn())
                {
                    return RedirectToAction("Login", "Account");
                }
                LocalReport lr = new LocalReport();
                string path = Path.Combine(Server.MapPath("~/Reports/Edition"), "RapportGlobal.rdlc");
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

                List<RapportGlobalModel> list = RapportGlobalService.Read(client, access, annee, mois);
                ReportDataSource rd = new ReportDataSource("RapportGlobalDataSet", list);
                lr.DataSources.Add(rd);

                List<BarChartModel> list2 = RapportGlobalService.getBarChart(client, access, annee, mois);
                ReportDataSource rd2 = new ReportDataSource("BarChartDataSet", list2);
                lr.DataSources.Add(rd2);

                List<PieChartModel> list3 = RapportGlobalService.getPieChart(client, access, annee, mois);
                ReportDataSource rd3 = new ReportDataSource("PieChartDataSet", list3);
                lr.DataSources.Add(rd3);


                IEnumerable<ReportParameter> ps = Enumerable.Empty<ReportParameter>();

                DateTime currentMonthDate = new DateTime(annee, mois, 1);

                string m4 = currentMonthDate.ToString("MMMM", CultureInfo.CreateSpecificCulture("fr"));
                string m3 = currentMonthDate.AddMonths(-1).ToString("MMMM", CultureInfo.CreateSpecificCulture("fr"));
                string m2 = currentMonthDate.AddMonths(-2).ToString("MMMM", CultureInfo.CreateSpecificCulture("fr"));
                string m1 = currentMonthDate.AddMonths(-3).ToString("MMMM", CultureInfo.CreateSpecificCulture("fr"));

                ps = ps.Concat(new[] { new ReportParameter("year", currentMonthDate.Year.ToString()) });
                ps = ps.Concat(new[] { new ReportParameter("m1", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(m1)) });
                ps = ps.Concat(new[] { new ReportParameter("m2", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(m2)) });
                ps = ps.Concat(new[] { new ReportParameter("m3", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(m3)) });
                ps = ps.Concat(new[] { new ReportParameter("m4", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(m4)) });

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

                Response.AddHeader("content-disposition", "attachment; filename=rapport_global." + fileNameExtension);
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
