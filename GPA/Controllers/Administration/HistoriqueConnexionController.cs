using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GPA.Models;
using GPA.BLL;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace GPA.Controllers
{
    public class HistoriqueConnexionController : Controller
    {
        public ActionResult GetHistorique([DataSourceRequest] DataSourceRequest request, int? annee, int? mois)
        {
          //  HistoriqueConnexionService historiqueConnexionService = new HistoriqueConnexionService();
            return Json(HistoriqueConnexionService.GetHistorique(annee, mois).ToList().OrderByDescending(x=>x.DateLogIn).ToDataSourceResult(request));

        }
    }
}
