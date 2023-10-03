using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GPA.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Data;
using GPA.BLL;
using GPA.Filters;


namespace GPA.Controllers
{
    public class RoleController : BaseController
    {       
        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {

            return Json(RoleService.GetAll().ToDataSourceResult(request));
        }

        public ActionResult FilterMenuCustomization_Roles()
        {
            return Json(RoleService.GetLabelsForMenu(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, RoleModel role)
        {
            if (ModelState.IsValid)
            {
                RoleService.Create(role);
            }
            return Json(new[] { role }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, RoleModel role)
        {
            if (ModelState.IsValid)
            {
                RoleService.Update(role);
            }
            return Json(new[] { role }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Delete([DataSourceRequest]DataSourceRequest request, RoleModel role)
        {
            if (ModelState.IsValid)
            {
                RoleService.Delete(role);
            }
            return Json(new[] { role }.ToDataSourceResult(request, ModelState));
        }
    }
}
