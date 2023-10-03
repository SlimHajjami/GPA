using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;

namespace GPA.BLL
{
    public class RapportConsommationCarburantMensuelService
    {
        public static List<RapportConsommationCarburantMensuelModel> Read(int client, DepartementsAccessRightsModel departementsAccess, bool mensuelImported, int departement, int annee, int mois)
        {
            using (Entities context = new Entities())
            {
                List<RapportConsommationCarburantMensuelModel> list = new List<RapportConsommationCarburantMensuelModel>();

                var vehicules = context.Vehicules.ToList();

                if (departementsAccess != null)
                {
                    var d = departementsAccess.Visualisation;
                    vehicules = vehicules.Where(x => x.DepartementId != null && d.Contains((int)x.DepartementId)).ToList();
                }
                if (client > 0)
                {
                    vehicules = vehicules.Where(x => x.ClientId == client).ToList();
                }
                if (departement > 0)
                {
                    vehicules = vehicules.Where(x => x.DepartementId == departement).ToList();
                }

                var vehsIds = vehicules.Select(y => y.IdVehicule);
                var carburant = context.Carburants.Where(x => vehsIds.Contains(x.VehiculeId)).ToList();
                var carburantPR = context.Carburants.Where(x => vehsIds.Contains(x.VehiculeId)).ToList();

                DateTime from = new DateTime(annee, mois, 1);
                DateTime to = from.AddMonths(1);
                DateTime fromPR = from.AddMonths(-1);

                int moisPR = mois;
                int anneePR = annee;

                if (moisPR == 1)
                {
                    moisPR = 12;
                    anneePR--;
                }
                else
                {
                    moisPR--;
                }
                carburant = carburant.Where(x => x.DateCarburant >= from && x.DateCarburant < to).ToList();
                carburantPR = carburantPR.Where(x => x.DateCarburant >= fromPR && x.DateCarburant < from).ToList();


                foreach (var v in vehicules)
                {
                    RapportConsommationCarburantMensuelModel row = new RapportConsommationCarburantMensuelModel
                    {
                        ClientId = v.ClientId,
                        Client = context.Clients.FirstOrDefault(x => x.IdClient == v.ClientId).NomClient,
                        VehiculeId = v.IdVehicule,
                        Vehicule = v.MatriculeVehicule,
                        DepartementId = v.DepartementId,
                        Kilometrage = 0,
                        KilometragePR = 0,
                        Consommation = 0,
                        ConsommationPR = 0,
                        C100KM = 0,
                        C100KMPR = 0               
                        
                    };
                    if (v.ConducteurId.HasValue && v.ConducteurId > 0)
                    {
                        int c = v.ConducteurId.Value;
                        var cc = context.VehiculeConducteurs.FirstOrDefault(x => x.IdConducteur == c);
                        if (cc != null)
                        {
                            row.Conducteur = cc.PrenomConducteur + " " + cc.NomConducteur;
                        }
                    }
                    if (v.DepartementId.HasValue && v.DepartementId > 0)
                    {
                        int d = v.DepartementId.Value;
                        row.Departement = context.VehiculeDepartements.FirstOrDefault(x => x.IdDepartement == d).NomDepartement;
                    }

                    var carbs = carburant.Where(x => x.VehiculeId == v.IdVehicule).ToList();
                    if (carbs.Count() > 0)
                    {
                        row.Consommation = carbs.Sum(x => x.QuantiteCarburant);
                    }

                    if (mensuelImported) //Cas 1: Client parenin (Kilometrage mensuel)
                    {
                        var KiloMensuel = context.KilometrageMensuels.FirstOrDefault(x => x.VehiculeId == v.IdVehicule && x.Date.Month == mois && x.Date.Year == annee);
                        if (KiloMensuel != null)
                        {
                            row.Kilometrage = KiloMensuel.Kilometrage;
                            if (row.Kilometrage != 0)
                            {
                                row.C100KM = row.Consommation / (row.Kilometrage / 100);
                            }
                        }
                    }
                    else
                    {//Cas 2: Client qui n'utilise pas le kilometrage mensuel
                        var kilos = context.LectureKilometrages.Where(x => x.VehiculeId == v.IdVehicule && x.DateLectureKilometrage >= from && x.DateLectureKilometrage < to);
                        if (kilos.Count() > 0)
                        {
                            row.Kilometrage = kilos.Max(x => x.Kilometrage) - kilos.Min(x => x.Kilometrage);
                        }

                        // changé 19/01/2018  
                        var carbs1 = carbs.Where(x => x.KilometrageFin > 0).ToList();

                        if (carbs1.Count() > 0 && carbs.Count() - carbs1.Count() > 1) //Cas 2.1 Client qui remplie minutieusement kilomotrage lors d'un plein d'essence
                        {
                            decimal kilosCarb = (decimal)(carbs.Max(x => x.KilometrageFin) - carbs.Min(x => x.KilometrageFin));
                            var consommationCarb = row.Consommation - carbs.OrderByDescending(x => x.DateCarburant).FirstOrDefault().QuantiteCarburant;
                            if (kilosCarb > 0 && consommationCarb > 0)
                            {
                                row.C100KM = consommationCarb / (kilosCarb / 100);
                            }
                        }
                        else if (row.Kilometrage > 0)//Cas 2.2 consommation en fonction du Kilometrage calculé de la table LectureKilometrages(journalier)
                        {
                            row.C100KM = row.Consommation / (row.Kilometrage / 100);
                        }
                    }


                    /****************PR*********************/
                    carbs = carburantPR.Where(x => x.VehiculeId == v.IdVehicule).ToList();
                    if (carbs.Count() > 0)
                    {
                        row.ConsommationPR = carbs.Sum(x => x.QuantiteCarburant);
                    }

                    if (mensuelImported)//modifié le 12/04/2018
                    {
                        var KiloMensuel = context.KilometrageMensuels.FirstOrDefault(x => x.VehiculeId == v.IdVehicule && x.Date.Month == moisPR && x.Date.Year == anneePR);
                        if (KiloMensuel != null)
                        {
                            row.KilometragePR = KiloMensuel.Kilometrage;
                            if (row.KilometragePR != 0)
                            {
                                row.C100KMPR = row.ConsommationPR / (row.KilometragePR / 100);
                            }
                        }
                    }
                    else
                    {
                        var kilos = context.LectureKilometrages.Where(x => x.VehiculeId == v.IdVehicule && x.DateLectureKilometrage >= fromPR && x.DateLectureKilometrage < from);
                        if (kilos.Count() > 0)
                        {
                            row.KilometragePR = kilos.Max(x => x.Kilometrage) - kilos.Min(x => x.Kilometrage);
                        }

                        // modifié le 19/01/2018
                        var carbsPr = carbs.Where(x => x.KilometrageFin > 0).ToList();

                        if (carbsPr.Count() > 0 && carbs.Count() - carbsPr.Count() > 1)
                        {
                            decimal kilosCarb = (decimal)(carbs.Max(x => x.KilometrageFin) - carbs.Min(x => x.KilometrageFin));
                            var consommationCarb = row.Consommation - carbs.OrderByDescending(x => x.DateCarburant).FirstOrDefault().QuantiteCarburant;
                            if (kilosCarb > 0 && consommationCarb > 0)
                            {
                                row.C100KM = consommationCarb / (kilosCarb / 100);
                            }
                        }
                        else if (row.KilometragePR > 0)
                        {
                            row.C100KMPR = row.ConsommationPR / (row.KilometragePR / 100);
                        }
                    }

                    list.Add(row);
                }

                return list;
            }
        }
    }
}
