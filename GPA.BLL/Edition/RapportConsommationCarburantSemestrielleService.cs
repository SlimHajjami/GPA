using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;
using System.Diagnostics;

namespace GPA.BLL
{
    public class RapportConsommationCarburantSemestrielleService
    {

        static string getModele(int? idModele)
        {
            if (idModele != null)
            {
                using (Entities context = new Entities())
                {
                    var modele = context.VehiculeModeles.FirstOrDefault(x => x.IdModele == idModele);
                    if (modele != null)
                    {
                        return modele.NomModele;
                    }
                    else return "";
                }
            }
            else
                return "";
        }

        public static List<RapportConsommationCarburantSemestrielleModel> Read(int client, DepartementsAccessRightsModel departementsAccess, int annee, int mois)
        {
            using (Entities context = new Entities())
            {
                DateTime currentMonth = new DateTime(annee, mois, 1);

                List<RapportConsommationCarburantSemestrielleModel> list = new List<RapportConsommationCarburantSemestrielleModel>();

                //  var vehicules = context.Vehicules.Where(x => x.ModeleVehicule != null).ToList();
                var vehicules = context.Vehicules.ToList();
                if (vehicules != null)
                {


                    if (departementsAccess != null)
                    {
                        var d = departementsAccess.Visualisation;
                        vehicules = vehicules.Where(x => x.DepartementId != null && d.Contains((int)x.DepartementId)).ToList();
                    }
                    if (client > 0)
                    {
                        vehicules = vehicules.Where(x => x.ClientId == client).ToList();
                    }

                    var vehsIds = vehicules.Select(y => y.IdVehicule);

                    foreach (var v in vehicules)
                    {

                        RapportConsommationCarburantSemestrielleModel row = new RapportConsommationCarburantSemestrielleModel
                        {
                            VehiculeId = v.IdVehicule,
                            Vehicule = v.MatriculeVehicule,
                            DepartementId = v.DepartementId,
                            Modele = getModele(v.IdModele),
                            C100KMModele = 0,
                            C1 = 0,
                            C2 = 0,
                            C3 = 0,
                            C4 = 0,
                            C5 = 0,
                            C6 = 0
                        };

                        if (v.DepartementId.HasValue && v.DepartementId > 0)
                        {
                            int d = v.DepartementId.Value;
                            var dep = context.VehiculeDepartements.FirstOrDefault(x => x.IdDepartement == d);
                            if (dep != null)
                            {
                                row.Departement = dep.NomDepartement;
                            }

                        }

                        if (v.ConducteurId.HasValue && v.ConducteurId > 0)
                        {
                            int c = v.ConducteurId.Value;
                            var cc = context.VehiculeConducteurs.FirstOrDefault(x => x.IdConducteur == c);
                            if (cc != null)
                            {
                                row.Conducteur = cc.PrenomConducteur + " " + cc.NomConducteur;
                            }
                        }

                        if (v.MiseCirculation.HasValue)
                        {
                            row.MiseCirculation = v.MiseCirculation.Value.ToShortDateString();
                        }


                        for (int i = -5; i <= 0; i++)
                        {
                            DateTime from = currentMonth.AddMonths(i);
                            DateTime to = from.AddMonths(1);
                            decimal C100 = 0;

                            var kilos = context.LectureKilometrages.Where(x => x.VehiculeId == v.IdVehicule && x.DateLectureKilometrage >= from && x.DateLectureKilometrage < to).ToList();
                            if (kilos.Count > 0)
                            {
                                decimal km = kilos.FirstOrDefault().Kilometrage;
                                if (kilos.Count > 1)
                                    km = kilos.Max(x => x.Kilometrage) - kilos.Min(x => x.Kilometrage);
                                if (km > 0)
                                {
                                    var carburant = context.Carburants.Where(x => x.VehiculeId == v.IdVehicule && x.DateCarburant >= from && x.DateCarburant < to).ToList();

                                    if (carburant.Count() > 0)
                                    {
                                        //-----------------------------------------------------------------------

                                        var carbs = carburant.Where(x => x.KilometrageFin > 0).ToList();

                                        if (carbs.Count > 0)
                                        {
                                            decimal kilosCarb = carbs.FirstOrDefault().KilometrageFin.GetValueOrDefault();
                                            if (carbs.Count > 1)
                                                kilosCarb = (decimal)(carbs.Max(x => x.KilometrageFin) - carbs.Min(x => x.KilometrageFin));
                                            var coutCarb = carbs.Sum(x => (decimal)x.CoutTotal) - carbs.OrderByDescending(x => x.DateCarburant).FirstOrDefault().CoutTotal;
                                            if (kilosCarb > 0 && coutCarb > 0)
                                            {
                                                C100 = (decimal)coutCarb / (kilosCarb / 100);
                                            }
                                        }

                                        //------------------------------------------------------------------------
                                        if (C100 == 0) //si ce n'est pas dejà calculé par l'estimation precedente
                                        {
                                            C100 = carburant.Sum(x => (decimal)x.CoutTotal) / (km / 100);
                                        }

                                    }
                                }
                            }

                            int j = i + 6;
                            row.GetType().GetProperty("C" + j).SetValue(row, C100, null);
                        }


                        list.Add(row);
                        //   }
                        //     catch (Exception ex)
                        //  {
                        //       var LineNumber = new StackTrace(ex, true).GetFrame(0).GetFileLineNumber();
                        //       Console.WriteLine("exception: "+ex.Message);
                        // throw;
                        //   }
                    }


                }


                return list;
            }
        }
    }
}
