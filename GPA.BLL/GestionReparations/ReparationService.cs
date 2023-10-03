using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;

namespace GPA.BLL
{
    public class ReparationService
    {
        public static List<ReparationModel> Read(int client, DepartementsAccessRightsModel departementsAccess, int departement, int annee, int mois)
        {
            using (Entities context = new Entities())
            {
                var list = context.Reparations.Select(e => new ReparationModel
                {
                    IdReparation = e.IdReparation,
                    ClientId = e.ClientId,
                    VehiculeId = e.VehiculeId,
                    DateReparation = e.DateReparation,
                    DateMiseService = e.DateMiseService,
                    HeuresArret = e.HeuresArret,
                    FournisseurId = e.FournisseurId,
                    DescriptionReparation = e.DescriptionReparation,
                    CoutReparation = e.CoutReparation,
                    KilometrageReparation = e.KilometrageReparation,
                    NoteReparation = e.NoteReparation
                }).ToList();
                if (client > 0)
                {
                    list = list.Where(x => x.ClientId == client).ToList();
                }
                if (departementsAccess != null)
                {
                    var d = departementsAccess.Visualisation;
                    var vehicules = context.Vehicules.Where(x => x.DepartementId != null && d.Contains((int)x.DepartementId)).Select(y => y.IdVehicule);
                    list = list.Where(x => vehicules.Contains(x.VehiculeId)).ToList();
                }
                if (departement > 0)
                {
                    var vehicules = context.Vehicules.Where(x => x.DepartementId == departement).Select(y => y.IdVehicule);
                    list = list.Where(x => vehicules.Contains(x.VehiculeId)).ToList();
                }
                if (annee > 0 && mois > 0)
                {
                    DateTime from = new DateTime(annee, mois, 1);
                    DateTime to = from.AddMonths(1);

                    list = list.Where(x => x.DateReparation >= from && x.DateReparation < to).ToList();
                }
                return list;
            }
        }

        public static ReparationModel Create(ReparationModel model)
        {
            using (Entities context = new Entities())
            {
                var entity = new Reparation
                {
                    ClientId = model.ClientId,
                    VehiculeId = model.VehiculeId,
                    DateReparation = model.DateReparation,
                    DateMiseService = model.DateMiseService,
                    HeuresArret = model.HeuresArret,
                    FournisseurId = model.FournisseurId,
                    DescriptionReparation = model.DescriptionReparation,
                    CoutReparation = model.CoutReparation,
                    KilometrageReparation = model.KilometrageReparation,
                    NoteReparation = model.NoteReparation
                };
                context.Reparations.Add(entity);
                context.SaveChanges();
                model.IdReparation = entity.IdReparation;
                if (model.KilometrageReparation.HasValue)
                {
                    if (model.KilometrageReparation > 0)
                    {
                        LectureKilometrageService.UpdateKilometrage(model.VehiculeId, model.DateReparation, model.KilometrageReparation.Value, "Lors de saisie Réparation");
                    }
                }
                return model;
            }
        }

        public static bool Update(ReparationModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.Reparations.FirstOrDefault(e => e.IdReparation == model.IdReparation);
                if (entity != null)
                {
                    if (entity.DateReparation == model.DateReparation)
                    {
                        if (model.KilometrageReparation.HasValue)
                        {
                            if (model.KilometrageReparation.Value > 0)
                            {
                                LectureKilometrageService.UpdateKilometrage(model.VehiculeId, model.DateReparation, model.KilometrageReparation.Value, "Lors de saisie Réparation");
                            }
                        }
                    }
                    else
                    {
                        if (model.KilometrageReparation.HasValue)
                        {
                            if (model.KilometrageReparation.Value > 0)
                            {
                                LectureKilometrageService.UpdateKilometrageDate(model.VehiculeId, entity.DateReparation, model.DateReparation, model.KilometrageReparation.Value);
                            }
                        }
                    }
                    entity.ClientId = model.ClientId;
                    entity.VehiculeId = model.VehiculeId;
                    entity.DateReparation = model.DateReparation;
                    entity.DateMiseService = model.DateMiseService;
                    entity.HeuresArret = model.HeuresArret;
                    entity.FournisseurId = model.FournisseurId;
                    entity.DescriptionReparation = model.DescriptionReparation;
                    entity.CoutReparation = model.CoutReparation;
                    entity.KilometrageReparation = model.KilometrageReparation;
                    entity.NoteReparation = model.NoteReparation;

                    objectAffected = context.SaveChanges();
                }
              
                return objectAffected > 0;
            }
        }

        public static bool UpdateCout(int id)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;
                decimal d = 0;

                var depenses = context.Depenses.Where(x => x.ReparationId == id);
                if (depenses.Count() > 0)
                {
                    d = depenses.Sum(y => y.QuantiteDepense * y.CoutUnitaireDepense);
                }
                var entity = context.Reparations.FirstOrDefault(e => e.IdReparation == id);
                if (entity != null)
                {
                    entity.CoutReparation = d;
                    objectAffected = context.SaveChanges();
                }

                return objectAffected > 0;
            }
        }

        public static bool Delete(ReparationModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.Reparations.FirstOrDefault(e => e.IdReparation == model.IdReparation);

                if (entry != null)
                {
                    context.Reparations.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }
        public static bool DeleteAll(int clientId, int year, int month)
        {
            
            using (Entities context = new Entities())
            {
                var list = context.Reparations.Where(e => e.ClientId == clientId).ToList();
                if (year > 0 && month > 0)
                {
                    list = list.Where(e => e.DateReparation.Year == year && e.DateReparation.Month == month).ToList();
                }
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var listDep = context.Depenses.Where(e => e.ReparationId == item.IdReparation);
                        if (listDep != null)
                        {
                            foreach (var dep in listDep)
                            {
                                context.Depenses.Remove(dep);
                            }
                        }
                        context.Reparations.Remove(item);
                    }
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }
    }
}
