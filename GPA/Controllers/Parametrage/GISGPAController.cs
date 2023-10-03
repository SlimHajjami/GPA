using Kendo.Mvc.UI;
using GPA.Models;
using GPA.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;



namespace GPA.Controllers
{
    public class GISGPAController : Controller
    {



        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int departement, int client)
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
            return Json(GISGPAService.Read(departement, client, access).ToDataSourceResult(request));
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, GISGPAModel model)
        {
            //var v = VehiculeService.Exist(model.VehiculeGPAMat);
            //if (v != null)
            //{
            //    ModelState.AddModelError("MatriculeVehicule", "Matricule qui existe déja!");
            //}
            if (model.ClientId == 0)
            {
                var myUser = new KPIPrincipal();
                model.ClientId = myUser.Client;
            }
            if (ModelState.IsValid)
            {
                model = GISGPAService.Create(model);
               
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, GISGPAModel model)
        {
            //if (model.ClientId == 0)
            //{
            //    var myUser = new KPIPrincipal();
            //    model.ClientId = myUser.Client;
            //}
            if (ModelState.IsValid)
            {
                GISGPAService.Update(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, GISGPAModel model)
        {
            if (ModelState.IsValid)
            {
                GISGPAService.Delete(model);
                
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult GetGIS(List<String> list, int? client)
        {
            return Json(GISGPAService.ReadFromGis((int)client), JsonRequestBehavior.AllowGet);
        }


        public ActionResult UpdateCreateDelete(
            [Bind(Prefix = "updated")]List<GISGPAModel> updated,
            [Bind(Prefix = "new")]List<GISGPAModel> added,
            [Bind(Prefix = "deleted")]List<GISGPAModel> deleted)
        {
            if (updated != null && updated.Count > 0)
            {
                for (int i = 0; i < updated.Count; i++)
                {
                    GISGPAService.Update(updated[i]);
                    
                }
            }

            if (added != null && added.Count > 0)
            {
                for (int i = 0; i < added.Count; i++)
                {
                    if (added[i].ClientId == 0)
                    {
                        var myUser = new KPIPrincipal();
                        added[i].ClientId = myUser.Client;
                    }
                    GISGPAService.Create(added[i]);
                  
                }

            }

            if (deleted != null && deleted.Count > 0)
            {
                for (int i = 0; i < deleted.Count; i++)
                {
                    GISGPAService.Delete(deleted[i]);
                }
            }

            return Json("Success!");
        }
    }
}
