using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;

namespace GPA.BLL
{
    public class FournisseurService
    {
        public static List<FournisseurModel> Read(int client)
        {
            using (Entities context = new Entities())
            {
                var list = context.Fournisseurs.Select(e => new FournisseurModel
                {
                    IdFournisseur = e.IdFournisseur,
                    ClientId = e.ClientId,
                    TypeFournisseur = e.TypeFournisseur,
                    NomFournisseur = e.NomFournisseur,
                    AdresseFournisseur = e.AdresseFournisseur,
                    VilleFournisseur = e.VilleFournisseur,
                    CodePostalFournisseur = e.CodePostalFournisseur,
                    ContactFournisseur = e.ContactFournisseur,
                    Tel1Fournisseur = e.Tel1Fournisseur,
                    Tel2Fournisseur = e.Tel2Fournisseur,
                    FaxFournisseur = e.FaxFournisseur,
                    SiteWebFournisseur = e.SiteWebFournisseur,
                    EmailFournisseur = e.EmailFournisseur
                }).ToList();
                if (client > 0)
                {
                    list = list.Where(x => x.ClientId == client).ToList();
                }
                return list;
            }
        }

        public static FournisseurModel Create(FournisseurModel model)
        {
            using (Entities context = new Entities())
            {
                var entity = new Fournisseur
                {
                    TypeFournisseur = model.TypeFournisseur,
                    ClientId = model.ClientId,
                    NomFournisseur = model.NomFournisseur,
                    AdresseFournisseur = model.AdresseFournisseur,
                    VilleFournisseur = model.VilleFournisseur,
                    CodePostalFournisseur = model.CodePostalFournisseur,
                    ContactFournisseur = model.ContactFournisseur,
                    Tel1Fournisseur = model.Tel1Fournisseur,
                    Tel2Fournisseur = model.Tel2Fournisseur,
                    FaxFournisseur = model.FaxFournisseur,
                    SiteWebFournisseur = model.SiteWebFournisseur,
                    EmailFournisseur = model.EmailFournisseur
                };
                context.Fournisseurs.Add(entity);
                context.SaveChanges();
                model.IdFournisseur = entity.IdFournisseur;
                return model;
            }
        }

        public static bool Update(FournisseurModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.Fournisseurs.FirstOrDefault(e => e.IdFournisseur == model.IdFournisseur);
                if (entity != null)
                {
                    entity.ClientId = model.ClientId;
                    entity.TypeFournisseur = model.TypeFournisseur;
                    entity.NomFournisseur = model.NomFournisseur;
                    entity.AdresseFournisseur = model.AdresseFournisseur;
                    entity.VilleFournisseur = model.VilleFournisseur;
                    entity.CodePostalFournisseur = model.CodePostalFournisseur;
                    entity.ContactFournisseur = model.ContactFournisseur;
                    entity.Tel1Fournisseur = model.Tel1Fournisseur;
                    entity.Tel2Fournisseur = model.Tel2Fournisseur;
                    entity.FaxFournisseur = model.FaxFournisseur;
                    entity.SiteWebFournisseur = model.SiteWebFournisseur;
                    entity.EmailFournisseur = model.EmailFournisseur;

                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }

        public static bool Delete(FournisseurModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.Fournisseurs.FirstOrDefault(e => e.IdFournisseur == model.IdFournisseur);

                if (entry != null)
                {
                    context.Fournisseurs.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }
    }
}
