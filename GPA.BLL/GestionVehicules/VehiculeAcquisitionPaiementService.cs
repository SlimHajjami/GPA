using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;

namespace GPA.BLL
{
    public class VehiculeAcquisitionPaiementService
    {
        public static List<VehiculeAcquisitionPaiementModel> Read(int acquisition)
        {
            using (Entities context = new Entities())
            {
                var list = context.VehiculeAcquisitionPaiements.Select(e => new VehiculeAcquisitionPaiementModel
                {
                    IdPaiement = e.IdPaiement,
                    AcquisitionId = e.AcquisitionId,
                    ReferencePaiement = e.ReferencePaiement,
                    DatePaiement = e.DatePaiement,
                    MontantPaiement = e.MontantPaiement,
                    NotePaiement = e.NotePaiement
                }).ToList();
                if(acquisition > 0)
                {
                    list = list.Where(x => x.AcquisitionId == acquisition).ToList();
                }
                return list;
            }
        }

        public static VehiculeAcquisitionPaiementModel Create(VehiculeAcquisitionPaiementModel model)
        {
            using (Entities context = new Entities())
            {
                var entity = new VehiculeAcquisitionPaiement
                {
                    AcquisitionId = model.AcquisitionId,
                    ReferencePaiement = model.ReferencePaiement,
                    DatePaiement = model.DatePaiement,
                    MontantPaiement = model.MontantPaiement,
                    NotePaiement = model.NotePaiement
                };
                context.VehiculeAcquisitionPaiements.Add(entity);
                context.SaveChanges();
                model.IdPaiement = entity.IdPaiement;
                return model;
            }
        }

        public static bool Update(VehiculeAcquisitionPaiementModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.VehiculeAcquisitionPaiements.FirstOrDefault(e => e.IdPaiement == model.IdPaiement);
                if (entity != null)
                {
                    entity.AcquisitionId = model.AcquisitionId;
                    entity.ReferencePaiement = model.ReferencePaiement;
                    entity.DatePaiement = model.DatePaiement;
                    entity.MontantPaiement = model.MontantPaiement;
                    entity.NotePaiement = model.NotePaiement;

                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }

        public static bool Delete(VehiculeAcquisitionPaiementModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.VehiculeAcquisitionPaiements.FirstOrDefault(e => e.IdPaiement == model.IdPaiement);

                if (entry != null)
                {
                    context.VehiculeAcquisitionPaiements.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }

        public static bool Delete(int acquisition)
        {
            using (Entities context = new Entities())
            {
                var entries = context.VehiculeAcquisitionPaiements.Where(e => e.AcquisitionId == acquisition);

                if (entries.Count() > 0)
                {
                    context.VehiculeAcquisitionPaiements.RemoveRange(entries);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }
    }
}
