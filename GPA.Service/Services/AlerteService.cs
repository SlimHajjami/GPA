using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPA.DAL;
using GPA.Service.Utilities;

namespace GPA.Service.Services
{
    public class AlerteService
    {
        public Emailer emailer { get; set; }

        public AlerteService()
        {
            using (Entities context = new Entities())
            {
                var p = context.Parametres.Where(x => x.KeyWord.Contains("Emailer"));
                emailer = new Emailer(p.FirstOrDefault(x => x.KeyWord == "Emailer.SmtpHost").Value,
                    p.FirstOrDefault(x => x.KeyWord == "Emailer.SmtpUser").Value,
                    p.FirstOrDefault(x => x.KeyWord == "Emailer.SmtpPassword").Value,
                    p.FirstOrDefault(x => x.KeyWord == "Emailer.FromEmail").Value,
                    p.FirstOrDefault(x => x.KeyWord == "Emailer.FromName").Value);
                int port = 0;
                if (int.TryParse(p.FirstOrDefault(x => x.KeyWord == "Emailer.SmtpPort").Value, out port))
                {
                    emailer.SmtpPort = port;
                }
                if (p.FirstOrDefault(x => x.KeyWord == "Emailer.Ssl").Value == "1")
                {
                    emailer.EnableSsl = true;
                }
                emailer.IsBodyHtml = true;
            }
        }

        public static List<AlerteEmail> getAlertesEmails(int type)
        {
            using (Entities context = new Entities())
            {
                return context.AlerteEmails.Where(x => x.TypeAlerteId == type).ToList();
            }
        }
     

        public int SendAssuranceAlertes()
        {
            int nbEmails = 0;
            using (Entities context = new Entities())
            {
                var emails = getAlertesEmails(1);
                foreach (var email in emails)
                {
                    var list = context.VehiculeDocuments.ToList();

                    var vehicules = context.Vehicules.Where(x => x.ClientId == email.ClientId).Select(y => y.IdVehicule);
                    list = list.Where(x => vehicules.Contains(x.VehiculeId)).ToList();

                    string body = "Bonjour,<br><br>Les assurances non payées:<br><br>" +
                                  "<table border='1' style='border-collapse: collapse;width:100%;'><tr><td><b>Véhiclue</b></td><td><b>Département</b></td><td><b>Date Expiration</b></td></tr>";
                    string subject = "GPA - Assurances Non Payés";

                    int i = 0;
                    foreach (var row in list)
                    {
                        if (row.ExpirationAssurance.HasValue)
                        {
                            DateTime now = DateTime.Now;
                            if (row.RappelAssurance.HasValue)
                            {
                                now = now.AddDays(row.RappelAssurance.Value);
                            }
                            if (row.ExpirationAssurance.Value <= now)
                            {
                                var vehicule = context.Vehicules.FirstOrDefault(x => x.IdVehicule == row.VehiculeId);
                                string departement = "";
                                if(vehicule.DepartementId > 0)
                                {
                                    departement = context.VehiculeDepartements.FirstOrDefault(x => x.IdDepartement == vehicule.DepartementId).NomDepartement;
                                }
                                body += "<tr><td>" + vehicule.MatriculeVehicule + "</td><td>" + departement + "</td><td>" + row.ExpirationAssurance.Value.ToShortDateString() + "</td></tr>";
                                i++;
                            }
                        }
                    }

                    if (i > 0)
                    {
                        body += "</table>";
                        string error = "";
                        List<string> Tos = new List<string>();
                        Tos.Add(email.AlerteEmail1);

                        emailer.SendEmail(subject, Tos, new List<string>(), body, out error);
                        Logger.LogInformation("Sending Assurance mail...");
                        if (error != "")
                        {
                            Logger.LogException(error);
                        }
                        nbEmails++;
                    }
                }
            }
            return nbEmails;
        }


        public int SendTaxeAlertes()
        {
            int nbEmails = 0;
            using (Entities context = new Entities())
            {
                var emails = getAlertesEmails(2);
                foreach (var email in emails)
                {
                    var list = context.VehiculeDocuments.ToList();

                    var vehicules = context.Vehicules.Where(x => x.ClientId == email.ClientId).Select(y => y.IdVehicule);
                    list = list.Where(x => vehicules.Contains(x.VehiculeId)).ToList();

                    string body = "Bonjour,<br><br>Les taxes de circulation non payées:<br><br>" +
                                  "<table border='1' style='border-collapse: collapse;width:100%;'><tr><td><b>Véhiclue</b></td><td><b>Département</b></td><td><b>Date Expiration</b></td></tr>";
                    string subject = "GPA - Taxes de Circulation Non Payés";

                    int i = 0;
                    foreach (var row in list)
                    {
                        if (row.ExpirationTaxeCirculation.HasValue)
                        {
                            DateTime now = DateTime.Now;
                            if (row.RappelTaxeCirculation.HasValue)
                            {
                                now = now.AddDays(row.RappelTaxeCirculation.Value);
                            }
                            if (row.ExpirationTaxeCirculation.Value <= now)
                            {
                                var vehicule = context.Vehicules.FirstOrDefault(x => x.IdVehicule == row.VehiculeId);
                                string departement = "";
                                if (vehicule.DepartementId > 0)
                                {
                                    departement = context.VehiculeDepartements.FirstOrDefault(x => x.IdDepartement == vehicule.DepartementId).NomDepartement;
                                }
                                body += "<tr><td>" + vehicule.MatriculeVehicule + "</td><td>" + departement + "</td><td>" + row.ExpirationTaxeCirculation.Value.ToShortDateString() + "</td></tr>";
                                i++;
                            }
                        }
                    }

                    if (i > 0)
                    {
                        body += "</table>";
                        string error = "";
                        List<string> Tos = new List<string>();
                        Tos.Add(email.AlerteEmail1);

                        emailer.SendEmail(subject, Tos, new List<string>(), body, out error);
                        Logger.LogInformation("Sending Taxe mail...");
                        if (error != "")
                        {
                            Logger.LogException(error);
                        }
                        nbEmails++;
                    }
                }
            }
            return nbEmails;
        }


