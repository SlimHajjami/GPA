using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;
using System.IO;

namespace GPA.BLL
{
    public class EntretienService
    {
        public static List<EntretienModel> Read(int client, DepartementsAccessRightsModel departementsAccess, int departement, int annee, int mois)
        {
            using (Entities context = new Entities())
            {
                var list = context.Entretiens.Select(e => new EntretienModel
                {
                    IdEntretien = e.IdEntretien,
                    ClientId = e.ClientId,
                    VehiculeId = e.VehiculeId,
                    DateEntretien = e.DateEntretien,
                    FournisseurId = e.FournisseurId,
                    DescriptionEntretien = e.DescriptionEntretien,
                    CoutEntretien = e.CoutEntretien,
                    KilometrageEntretien = e.KilometrageEntretien,
                    NoteEntretien = e.NoteEntretien
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
                if (annee > 0 && mois > 0)
                {
                    DateTime from = new DateTime(annee, mois, 1);
                    DateTime to = from.AddMonths(1);

                    list = list.Where(x => x.DateEntretien >= from && x.DateEntretien < to).ToList();
                }
                return list;
            }
        }

        public static EntretienModel Create(EntretienModel model)
        {
            using (Entities context = new Entities())
            {
                var entity = new Entretien
                {
                    ClientId = model.ClientId,
                    VehiculeId = model.VehiculeId,
                    DateEntretien = model.DateEntretien,
                    FournisseurId = model.FournisseurId,
                    DescriptionEntretien = model.DescriptionEntretien,
                    CoutEntretien = model.CoutEntretien,
                    KilometrageEntretien = model.KilometrageEntretien,
                    NoteEntretien = model.NoteEntretien
                };
                context.Entretiens.Add(entity);
                context.SaveChanges();
                model.IdEntretien = entity.IdEntretien;
                if (model.KilometrageEntretien.HasValue)
                {
                    if (model.KilometrageEntretien > 0)
                    {
                        LectureKilometrageService.UpdateKilometrage(model.VehiculeId, model.DateEntretien, model.KilometrageEntretien.Value, "Lors de saisie Entretien");
                    }
                }
                return model;
            }
        }

        public static bool Update(EntretienModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.Entretiens.FirstOrDefault(e => e.IdEntretien == model.IdEntretien);
                if (entity != null)
                {
                    if (entity.DateEntretien == model.DateEntretien)
                    {
                        if (model.KilometrageEntretien.HasValue)
                        {
                            if (model.KilometrageEntretien.Value > 0)
                            {
                                LectureKilometrageService.UpdateKilometrage(model.VehiculeId, model.DateEntretien, model.KilometrageEntretien.Value,"Lors de saisie Entretien");
                            }
                        }
                    }
                    else
                    {
                        if (model.KilometrageEntretien.HasValue)
                        {
                            if (model.KilometrageEntretien.Value > 0)
                            {
                                LectureKilometrageService.UpdateKilometrageDate(model.VehiculeId, entity.DateEntretien, model.DateEntretien, model.KilometrageEntretien.Value);
                            }
                        }
                    }
                    entity.ClientId = model.ClientId;
                    entity.VehiculeId = model.VehiculeId;
                    entity.DateEntretien = model.DateEntretien;
                    entity.FournisseurId = model.FournisseurId;
                    entity.DescriptionEntretien = model.DescriptionEntretien;
                    entity.CoutEntretien = model.CoutEntretien;
                    entity.KilometrageEntretien = model.KilometrageEntretien;
                    entity.NoteEntretien = model.NoteEntretien;

                    objectAffected = context.SaveChanges();
                }
                //if (model.KilometrageEntretien.HasValue)
                //{
                //    if (model.KilometrageEntretien > 0)
                //    {
                //        LectureKilometrageService.UpdateKilometrage(model.VehiculeId, model.DateEntretien, model.KilometrageEntretien.Value);
                //    }
                //}
                return objectAffected > 0;
            }
        }

        public static bool UpdateCout(int id)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;
                decimal d = 0;

                var depenses = context.Depenses.Where(x => x.EntretienId == id);
                if (depenses.Count() > 0)
                {
                    d = depenses.Sum(y => y.QuantiteDepense * y.CoutUnitaireDepense);
                }
                var entity = context.Entretiens.FirstOrDefault(e => e.IdEntretien == id);
                if (entity != null)
                {
                    entity.CoutEntretien = d;
                    objectAffected = context.SaveChanges();
                }

                return objectAffected > 0;
            }
        }

