using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;

namespace GPA.BLL
{
    public class RapportDepensesService
    {
        public static List<RapportDepensesModel> Read(int client, DepartementsAccessRightsModel departementsAccess, int departement, DateTime? from, DateTime? to)
        {
            using (Entities context = new Entities())
            {
                List<RapportDepensesModel> list = new List<RapportDepensesModel>();

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
                var entretiens = context.Entretiens.Where(x => vehsIds.Contains(x.VehiculeId)).ToList();
                var reparations = context.Reparations.Where(x => vehsIds.Contains(x.VehiculeId)).ToList();
                var carburant = context.Carburants.Where(x => vehsIds.Contains(x.VehiculeId)).ToList();                
                
                if (from.HasValue && to.HasValue)
                {
                    var dateTo = (DateTime) to;
                    entretiens = entretiens.Where(x => x.DateEntretien >= from && x.DateEntretien <= dateTo.AddDays(1)).ToList();
                    reparations = reparations.Where(x => x.DateReparation >= from && x.DateReparation <= dateTo.AddDays(1)).ToList();
                    carburant = carburant.Where(x => x.DateCarburant >= from && x.DateCarburant <= dateTo.AddDays(1)).ToList();
                }

                foreach (var v in vehicules)
                {
                    RapportDepensesModel row = new RapportDepensesModel 
                    {
                        ClientId = v.ClientId,
                        Client = context.Clients.FirstOrDefault(x => x.IdClient == v.ClientId).NomClient,
                        VehiculeId = v.IdVehicule,
                        Vehicule = v.MatriculeVehicule,
                        DepartementId = v.DepartementId,
                        Entretien = 0,
                        Reparation = 0,
                        Carburant = 0
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

                    var ents = entretiens.Where(x => x.VehiculeId == v.IdVehicule).ToList();
                    if(ents.Count() > 0)
                    {
                        row.Entretien = ents.Sum(x => x.CoutEntretien);
                    }

                    var reps = reparations.Where(x => x.VehiculeId == v.IdVehicule).ToList();
                    if (reps.Count() > 0)
                    {
                        row.Reparation = reps.Sum(x => x.CoutReparation);
                    }

                    var carbs = carburant.Where(x => x.VehiculeId == v.IdVehicule).ToList();
                    if (carbs.Count() > 0)
                    {
                        row.Carburant = carbs.Sum(x => (decimal)x.CoutTotal);
                    }

                    list.Add(row);
                }

                return list;
            }
        }
    }
}
