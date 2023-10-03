using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;

namespace GPA.BLL
{
    public class EntretienMaitreService
    {
        public static List<EntretienMaitreModel> Read(int client)
        {
            using (Entities context = new Entities())
            {
                var list = context.EntretienMaitres.Select(e => new EntretienMaitreModel
                {
                    IdEntretienMaitre = e.IdEntretienMaitre,
                    ClientId = e.ClientId,
                    DescriptionEntretienMaitre = e.DescriptionEntretienMaitre,
                    RepeterNb = e.RepeterNb,
                    RepeterPeriode = e.RepeterPeriode,
                    RepeterKilometrage = e.RepeterKilometrage,
                    RappelNb = e.RappelNb,
                    RappelPeriode = e.RappelPeriode,
                    RappelKilometrage = e.RappelKilometrage
                }).ToList();
                if (client > 0)
                {
                    list = list.Where(x => x.ClientId == client).ToList();
                }
                foreach(var row in list)
                {
                    if (row.RepeterPeriode > 0 && row.RepeterNb > 0)
                    {
                        string periode = context.REFERENTIELs.FirstOrDefault(x => x.KEY_REF == "PERIODE" && x.CODE_REF == row.RepeterPeriode).VALUE;
                        if (row.RepeterNb > 0)
                        {
                            row.Repeter += row.RepeterNb + " " + periode;
                        }
                        if (row.RepeterKilometrage > 0)
                        {
                            row.Repeter += " ou " + row.RepeterKilometrage + " km"; 
                        }
                    }
                    else
                    {
                        if (row.RepeterKilometrage > 0)
                        {
                            row.Repeter += row.RepeterKilometrage + " km";
                        }
                    }

                    if (row.RappelPeriode > 0 && row.RappelNb > 0)
                    {
                        string periode = context.REFERENTIELs.FirstOrDefault(x => x.KEY_REF == "PERIODE" && x.CODE_REF == row.RappelPeriode).VALUE;
                        if (row.RappelNb > 0)
                        {
                            row.Rappel += row.RappelNb + " " + periode;
                        }
                        if (row.RappelKilometrage > 0)
                        {
                            row.Rappel += " ou " + row.RappelKilometrage + " km";
                        }
                    }
                    else
                    {
                        if (row.RappelKilometrage > 0)
                        {
                            row.Rappel += row.RappelKilometrage + " km";
                        }
                    }
                }
                return list;
            }
        }

        public static EntretienMaitreModel Get(int id)
        {
            using (Entities context = new Entities())
            {
                var row = context.EntretienMaitres.Select(e => new EntretienMaitreModel
                {
                    IdEntretienMaitre = e.IdEntretienMaitre,
                    ClientId = e.ClientId,
                    DescriptionEntretienMaitre = e.DescriptionEntretienMaitre,
                    RepeterNb = e.RepeterNb,
                    RepeterPeriode = e.RepeterPeriode,
                    RepeterKilometrage = e.RepeterKilometrage,
                    RappelNb = e.RappelNb,
                    RappelPeriode = e.RappelPeriode,
                    RappelKilometrage = e.RappelKilometrage
                }).ToList().FirstOrDefault(x => x.IdEntretienMaitre == id);


                if (row.RepeterPeriode > 0 && row.RepeterNb > 0)
                {
                    string periode = context.REFERENTIELs.FirstOrDefault(x => x.KEY_REF == "PERIODE" && x.CODE_REF == row.RepeterPeriode).VALUE;
                    if (row.RepeterNb > 0)
                    {
                        row.Repeter += row.RepeterNb + " " + periode;
                    }
                    if (row.RepeterKilometrage > 0)
                    {
                        row.Repeter += " ou " + row.RepeterKilometrage + " km";
                    }
                }
                else
                {
                    if (row.RepeterKilometrage > 0)
                    {
                        row.Repeter += row.RepeterKilometrage + " km";
                    }
                }

                if (row.RappelPeriode > 0 && row.RappelNb > 0)
                {
                    string periode = context.REFERENTIELs.FirstOrDefault(x => x.KEY_REF == "PERIODE" && x.CODE_REF == row.RappelPeriode).VALUE;
                    if (row.RappelNb > 0)
                    {
                        row.Rappel += row.RappelNb + " " + periode;
                    }
                    if (row.RappelKilometrage > 0)
                    {
                        row.Rappel += " ou " + row.RappelKilometrage + " km";
                    }
                }
                else
                {
                    if (row.RappelKilometrage > 0)
                    {
                        row.Rappel += row.RappelKilometrage + " km";
                    }
                }

                return row;
            }
        }

        public static EntretienMaitreModel Create(EntretienMaitreModel model)
        {
            using (Entities context = new Entities())
            {
                var entity = new EntretienMaitre
                {
                    ClientId = model.ClientId,
                    DescriptionEntretienMaitre = model.DescriptionEntretienMaitre,
                    RepeterNb = model.RepeterNb,
                    RepeterPeriode = model.RepeterPeriode,
                    RepeterKilometrage = model.RepeterKilometrage,
                    RappelNb = model.RappelNb,
                    RappelPeriode = model.RappelPeriode,
                    RappelKilometrage = model.RappelKilometrage
                };
                context.EntretienMaitres.Add(entity);
                context.SaveChanges();
                model.IdEntretienMaitre = entity.IdEntretienMaitre;
                return model;
            }
        }

        public static bool Update(EntretienMaitreModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.EntretienMaitres.FirstOrDefault(e => e.IdEntretienMaitre == model.IdEntretienMaitre);
                if (entity != null)
                {
                    entity.ClientId = model.ClientId;
                    entity.DescriptionEntretienMaitre = model.DescriptionEntretienMaitre;
                    entity.RepeterNb = model.RepeterNb;
                    entity.RepeterPeriode = model.RepeterPeriode;
                    entity.RepeterKilometrage = model.RepeterKilometrage;
                    entity.RappelNb = model.RappelNb;
                    entity.RappelPeriode = model.RappelPeriode;
                    entity.RappelKilometrage = model.RappelKilometrage;

                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }

        public static bool Delete(EntretienMaitreModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.EntretienMaitres.FirstOrDefault(e => e.IdEntretienMaitre == model.IdEntretienMaitre);

                if (entry != null)
                {
                    context.EntretienMaitres.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }
    }
}