        public static bool Delete(EntretienModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.Entretiens.FirstOrDefault(e => e.IdEntretien == model.IdEntretien);

                if (entry != null)
                {
                    context.Entretiens.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }

        public static int Import(string filePath, string fileFormat, int client)
        {
            int importedRows = 0;
            if (fileFormat == ".csv")
            {
                using (Entities context = new Entities())
                {
                    var reader = new StreamReader(File.OpenRead(filePath));

                    List<VEH> vehicules = context.Vehicules.Where(x => x.ClientId == client)
                        .Select(e => new VEH
                        {
                            id = e.IdVehicule,
                            matricule = e.MatriculeVehicule
                        }).ToList();
                    List<ENT> entretiens = new List<ENT>();
                    List<DPN> depenses = new List<DPN>();

                 //   string description = "";     18/4/2018, supposé regrouper les dépenses d'une seul entretien/réparation, mais pour un cas de fichier réel parenin ca fonctionne pas
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');
                        if (values.Length > 0)
                        {
                            var v = vehicules.Find(x => x.matricule.ToUpper() == values[0].ToUpper());
                            if (v != null)
                            {
                            //    if (values[3] != description)
                              //  {
                                    string kilo = "0";
                                    if (values.Length > 8)
                                    {
                                        kilo = values[8];
                                    }
                                    entretiens.Add(new ENT
                                    {
                                        type = values[1],
                                        vehicule = v.id,
                                        date = values[2],
                                        description = values[3],
                                        kilometrage = kilo,
                                        note = values[4]
                                    });
                               // }

                                depenses.Add(new DPN
                                {
                                    entretien = values[3] + v.id,
                                    type = values[5],
                                    cout = values[6],
                                    quantite = values[7]
                                });

                              //  description = values[3];
                            }
                            
                        }
                    }

                    reader.Close();
                    importedRows = InsertCSV(entretiens, depenses, client);
                }

                File.Delete(filePath);
            }
            return importedRows;
        }

        private static int InsertCSV(List<ENT> entretiens, List<DPN> depenses, int client)
        {
            int addedENT = 0;
            foreach (ENT e in entretiens)
            {
                var d = e.date.Split('/');
                if (d.Length == 3)
                {
                    int year = 0;
                    int month = 0;
                    int day = 0;
                    if (int.TryParse(d[2], out year) && int.TryParse(d[1], out month) && int.TryParse(d[0], out day))
                    {
                        DateTime date = new DateTime(year, month, day, 0, 0, 0);
                        int kilo = 0;
                        int.TryParse(e.kilometrage, out kilo);

                        int eID = 0;
                        int rID = 0;
                        if (e.type == "TRUE")
                        {
                            EntretienModel entretien = new EntretienModel 
                            { 
                                VehiculeId = e.vehicule,
                                ClientId = client,
                                DateEntretien = date,
                                CoutEntretien = 0,
                                DescriptionEntretien = e.description,
                                KilometrageEntretien = kilo,
                                NoteEntretien = e.note
                            };

                            var result = Create(entretien);
                            eID = result.IdEntretien;
                            if (eID > 0)
                            {
                                addedENT++;
                            }
                        }
                        else if (e.type == "FALSE")
                        {
                            ReparationModel reparation = new ReparationModel
                            {
                                VehiculeId = e.vehicule,
                                ClientId = client,
                                DateReparation = date,
                                DateMiseService = date,
                                HeuresArret = 0,
                                CoutReparation = 0,
                                DescriptionReparation = e.description,
                                KilometrageReparation = kilo,
                                NoteReparation = e.note
                            };

                            var result = ReparationService.Create(reparation);
                            rID = result.IdReparation;
                            if (rID > 0)
                            {
                                addedENT++;
                            }
                        }

                        if (eID > 0 || rID > 0)
                        {
                            string et = e.description + e.vehicule;
                            var entretienDepenses = depenses.Where(x => x.entretien == et);
                            foreach (DPN dp in entretienDepenses)
                            {
                                decimal cout = 0;
                                decimal quantite = 0;
                                int type = 0;
                                decimal.TryParse(dp.cout, out cout);
                                decimal.TryParse(dp.quantite, out quantite);
                                int.TryParse(dp.type, out type);

                                DepenseModel depense = new DepenseModel 
                                {
                                    ReparationId = rID,
                                    EntretienId = eID,
                                    QuantiteDepense = quantite,
                                    CoutUnitaireDepense = cout,
                                    TypeId = type
                                };
                                DepenseService.Create(depense);
                            }
                        }

                        if (eID > 0)
                        {
                            UpdateCout(eID);
                        }
                        else if (rID > 0)
                        {
                            ReparationService.UpdateCout(rID);
                        }
                    }
                }
            }

            return addedENT;
        }

        public static bool DeleteAll(int clientId, int year, int month)
        {
            using (Entities context = new Entities())
            {
                var list = context.Entretiens.Where(e => e.ClientId == clientId).ToList();
                if (year > 0 && month > 0)
                {
                    list = list.Where(e => e.DateEntretien.Year == year && e.DateEntretien.Month == month).ToList();
                }
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var listDep = context.Depenses.Where(e => e.EntretienId == item.IdEntretien);
                        if (listDep != null)
                        {
                            foreach (var dep in listDep)
                            {
                                context.Depenses.Remove(dep);
                            }
                        }
                      
                        context.Entretiens.Remove(item);
                    }
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }
    }
}
