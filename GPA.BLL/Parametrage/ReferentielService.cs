using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPA.Models;
using GPA.DAL;
using System.Web.Mvc;

namespace GPA.BLL
{
    public class ReferentielService
    {
        public static IEnumerable<ReferentielViewModel> Read(string key_ref)
        {
            using (Entities context = new Entities())
            {
                return context.REFERENTIELs.Where(x => x.KEY_REF == key_ref).Select(comp => new ReferentielViewModel
                {
                    ID_REFERENTIELS = comp.ID_REFERENTIELS,
                    KEY_REF = comp.KEY_REF,
                    CODE_REF = comp.CODE_REF,
                    VALUE = comp.VALUE
                }).OrderBy(m => m.CODE_REF).ToList();
            }
        }

        public static ReferentielViewModel Create(ReferentielViewModel refre, string key_ref)
        {
            using (Entities context = new Entities())
            {
                var entity = new REFERENTIEL
                {
                    KEY_REF = key_ref,
                    CODE_REF = context.REFERENTIELs.Where(x => x.KEY_REF == key_ref).Count() != 0 ? context.REFERENTIELs.Where(x => x.KEY_REF == key_ref).Max(x => x.CODE_REF) + 1 : 1,
                    VALUE = refre.VALUE
                };

                if (entity.KEY_REF == "TYPE_CARBURANT")
                { var ent = new  CarburantCoutUnitaire
                    {

                        Code_refCarburant = entity.CODE_REF,
                        DateChangementTarif = DateTime.Now,
                        CoutUnitaireCarburant = 0
                    };
                    context.CarburantCoutUnitaires.Add(ent);
                }
                context.REFERENTIELs.Add(entity);                
                context.SaveChanges();
                refre.ID_REFERENTIELS = entity.ID_REFERENTIELS;

                return refre;
            }
        } 

        public static bool Update(ReferentielViewModel refer)
        {
            using (Entities context = new Entities())
            {
                var objectAffected = 0;
                var entity = context.REFERENTIELs.FirstOrDefault(c => c.ID_REFERENTIELS == refer.ID_REFERENTIELS);
                if (entity != null)
                {

                    entity.VALUE = refer.VALUE;

                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }

        public static bool Delete(ReferentielViewModel refr)
        {
            using (Entities context = new Entities())
            {
                List<REFERENTIEL> refere = context.REFERENTIELs.Where(c => c.ID_REFERENTIELS == refr.ID_REFERENTIELS).ToList();

                foreach (var c in refere)
                {
                    if (c.KEY_REF == "TYPE_CARBURANT")
                    {
                        var ctCarburant = context.CarburantCoutUnitaires.FirstOrDefault(x => x.Code_refCarburant == c.CODE_REF);
                        if (ctCarburant != null) context.CarburantCoutUnitaires.Remove(ctCarburant);
                    }
                        context.REFERENTIELs.Remove(c);
                }
                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }

        public static List<TYPE_REFERENTIEL> GetReferentielKeys()
        {
            using (Entities context = new Entities())
            {
                return context.TYPE_REFERENTIEL.ToList();
            }
        }

        public static List<SelectListItem> GetReferentiel(string keyRef)
        {
            using (Entities context = new Entities())
            {
                var list = context.REFERENTIELs
                            .Where(x => x.KEY_REF == keyRef).OrderBy(y => y.CODE_REF)
                            .AsEnumerable()
                            .Select(z => new SelectListItem { Value = z.CODE_REF.ToString(), Text = z.VALUE})
                            .ToList();
                return list;
            }
           
                     
        }
    }
}
