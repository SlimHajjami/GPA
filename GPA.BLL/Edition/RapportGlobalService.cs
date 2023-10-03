using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;
using System.Globalization;

namespace GPA.BLL
{
    public class RapportGlobalService
    {
        public static List<RapportGlobalModel> Read(int client, DepartementsAccessRightsModel departementsAccess, int annee, int mois)
        {
            using (Entities context = new Entities())
            {
                DateTime currentMonth = new DateTime(annee, mois, 1);
                List<RapportGlobalModel> list = new List<RapportGlobalModel>();

                var departements = context.VehiculeDepartements.ToList();

                if (departementsAccess != null)
                {
                    var d = departementsAccess.Visualisation;
                    departements = departements.Where(x => /*x.IdDepartement != null &&*/ d.Contains(x.IdDepartement)).ToList();
                }
                if (client > 0)
                {
                    departements = departements.Where(x => x.ClientId == client).ToList();
                }
                if (departements.Count() > 0)
                {               
                            foreach (var dep in departements)
                            {
                                RapportGlobalModel row = new RapportGlobalModel
                                {
                                    ClientId = dep.ClientId,
                                    Client = context.Clients.FirstOrDefault(x => x.IdClient == dep.ClientId).NomClient,
                                    DepartementId = dep.IdDepartement,
                                    Departement = dep.NomDepartement,
                                    NbVehicules = context.Vehicules.Where(x => x.DepartementId == dep.IdDepartement).Count(),
                                    E1 = 0,
                                    E2 = 0,
                                    E3 = 0,
                                    E4 = 0,
                                    R1 = 0,
                                    R2 = 0,
                                    R3 = 0,
                                    R4 = 0,
                                    C1 = 0,
                                    C2 = 0,
                                    C3 = 0,
                                    C4 = 0
                                };

                                    if (row.NbVehicules > 0)
                                    {
                                        var vehicules = context.Vehicules.Where(x => x.DepartementId == dep.IdDepartement).ToList();
                                        var vehsIds = vehicules.Select(y => y.IdVehicule);

                                        for (int i = -3; i <= 0; i++)
                                        {
                                            int j = i + 4;
                                            DateTime from = currentMonth.AddMonths(i);
                                            DateTime to = from.AddMonths(1);

                                            var entretiens = context.Entretiens.Where(x => vehsIds.Contains(x.VehiculeId) && x.DateEntretien >= from && x.DateEntretien < to).ToList();
                                            if (entretiens.Count() > 0)
                                            {
                                                row.GetType().GetProperty("E" + j).SetValue(row, Math.Round(entretiens.Sum(x => x.CoutEntretien), 2), null);
                                            }

                                            var reparations = context.Reparations.Where(x => vehsIds.Contains(x.VehiculeId) && x.DateReparation >= from && x.DateReparation < to).ToList();
                                            if (reparations.Count() > 0)
                                            {
                                                row.GetType().GetProperty("R" + j).SetValue(row, Math.Round(reparations.Sum(x => x.CoutReparation), 2), null);
                                            }

                                            var carburant = context.Carburants.Where(x => vehsIds.Contains(x.VehiculeId) && x.DateCarburant >= from && x.DateCarburant < to).ToList();
                                            if (carburant.Count() > 0)
                                            {
                                                row.GetType().GetProperty("C" + j).SetValue(row, Math.Round(carburant.Sum(x => (decimal)x.CoutTotal), 2), null);
                                            }
                                        }
                                        list.Add(row);
                                    }
                                }
                 }
                var vehiculesSansDep = context.Vehicules.Where(x=> x.ClientId == client && x.DepartementId == null).ToList();
                if ( client > 0 && vehiculesSansDep.Count() > 0)
                {
                    RapportGlobalModel row = new RapportGlobalModel
                    {
                        ClientId = client,
                        Client = context.Clients.FirstOrDefault(x => x.IdClient == client).NomClient,
                        Departement = context.Clients.FirstOrDefault(x => x.IdClient == client).NomClient,
                        NbVehicules = context.Vehicules.Where(x => x.ClientId == client && x.DepartementId == null).Count(),
                        E1 = 0,
                        E2 = 0,
                        E3 = 0,
                        E4 = 0,
                        R1 = 0,
                        R2 = 0,
                        R3 = 0,
                        R4 = 0,
                        C1 = 0,
                        C2 = 0,
                        C3 = 0,
                        C4 = 0
                    };
                    if (row.NbVehicules > 0)
                    {
                     //   var vehicules = context.Vehicules.Where(x => x.ClientId == client).ToList();
                        var vehsIds = vehiculesSansDep.Select(y => y.IdVehicule);

                        for (int i = -3; i <= 0; i++)
                        {
                            int j = i + 4;
                            DateTime from = currentMonth.AddMonths(i);
                            DateTime to = from.AddMonths(1);

                            var entretiens = context.Entretiens.Where(x => vehsIds.Contains(x.VehiculeId) && x.DateEntretien >= from && x.DateEntretien < to).ToList();
                            if (entretiens.Count() > 0)
                            {
                                row.GetType().GetProperty("E" + j).SetValue(row, Math.Round(entretiens.Sum(x => x.CoutEntretien), 2), null);
                            }

                            var reparations = context.Reparations.Where(x => vehsIds.Contains(x.VehiculeId) && x.DateReparation >= from && x.DateReparation < to).ToList();
                            if (reparations.Count() > 0)
                            {
                                row.GetType().GetProperty("R" + j).SetValue(row, Math.Round(reparations.Sum(x => x.CoutReparation), 2), null);
                            }

                            var carburant = context.Carburants.Where(x => vehsIds.Contains(x.VehiculeId) && x.DateCarburant >= from && x.DateCarburant < to).ToList();
                            if (carburant.Count() > 0)
                            {
                                row.GetType().GetProperty("C" + j).SetValue(row, Math.Round(carburant.Sum(x => (decimal)x.CoutTotal), 2), null);
                            }
                        }
                        list.Add(row);
                    }


                }

                return list;
            }
        }

