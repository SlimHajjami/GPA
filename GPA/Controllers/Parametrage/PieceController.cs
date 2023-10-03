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
    public class PieceController : Controller
    {
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int client)
        {
            var myUser = new KPIPrincipal();
            if (myUser.Profile > 1)
            {
                client = myUser.Client;
            }
            return Json(PieceService.Read(client).ToDataSourceResult(request));
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, PieceModel model)
        {
            if (model.ClientId == 0)
            {
                var myUser = new KPIPrincipal();
                model.ClientId = myUser.Client;
            }
            if (ModelState.IsValid)
            {
                model = PieceService.Create(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public string CreateFromEntretien([DataSourceRequest] DataSourceRequest request, int client, int fournisseur, string constructeur, string numero, string nom, decimal prix, DateTime datePrix)
        {
            int clientId = 0;
            if (client == 0)
            {
                var myUser = new KPIPrincipal();
                clientId = myUser.Client;
            }else
            {
                clientId = client;
            }
            PieceModel piece = new PieceModel()
            {
                ClientId = clientId,
                NumeroPiece = numero,
                NomPiece = nom,
                PrixPiece = prix,
                DatePrixPiece = datePrix,
                FournisseurId = fournisseur,
                Constructeur = constructeur,
            };
            PieceService.Create(piece);
            return "";
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, PieceModel model)
        {
            if (ModelState.IsValid)
            {
                PieceService.Update(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, PieceModel model)
        {
            if (ModelState.IsValid)
            {
                PieceService.Delete(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        } 
    }
}
