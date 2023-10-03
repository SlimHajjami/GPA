using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;

namespace GPA.BLL
{
    public class ParametresDepenseService
    {
        public static List<ParametresDepenseModel> Read(int client)
        {
            using (Entities context = new Entities())
            {
                var list = context.ParametresDepenses.Select(e => new ParametresDepenseModel
                {
                    Id = e.Id,
                    ClientId = e.ClientId,
                    Depense = e.Depense
                }).ToList();
                if (client > 0)
                {
                    list = list.Where(x => x.ClientId == client).ToList();
                }
                return list;
            }
        }

        public static ParametresDepenseModel Create(ParametresDepenseModel model)
        {
            using (Entities context = new Entities())
            {
                var entity = new ParametresDepens
                {
                    ClientId = model.ClientId,
                    Depense = model.Depense
                };
                context.ParametresDepenses.Add(entity);
                context.SaveChanges();
                model.Id = entity.Id;
                return model;
            }
        }

        public static bool Update(ParametresDepenseModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.ParametresDepenses.FirstOrDefault(e => e.Id == model.Id);
                if (entity != null)
                {
                    entity.ClientId = model.ClientId;
                    entity.Depense = model.Depense;
                   

                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }

        public static bool Delete(ParametresDepenseModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.ParametresDepenses.FirstOrDefault(e => e.Id == model.Id);

                if (entry != null)
                {
                    context.ParametresDepenses.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }
    }
}
