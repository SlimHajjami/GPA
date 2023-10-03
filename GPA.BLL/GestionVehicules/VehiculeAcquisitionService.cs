using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;

namespace GPA.BLL
{
    public class VehiculeAcquisitionService
    {
        public static List<VehiculeAcquisitionModel> Read(int departement, int client, DepartementsAccessRightsModel departementsAccess)
        {
            using (Entities context = new Entities())
            {
                var list = context.VehiculeAcquisitions.Select(e => new VehiculeAcquisitionModel
                {
                    IdAcquisition = e.IdAcquisition,
                    Client = context.Clients.FirstOrDefault(x => x.IdClient == context.Vehicules.FirstOrDefault(y => y.IdVehicule == e.VehiculeId).ClientId).NomClient,
                    VehiculeId = e.VehiculeId,
                    IsLocation = e.IsLocation,
                    FournisseurId = e.FournisseurId,
                    DateAcquisition = e.DateAcquisition,
                    KilometrageAcquisition = e.KilometrageAcquisition,
                    PrixAcquisition = e.PrixAcquisition,
                    ExpirationGarantie = e.ExpirationGarantie,
                    PrixVente = e.PrixVente,
                    NoteAcquisition = e.NoteAcquisition
                }).ToList();

                if(departement > 0)
                {
                    var vehicules = context.Vehicules.Where(x => x.DepartementId == departement).Select(y => y.IdVehicule);
                    list = list.Where(x => vehicules.Contains(x.VehiculeId)).ToList();
                }

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

                foreach (var l in list)
                {
                    var paiements = context.VehiculeAcquisitionPaiements.Where(x => x.AcquisitionId == l.IdAcquisition);
                    if (paiements.Count() > 0)
                    {
                        l.RestePayer = l.PrixAcquisition - paiements.Sum(x => x.MontantPaiement);
                    }
                    else
                    {
                        l.RestePayer = l.PrixAcquisition;
                    }
                }
                return list;
            }
        }

        public static VehiculeAcquisitionModel Create(VehiculeAcquisitionModel model)
        {
            using (Entities context = new Entities())
            {
                var entity = new VehiculeAcquisition
                {
                    VehiculeId = model.VehiculeId,
                    IsLocation = model.IsLocation,
                    FournisseurId = model.FournisseurId,
                    DateAcquisition = model.DateAcquisition,
                    KilometrageAcquisition = model.KilometrageAcquisition,
                    PrixAcquisition = model.PrixAcquisition,
                    ExpirationGarantie = model.ExpirationGarantie,
                    PrixVente = model.PrixVente,
                    NoteAcquisition = model.NoteAcquisition
                };
                context.VehiculeAcquisitions.Add(entity);
                context.SaveChanges();
                model.IdAcquisition = entity.IdAcquisition;
                return model;
            }
        }

        public static bool Create(int vehicule)
        {
            using (Entities context = new Entities())
            {
                var entity = new VehiculeAcquisition
                {
                    VehiculeId = vehicule
                };
                context.VehiculeAcquisitions.Add(entity);
                int objectAffected = context.SaveChanges();
                return objectAffected > 0;
            }
        }

        public static bool Update(VehiculeAcquisitionModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.VehiculeAcquisitions.FirstOrDefault(e => e.IdAcquisition == model.IdAcquisition);
                if (entity != null)
                {
                    if (entity.DateAcquisition.HasValue && entity.DateAcquisition.Value == model.DateAcquisition.Value)
                    {
                        if (model.DateAcquisition.HasValue && model.KilometrageAcquisition.HasValue)
                        {
                            
                            LectureKilometrageService.UpdateKilometrage(model.VehiculeId, model.DateAcquisition.Value, model.KilometrageAcquisition.Value, "Lors de saisie Acquisition");
                            
                        }
                    }
                    else
                    {
                        if (model.DateAcquisition.HasValue && model.KilometrageAcquisition.HasValue)
                        {
                            if (entity.DateAcquisition.HasValue)
                            {
                                LectureKilometrageService.UpdateKilometrageDate(model.VehiculeId, entity.DateAcquisition.Value, model.DateAcquisition.Value, model.KilometrageAcquisition.Value);

                            }
                            else
                            {
                                LectureKilometrageService.UpdateKilometrage(model.VehiculeId, model.DateAcquisition.Value, model.KilometrageAcquisition.Value, "Lors de saisie Acquisition");

                                //var clientId = context.Clients.Where(w => w.NomClient == model.Client).FirstOrDefault().IdClient;
                                //var modele = new LectureKilometrageModel
                                //{
                                //    ClientId = clientId,
                                //    VehiculeId = model.VehiculeId,
                                //    DateLectureKilometrage = model.DateAcquisition.Value,
                                //    Kilometrage = model.KilometrageAcquisition.Value,
                                //    Note = "Lors d'une mise à jour Aquisition"
                                //};
                                //LectureKilometrageService.Create(modele);
                            }


                        }
                    }
                    entity.VehiculeId = model.VehiculeId;
                    entity.IsLocation = model.IsLocation;//.GetValueOrDefault();
                    entity.FournisseurId = model.FournisseurId;//.GetValueOrDefault();
                    entity.DateAcquisition = model.DateAcquisition;//.GetValueOrDefault();
                    entity.KilometrageAcquisition = model.KilometrageAcquisition;//.GetValueOrDefault();
                    entity.PrixAcquisition = model.PrixAcquisition;//.GetValueOrDefault();
                    entity.ExpirationGarantie = model.ExpirationGarantie;//.GetValueOrDefault();
                    entity.PrixVente = model.PrixVente;//.GetValueOrDefault();
                    entity.NoteAcquisition = model.NoteAcquisition;

                    objectAffected = context.SaveChanges();
                }

                //if (model.DateAcquisition.HasValue && model.KilometrageAcquisition.HasValue)
                //{
                //    LectureKilometrageService.UpdateKilometrage(model.VehiculeId, model.DateAcquisition.Value, model.KilometrageAcquisition.Value, "Lors de saisie Acquisition");
                //}

                return objectAffected > 0;
            }
        }

        public static bool Delete(VehiculeAcquisitionModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.VehiculeAcquisitions.FirstOrDefault(e => e.IdAcquisition == model.IdAcquisition);

                if (entry != null)
                {
                    context.VehiculeAcquisitions.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }

        public static bool Delete(int vehicule)
        {
            using (Entities context = new Entities())
            {
                var entry = context.VehiculeAcquisitions.FirstOrDefault(e => e.VehiculeId == vehicule);

                if (entry != null)
                {
                    context.VehiculeAcquisitions.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }
    }
}
