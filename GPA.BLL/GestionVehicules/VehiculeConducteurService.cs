using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;

namespace GPA.BLL
{
    public class VehiculeConducteurService
    {
        public static List<VehiculeConducteurModel> Read(int client)
        {
            using (Entities context = new Entities())
            {
                var list = context.VehiculeConducteurs.Select(e => new VehiculeConducteurModel
                {
                    IdConducteur = e.IdConducteur,
                    ClientId = e.ClientId,
                    NomConducteur = e.NomConducteur,
                    PrenomConducteur = e.PrenomConducteur,
                    NomPrenomConducteur = e.NomConducteur + " " + e.PrenomConducteur,
                    DateNaissanceConducteur = e.DateNaissanceConducteur,
                    DateEmbaucheConducteur = e.DateEmbaucheConducteur,
                    NumeroPermisConducteur = e.NumeroPermisConducteur,
                    AdresseConducteur = e.AdresseConducteur,
                    VilleConducteur = e.VilleConducteur,
                    CodePostalConducteur = e.CodePostalConducteur,
                    Tel1Conducteur = e.Tel1Conducteur,
                    Tel2Conducteur = e.Tel2Conducteur,
                    EmailConducteur = e.EmailConducteur
                }).ToList();
                if (client > 0)
                {
                    list = list.Where(x => x.ClientId == client).ToList();
                }
                return list;
            }
        }

        public static VehiculeConducteurModel Create(VehiculeConducteurModel model)
        {
            using (Entities context = new Entities())
            {
                var entity = new VehiculeConducteur
                {
                    ClientId = model.ClientId,
                    NomConducteur = model.NomConducteur,
                    PrenomConducteur = model.PrenomConducteur,
                    DateNaissanceConducteur = model.DateNaissanceConducteur,
                    DateEmbaucheConducteur = model.DateEmbaucheConducteur,
                    NumeroPermisConducteur = model.NumeroPermisConducteur,
                    AdresseConducteur = model.AdresseConducteur,
                    VilleConducteur = model.VilleConducteur,
                    CodePostalConducteur = model.CodePostalConducteur,
                    Tel1Conducteur = model.Tel1Conducteur,
                    Tel2Conducteur = model.Tel2Conducteur,
                    EmailConducteur = model.EmailConducteur
                };
                context.VehiculeConducteurs.Add(entity);
                context.SaveChanges();
                model.IdConducteur = entity.IdConducteur;
                return model;
            }
        }

        public static bool Update(VehiculeConducteurModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.VehiculeConducteurs.FirstOrDefault(e => e.IdConducteur == model.IdConducteur);
                if (entity != null)
                {
                    entity.ClientId = model.ClientId;
                    entity.NomConducteur = model.NomConducteur;
                    entity.PrenomConducteur = model.PrenomConducteur;
                    entity.DateNaissanceConducteur = model.DateNaissanceConducteur;
                    entity.DateEmbaucheConducteur = model.DateEmbaucheConducteur;
                    entity.NumeroPermisConducteur = model.NumeroPermisConducteur;
                    entity.AdresseConducteur = model.AdresseConducteur;
                    entity.VilleConducteur = model.VilleConducteur;
                    entity.CodePostalConducteur = model.CodePostalConducteur;
                    entity.Tel1Conducteur = model.Tel1Conducteur;
                    entity.Tel2Conducteur = model.Tel2Conducteur;
                    entity.EmailConducteur = model.EmailConducteur;

                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }

        public static bool Delete(VehiculeConducteurModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.VehiculeConducteurs.FirstOrDefault(e => e.IdConducteur == model.IdConducteur);

                if (entry != null)
                {
                    context.VehiculeConducteurs.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }
    }
}
