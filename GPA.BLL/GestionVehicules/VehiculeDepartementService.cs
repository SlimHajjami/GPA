using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;

namespace GPA.BLL
{
    public class VehiculeDepartementService
    {
        public static List<VehiculeDepartementModel> Read(int client, DepartementsAccessRightsModel departementsAccess)
        {
            using (Entities context = new Entities())
            {
                var list = context.VehiculeDepartements.Select(e => new VehiculeDepartementModel
                {
                    IdDepartement = e.IdDepartement,
                    ClientId = e.ClientId,
                    NomDepartement = e.NomDepartement
                }).ToList();
                if (client > 0)
                {
                    list = list.Where(x => x.ClientId == client).ToList();
                }
                if (departementsAccess != null)
                {
                    var d = departementsAccess.Visualisation;
                    list = list.Where(x => x.IdDepartement != null && d.Contains((int)x.IdDepartement)).ToList();
                }
                return list;
            }
        }

        public static VehiculeDepartementModel Get(int id)
        {
            using (Entities context = new Entities())
            {
                var list = context.VehiculeDepartements.Select(e => new VehiculeDepartementModel
                {
                    IdDepartement = e.IdDepartement,
                    ClientId = e.ClientId,
                    NomDepartement = e.NomDepartement
                }).FirstOrDefault(x => x.IdDepartement == id);
                return list;
            }
        }

        public static VehiculeDepartementModel Create(VehiculeDepartementModel model)
        {
            using (Entities context = new Entities())
            {
                var entity = new VehiculeDepartement
                {
                    ClientId = model.ClientId,
                    NomDepartement = model.NomDepartement
                };
                context.VehiculeDepartements.Add(entity);
                context.SaveChanges();
                model.IdDepartement = entity.IdDepartement;
                return model;
            }
        }

        public static bool Update(VehiculeDepartementModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.VehiculeDepartements.FirstOrDefault(e => e.IdDepartement == model.IdDepartement);
                if (entity != null)
                {
                    entity.ClientId = model.ClientId;
                    entity.NomDepartement = model.NomDepartement;

                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }

        public static bool Delete(VehiculeDepartementModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.VehiculeDepartements.FirstOrDefault(e => e.IdDepartement == model.IdDepartement);

                if (entry != null)
                {
                    context.VehiculeDepartements.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }
    }
}
