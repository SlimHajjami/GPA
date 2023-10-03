using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;

namespace GPA.BLL
{
    public class AlerteService
    {
        public static int GetNbDocumentsAlertes(int type, int client, DepartementsAccessRightsModel departementsAccess)
        {
            int nbAlertes = 0;

            using (Entities context = new Entities())
            {
                var list = context.VehiculeDocuments.ToList();
                if (client > 0)
                {
                    var vehicules = context.Vehicules.Where(x => x.ClientId == client).Select(y => y.IdVehicule);
                    list = list.Where(x => vehicules.Contains(x.VehiculeId)).ToList();
                }
                if (departementsAccess != null)
                {
                    var d = departementsAccess.Visualisation;
                    var vehicules = context.Vehicules.Where(x => x.DepartementId != null && d.Contains((int)x.DepartementId)).Select(y => y.IdVehicule);
                    var vehiculesNoDep = context.Vehicules.Where(x => x.DepartementId == null).Select(y => y.IdVehicule);//cas: pas de departements
                    list = list.Where(x => vehicules.Contains(x.VehiculeId) || vehiculesNoDep.Contains(x.VehiculeId)).ToList();
                }

                
                if (type == 1)
                {
                    foreach (var row in list)
                    {
                        if(row.ExpirationAssurance.HasValue)
                        {
                            DateTime now = DateTime.Now;
                            if (row.RappelAssurance.HasValue)
                            {
                                now = now.AddDays(row.RappelAssurance.Value);
                            }
                            if (row.ExpirationAssurance.Value <= now)
                            {
                                nbAlertes++;
                            }
                        }
                    }
                }
                else if (type == 2)
                {
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
                                nbAlertes++;
                            }
                        }
                    }
                }
                else if (type == 3)
                {
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
                                nbAlertes++;
                            }
                        }
                    }
                }
                else
                {
                    nbAlertes = 0;
                }
            }

            return nbAlertes;
        }

        public static int GetNbEntretienAlertes(int client, DepartementsAccessRightsModel departementsAccess)
        {
            int nbAlertes = 0;

            using (Entities context = new Entities())
            {
                var list = context.EntretienProgrammes.ToList();
                if (client > 0)
                {
                    var vehicules = context.Vehicules.Where(x => x.ClientId == client).Select(y => y.IdVehicule);
                    list = list.Where(x => vehicules.Contains(x.VehiculeId)).ToList();
                }
                if (departementsAccess != null)
                {
                    var d = departementsAccess.Visualisation;
                    var vehicules = context.Vehicules.Where(x => x.DepartementId != null && d.Contains((int)x.DepartementId)).Select(y => y.IdVehicule);
                    list = list.Where(x => vehicules.Contains(x.VehiculeId)).ToList();
                }

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
                            nbAlertes++;
                        }
                    }
                }
            }

            return nbAlertes;
        }
    }
}
