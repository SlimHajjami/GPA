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
    public class FournisseurController : Controller
    {
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int client)
        {
            var myUser = new KPIPrincipal();
            if (myUser.Profile > 1)
            {
                client = myUser.Client;
            }
            return Json(FournisseurService.Read(client).ToDataSourceResult(request));
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, FournisseurModel model)
        {
            if (model.ClientId == 0)
            {
                var myUser = new KPIPrincipal();
                model.ClientId = myUser.Client;
            }
            if (ModelState.IsValid)
            {
                model = FournisseurService.Create(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public string CreateFromEntretien([DataSourceRequest] DataSourceRequest request, int client, string nom, int type, string adresse, string ville, string codePostal, string contact, string tel1, string tel2, string fax, string email, string siteWeb)
        {
            int clientId = 0;
            if (client == 0)
            {
                var myUser = new KPIPrincipal();
                clientId = myUser.Client;
            }
            else
            {
                clientId = client;
            }
            FournisseurModel fournisseur = new FournisseurModel()
            {
                ClientId = clientId,
                NomFournisseur = nom,
                TypeFournisseur = type,
                AdresseFournisseur = adresse,
                VilleFournisseur = ville,
                CodePostalFournisseur = codePostal,
                ContactFournisseur = contact,
                Tel1Fournisseur = tel1,
                Tel2Fournisseur = tel2,
                FaxFournisseur = fax,
                EmailFournisseur = email,
                SiteWebFournisseur = siteWeb,
            };
            FournisseurService.Create(fournisseur);
            return "";
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, FournisseurModel model)
        {
            if (ModelState.IsValid)
            {
                FournisseurService.Update(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, FournisseurModel model)
        {
            if (ModelState.IsValid)
            {
                FournisseurService.Delete(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        } 
    }
}
