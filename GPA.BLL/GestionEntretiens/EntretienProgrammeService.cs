using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;

namespace GPA.BLL
{
    public class EntretienProgrammeService
    {
        public static List<EntretienProgrammeModel> Read(int client, DepartementsAccessRightsModel departementsAccess, int departement, int statut, int annee, int mois)
        {
            using (Entities context = new Entities())
            {
                var list = context.EntretienProgrammes.Select(e => new EntretienProgrammeModel
                {
                    IdEntretienProgramme = e.IdEntretienProgramme,
                    ClientId = e.ClientId,
                    VehiculeId = e.VehiculeId,
                    EntretienMaitreId = e.EntretienMaitreId,
                    RepeterNb = e.RepeterNb,
                    RepeterPeriode = e.RepeterPeriode,
                    RepeterKilometrage = e.RepeterKilometrage,
                    RappelNb = e.RappelNb,
                    RappelPeriode = e.RappelPeriode,
                    RappelKilometrage = e.RappelKilometrage,
                    DateExecution = e.DateExecution,
                    KilometrageExecution = e.KilometrageExecution,
                    ProchaineDate = e.ProchaineDate,
                    ProchaineKilometrage = e.ProchaineKilometrage
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
                    if (statut == 1)
                    {
                        list = list.Where(x => x.DateExecution >= from && x.DateExecution < to).ToList();
                    }
                    else if(statut == 2)
                    {
                        list = list.Where(x => x.ProchaineDate >= from && x.ProchaineDate < to).ToList();
                    }
                    else
                    {
                        list = list.Where(x => (x.DateExecution >= from && x.DateExecution < to) || x.ProchaineDate >= from && x.ProchaineDate < to).ToList();
                    }
                }
                foreach (var row in list)
                {
                    /******Repeter*******/
                    if (row.RepeterPeriode > 0 && row.RepeterNb > 0)
                    {
                        string periode = context.REFERENTIELs.FirstOrDefault(x => x.KEY_REF == "PERIODE" && x.CODE_REF == row.RepeterPeriode).VALUE;
                        if (row.RepeterNb > 0)
                        {
                            row.Repeter += row.RepeterNb + " " + periode;
                        }
                        if (row.RepeterKilometrage > 0)
                        {
                            row.Repeter += " ou " + row.RepeterKilometrage + " km";
                        }
                    }
                    else
                    {
                        if (row.RepeterKilometrage > 0)
                        {
                            row.Repeter += row.RepeterKilometrage + " km";
                        }
                    }

                    /******Rappel*******/
                    if (row.RappelPeriode > 0 && row.RappelNb > 0)
                    {
                        string periode = context.REFERENTIELs.FirstOrDefault(x => x.KEY_REF == "PERIODE" && x.CODE_REF == row.RappelPeriode).VALUE;
                        if (row.RappelNb > 0)
                        {
                            row.Rappel += row.RappelNb + " " + periode;
                        }
                        if (row.RappelKilometrage > 0)
                        {
                            row.Rappel += " ou " + row.RappelKilometrage + " km";
                        }
                    }
                    else
                    {
                        if (row.RappelKilometrage > 0)
                        {
                            row.Rappel += row.RappelKilometrage + " km";
                        }
                    }

                    /******Execution*******/
                    if (row.DateExecution.HasValue)
                    {
                        row.Execution += row.DateExecution.Value.ToShortDateString();
                        if (row.KilometrageExecution > 0)
                        {
                            row.Execution += " et " + row.KilometrageExecution + " km";
                        }
                    }
                    else
                    {
                        if (row.KilometrageExecution > 0)
                        {
                            row.Execution += row.KilometrageExecution + " km";
                        }
                    }

                    /******Prochain*******/
                    if (row.ProchaineDate.HasValue)
                    {
                        row.Prochain += row.ProchaineDate.Value.ToShortDateString();
                        if (row.ProchaineKilometrage > 0)
                        {
                            row.Prochain += " ou " + row.ProchaineKilometrage + " km";
                        }
                    }
                    else
                    {
                        if (row.ProchaineKilometrage > 0)
                        {
                            row.Prochain += row.ProchaineKilometrage + " km";
                        }
                    }
                }
                return list;
            }
        }

        public static EntretienProgrammeModel Create(EntretienProgrammeModel model)
        {
            using (Entities context = new Entities())
            {
                model.ProchaineDate = DateTime.Now;
                model.ProchaineKilometrage = 0;

                var entity = new EntretienProgramme
                {
                    ClientId = model.ClientId,
                    VehiculeId = model.VehiculeId,
                    EntretienMaitreId = model.EntretienMaitreId,
                    RepeterNb = model.RepeterNb,
                    RepeterPeriode = model.RepeterPeriode,
                    RepeterKilometrage = model.RepeterKilometrage,
                    RappelNb = model.RappelNb,
                    RappelPeriode = model.RappelPeriode,
                    RappelKilometrage = model.RappelKilometrage,
                    DateExecution = model.DateExecution,
                    KilometrageExecution = model.KilometrageExecution,
                    ProchaineDate = model.ProchaineDate,
                    ProchaineKilometrage = model.ProchaineKilometrage
                };
                context.EntretienProgrammes.Add(entity);
                context.SaveChanges();
                model.IdEntretienProgramme = entity.IdEntretienProgramme;
                return model;
            }
        }

        public static bool Update(EntretienProgrammeModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.EntretienProgrammes.FirstOrDefault(e => e.IdEntretienProgramme == model.IdEntretienProgramme);
                if (entity != null)
                {
                    entity.ClientId = model.ClientId;
                    entity.VehiculeId = model.VehiculeId;
                    entity.EntretienMaitreId = model.EntretienMaitreId;
                    entity.RepeterNb = model.RepeterNb;
                    entity.RepeterPeriode = model.RepeterPeriode;
                    entity.RepeterKilometrage = model.RepeterKilometrage;
                    entity.RappelNb = model.RappelNb;
                    entity.RappelPeriode = model.RappelPeriode;
                    entity.RappelKilometrage = model.RappelKilometrage;
                    entity.DateExecution = model.DateExecution;
                    entity.KilometrageExecution = model.KilometrageExecution;
                    entity.ProchaineDate = model.ProchaineDate;
                    entity.ProchaineKilometrage = model.ProchaineKilometrage;

                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }

        public static bool UpdateAfterExecute(int id, DateTime dateExecution, int KilometrageExecution)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var model = context.EntretienProgrammes.FirstOrDefault(e => e.IdEntretienProgramme == id);
                if (model != null)
                {
                    model.DateExecution = dateExecution;
                    model.KilometrageExecution = KilometrageExecution;

                    model = UpdateProchain(model);

                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }

        public static bool Delete(EntretienProgrammeModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.EntretienProgrammes.FirstOrDefault(e => e.IdEntretienProgramme == model.IdEntretienProgramme);

                if (entry != null)
                {
                    context.EntretienProgrammes.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }

        private static EntretienProgramme UpdateProchain(EntretienProgramme model)
        {
            if (model.RepeterNb.HasValue && model.RepeterPeriode.HasValue)
            {
                if (model.RepeterNb > 0 && model.RepeterPeriode > 0)
                {
                    DateTime date = DateTime.Now;
                    if (model.DateExecution.HasValue)
                    {
                        date = model.DateExecution.Value;
                    }
                    if (model.RepeterPeriode == 1)
                    {
                        model.ProchaineDate = date.AddDays(model.RepeterNb.Value);
                    }
                    else if (model.RepeterPeriode == 2)
                    {
                        model.ProchaineDate = date.AddDays(model.RepeterNb.Value * 7);
                    }
                    else if (model.RepeterPeriode == 3)
                    {
                        model.ProchaineDate = date.AddMonths(model.RepeterNb.Value);
                    }
                    else if (model.RepeterPeriode == 4)
                    {
                        model.ProchaineDate = date.AddYears(model.RepeterNb.Value);
                    }
                }
            }

            if (model.RepeterKilometrage.HasValue)
            {
                if (model.RepeterKilometrage > 0)
                {
                    var lectureKilo = LectureKilometrageService.GetLastKilometrage(model.VehiculeId);
                    if (lectureKilo != null)
                    {
                        int kilo = lectureKilo.Kilometrage;

                        model.ProchaineKilometrage = lectureKilo.Kilometrage + model.RepeterKilometrage;
                    }
                }
            }

            return model;
        }
    }
}
