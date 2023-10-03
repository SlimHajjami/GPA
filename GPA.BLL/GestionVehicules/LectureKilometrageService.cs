using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;
using System.Data.Entity;
using System.IO;
using System.Globalization;

namespace GPA.BLL
{
    public class LectureKilometrageService
    {
        #region Kilometrage journalier
        public static List<LectureKilometrageModel> Read(int dep, int client, int annee,int mois, int vehicule, DepartementsAccessRightsModel departementsAccess)
        {
            using (Entities context = new Entities())
            {
                var list = context.LectureKilometrages.Select(e => new LectureKilometrageModel
                {
                    IdLectureKilometrage = e.IdLectureKilometrage,
                    ClientId = e.ClientId,
                    VehiculeId = e.VehiculeId,
                    DateLectureKilometrage = e.DateLectureKilometrage,
                    Kilometrage = e.Kilometrage,
                    Note = e.Note
                }).ToList();
                
                if (client > 0)
                {
                    list = list.Where(x => x.ClientId == client).ToList();
                }
                foreach (var item in list)
                {
                    var kmPR = list.Where(x => x.VehiculeId == item.VehiculeId && x.DateLectureKilometrage < item.DateLectureKilometrage).OrderByDescending(y => y.DateLectureKilometrage).FirstOrDefault();
                    if (kmPR != null)
                    {
                        item.KilometragePR = kmPR.Kilometrage;

                    }
                }
                if (dep > 0 )
                {
                    var vehsIds = context.Vehicules.Where(x => x.ClientId == client && x.DepartementId == dep).Select(y => y.IdVehicule);
                    list = list.Where(x => vehsIds.Contains(x.VehiculeId)).ToList();
                }
                if (annee > 0)
                {
                    list = list.Where(x => x.DateLectureKilometrage.Year == annee).ToList();
                }
                if (mois > 0)
                {
                    list = list.Where(x => x.DateLectureKilometrage.Month == mois).ToList();
                }
                if (vehicule > 0)
                {
                    list = list.Where(x => x.VehiculeId == vehicule).ToList();
                }
                //if (departementsAccess != null)
                //{
                //    var d = departementsAccess.Visualisation;
                //    var vehsIds = context.Vehicules.Where(x => x.DepartementId != null && d.Contains((int)x.DepartementId)).Select(y => y.IdVehicule);
                //    list = list.Where(x => vehsIds.Contains(x.VehiculeId)).ToList();
                //}
                return list;
            }
        }

        public static LectureKilometrageModel GetLastKilometrage(int vehicule)
        {
            using (Entities context = new Entities())
            {
                var entity = context.LectureKilometrages.Select(e => new LectureKilometrageModel
                {
                    IdLectureKilometrage = e.IdLectureKilometrage,
                    ClientId = e.ClientId,
                    VehiculeId = e.VehiculeId,
                    DateLectureKilometrage = e.DateLectureKilometrage,
                    Kilometrage = e.Kilometrage
                })
                .Where(x => x.VehiculeId == vehicule)
                .OrderByDescending(t => t.DateLectureKilometrage)
                .FirstOrDefault();
                return entity;
            }
        }
        //public static LectureKilometrageModel GetKilomterageDebutByIdVehicule(int idVehicule, DateTime dateCarburant)
        //{
        //    using (Entities context = new Entities())
        //    {
        //        var entity = context.Carburants.Select(e => new LectureKilometrageModel
        //        {
        //            ClientId = e.ClientId,
        //            VehiculeId = e.VehiculeId,
        //            DateLectureKilometrage = e.DateCarburant,
        //            Kilometrage = (int)e.KilometrageFin,
        //        })
        //        .Where(x => x.VehiculeId == idVehicule && x.DateLectureKilometrage < dateCarburant)
        //        .OrderByDescending(t => t.DateLectureKilometrage)
        //        .FirstOrDefault();
        //        return entity;
        //    }
        //}

        public static LectureKilometrageModel GetKilomteragePrecedentByIdVehicule(int idVehicule, DateTime date)
        {
            using (Entities context = new Entities())
            {
                var entity = context.LectureKilometrages.Select(e => new LectureKilometrageModel
                {
                    ClientId = e.ClientId,
                    VehiculeId = e.VehiculeId,
                    DateLectureKilometrage = e.DateLectureKilometrage,
                    Kilometrage = (int)e.Kilometrage,
                })
                .Where(x => x.VehiculeId == idVehicule && DbFunctions.TruncateTime(x.DateLectureKilometrage) <= date.Date)
                .OrderByDescending(t => t.DateLectureKilometrage)
                .FirstOrDefault();
                return entity;
            }
        }
        public static LectureKilometrageModel Create(LectureKilometrageModel model)
        {
            using (Entities context = new Entities())
            {
                var entity = new LectureKilometrage
                {
                    ClientId = model.ClientId,
                    VehiculeId = model.VehiculeId,
                    DateLectureKilometrage = model.DateLectureKilometrage,
                    Kilometrage = model.Kilometrage,
                    Note =model.Note  //"Lors d'une mise à jour manuelle"
                };
                context.LectureKilometrages.Add(entity);

                /**
                 *Enrgistrer la date de la derniere lecture odometre pour l'utiliser par le service
                 **/
           //     var updateEntity = context.GISLastUpdates.FirstOrDefault();
             //   if (updateEntity != null)
            //    {
              //      if (updateEntity.DateUpdate < model.DateLectureKilometrage)
              //      {
              //          updateEntity.DateUpdate = model.DateLectureKilometrage;
            //        }

           //     }

                context.SaveChanges();
                model.IdLectureKilometrage = entity.IdLectureKilometrage;
                model.Note = entity.Note;
                return model;
            }
        }

