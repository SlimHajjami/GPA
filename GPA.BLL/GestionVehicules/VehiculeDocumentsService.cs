using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;
    
namespace GPA.BLL
{
    public class VehiculeDocumentsService
    {
        public static List<VehiculeDocumentsModel> Read(int departement, int client, DepartementsAccessRightsModel departementsAccess, int type, int annee, int mois)
        {
            using (Entities context = new Entities())
            {
                var list = context.VehiculeDocuments.Select(e => new VehiculeDocumentsModel
                {
                    IdDocuments = e.IdDocuments,
                    Client = context.Clients.FirstOrDefault(x => x.IdClient == context.Vehicules.FirstOrDefault(y => y.IdVehicule == e.VehiculeId).ClientId).NomClient,
                    VehiculeId = e.VehiculeId,
                    Vehicule = context.Vehicules.FirstOrDefault(x => x.IdVehicule == e.VehiculeId).MatriculeVehicule,
                    Departement = context.VehiculeDepartements.FirstOrDefault(x => x.IdDepartement == context.Vehicules.FirstOrDefault(y => y.IdVehicule == e.VehiculeId).DepartementId).NomDepartement,
                    FournisseurAssuranceId = e.FournisseurAssuranceId,
                    FournisseurAssurance = context.Fournisseurs.FirstOrDefault(x => x.IdFournisseur == e.FournisseurAssuranceId).NomFournisseur,
                    DateAssurance = e.DateAssurance,
                    ExpirationAssurance = e.ExpirationAssurance,
                    RappelAssurance = e.RappelAssurance,
                    NoteAssurance = e.NoteAssurance,
                    DateTaxeCirculation = e.DateTaxeCirculation,
                    ExpirationTaxeCirculation = e.ExpirationTaxeCirculation,
                    RappelTaxeCirculation = e.RappelTaxeCirculation,
                    NoteTaxeCirculation = e.NoteTaxeCirculation,
                    DerniereVisiteTechnique = e.DerniereVisiteTechnique,
                    ProchaineVisiteTechnique = e.ProchaineVisiteTechnique,
                    RappelVisiteTechnique = e.RappelVisiteTechnique,
                    NoteVisiteTechnique = e.NoteVisiteTechnique
                }).ToList();

                if (departement > 0)
                {
                    var vehicules = context.Vehicules.Where(x => x.DepartementId == departement).Select(y => y.IdVehicule);
                    list = list.Where(x => vehicules.Contains(x.VehiculeId)).ToList();
                }

                if (client > 0)
                {
                    var vehicules = context.Vehicules.Where(x => x.ClientId == client).Select(y => y.IdVehicule);
                    list = list.Where(x => vehicules.Contains(x.VehiculeId)).ToList();
                }

                if (departementsAccess != null)
                {
                    var d = departementsAccess.Visualisation;
                    var vehicules = context.Vehicules.Where(x => x.DepartementId != null && d.Contains((int)x.DepartementId)).Select(y => y.IdVehicule);
                    list = list.Where(x => vehicules.Contains(x.VehiculeId)).ToList();
                }

                if (annee > 0 && mois > 0)
                {
                    DateTime from = new DateTime(annee, mois, 1);
                    DateTime to = from.AddMonths(1);

                    if (type == 0)
                    {
                        list = list.Where(x => (x.ExpirationAssurance >= from && x.ExpirationAssurance < to)
                                          || (x.ExpirationTaxeCirculation >= from && x.ExpirationTaxeCirculation < to)
                                          || (x.ProchaineVisiteTechnique >= from && x.ProchaineVisiteTechnique < to)).ToList();
                    }
                    else if (type == 1)
                    {
                        list = list.Where(x => x.ExpirationAssurance >= from && x.ExpirationAssurance < to).ToList();
                    }
                    else if (type == 2)
                    {
                        list = list.Where(x => x.ExpirationTaxeCirculation >= from && x.ExpirationTaxeCirculation < to).ToList();
                    }
                    else if (type == 3)
                    {
                        list = list.Where(x => x.ProchaineVisiteTechnique >= from && x.ProchaineVisiteTechnique < to).ToList();
                    }
                }
                return list;
            }
        }

        public static VehiculeDocumentsModel Create(VehiculeDocumentsModel model)
        {
            using (Entities context = new Entities())
            {
                var entity = new VehiculeDocument
                {
                    VehiculeId = model.VehiculeId,
                    FournisseurAssuranceId = model.FournisseurAssuranceId,
                    DateAssurance = model.DateAssurance,
                    ExpirationAssurance = model.ExpirationAssurance,
                    RappelAssurance = model.RappelAssurance,
                    NoteAssurance = model.NoteAssurance,
                    DateTaxeCirculation = model.DateTaxeCirculation,
                    ExpirationTaxeCirculation = model.ExpirationTaxeCirculation,
                    RappelTaxeCirculation = model.RappelTaxeCirculation,
                    NoteTaxeCirculation = model.NoteTaxeCirculation,
                    DerniereVisiteTechnique = model.DerniereVisiteTechnique,
                    ProchaineVisiteTechnique = model.ProchaineVisiteTechnique,
                    RappelVisiteTechnique = model.RappelVisiteTechnique,
                    NoteVisiteTechnique = model.NoteVisiteTechnique
                };
                context.VehiculeDocuments.Add(entity);
                context.SaveChanges();
                model.IdDocuments = entity.IdDocuments;
                return model;
            }
        }

        public static bool Create(int vehicule)
        {
            using (Entities context = new Entities())
            {
                var entity = new VehiculeDocument
                {
                    VehiculeId = vehicule
                };
                context.VehiculeDocuments.Add(entity);
                int objectAffected = context.SaveChanges();
                return objectAffected > 0;
            }
        }

        public static bool Update(VehiculeDocumentsModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.VehiculeDocuments.FirstOrDefault(e => e.IdDocuments == model.IdDocuments);
                if (entity != null)
                {
                    entity.VehiculeId = model.VehiculeId;
                    entity.FournisseurAssuranceId = model.FournisseurAssuranceId;
                    entity.DateAssurance = model.DateAssurance;
                    entity.ExpirationAssurance = model.ExpirationAssurance;
                    entity.RappelAssurance = model.RappelAssurance;
                    entity.NoteAssurance = model.NoteAssurance;
                    entity.DateTaxeCirculation = model.DateTaxeCirculation;
                    entity.ExpirationTaxeCirculation = model.ExpirationTaxeCirculation;
                    entity.RappelTaxeCirculation = model.RappelTaxeCirculation;
                    entity.NoteTaxeCirculation = model.NoteTaxeCirculation;
                    entity.DerniereVisiteTechnique = model.DerniereVisiteTechnique;
                    entity.ProchaineVisiteTechnique = model.ProchaineVisiteTechnique;
                    entity.RappelVisiteTechnique = model.RappelVisiteTechnique;
                    entity.NoteVisiteTechnique = model.NoteVisiteTechnique;

                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }

        public static bool Delete(VehiculeDocumentsModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.VehiculeDocuments.FirstOrDefault(e => e.IdDocuments == model.IdDocuments);

                if (entry != null)
                {
                    context.VehiculeDocuments.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }

        public static bool Delete(int vehicule)
        {
            using (Entities context = new Entities())
            {
                var entry = context.VehiculeDocuments.FirstOrDefault(e => e.VehiculeId == vehicule);

                if (entry != null)
                {
                    context.VehiculeDocuments.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }
    }
}
