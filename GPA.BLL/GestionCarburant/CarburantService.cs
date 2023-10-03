using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;
using Excel;
using System.Data;
using System.IO;
using System.Data.Entity;

namespace GPA.BLL
{
    public class CarburantService
    {
        public static List<CarburantModel> Read(int client, DepartementsAccessRightsModel departementsAccess, int departement, int annee, int mois, int vehicule)
        {
            using (Entities context = new Entities())
            {
                var list = context.Carburants.Select(e => new CarburantModel
                {
                    IdCarburant = e.IdCarburant,
                    ClientId = e.ClientId,
                    VehiculeId = e.VehiculeId,
                    Description = e.Description,
                    DateCarburant = e.DateCarburant,
                    QuantiteCarburant = e.QuantiteCarburant,
                    CoutUnitaireCarburant = e.CoutUnitaireCarburant,
                    FournisseurId = e.FournisseurId,
                    KilometrageDebut = e.KilometrageDebut,
                    OdometreActuel = context.LectureKilometrages.Where(x => x.ClientId == e.ClientId && x.VehiculeId == e.VehiculeId).Any() ? context.LectureKilometrages.Where(x => x.ClientId == e.ClientId && x.VehiculeId == e.VehiculeId).OrderByDescending(i => i.DateLectureKilometrage).FirstOrDefault().Kilometrage : 0,
                    KilometrageFin = e.KilometrageFin,
                    NoteCarburant = e.NoteCarburant,
                    CoutTotalCarburant = (decimal)e.CoutTotal
                }).ToList();
                if (client > 0)
                {
                    list = list.Where(x => x.ClientId == client).ToList();
                }
                if (departementsAccess != null)
                {
                    var d = departementsAccess.Visualisation;
                    var vehicules = context.Vehicules.Where(x => x.DepartementId != null && d.Contains((int)x.DepartementId)).Select(y => y.IdVehicule);
                    list = list.Where(x => vehicules.Contains(x.VehiculeId)).ToList();
                }
                if (departement > 0)
                {
                    var vehicules = context.Vehicules.Where(x => x.DepartementId == departement).Select(y => y.IdVehicule);
                    list = list.Where(x => vehicules.Contains(x.VehiculeId)).ToList();
                }
                if (vehicule > 0)
                {
                    list = list.Where(x => x.VehiculeId == vehicule).ToList();

                }
                if (annee > 0 && mois == 0)
                {
                    DateTime from = new DateTime(annee, 1, 1);
                    DateTime to = from.AddYears(1);

                    list = list.Where(x => x.DateCarburant >= from && x.DateCarburant < to).ToList();
                }
                else if (annee > 0 && mois > 0)
                {
                    DateTime from = new DateTime(annee, mois, 1);
                    DateTime to = from.AddMonths(1);

                    list = list.Where(x => x.DateCarburant >= from && x.DateCarburant < to).ToList();
                }
                return list;
            }
        }
        public static bool EntreeExiste(CarburantModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.Carburants.FirstOrDefault(e => e.VehiculeId == model.VehiculeId && DbFunctions.TruncateTime(e.DateCarburant)==model.DateCarburant.Date);
                if (entity != null)
                {
                   
                    objectAffected++;
                }
                
                return objectAffected > 0;
            }
           
        }
        public static CarburantModel Create(CarburantModel model, bool imported)
        {
            using (Entities context = new Entities())
            {
                var qtte = model.QuantiteCarburant;
                var coutTotal = model.CoutTotalCarburant;
                if (!(qtte > 0) && model.CoutUnitaireCarburant != 0)
                {
                    qtte = coutTotal / model.CoutUnitaireCarburant;
                }
                else if(!(coutTotal >0))
                {
                    coutTotal = qtte * model.CoutUnitaireCarburant;
                }
                else if (coutTotal != (qtte * model.CoutUnitaireCarburant) && !imported && model.CoutUnitaireCarburant != 0)
                {
                    qtte = coutTotal / model.CoutUnitaireCarburant;
                }
                var entity = new Carburant
                {
                    ClientId = model.ClientId,
                    VehiculeId = model.VehiculeId,
                    Description = model.Description,
                    DateCarburant = model.DateCarburant,
                    QuantiteCarburant = qtte,
                    CoutUnitaireCarburant = model.CoutUnitaireCarburant,
                    FournisseurId = model.FournisseurId,
                    KilometrageDebut = model.KilometrageDebut,
                    KilometrageFin = model.KilometrageFin,
                    NoteCarburant = model.NoteCarburant,
                    CoutTotal = coutTotal
                };
                context.Carburants.Add(entity);
                context.SaveChanges();
                model.IdCarburant = entity.IdCarburant;
                if (model.KilometrageFin.HasValue)
                {
                    if (model.KilometrageFin.Value > 0)
                    {
                        LectureKilometrageService.UpdateKilometrage(model.VehiculeId, model.DateCarburant, model.KilometrageFin.Value,"Lors de saisie carburant");
                    }
                }
                return model;
            }
        }

