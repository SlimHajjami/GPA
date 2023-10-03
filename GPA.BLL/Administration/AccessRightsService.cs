using System.Collections.Generic;
using System.Data;
using System.Linq;
using GPA.DAL;
using GPA.Models;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace GPA.BLL
{
    public class AccessRightsService
    {
        public static bool Create(ref AccessRightModel role)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var myEntity = new AccessRight
                {
                    AccessRight1 = role.AccessRight,
                    RoleId = role.RoleId
                };

                context.AccessRights.Add(myEntity);
                objectAffected = context.SaveChanges();
                role.IdAccessRight = myEntity.IdAccessRight;
                return objectAffected > 0;
            }
        }

        public static bool Update(AccessRightModel role)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;
                var myEntity = context.AccessRights.FirstOrDefault(o => o.IdAccessRight == role.IdAccessRight);
                if (myEntity != null)
                {
                    myEntity.AccessRight1 = role.AccessRight;
                    myEntity.RoleId = role.RoleId;

                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }

        public static Enumerators.AccessRights[] GetByRoleId(int key)
        {
            using (Entities context = new Entities())
            {
                var myEntity = context.AccessRights.Where(item => item.RoleId == key).AsParallel().Select(item => item.AccessRightsAsEnum).ToArray();
                return myEntity;
            }
        }

        public static Dictionary<string, Dictionary<Enumerators.AccessRights, string>> AllAccessRights
        {
            get
            {
                var myRights = new Dictionary<string, Dictionary<Enumerators.AccessRights, string>>();
                myRights.Add("Modules", GetDisplayName(Modules));
               
                myRights.Add("Administration", GetDisplayName(Administration));
                myRights.Add("Paramétrage", GetDisplayName(Parametrage));
                myRights.Add("Gestion véhicules", GetDisplayName(GestionVehicules));
                myRights.Add("Entretiens", GetDisplayName(Entretiens));
                myRights.Add("Réparations", GetDisplayName(Reparations));
                myRights.Add("Carburant", GetDisplayName(Carburant));
                myRights.Add("Editions", GetDisplayName(Editions));
                myRights.Add("Alertes", GetDisplayName(Alertes));
                return myRights;
            }
        }

        public static Dictionary<Enumerators.AccessRights, string> GetDisplayName(List<Enumerators.AccessRights> list)
        {
            return list.ToDictionary(e => e, e => EnumHelper<Enumerators.AccessRights>.GetDisplayValue(e));
        }

        public static bool SetAccessRightsToRole(int key, Enumerators.AccessRights[] accessRights)
        {
            using (Entities context = new Entities())
            {
                int affectedObjects = 0;
                var oldAccessRightsList = context.AccessRights.Where(item => item.RoleId == key);
                context.AccessRights.RemoveRange(oldAccessRightsList);
                affectedObjects = context.SaveChanges();
                if (affectedObjects <= 0 && oldAccessRightsList.Any())
                {
                    return false;
                }

                ConcurrentQueue<AccessRight> newAccessRightList = new ConcurrentQueue<AccessRight>();
                Parallel.ForEach(accessRights, item =>
                {
                    var newObject = new AccessRight { RoleId = key, AccessRight1 = (int)item, IdAccessRight = 0 };
                    newAccessRightList.Enqueue(newObject);
                });
                context.AccessRights.AddRange(newAccessRightList);
                affectedObjects = context.SaveChanges();
                if (affectedObjects <= 0)
                {
                    return false;
                }
                return true;
            }
        }

        public static Enumerators.AccessRights[] GetAll()
        {
            return EnumHelper<Enumerators.AccessRights>.GetValues(Enumerators.AccessRights.Nothing).Where(item => item != Enumerators.AccessRights.Nothing).ToArray();
        }

        #region Departements Access Rights

        public static DepartementsAccessRightsModel GetDepartementsAccessRights(int key)
        {
            using (Entities context = new Entities())
            {
                DepartementsAccessRightsModel result = new DepartementsAccessRightsModel();

                var idsVisualisation = context.AccessRightDepartements.Where(item => item.UtilisateurId == key && item.Visualisation == true).ToList();
                if (idsVisualisation.Count != 0)
                {
                    foreach (var item in idsVisualisation)
                    {
                        result.Visualisation.Add((int)item.DepartementId);
                    }
                }
                var idsSaisie = context.AccessRightDepartements.Where(item => item.UtilisateurId == key && item.Saisie == true).ToList();
                if (idsSaisie.Count != 0)
                {
                    foreach (var item1 in idsSaisie)
                    {
                        result.Saisie.Add((int)item1.DepartementId);
                    }
                }
                var idsValidation = context.AccessRightDepartements.Where(item => item.UtilisateurId == key && item.Validation == true).ToList();
                if (idsValidation.Count != 0)
                {
                    foreach (var item2 in idsValidation)
                    {
                        result.Validation.Add((int)item2.DepartementId);
                    }
                }
                return result;
            }
        }

        public static List<AccessRightDepartementModel> GetDepartementAccessRights(int idUtilisateur, int client)
        {
            using (Entities context = new Entities())
            {
                List<AccessRightDepartementModel> results = new List<AccessRightDepartementModel>();
                var entities = VehiculeDepartementService.Read(client, null);
                if (entities.Count != 0)
                {
                    foreach (var entity in entities)
                    {
                        AccessRightDepartementModel result = new AccessRightDepartementModel()
                        {
                            Libelle = entity.NomDepartement,
                            DepartementId = entity.IdDepartement
                        };
                        if (context.AccessRightDepartements.Any(i => i.DepartementId == entity.IdDepartement && i.UtilisateurId == idUtilisateur))
                        {
                            var projetAccessRights = context.AccessRightDepartements.FirstOrDefault(i => i.DepartementId == entity.IdDepartement && i.UtilisateurId == idUtilisateur);
                            result.Visualisation = (bool)projetAccessRights.Visualisation;
                            result.Saisie = (bool)projetAccessRights.Saisie;
                            result.Validation = (bool)projetAccessRights.Validation;
                            result.IdAccessRightDepartement = projetAccessRights.IdAccessRightDepartement;
                        }
                        results.Add(result);
                    }
                }
                return results;
            }
        }

        public static void DepartementAccessRightsUpdate(int idUtilisateur, AccessRightDepartementModel model)
        {
            using (Entities context = new Entities())
            {
                if (model.IdAccessRightDepartement == 0)
                {
                    AccessRightDepartement entity = new AccessRightDepartement()
                    {
                        UtilisateurId = idUtilisateur,
                        DepartementId = model.DepartementId,
                        Saisie = model.Saisie,
                        Validation = model.Validation,
                        Visualisation = model.Visualisation
                    };
                    context.AccessRightDepartements.Add(entity);
                    context.SaveChanges();
                }
                else
                {
                    var entity = context.AccessRightDepartements.FirstOrDefault(i => i.IdAccessRightDepartement == model.IdAccessRightDepartement);
                    entity.Saisie = model.Saisie;
                    entity.Validation = model.Validation;
                    entity.Visualisation = model.Visualisation;
                    context.SaveChanges();
                }
            }
        }

        #endregion


        public static List<Enumerators.AccessRights> Modules = new List<Enumerators.AccessRights> 
        { 
            Enumerators.AccessRights.MODULES_ADMINISTRATION,
            Enumerators.AccessRights.MODULES_PARAMETRAGE,
            Enumerators.AccessRights.MODULES_GESTION_VEHICULES,
            Enumerators.AccessRights.MODULES_ENTRETIENS,
            Enumerators.AccessRights.MODULES_REPARATIONS,
            Enumerators.AccessRights.MODULES_CARBURANT,
            Enumerators.AccessRights.MODULES_EDITIONS,
            Enumerators.AccessRights.MODULES_ALERTES
        };

        public static List<Enumerators.AccessRights> Administration = new List<Enumerators.AccessRights> 
        { 
            Enumerators.AccessRights.CLIENTS,
            Enumerators.AccessRights.USERS,
            Enumerators.AccessRights.ROLES,
            Enumerators.AccessRights.HISTORIQUE
        };

        public static List<Enumerators.AccessRights> Parametrage = new List<Enumerators.AccessRights> 
        { 
            Enumerators.AccessRights.REFERENTIELS,
            Enumerators.AccessRights.FOURNISSEURS,
            Enumerators.AccessRights.PIECES,
            Enumerators.AccessRights.ALERTE_EMAIL,
            Enumerators.AccessRights.MODELE_C100KM,
            Enumerators.AccessRights.GIS_GPA,
            Enumerators.AccessRights.DEPENSES,

        };

        public static List<Enumerators.AccessRights> GestionVehicules = new List<Enumerators.AccessRights>
        {
            Enumerators.AccessRights.CONDUCTEURS,
            Enumerators.AccessRights.DEPARTEMENTS,
            Enumerators.AccessRights.VEHICULES,
            Enumerators.AccessRights.ACQUISITIONS,
            Enumerators.AccessRights.DOCUMENTS,
            Enumerators.AccessRights.KILOMETRAGES,
            Enumerators.AccessRights.KILOMETRAGE_MENSUEL,
            Enumerators.AccessRights.IMPORT_KILOMETRAGE_MENSUEL,
            Enumerators.AccessRights.AUTRES_DEPENSES,
        };

        public static List<Enumerators.AccessRights> Entretiens = new List<Enumerators.AccessRights>
        {
            Enumerators.AccessRights.ENTRETIENS,
            Enumerators.AccessRights.IMPORT_ENTRETIENS_REPARATIONS,
            Enumerators.AccessRights.ENTRETIENS_MAITRES,
            Enumerators.AccessRights.PROGRAMME_ENTRETIEN,
        };

        public static List<Enumerators.AccessRights> Reparations = new List<Enumerators.AccessRights>
        {
            Enumerators.AccessRights.REPARATIONS,
        };

        public static List<Enumerators.AccessRights> Carburant = new List<Enumerators.AccessRights>
        {
            Enumerators.AccessRights.CARBURANT,
        };

        public static List<Enumerators.AccessRights> Editions = new List<Enumerators.AccessRights>
        {
            Enumerators.AccessRights.RAPPORT_DEPENSES,
            Enumerators.AccessRights.RAPPORT_DEPENSES_GLOBAL,
            Enumerators.AccessRights.RAPPORT_CONSOMMATION_CARBURANT_MENSUEL,
            Enumerators.AccessRights.RAPPORT_CONSOMMATION_CARBURANT_SEMESTRIELLE,
            Enumerators.AccessRights.RAPPORT_ENTRETIEN_REPARATION_MENSUEL,
            Enumerators.AccessRights.RAPPORT_GLOBAL,
            Enumerators.AccessRights.RAPPORT_ENTRETIEN_REPARATION,
            Enumerators.AccessRights.RAPPORT_DEPENSES_DETAILLEES,
        };

        public static List<Enumerators.AccessRights> Alertes = new List<Enumerators.AccessRights>
        {
            Enumerators.AccessRights.ALERTES_ASSURANCE,
            Enumerators.AccessRights.ALERTES_TAXE_CIRCULATION,
            Enumerators.AccessRights.ALERTES_VISITE_TECHNIQUE,
            Enumerators.AccessRights.ALERTES_ENTRETIEN,
        };
    }
}
