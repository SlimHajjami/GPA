using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;

namespace GPA.BLL
{
    public class ModeleC100KMService
    {
        public static List<ModeleC100KMModel> Read(int client)
        {
            using (Entities context = new Entities())
            {
                var modeles = context.Vehicules.Where(x => x.ClientId == client && x.IdModele > 0).Select(y => y.IdModele).Distinct();

                if (modeles.Count() > 0)
                {
                    var l1 = context.ModeleC100KM.Where(x => x.ClientId == client).Select(y => y.ModeleId).ToList();
                //    var l2 = modeles.Except(l1);

                    foreach (var m in modeles)
                    {
                        if (m != null)
                        {
                            if (!l1.Contains((int)m))
                            {
                                Create(new ModeleC100KMModel
                                {
                                    ClientId = client,
                                    ModeleId = (int)m,
                                    C100KM = 0
                                });
                            }
                          
                        }
                    }
                }


                var list = context.ModeleC100KM.Select(e => new  ModeleC100KMModel
                {
                    IdModeleC100KM = e.IdModeleC100KM,
                    ClientId = e.ClientId,
                    ModeleId = e.ModeleId,
                    Modele = context.VehiculeModeles.FirstOrDefault(x => x.IdModele == e.ModeleId).NomModele,
                    C100KM = e.C100KM
                }).ToList();
                if (client > 0)
                {
                    list = list.Where(x => x.ClientId == client).ToList();
                }
                return list;
            }
        }

        public static ModeleC100KMModel Create(ModeleC100KMModel model)
        {
            using (Entities context = new Entities())
            {
                var entity = new  ModeleC100KM
                {
                    ClientId = model.ClientId,
                    ModeleId = model.ModeleId,
                    C100KM = model.C100KM
                };
                context.ModeleC100KM.Add(entity);
                model.IdModeleC100KM = entity.IdModeleC100KM;
                context.SaveChanges();
                return model;
            }
        }

        public static bool Update( ModeleC100KMModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.ModeleC100KM.FirstOrDefault(e => e.ClientId == model.ClientId && e.ModeleId == model.ModeleId);
                if (entity != null)
                {
                    entity.C100KM = model.C100KM;

                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }

        public static bool Delete(ModeleC100KMModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.ModeleC100KM.FirstOrDefault(e => e.ClientId == model.ClientId && e.ModeleId == model.ModeleId);

                if (entry != null)
                {
                    context.ModeleC100KM.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }
    }
}