        public static bool Update(CarburantModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.Carburants.FirstOrDefault(e => e.IdCarburant == model.IdCarburant);
                
                if (entity != null)
                {
                    if (entity.DateCarburant == model.DateCarburant)
                    {
                        if (model.KilometrageFin.HasValue)
                        {
                            if (model.KilometrageFin.Value > 0)
                            {
                                LectureKilometrageService.UpdateKilometrage(model.VehiculeId, model.DateCarburant, model.KilometrageFin.Value, "Lors de saisie carburant");
                            }
                        }
                    }
                    else
                    {
                        if (model.KilometrageFin.HasValue)
                        {
                            if (model.KilometrageFin.Value > 0)
                            {
                                LectureKilometrageService.UpdateKilometrageDate(model.VehiculeId, entity.DateCarburant, model.DateCarburant, model.KilometrageFin.Value);
                            }
                        }
                    }
                    var qtte = model.QuantiteCarburant;
                    var coutTotal = model.CoutTotalCarburant;
                    if (!(qtte > 0))
                    {
                        qtte = coutTotal / model.CoutUnitaireCarburant;
                    }
                    else if (!(coutTotal > 0))
                    {
                        coutTotal = qtte * model.CoutUnitaireCarburant;
                    }
                    else if (coutTotal != (qtte * model.CoutUnitaireCarburant))
                    {
                        qtte = coutTotal / model.CoutUnitaireCarburant;
                    }
                    entity.ClientId = model.ClientId;
                    entity.VehiculeId = model.VehiculeId;
                    entity.Description = model.Description;
                    entity.DateCarburant = model.DateCarburant;
                    entity.QuantiteCarburant = qtte;
                    entity.CoutUnitaireCarburant = model.CoutUnitaireCarburant;
                    entity.FournisseurId = model.FournisseurId;
                    entity.KilometrageDebut = model.KilometrageDebut;
                    entity.KilometrageFin = model.KilometrageFin;
                    entity.NoteCarburant = model.NoteCarburant;
                    entity.CoutTotal = coutTotal;

                    objectAffected = context.SaveChanges();
                }
               
                return objectAffected > 0;
            }
        }
        public static LectureKilometrageModel GetKilomterageDebutByIdVehicule(int idVehicule, DateTime dateCarburant)
        {
            using (Entities context = new Entities())
            {
                var entity = context.Carburants.Select(e => new LectureKilometrageModel
                {
                    ClientId = e.ClientId,
                    VehiculeId = e.VehiculeId,
                    DateLectureKilometrage = e.DateCarburant,
                    Kilometrage = e.KilometrageFin.HasValue? (int)e.KilometrageFin : 0,
                })
                .Where(x => x.VehiculeId == idVehicule && x.DateLectureKilometrage < dateCarburant)
                .OrderByDescending(t => t.DateLectureKilometrage)
                .FirstOrDefault();
                return entity;
            }
        }
        public static bool Delete(CarburantModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.Carburants.FirstOrDefault(e => e.IdCarburant == model.IdCarburant);

                if (entry != null)
                {
                    context.Carburants.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }
        public static bool DeleteAll(int clientId)
        {
            using (Entities context = new Entities())
            {
                var list = context.Carburants.Where(e => e.ClientId == clientId).ToList();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        context.Carburants.Remove(item);
                    }
                }
               
                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }
        public static List<RapportMensuelCarburantModel> Report(int client, DepartementsAccessRightsModel departementsAccess, int departement, int annee, int mois)
        {
            using (Entities context = new Entities())
            {
                List<RapportMensuelCarburantModel> list = new List<RapportMensuelCarburantModel>();
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
                carburant = carburant.Where(x => x.DateCarburant >= from && x.DateCarburant < to).ToList();
                carburantPR = carburantPR.Where(x => x.DateCarburant >= fromPR && x.DateCarburant < from).ToList();


                foreach (var v in vehicules)
                {
                    RapportMensuelCarburantModel row = new RapportMensuelCarburantModel
                    {
                        ClientId = v.ClientId,
                        Client = context.Clients.FirstOrDefault(x => x.IdClient == v.ClientId).NomClient,
                        VehiculeId = v.IdVehicule,
                        Vehicule = v.MatriculeVehicule,
                        DepartementId = v.DepartementId,
                        Kilometrage = 0,
                        KilometragePR = 0,
                        Consommation = 0,
                        Cout = 0
                        // departement
                        
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
                        row.Cout = carbs.Sum(x => (decimal)x.CoutTotal);
                      //  row.Cout = carbs.Sum(x => (x.QuantiteCarburant * x.CoutUnitaireCarburant));
                      //  var coutUnitaire = context.CarburantCoutUnitaires.FirstOrDefault(x=> x.Code_refCarburant == v.TypeCarburantId).CoutUnitaireCarburant;
                       // var somme= row.Consommation * (decimal)coutUnitaire;
                    }

                    var kilos = context.LectureKilometrages.Where(x => x.VehiculeId == v.IdVehicule && x.DateLectureKilometrage >= from && x.DateLectureKilometrage < to);
                    if (kilos.Count() > 0)
                    {
                        row.Kilometrage = kilos.Max(x => x.Kilometrage) - kilos.Min(x => x.Kilometrage);
                    }

                    //if (row.Kilometrage > 0)
                    //{
                    //    row.C100KM = row.Consommation / (row.Kilometrage / 100);
                    //}


                    /****************PR*********************/
                    carbs = carburantPR.Where(x => x.VehiculeId == v.IdVehicule).ToList();
                    //if (carbs.Count() > 0)
                    //{
                    //    row.ConsommationPR = carbs.Sum(x => x.QuantiteCarburant);
                    //}

                    kilos = context.LectureKilometrages.Where(x => x.VehiculeId == v.IdVehicule && x.DateLectureKilometrage >= fromPR && x.DateLectureKilometrage < from);
                    if (kilos.Count() > 0)
                    {
                        row.KilometragePR = kilos.Max(x => x.Kilometrage) - kilos.Min(x => x.Kilometrage);
                    }

                    //if (row.KilometragePR > 0)
                    //{
                    //    row.C100KMPR = row.ConsommationPR / (row.KilometragePR / 100);
                    //}

                    list.Add(row);
                }


                return list;
            }
        }

        private static IExcelDataReader GetExcelReader(string filePath, string fileFormat)
        {
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            if (fileFormat == ".xls")
            {
                IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                return excelReader;
            }
            else if (fileFormat == ".xlsx")
            {
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                return excelReader;
            }
            return null;
        }

        public static int Import(string filePath, string fileFormat, CarburantColumns columns, int client)
        {
            int importedRows = 0;
            if (fileFormat == ".xls" || fileFormat == ".xlsx")
            {
                IExcelDataReader excelReader = GetExcelReader(filePath, fileFormat);
                if (excelReader != null)
                {
                    excelReader.IsFirstRowAsColumnNames = true;
                    DataSet result = excelReader.AsDataSet();
                    DataTable articles = result.Tables[0];

                    var fournisseurs = FournisseurService.Read(client).Where(x => x.TypeFournisseur == 6);
                    var vehicules = VehiculeService.Read(0, client, null);

                    foreach (DataRow row in articles.Rows)
                    {
                        if (!string.IsNullOrEmpty(columns.Matricule))
                        {
                            var v = vehicules.FirstOrDefault(x => x.MatriculeVehicule == row[columns.Matricule].ToString());
                            if (v != null)
                            {
                                int vehicule = v.IdVehicule;


                                DateTime dateCarburant = DateTime.Now;
                                if (!string.IsNullOrEmpty(columns.Date))
                                {
                                    DateTime d;
                                    var val = Double.Parse(row[columns.Date].ToString());
                                    var date = DateTime.FromOADate(val);
                                    dateCarburant = date;
                                    //if (DateTime.TryParse(row[columns.Date].ToString(), out d))
                                    //{
                                    //    dateCarburant = d;
                                    //}
                                }

                                decimal quantite = 0;
                                if (!string.IsNullOrEmpty(columns.Quantite))
                                {
                                    decimal.TryParse(row[columns.Quantite].ToString().Replace('.', ','), out quantite);
                                }

                                decimal cout = 0;
                                if (!string.IsNullOrEmpty(columns.Cout))
                                {
                                    decimal.TryParse(row[columns.Cout].ToString().Replace('.', ','), out cout);
                                }

                                decimal coutTotal = 0;
                                if (!string.IsNullOrEmpty(columns.CoutTotal))
                                {
                                    decimal.TryParse(row[columns.CoutTotal].ToString().Replace('.',','), out coutTotal);
                                }

                                string description = "";
                                if (!string.IsNullOrEmpty(columns.Description))
                                {
                                    description = row[columns.Description].ToString();
                                }

                                int fournisseur = 0;
                                if (!string.IsNullOrEmpty(columns.Fournisseur))
                                {
                                    var f = fournisseurs.FirstOrDefault(x => x.NomFournisseur == row[columns.Fournisseur].ToString());
                                    if (f != null)
                                    {
                                        fournisseur = f.IdFournisseur;
                                    }
                                }

                                int kiloDebut = 0;
                                if (!string.IsNullOrEmpty(columns.KilometrageDebut))
                                {
                                    int.TryParse(row[columns.KilometrageDebut].ToString(), out kiloDebut);
                                }

                                int kiloFin = 0;
                                if (!string.IsNullOrEmpty(columns.KilometrageFin))
                                {
                                    int.TryParse(row[columns.KilometrageFin].ToString(), out kiloFin);
                                }

                                string note = "";
                                if (!string.IsNullOrEmpty(columns.Note))
                                {
                                    note = row[columns.Note].ToString();
                                }


                                CarburantModel carburant = new CarburantModel
                                {
                                    ClientId = client,
                                    VehiculeId = vehicule,
                                    DateCarburant = dateCarburant,
                                    QuantiteCarburant = quantite,
                                    CoutUnitaireCarburant = cout,
                                    CoutTotalCarburant = coutTotal,
                                    Description = description,
                                    FournisseurId = fournisseur,
                                    KilometrageDebut = kiloDebut,
                                    KilometrageFin = kiloDebut,
                                    NoteCarburant = note
                                };
                                bool imported = carburant.QuantiteCarburant > 0 && carburant.CoutTotalCarburant > 0 && carburant.CoutUnitaireCarburant > 0;
                                var model = Create(carburant,imported);

                                importedRows++;
                            }
                        }
                    }

                    excelReader.Close();
                }
                File.Delete(filePath);
            }
            return importedRows;
        }
    }
}
