using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace GPA.Models
{
    public static class Enumerators
    {
        public enum AccessRights
        {
            Nothing = 0,

            #region Modules

            [Display(Name = "Administration")]
            MODULES_ADMINISTRATION = 1,

            [Display(Name = "Paramétrage")]
            MODULES_PARAMETRAGE = 2,

            [Display(Name = "Gestion véhicules")]
            MODULES_GESTION_VEHICULES = 3,

            [Display(Name = "Entretiens")]
            MODULES_ENTRETIENS = 4,

            [Display(Name = "Réparations")]
            MODULES_REPARATIONS = 5,

            [Display(Name = "Carburant")]
            MODULES_CARBURANT = 6,

            [Display(Name = "Editions")]
            MODULES_EDITIONS = 7,

            [Display(Name = "Alertes")]
            MODULES_ALERTES = 8,

            #endregion

            #region Administration------------------------

            [Display(Name = "Clients")]
            CLIENTS = 21,

            [Display(Name = "Utilisateurs")]
            USERS = 22,

            [Display(Name = "Rôles")]
            ROLES = 23,

            [Display(Name = "Historique de connexion")]
            HISTORIQUE = 24,

            #endregion

            #region Gestion Vehicules

            [Display(Name = "Conducteurs")]
            CONDUCTEURS = 61,

            [Display(Name = "Départements")]
            DEPARTEMENTS = 62,

            [Display(Name = "Véhicules")] 
            VEHICULES = 63,

            [Display(Name = "Achats / Locations")] 
            ACQUISITIONS = 64,

            [Display(Name = "Assurances / Taxes / Visites")]
            DOCUMENTS = 65,

            [Display(Name = "Kilométrages journaliers")]
            KILOMETRAGES = 66,

            [Display(Name = "Kilométrage mensuel")]
            KILOMETRAGE_MENSUEL = 67,

            [Display(Name = "Import kilométrage mensuel")]
            IMPORT_KILOMETRAGE_MENSUEL = 68,
            [Display(Name = "Autres dépenses")]
            AUTRES_DEPENSES = 69,
            #endregion

            #region Paramétrage

            [Display(Name = "Référentiels")]
            REFERENTIELS = 81,

            [Display(Name = "Fournisseurs")]
            FOURNISSEURS = 82,

            [Display(Name = "Pièces")]
            PIECES = 83,

            [Display(Name = "Alertes par email")]
            ALERTE_EMAIL = 84,

            [Display(Name = "Modèles C/100KM")]
            MODELE_C100KM = 85,

            [Display(Name = "GIS/GPA")]
            GIS_GPA = 86,

            [Display(Name = "Dépenses")]
            DEPENSES = 87,

            #endregion

            #region Gestion Entretiens

            [Display(Name = "Entretiens")]
            ENTRETIENS = 101,

            [Display(Name = "Import entretiens réparations")]
            IMPORT_ENTRETIENS_REPARATIONS = 102,

            [Display(Name = "Entretiens maîtres")]
            ENTRETIENS_MAITRES = 103,

            [Display(Name = "Programme d'entretien")]
            PROGRAMME_ENTRETIEN = 104,

            #endregion

            #region Gestion Réparations

            [Display(Name = "Réparations")]
            REPARATIONS = 121,

            #endregion

            #region Gestion Carburant

            [Display(Name = "Carburant")]
            CARBURANT = 141,

            #endregion

            #region Edition

            [Display(Name = "Rapport dépenses")]
            RAPPORT_DEPENSES = 161,

            [Display(Name = "Rapport dépenses global")]
            RAPPORT_DEPENSES_GLOBAL = 162,

            [Display(Name = "Rapport carburant mensuel")]
            RAPPORT_CONSOMMATION_CARBURANT_MENSUEL = 163,

            [Display(Name = "Rapport carburant semestrielle")]
            RAPPORT_CONSOMMATION_CARBURANT_SEMESTRIELLE = 164,

            [Display(Name = "Rapport entretien réparation mensuel")]
            RAPPORT_ENTRETIEN_REPARATION_MENSUEL = 165,

            [Display(Name = "Rapport global")]
            RAPPORT_GLOBAL = 166,

            [Display(Name = "Rapport entretien réparation")]
            RAPPORT_ENTRETIEN_REPARATION = 167,

            [Display(Name = "Rapport dépenses détaillées")]
            RAPPORT_DEPENSES_DETAILLEES = 168,

            #endregion

            #region Alertes

            [Display(Name = "Alertes assurance")]
            ALERTES_ASSURANCE = 181,

            [Display(Name = "Alertes taxe circulation")]
            ALERTES_TAXE_CIRCULATION = 182,

            [Display(Name = "Alertes visite technique")]
            ALERTES_VISITE_TECHNIQUE = 183,

            [Display(Name = "Alertes entretien")]
            ALERTES_ENTRETIEN = 184,

            #endregion
        }
    }
}
