using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;

namespace GPA.BLL
{
    public class RapportEntretienReparationService
    {
        public static List<RapportEntretienReparationModel> Read(int client, DepartementsAccessRightsModel departementsAccess, int departement, DateTime? from, DateTime? to)
        {
            using (Entities context = new Entities())
            {
                List<RapportEntretienReparationModel> list = new List<RapportEntretienReparationModel>();
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
                if (from.HasValue && to.HasValue)
                {
                    var dateTo = (DateTime)to;
                    entretiens = entretiens.Where(x => x.DateEntretien >= from && x.DateEntretien <= dateTo.AddDays(1)).ToList();
                    reparations = reparations.Where(x => x.DateReparation >= from && x.DateReparation <= dateTo.AddDays(1)).ToList();
                }
                foreach (var v in vehicules)
                {

                    var entretiensVeh = entretiens.Where(x => x.VehiculeId == v.IdVehicule).ToList();
                    if (entretiensVeh.Count()>0)
                    {
                        foreach (var ent in entretiensVeh)
                        {
                            RapportEntretienReparationModel row = new RapportEntretienReparationModel
                            {
                                ClientId = v.ClientId,
                                Client = context.Clients.FirstOrDefault(x => x.IdClient == v.ClientId).NomClient,
                                VehiculeId = v.IdVehicule,
                                Vehicule = v.MatriculeVehicule,
                                DepartementId = v.DepartementId,
                                Kilometrage = 0,
                                Type = "Entretien",
                                Date = "",
                                Description = "",
                                Quantite = 0,
                                CoutTotal = 0,
                                CoutUnite = 0
                            };
                            if (v.DepartementId.HasValue && v.DepartementId > 0)
                            {
                                int d = v.DepartementId.Value;
                                row.Departement = context.VehiculeDepartements.FirstOrDefault(x => x.IdDepartement == d).NomDepartement;
                            }
                            row.Description = ent.DescriptionEntretien;
                            row.Date = ent.DateEntretien.ToShortDateString();
                            row.CoutTotal = ent.CoutEntretien;
                            if (ent.KilometrageEntretien.HasValue && ent.KilometrageEntretien>0)
                            {
                                row.Kilometrage = ent.KilometrageEntretien;
                            }
                            list.Add(row);
                            
                        }
                    }
                    
                    var reparationsVeh = reparations.Where(x=>x.VehiculeId == v.IdVehicule).ToList();
                    if (reparationsVeh.Count()>0)
                    {
                        foreach (var rep in reparationsVeh)
                        {
                            RapportEntretienReparationModel row = new RapportEntretienReparationModel
                            {
                                ClientId = v.ClientId,
                                Client = context.Clients.FirstOrDefault(x => x.IdClient == v.ClientId).NomClient,
                                VehiculeId = v.IdVehicule,
                                Vehicule = v.MatriculeVehicule,
                                DepartementId = v.DepartementId,
                                Kilometrage = 0,
                                Type = "Reparation",
                                Date = "",
                                Description = "",
                                Quantite = 0,
                                CoutTotal = 0,
                                CoutUnite = 0
                            };
                            if (v.DepartementId.HasValue && v.DepartementId > 0)
                            {
                                int d = v.DepartementId.Value;
                                row.Departement = context.VehiculeDepartements.FirstOrDefault(x => x.IdDepartement == d).NomDepartement;
                            }
                            row.Description = rep.DescriptionReparation;
                            row.Date = rep.DateReparation.ToShortDateString();
                            row.CoutTotal = rep.CoutReparation;
                            if (rep.KilometrageReparation.HasValue && rep.KilometrageReparation > 0)
                            {
                                row.Kilometrage = rep.KilometrageReparation;
                            }
                            list.Add(row);
                        }
                    }
                    
                }

                    return list;
            }
        }
    }
}