        public static List<BarChartModel> getBarChart(int client, DepartementsAccessRightsModel departementsAccess, int year, int month)
        {
            using (Entities context = new Entities())
            {
                DateTime currentMonth = new DateTime(year, month, 1);
                List<BarChartModel> result = new List<BarChartModel>();

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

                var vehsIds = vehicules.Select(y => y.IdVehicule);

                int j = 1;
                for (int i = -3; i <= 0; i++)
                {
                    DateTime from = currentMonth.AddMonths(i);
                    DateTime to = from.AddMonths(1);

                    #region Carburant

                    decimal cout = 0;
                    var carburant = context.Carburants.Where(x => vehsIds.Contains(x.VehiculeId) && x.DateCarburant >= from && x.DateCarburant < to).ToList();
                    if (carburant.Count() > 0)
                    {
                        cout = carburant.Sum(x => x.CoutUnitaireCarburant * x.QuantiteCarburant);
                    }

                    string m = from.ToString("MMMM", CultureInfo.CreateSpecificCulture("fr"));
                    result.Add(new BarChartModel
                    {
                        Mois = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(m),
                        MoisRank = j,
                        TypeCharge = "Carburant",
                        Charges = cout
                    });

                    #endregion

                    #region Entretien

                    cout = 0;
                    var entretiens = context.Entretiens.Where(x => vehsIds.Contains(x.VehiculeId) && x.DateEntretien >= from && x.DateEntretien < to).ToList();
                    if (entretiens.Count() > 0)
                    {
                        cout = entretiens.Sum(x => x.CoutEntretien);
                    }

                    m = from.ToString("MMMM", CultureInfo.CreateSpecificCulture("fr"));
                    result.Add(new BarChartModel
                    {
                        Mois = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(m),
                        MoisRank = j,
                        TypeCharge = "Entretien",
                        Charges = cout
                    });

                    #endregion

                    #region Réparations

                    cout = 0;
                    var reparations = context.Reparations.Where(x => vehsIds.Contains(x.VehiculeId) && x.DateReparation >= from && x.DateReparation < to).ToList();
                    if (reparations.Count() > 0)
                    {
                        cout = reparations.Sum(x => x.CoutReparation);
                    }

                    m = from.ToString("MMMM", CultureInfo.CreateSpecificCulture("fr"));
                    result.Add(new BarChartModel
                    {
                        Mois = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(m),
                        MoisRank = j,
                        TypeCharge = "Réparation",
                        Charges = cout
                    });

                    #endregion

                    j++;
                }

                return result;
            }
        }

        public static List<PieChartModel> getPieChart(int client, DepartementsAccessRightsModel departementsAccess, int year, int month)
        {
            using (Entities context = new Entities())
            {
                DateTime currentMonth = new DateTime(year, month, 1);
                List<PieChartModel> result = new List<PieChartModel>();

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

                var vehsIds = vehicules.Select(y => y.IdVehicule);
                var carburant = context.Carburants.Where(x => vehsIds.Contains(x.VehiculeId)).ToList();
                var entretiens = context.Entretiens.Where(x => vehsIds.Contains(x.VehiculeId)).ToList();
                var reparations = context.Reparations.Where(x => vehsIds.Contains(x.VehiculeId)).ToList();

                #region Carburant

                DateTime from = currentMonth.AddMonths(-3);
                DateTime to = currentMonth.AddMonths(1);

                decimal cout = 0;
                carburant = carburant.Where(x => x.DateCarburant >= from && x.DateCarburant < to).ToList();
                if (carburant.Count() > 0)
                {
                    cout = carburant.Sum(x => (decimal)x.CoutTotal);
                }

                result.Add(new PieChartModel
                {
                    TypeCharge = "Carburant",
                    Charges = cout
                });

                #endregion

                #region Entretien

                from = currentMonth.AddMonths(-3);
                to = currentMonth.AddMonths(1);

                cout = 0;
                entretiens = entretiens.Where(x => x.DateEntretien >= from && x.DateEntretien < to).ToList();
                if (entretiens.Count() > 0)
                {
                    cout = entretiens.Sum(x => x.CoutEntretien);
                }

                result.Add(new PieChartModel
                {
                    TypeCharge = "Entretien",
                    Charges = cout
                });

                #endregion

                #region Reparation

                from = currentMonth.AddMonths(-3);
                to = currentMonth.AddMonths(1);

                cout = 0;
                reparations = reparations.Where(x => x.DateReparation >= from && x.DateReparation < to).ToList();
                if (reparations.Count() > 0)
                {
                    cout = reparations.Sum(x => x.CoutReparation);
                }
                result.Add(new PieChartModel
                {
                    TypeCharge = "Réparation",
                    Charges = cout
                });

                #endregion

                decimal total = result.Sum(x => x.Charges);
                if (total != 0)
                {
                    foreach (var r in result)
                    {
                        r.Charges = Math.Round((r.Charges / total) * 100, 2);
                    }
                }
               

                return result;
            }
        }
    }

    public class BarChartModel
    {
        public string Mois { get; set; }
        public string TypeCharge { get; set; }
        public int MoisRank { get; set; }
        public decimal Charges { get; set; }
    }

    public class PieChartModel
    {
        public string TypeCharge { get; set; }
        public decimal Charges { get; set; }
    }
}
