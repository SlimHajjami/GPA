using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;

namespace GPA.BLL
{
    public class DepenseService
    {
        public static List<DepenseModel> Read(int entretien, int reparation)
        {
            using (Entities context = new Entities())
            {
                var list = context.Depenses.Select(e => new DepenseModel
                {
                    IdDepense = e.IdDepense,
                    TypeId = e.TypeId,
                    EntretienId = e.EntretienId,
                    ReparationId = e.ReparationId,
                    QuantiteDepense = e.QuantiteDepense,
                    CoutUnitaireDepense = e.CoutUnitaireDepense,
                    PieceId = e.PieceId,
                    ExpirationGarantie = e.ExpirationGarantie,
                    Description = e.Description,
                    NoteDepense = e.NoteDepense
                }).ToList();
                if (entretien > 0)
                {
                    list = list.Where(x => x.EntretienId == entretien).ToList();
                }
                if (reparation > 0)
                {
                    list = list.Where(x => x.ReparationId == reparation).ToList();
                }
                return list;
            }
        }

        public static DepenseModel Create(DepenseModel model)
        {
            using (Entities context = new Entities())
            {
                var entity = new Depense
                {
                    TypeId = model.TypeId,
                    EntretienId = model.EntretienId,
                    ReparationId = model.ReparationId,
                    QuantiteDepense = model.QuantiteDepense,
                    CoutUnitaireDepense = model.CoutUnitaireDepense,
                    PieceId = model.PieceId,
                    ExpirationGarantie = model.ExpirationGarantie,
                    Description = model.Description,
                    NoteDepense = model.NoteDepense
                };
                context.Depenses.Add(entity);
                context.SaveChanges();
                model.IdDepense = entity.IdDepense;
                return model;
            }
        }

        public static bool Update(DepenseModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.Depenses.FirstOrDefault(e => e.IdDepense == model.IdDepense);
                if (entity != null)
                {
                    entity.TypeId = model.TypeId;
                    entity.EntretienId = model.EntretienId;
                    entity.ReparationId = model.ReparationId;
                    entity.QuantiteDepense = model.QuantiteDepense;
                    entity.CoutUnitaireDepense = model.CoutUnitaireDepense;
                    entity.PieceId = model.PieceId;
                    entity.ExpirationGarantie = model.ExpirationGarantie;
                    entity.Description = model.Description;
                    entity.NoteDepense = model.NoteDepense;

                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }

        public static bool Delete(DepenseModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.Depenses.FirstOrDefault(e => e.IdDepense == model.IdDepense);

                if (entry != null)
                {
                    context.Depenses.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }

        public static bool DeleteDepensesEntretien(int entretien)
        {
            using (Entities context = new Entities())
            {
                var entries = context.Depenses.Where(e => e.EntretienId == entretien);

                if (entries.Count() > 0)
                {
                    context.Depenses.RemoveRange(entries);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }

        public static bool DeleteDepensesReparation(int reparation)
        {
            using (Entities context = new Entities())
            {
                var entries = context.Depenses.Where(e => e.ReparationId == reparation);

                if (entries.Count() > 0)
                {
                    context.Depenses.RemoveRange(entries);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }
    }
}
