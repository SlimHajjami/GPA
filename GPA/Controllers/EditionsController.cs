using GPB.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GPB.Controllers
{
    [Authorize]
    public class EditionsController : BaseController
    {
        #region Tableau de bord -----------------

        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region Approvisionnement --------------

        public ActionResult RapportRecapitulatifApprovisionnement()
        {
            return View();
        }

        #endregion

        #region SuiviTravaux ---------------

        public ActionResult RapportEtatConsommation()
        {
            return View();
        }

        #endregion

        #region Béton ---------------------

        public ActionResult RapportVentes()
        {
            return View();
        }

        public ActionResult RapportRecapitulatifBeton()
        {
            return View();
        }

        #endregion

        #region Atelier d'armature ------------------

        public ActionResult RapportQuantiteEntree()
        {
            return View();
        }

        public ActionResult RapportRecapitulatifArmature()
        {
            return View();
        }

        #endregion

        #region Suivi matériels 

        public ActionResult RapportOutillages()
        {
            return View();
        }

        public ActionResult RapportEngins()
        {
            return View();
        }

        #endregion

        #region Suivi Financier ---------------

        public ActionResult RapportReglementEntreprise()
        {
            ViewBag.ListEntreprises = new EntrepriseService().GetEntreprises();
            return View();
        }

        public ActionResult RapportDepenses()
        {
            ViewBag.ListTypes = cmbBoxsService.GetReferentiel("TYPE_DEPENSE");
            return View();
        }

        public ActionResult RapportRecapitulatifSuiviFinancier()
        {
            return View();
        }

        #endregion

        #region RH ----------------------

        public ActionResult RapportEmployees()
        {
            return View();
        }

        public ActionResult RapportRecrutements()
        {
            return View();
        }
        #endregion

    }
}
