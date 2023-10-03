using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPA.DAL;
using GPA.Models;
using System.Data.Entity;

namespace GPA.BLL
{
    public class CarburantCoutService
    {
        public static List<CarburantCoutUnitaireModel> Read()
        {
            using (Entities context = new Entities())
            {
                List<CarburantCoutUnitaireModel> result = new List<CarburantCoutUnitaireModel>();
                var list = context.CarburantCoutUnitaires.Select(e => new CarburantCoutUnitaireModel
                {
                    IdCarburant = e.IdCoutUnitaireCarburant,
                    CodeCarburant =e.Code_refCarburant,
                    CoutUnitaireCarburant = e.CoutUnitaireCarburant,
                   DateChangementTarif = e.DateChangementTarif,
                    TypeCarburant = context.REFERENTIELs.FirstOrDefault(x => x.KEY_REF == "TYPE_CARBURANT" && x.CODE_REF == e.Code_refCarburant).VALUE,
                    
                }).ToList();

                var listTypeCarburant = context.REFERENTIELs.Where(x => x.KEY_REF == "TYPE_CARBURANT").ToList();
                foreach (var item in listTypeCarburant)
                {
                    var recordToShow = list.Where(r => r.CodeCarburant == item.CODE_REF).OrderByDescending(i => i.DateChangementTarif).FirstOrDefault();
                    result.Add(recordToShow);
                }           
                return result;
            }
        }
        public static CarburantCoutUnitaireModel GetPrixUnitaireByIdVehicule(int idVehicule, DateTime dateCarburant)
        {
            var date = dateCarburant.Date;
            using (Entities context = new Entities())
            {
                var vehicule = context.Vehicules.FirstOrDefault(e => e.IdVehicule == idVehicule);
                var coutCarburant = context.CarburantCoutUnitaires.Select(c => new CarburantCoutUnitaireModel
                {
                    IdCarburant = c.IdCoutUnitaireCarburant,
                    CodeCarburant = c.Code_refCarburant,
                    CoutUnitaireCarburant = c.CoutUnitaireCarburant,
                    DateChangementTarif = c.DateChangementTarif,
                    TypeCarburant = context.REFERENTIELs.FirstOrDefault(x => x.KEY_REF == "TYPE_CARBURANT" && x.CODE_REF == c.Code_refCarburant).VALUE,
                })
                .Where(i => i.CodeCarburant == vehicule.TypeCarburantId && DbFunctions.TruncateTime(i.DateChangementTarif) <= date)
                .OrderByDescending(i=>i.DateChangementTarif)
                .FirstOrDefault();
                return coutCarburant;
            }
        }

        public static CarburantCoutUnitaireModel Create(CarburantCoutUnitaireModel model)
        {
            using (Entities context = new Entities())
            {
                var entity = new CarburantCoutUnitaire
                {
                    
                    Code_refCarburant = model.CodeCarburant,
                    CoutUnitaireCarburant = model.CoutUnitaireCarburant,
                    DateChangementTarif = model.DateChangementTarif
                };
                context.CarburantCoutUnitaires.Add(entity);
                context.SaveChanges();
                model.IdCarburant = entity.IdCoutUnitaireCarburant;
                return model;
            }
        }


        public static bool Update(CarburantCoutUnitaireModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;
                var entity = context.CarburantCoutUnitaires.FirstOrDefault(e => e.IdCoutUnitaireCarburant == model.IdCarburant);
                if (entity != null)
                {

                    entity.Code_refCarburant = model.CodeCarburant;
                    entity.CoutUnitaireCarburant = model.CoutUnitaireCarburant;
                    entity.DateChangementTarif = model.DateChangementTarif;

                    objectAffected = context.SaveChanges();
                }
                
                return objectAffected > 0 ;
            }
        }

        public static bool Delete(CarburantCoutUnitaireModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.CarburantCoutUnitaires.FirstOrDefault(e => e.IdCoutUnitaireCarburant == model.IdCarburant);

                if (entry != null)
                {
                    context.CarburantCoutUnitaires.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }

    }
}
