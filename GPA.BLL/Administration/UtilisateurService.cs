using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;

namespace GPA.BLL
{
    public class UtilisateurService
    {
        public static List<UtilisateurModel> Read(int profile, int client)
        {
            using (Entities context = new Entities())
            {
                var list = context.Utilisateurs.Select(e => new UtilisateurModel
                {
                    IdUtilisateur = e.IdUtilisateur,
                    RoleId = e.RoleId,
                    ClientId = e.ClientId,
                    NomUtilisateur = e.NomUtilisateur,
                    PrenomUtilisateur = e.PrenomUtilisateur,
                    LoginUtilisateur = e.LoginUtilisateur,
                    PasswordUtilisateur = e.PasswordUtilisateur,
                    EmailUtilisateur = e.EmailUtilisateur,
                    FonctionUtilisateur = e.FonctionUtilisateur
                }).ToList();
                if (profile > 1)
                {
                    var roles = RoleService.GetAll().Where(x => x.ProfileRole >= profile).Select(y => y.IdRole);
                    list = list.Where(x => x.ClientId == client && roles.Contains(x.RoleId)).ToList();
                }
                else
                {
                    if (client > 0)
                    {
                        list = list.Where(x => x.ClientId == client).ToList();
                    }
                }
                return list;
            }
        }

        public static UtilisateurModel GetByUsername(string login)
        {
            using (Entities context = new Entities())
            {
                var myUser = context.Utilisateurs.Where(u => u.LoginUtilisateur.ToLower() == login.ToLower())
                            .Select(e => new UtilisateurModel
                            {
                                IdUtilisateur = e.IdUtilisateur,
                                RoleId = e.RoleId,
                                ClientId = e.ClientId,
                                NomUtilisateur = e.NomUtilisateur,
                                PrenomUtilisateur = e.PrenomUtilisateur,
                                LoginUtilisateur = e.LoginUtilisateur,
                                PasswordUtilisateur = e.PasswordUtilisateur,
                                EmailUtilisateur = e.EmailUtilisateur,
                                FonctionUtilisateur = e.FonctionUtilisateur
                            }).ToList().SingleOrDefault();
                return myUser;
            }
        }

        public static IEnumerable<string> GetUserEmails()
        {
            using (Entities context = new Entities())
            {
                return context.Utilisateurs.Select(e => e.EmailUtilisateur).ToList().Distinct();
            }
        }

        public static bool IsValid(string login, string password, ref UtilisateurModel user)
        {
            using (Entities context = new Entities())
            {
                bool IsValid = false;
                var myUser = context.Utilisateurs.FirstOrDefault(u => u.LoginUtilisateur == login);
                if (myUser != null)
                {
                    if (myUser.PasswordUtilisateur == password)
                    {
                        user = new UtilisateurModel
                        {
                            LoginUtilisateur = myUser.LoginUtilisateur,
                            PasswordUtilisateur = myUser.PasswordUtilisateur,
                            NomUtilisateur = myUser.NomUtilisateur,
                            PrenomUtilisateur = myUser.PrenomUtilisateur,
                            IdUtilisateur = myUser.IdUtilisateur,
                            RoleId = myUser.RoleId,
                        };
                        IsValid = true;
                    }
                }

                return IsValid;
            }
        }

        public static UtilisateurModel Create(UtilisateurModel model)
        {
            using (Entities context = new Entities())
            {
                var entity = new Utilisateur
                {
                    RoleId = model.RoleId,
                    ClientId = model.ClientId,
                    NomUtilisateur = model.NomUtilisateur,
                    PrenomUtilisateur = model.PrenomUtilisateur,
                    LoginUtilisateur = model.LoginUtilisateur,
                    PasswordUtilisateur = model.PasswordUtilisateur,
                    EmailUtilisateur = model.EmailUtilisateur,
                    FonctionUtilisateur = model.FonctionUtilisateur
                };
                context.Utilisateurs.Add(entity);
                context.SaveChanges();
                model.IdUtilisateur = entity.IdUtilisateur;
                return model;
            }
        }

        public static bool Update(UtilisateurModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.Utilisateurs.FirstOrDefault(e => e.IdUtilisateur == model.IdUtilisateur);
                if (entity != null)
                {
                    entity.RoleId = model.RoleId;
                    entity.ClientId = model.ClientId;
                    entity.NomUtilisateur = model.NomUtilisateur;
                    entity.PrenomUtilisateur = model.PrenomUtilisateur;
                    entity.LoginUtilisateur = model.LoginUtilisateur;
                    entity.PasswordUtilisateur = model.PasswordUtilisateur;
                    entity.EmailUtilisateur = model.EmailUtilisateur;
                    entity.FonctionUtilisateur = model.FonctionUtilisateur;

                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }

        public static bool Delete(UtilisateurModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.Utilisateurs.FirstOrDefault(e => e.IdUtilisateur == model.IdUtilisateur);

                if (entry != null)
                {
                    context.Utilisateurs.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }
    }
}