        public int SendVisiteTechniqueAlertes()
        {
            int nbEmails = 0;
            using (Entities context = new Entities())
            {
                var emails = getAlertesEmails(3);
                foreach (var email in emails)
                {
                    var list = context.VehiculeDocuments.ToList();

                    var vehicules = context.Vehicules.Where(x => x.ClientId == email.ClientId).Select(y => y.IdVehicule);
                    list = list.Where(x => vehicules.Contains(x.VehiculeId)).ToList();

                    string body = "Bonjour,<br><br>Les visites techniques non faites:<br><br>" +
                                  "<table border='1' style='border-collapse: collapse;width:100%;'><tr><td><b>Véhiclue</b></td><td><b>Département</b></td><td><b>Prochaine Visite</b></td></tr>";
                    string subject = "GPA - Visites Technique Non Faites";

                    int i = 0;
                    foreach (var row in list)
                    {
                        if (row.ProchaineVisiteTechnique.HasValue)
                        {
                            DateTime now = DateTime.Now;
                            if (row.RappelVisiteTechnique.HasValue)
                            {
                                now = now.AddDays(row.RappelVisiteTechnique.Value);
                            }
                            if (row.ProchaineVisiteTechnique.Value <= now)
                            {
                                var vehicule = context.Vehicules.FirstOrDefault(x => x.IdVehicule == row.VehiculeId);
                                string departement = "";
                                if (vehicule.DepartementId > 0)
                                {
                                    departement = context.VehiculeDepartements.FirstOrDefault(x => x.IdDepartement == vehicule.DepartementId).NomDepartement;
                                }
                                body += "<tr><td>" + vehicule.MatriculeVehicule + "</td><td>" + departement + "</td><td>" + row.ProchaineVisiteTechnique.Value.ToShortDateString() + "</td></tr>";
                                i++;
                            }
                        }                      
                    }

                    if (i > 0)
                    {
                        body += "</table>";
                        string error = "";
                        List<string> Tos = new List<string>();
                        Tos.Add(email.AlerteEmail1);

                        emailer.SendEmail(subject, Tos, new List<string>(), body, out error);
                        Logger.LogInformation("Sending Visite Technique mail...");
                        if (error != "")
                        {
                            Logger.LogException(error);
                        }
                        nbEmails++;
                    }
                }
            }
            return nbEmails;
        }


        public int sendEntretienAlertes()
        {
            int nbEmails = 0;
            using (Entities context = new Entities())
            {
                var emails = getAlertesEmails(4);
                foreach (var email in emails)
                {
                    var list = context.EntretienProgrammes.ToList();

                    var vehicules = context.Vehicules.Where(x => x.ClientId == email.ClientId).Select(y => y.IdVehicule);
                    list = list.Where(x => vehicules.Contains(x.VehiculeId)).ToList();

                    string body = "Bonjour,<br><br>Les entretiens non faits:<br><br>" +
                                  "<table border='1' style='border-collapse: collapse;width:100%;'><tr><td><b>Véhiclue</b></td><td><b>Département</b></td><td><b>Date Entretien</b></td><td><b>Description</b></td></tr>";
                    string subject = "GPA - Entretiens Non Faits";

                    int i = 0;
                    foreach (var row in list)
                    {
                        if (row.ProchaineDate.HasValue)
                        {
                            DateTime now = DateTime.Now;
                            if (row.RappelNb.HasValue && row.RappelPeriode.HasValue)
                            {
                                if (row.RappelPeriode == 1)
                                {
                                    now = now.AddDays(row.RappelNb.Value);
                                }
                                else if (row.RappelPeriode == 2)
                                {
                                    now = now.AddDays(row.RappelNb.Value * 7);
                                }
                                else if (row.RappelPeriode == 3)
                                {
                                    now = now.AddMonths(row.RappelNb.Value);
                                }
                                else if (row.RappelPeriode == 4)
                                {
                                    now = now.AddYears(row.RappelNb.Value);
                                }
                            }

                            if (row.ProchaineDate.Value <= now)
                            {
                                var vehicule = context.Vehicules.FirstOrDefault(x => x.IdVehicule == row.VehiculeId);
                                string departement = "";
                                if (vehicule.DepartementId > 0)
                                {
                                    departement = context.VehiculeDepartements.FirstOrDefault(x => x.IdDepartement == vehicule.DepartementId).NomDepartement;
                                }
                                string description = context.EntretienMaitres.FirstOrDefault(x => x.IdEntretienMaitre == row.EntretienMaitreId).DescriptionEntretienMaitre;
                                body += "<tr><td>" + vehicule.MatriculeVehicule + "</td><td>" + departement + "</td><td>" + row.ProchaineDate.Value.ToShortDateString() + "</td><td>" +description + "</td></tr>";
                                i++;
                            }
                        }
                    }

                    if (i > 0)
                    {
                        body += "</table>";
                        string error = "";
                        List<string> Tos = new List<string>();
                        Tos.Add(email.AlerteEmail1);

                        emailer.SendEmail(subject, Tos, new List<string>(), body, out error);
                        Logger.LogInformation("Sending Entretien mail...");
                        if (error != "")
                        {
                            Logger.LogException(error);
                        }
                        nbEmails++;
                    }
                }
            }
            return nbEmails;
        }
    }
}
