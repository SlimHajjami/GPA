using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;

namespace GPA.BLL
{
    public class AutresDepenseService
    {
        public static List<AutresDepenseModel> Read(int dep, int client, int annee, int mois, int vehicule)
        {
            using (Entities context = new Entities())
            {
                var list = context.DepensesClients.Select(e => new AutresDepenseModel
                {
                    Id = e.Id,  
                    DepenseId = e.DepenseId,
                    DateDepenses = e.DateDepenses,
                    Kilometrage = e.Kilometrage,
                    CoutTotal = e.CoutTotal,
                    Note = e.NoteDepenses,
                    VehiculeId = e.VehiculeId,

                }).ToList();
                if (client > 0)
                {
                    var depenses = context.ParametresDepenses.Where(x => x.ClientId == client).Select(y=>y.Id);
                    list = list.Where(x => depenses.Contains(x.DepenseId)).ToList();
                    if (dep > 0)
                        {
                            var vehsIds = context.Vehicules.Where(x => x.ClientId == client && x.DepartementId == dep).Select(y => y.IdVehicule);
                            list = list.Where(x => x.VehiculeId.HasValue && vehsIds.Contains((int)x.VehiculeId)).ToList();
                        }
                }
               
                if (annee > 0)
                {
                    list = list.Where(x => x.DateDepenses.Year == annee).ToList();
                }
                if (mois > 0)
                {
                    list = list.Where(x => x.DateDepenses.Month == mois).ToList();
                }
                if (vehicule > 0)
                {
                    list = list.Where(x => x.VehiculeId == vehicule).ToList();
                }
                return list;
            }
        }

        public static AutresDepenseModel Create(AutresDepenseModel model)
        {
            using (Entities context = new Entities())
            {
                var entity = new DepensesClient
                {
                    DepenseId = model.DepenseId,
                    DateDepenses = model.DateDepenses,
                    Kilometrage = model.Kilometrage,
                    NoteDepenses = model.Note,
                    CoutTotal = model.CoutTotal,
                    VehiculeId = model.VehiculeId

                };
                context.DepensesClients.Add(entity);
                context.SaveChanges();
                model.Id = entity.Id;
                return model;
            }
        }

        public static bool Update(AutresDepenseModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.DepensesClients.FirstOrDefault(e => e.Id == model.Id);
                if (entity != null)
                {
                    entity.DepenseId = model.DepenseId;
                    entity.Kilometrage = model.Kilometrage;
                    entity.NoteDepenses = model.Note;
                    entity.DateDepenses = model.DateDepenses;
                    entity.CoutTotal = model.CoutTotal;
                    entity.VehiculeId = model.VehiculeId;

                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }

        public static bool Delete(AutresDepenseModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.DepensesClients.FirstOrDefault(e => e.Id == model.Id);

                if (entry != null)
                {
                    context.DepensesClients.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }
    }
}