        public static bool Update(LectureKilometrageModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.LectureKilometrages.FirstOrDefault(e => e.IdLectureKilometrage == model.IdLectureKilometrage);
                if (entity != null)
                {
                    entity.ClientId = model.ClientId;
                    entity.VehiculeId = model.VehiculeId;
                    entity.DateLectureKilometrage = model.DateLectureKilometrage;
                    entity.Kilometrage = model.Kilometrage;
                    entity.Note = model.Note;

                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }

        public static void UpdateKilometrage(int vehicule, DateTime date, int kilometrage, string note)
        {
            using (Entities context = new Entities())
            {
                var vh = context.Vehicules.FirstOrDefault(x => x.IdVehicule == vehicule);
                var model = new LectureKilometrageModel
                {
                    VehiculeId = vehicule,
                    ClientId = vh.ClientId,
                    DateLectureKilometrage = date,
                    Kilometrage = kilometrage,
                    Note = note
                };

                var kilometrages = context.LectureKilometrages.Where(e => e.VehiculeId == vehicule);

                if (kilometrages.Count() == 0)
                {
                    Create(model);
                }
                else
                {
                    var k = kilometrages.FirstOrDefault(x => DbFunctions.TruncateTime(x.DateLectureKilometrage) == date.Date);
                    if (k != null)
                    {
                        model.IdLectureKilometrage = k.IdLectureKilometrage;
                        Update(model);
                    }
                    else
                    {
                        Create(model);
                    }
                }
            }
        }
        public static void UpdateKilometrageDate(int vehicule, DateTime oldDate, DateTime date, int kilometrage)
        {
            using (Entities context = new Entities())
            {
                var vh = context.Vehicules.FirstOrDefault(x => x.IdVehicule == vehicule);
                var model = new LectureKilometrageModel
                {
                    VehiculeId = vehicule,
                    ClientId = vh.ClientId,
                    DateLectureKilometrage = date,
                    Kilometrage = kilometrage
                };

                var kilometrages = context.LectureKilometrages.Where(e => e.VehiculeId == vehicule);

                if (kilometrages.Count() == 0)
                {
                    Create(model);
                }
                else
                {
                    var k = kilometrages.FirstOrDefault(x => DbFunctions.TruncateTime(x.DateLectureKilometrage) == oldDate.Date);
                    if (k != null)
                    {
                        model.IdLectureKilometrage = k.IdLectureKilometrage;
                        Update(model);
                    }
                    else
                    {
                        Create(model);
                    }
                }
            }

        }

        public static bool Delete(LectureKilometrageModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.LectureKilometrages.FirstOrDefault(e => e.IdLectureKilometrage == model.IdLectureKilometrage);

                if (entry != null)
                {
                    context.LectureKilometrages.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }
        #endregion
        
        #region Kilometrage mensuel

        public static List<KilometrageMensuelModel> ReadMensuel(int dep, int client, int annee, int mois, int vehicule, DepartementsAccessRightsModel departementsAccess)
        {
            using (Entities context = new Entities())
            {
                var list = context.KilometrageMensuels.Select(e => new KilometrageMensuelModel
                {
                    IdKilometrageMensuel = e.IdKilometrageMensuel,
                    ClientId = e.ClientId,
                    VehiculeId = e.VehiculeId,
                    DateKilometrageMensuel = e.Date,
                    Kilometrage = e.Kilometrage,
                    Note = e.Note
                }).ToList();

                if (client > 0)
                {
                    list = list.Where(x => x.ClientId == client).ToList();
                }
                //foreach (var item in list)
                //{
                //    var kmPR = list.Where(x => x.VehiculeId == item.VehiculeId && x.DateKilometrageMensuel < item.DateKilometrageMensuel).OrderByDescending(y => y.DateLectureKilometrage).FirstOrDefault();
                //    if (kmPR != null)
                //    {
                //        item.KilometragePR = kmPR.Kilometrage;

                //    }
                //}
                if (dep > 0)
                {
                    var vehsIds = context.Vehicules.Where(x => x.ClientId == client && x.DepartementId == dep).Select(y => y.IdVehicule);
                    list = list.Where(x => vehsIds.Contains(x.VehiculeId)).ToList();
                }
                if (annee > 0)
                {
                    list = list.Where(x => x.DateKilometrageMensuel.Year == annee).ToList();
                }
                if (mois > 0)
                {
                    list = list.Where(x => x.DateKilometrageMensuel.Month == mois).ToList();
                }
                if (vehicule > 0)
                {
                    list = list.Where(x => x.VehiculeId == vehicule).ToList();
                }
                //if (departementsAccess != null)
                //{
                //    var d = departementsAccess.Visualisation;
                //    var vehsIds = context.Vehicules.Where(x => x.DepartementId != null && d.Contains((int)x.DepartementId)).Select(y => y.IdVehicule);
                //    list = list.Where(x => vehsIds.Contains(x.VehiculeId)).ToList();
                //}
                return list;
            }
        }

        public static KilometrageMensuelModel CreateMensuel(KilometrageMensuelModel model)
        {
            using (Entities context = new Entities())
            {
                var entity = new KilometrageMensuel
                {
                    ClientId = model.ClientId,
                    VehiculeId = model.VehiculeId,
                    Date = model.DateKilometrageMensuel,
                    Kilometrage = model.Kilometrage,
                    Note = model.Note  //"Lors d'une mise à jour manuelle"
                };
                context.KilometrageMensuels.Add(entity);

                /**
                 *Enrgistrer la date de la derniere lecture odometre pour l'utiliser par le service
                 **/
                //     var updateEntity = context.GISLastUpdates.FirstOrDefault();
                //   if (updateEntity != null)
                //    {
                //      if (updateEntity.DateUpdate < model.DateLectureKilometrage)
                //      {
                //          updateEntity.DateUpdate = model.DateLectureKilometrage;
                //        }

                //     }

                context.SaveChanges();
                model.IdKilometrageMensuel = entity.IdKilometrageMensuel;
                //model.Note = entity.Note;
                return model;
            }
        }

        public static bool UpdateMensuel(KilometrageMensuelModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.KilometrageMensuels.FirstOrDefault(e => e.IdKilometrageMensuel == model.IdKilometrageMensuel);
                if (entity != null)
                {
                    entity.ClientId = model.ClientId;
                    entity.VehiculeId = model.VehiculeId;
                    entity.Date = model.DateKilometrageMensuel;
                    entity.Kilometrage = model.Kilometrage;
                    entity.Note = model.Note;

                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }
        public static bool DeleteMensuel(KilometrageMensuelModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.KilometrageMensuels.FirstOrDefault(e => e.IdKilometrageMensuel == model.IdKilometrageMensuel);

                if (entry != null)
                {
                    context.KilometrageMensuels.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }

        public static bool DeleteAllMensuel(int clientId)
        {
            using (Entities context = new Entities())
            {
                var list = context.KilometrageMensuels.Where(e => e.ClientId == clientId).ToList();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        context.KilometrageMensuels.Remove(item);
                    }
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

                    List<KmMensuel> kilometrageMensuel = new List<KmMensuel>();

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');
                        if (values.Length > 0)
                        {
                            var v = vehicules.Find(x => x.matricule == values[3]);
                            if (v != null)
                            {
                                kilometrageMensuel.Add(new KmMensuel
                                {
                                    vehicule = v.id,
                                    month = values[1],
                                    year = values[0],
                                    kilometrage = values[5]
                                });
                               
                            }
                        }
                    }

                    reader.Close();
                    importedRows = InsertCSV(kilometrageMensuel, client);
                }

                File.Delete(filePath);
            }
            return importedRows;
        }

        private static int InsertCSV(List<KmMensuel> kilometrageMensuel, int client)
        {
            int addedKm = 0;
            foreach (KmMensuel k in kilometrageMensuel)
            {
                int year = 0;
                int month = 0;
                if (int.TryParse(k.month, out month) && int.TryParse(k.year, out year))
                {
                    DateTime date = new DateTime(year, month, 1, 0, 0, 0);
                    decimal kilo = 0;
                    var style = NumberStyles.AllowDecimalPoint;
                    var culture = CultureInfo.CreateSpecificCulture("en-US");
                    
                    var ok = Decimal.TryParse(k.kilometrage, style, culture, out kilo);

                    KilometrageMensuelModel kilometrage = new KilometrageMensuelModel
                    {
                        ClientId = client,
                        VehiculeId = k.vehicule,
                        DateKilometrageMensuel = date,
                        Kilometrage = kilo
                    };
                    var result = CreateMensuel(kilometrage);
                    if (result.IdKilometrageMensuel>0)
                    {
                        addedKm++;
                    }
                }
            }            
            return addedKm;
        }



        #endregion
    }
}
