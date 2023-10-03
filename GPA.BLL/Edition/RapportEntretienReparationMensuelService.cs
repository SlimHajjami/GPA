using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;

namespace GPA.BLL
{
    public class RapportEntretienReparationMensuelService
    {
        public static List<RapportEntretienReparationMensuelModel> Read(int client, DepartementsAccessRightsModel departementsAccess, bool mensuelImported, int departement, int annee, int mois)
        {
            using (Entities context = new Entities())
            {
                List<RapportEntretienReparationMensuelModel> list = new List<RapportEntretienReparationMensuelModel>();

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
                var entretiens = context.Entretiens.Where(x => vehsIds.Contains(x.VehiculeId)).ToList();
                var reparations = context.Reparations.Where(x => vehsIds.Contains(x.VehiculeId)).ToList();

                DateTime from = new DateTime(annee, mois, 1);
                DateTime to = from.AddMonths(1);
                carburant = carburant.Where(x => x.DateCarburant >= from && x.DateCarburant < to).ToList();
                entretiens = entretiens.Where(x => x.DateEntretien >= from && x.DateEntretien < to).ToList();
                reparations = reparations.Where(x => x.DateReparation >= from && x.DateReparation < to).ToList();


                foreach (var v in vehicules)
                {
                    RapportEntretienReparationMensuelModel row = new RapportEntretienReparationMensuelModel
                    {
                        ClientId = v.ClientId,
                        Client = context.Clients.FirstOrDefault(x => x.IdClient == v.ClientId).NomClient,
                        VehiculeId = v.IdVehicule,
                        Vehicule = v.MatriculeVehicule,
                        DepartementId = v.DepartementId,
                        Kilometrage = 0,
                        Consommation = 0,
                        Entretien = 0,
                        Reparation = 0,
                        CoutTotal = 0,
                        CoutKM = 0,
                        ER100KM = 0,                      
                        C100KM = 0
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
                        row.Consommation = carbs.Sum(x => (decimal)x.CoutTotal);
                    }

                    var ents = entretiens.Where(x => x.VehiculeId == v.IdVehicule).ToList();
                    if (ents.Count() > 0)
                    {
                        row.Entretien = ents.Sum(x => x.CoutEntretien);
                    }

                    var reps = reparations.Where(x => x.VehiculeId == v.IdVehicule).ToList();
                    if (reps.Count() > 0)
                    {
                        row.Reparation = reps.Sum(x => x.CoutReparation);
                    }
                    
                    

                    if (mensuelImported)
                    {
                        var KiloMensuel = context.KilometrageMensuels.FirstOrDefault(x => x.VehiculeId == v.IdVehicule && x.Date.Month == mois && x.Date.Year == annee);
                        if (KiloMensuel != null)
                        {
                            row.Kilometrage = KiloMensuel.Kilometrage;
                        }
                    }
                    else
                    {
                        var kilos = context.LectureKilometrages.Where(x => x.VehiculeId == v.IdVehicule && x.DateLectureKilometrage >= from && x.DateLectureKilometrage < to);
                        if (kilos.Count() > 0)
                        {
                            row.Kilometrage = kilos.Max(x => x.Kilometrage) - kilos.Min(x => x.Kilometrage);
                        }
                    }

                    if (row.Kilometrage > 0)
                    {
                        row.C100KM = row.Consommation / (row.Kilometrage / 100);//(carbs.Sum(x => x.QuantiteCarburant) / row.Kilometrage) * 100; //
                    }

                    row.CoutTotal = row.Consommation + row.Entretien + row.Reparation;

                    if(row.Kilometrage > 0)
                    {
                        row.CoutKM = row.CoutTotal / row.Kilometrage;
                        row.ER100KM = (row.Entretien + row.Reparation) / (row.Kilometrage / 100);
                    }

                    list.Add(row);
                }

                return list;
            }
        }
    }
}
