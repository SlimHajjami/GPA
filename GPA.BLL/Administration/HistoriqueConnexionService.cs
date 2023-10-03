using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPA.Models;
using GPA.DAL;

namespace GPA.BLL
{
    public class HistoriqueConnexionService
    {

        /* static HistoriqueConnexionService()//int userId)
        {
            this.userId = userId;
        }

        private int userId;*/

        public static bool UserLogged(UtilisateurModel user,string ip)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = new HISTORIQUE_CONNEXION
                {
                    USER_ID = user.IdUtilisateur,
                    AdresseIP = ip,
                    LogIn_Date = DateTime.Now.ToUniversalTime(),

                };

                context.HISTORIQUE_CONNEXION.Add(entity);
                objectAffected = context.SaveChanges();

                return objectAffected > 0;
            }
        }

        public static void UserDisconnected(int userId)
        {
            using (Entities context = new Entities())
            {
                HISTORIQUE_CONNEXION entity;
                // int entryUpdated = 0;

                entity = context.HISTORIQUE_CONNEXION.OrderByDescending(d => d.LogIn_Date).FirstOrDefault(p => p.USER_ID == userId && p.LogOut_Date == null);
                if (entity != null)
                {
                    entity.LogOut_Date = DateTime.Now.ToUniversalTime();
                    //   entryUpdated = 
                    context.SaveChanges();
                }
                //  return entryUpdated > 0;

            }
        }

        public static string isActive(int ID)
        {
            using (Entities context = new Entities())
            {
                HISTORIQUE_CONNEXION entity;
                entity = context.HISTORIQUE_CONNEXION.FirstOrDefault(p => p.USER_ID == ID && p.LogOut_Date == null);
                if (entity != null)
                {
                    return "connecté";
                }
                else
                    return "non connecté";
            }
        }

        public static TimeSpan ActivityTime(DateTime EntryDate, DateTime ExitDate)
        {
            TimeSpan result = ExitDate - EntryDate;

            return result;
        }
        public static List<HistoriqueConnexionViewModel> GetHistorique(int? annee, int? mois)
        {
            using (Entities context = new Entities())
            {
                List<HistoriqueConnexionViewModel> results = new List<HistoriqueConnexionViewModel>();
                //  List<PCB_HISTORIQUE_CONNEXION> historiques  = new List<PCB_HISTORIQUE_CONNEXION>();

                var entities = context.HISTORIQUE_CONNEXION.ToList();

                if (annee != null)
                {
                    entities = entities.Where(p => p.LogIn_Date.Year == annee).ToList();
                }
                if (mois != null)
                {
                    entities = entities.Where(p => p.LogIn_Date.Month == mois).ToList();
                }
                //if (etat != null)
                //{
                //    if (etat.Equals("connecté"))
                //    {
                //        entities = entities.Where(p => p.LogOut_Date == null).ToList();
                //    }
                //    else
                //        if (etat.Equals("non connecté"))
                //    {
                //        entities = entities.Where(p => p.LogOut_Date != null).ToList();

                //    }
                //}

                if (entities.Count != 0)
                {
                    foreach (var entity in entities)
                    {
                        string utilisateur = "";
                        var _utilisateur = context.Utilisateurs.FirstOrDefault(i => i.IdUtilisateur == entity.USER_ID);
                        if (_utilisateur != null)
                            utilisateur = _utilisateur.PrenomUtilisateur + " " + _utilisateur.NomUtilisateur;
                        string role = "";
                        var _role = context.Roles.FirstOrDefault(i => i.IdRole == _utilisateur.RoleId);
                        if (_role != null)
                            role = _role.NomRole;

                        TimeSpan tempactivité = new TimeSpan(0, 0, 0);
                        if (entity.LogOut_Date != null)
                            tempactivité = ActivityTime((DateTime)entity.LogIn_Date, (DateTime)entity.LogOut_Date);
                        else tempactivité = ActivityTime((DateTime)entity.LogIn_Date, (DateTime)System.DateTime.Now.ToUniversalTime());

                        var days = tempactivité.Days;
                        var hours = tempactivité.Hours;
                        var minutes = tempactivité.Minutes;
                        string tempsformatte = "";
                        if ((days > 0 ) || hours > 0)
                        {
                            tempsformatte = "1hr";
                        }// if (days > 0) tempsformatte = String.Format("{0}days {1}hrs {2}mins", days, hours, minutes);
                       // else if (hours < 8 && hours > 0) tempsformatte = String.Format("{0} hrs {1} mins", hours, minutes);
                        else tempsformatte = String.Format("{0} mins", minutes);

                        HistoriqueConnexionViewModel result = new HistoriqueConnexionViewModel()
                        {
                            Id = entity.ID,
                            UserId = entity.USER_ID,
                            Utilisateur = utilisateur,
                            Role = role,
                            DateLogIn = entity.LogIn_Date,
                            DateToDisplay = entity.LogIn_Date.ToString(),
                            DateLogOut = entity.LogOut_Date,
                            Etat = isActive(entity.USER_ID),
                            TempsActivite = tempsformatte,
                            AdresseIP = entity.AdresseIP


                        };
                        results.Add(result);
                    }
                }
              
                return results;
            }

        }
    }
}
