using GPA.DAL;
using GPA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPA.BLL
{
    public class VehiculeModeleService
    {
        public static IEnumerable<VehiculeModeleModel> Read(int codRef)
        {
            using (Entities context = new Entities())
            {
                return context.VehiculeModeles.Where(x => x.CodeRefMarque == codRef).Select(comp => new VehiculeModeleModel
                {
                    IdModele = comp.IdModele,
                    CodeRefMarque = comp.CodeRefMarque,
                    NomModele = comp.NomModele
                    
                }).ToList();
            }

        }

        public static VehiculeModeleModel Create (VehiculeModeleModel model, int codRef)
        {
            using (Entities context = new Entities())
            {
                var entity = new VehiculeModele{
               
                    CodeRefMarque = codRef,
                    NomModele = model.NomModele
                };
                context.VehiculeModeles.Add(entity);
                context.SaveChanges();
                model.IdModele = entity.IdModele;
                return model;
            }
           
        }

        public static bool Update (VehiculeModeleModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;
                var entry = context.VehiculeModeles.FirstOrDefault(e => e.IdModele == model.IdModele);
                if (entry != null)
                {
                    entry.NomModele = model.NomModele;
                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }

        public static bool Delete (VehiculeModeleModel model)
        {
            using (Entities context= new Entities())
            {
                var entry = context.VehiculeModeles.FirstOrDefault(e => e.IdModele == model.IdModele);
                if (entry != null)
                {
                    context.VehiculeModeles.Remove(entry);
                }
                var objectaffected = context.SaveChanges();
                return objectaffected > 0;
            }

        }
    }
}
