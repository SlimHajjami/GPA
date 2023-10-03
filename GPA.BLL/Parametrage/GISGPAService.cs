using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using GPA.DAL;
using GPA.Models;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Web.Configuration;

namespace GPA.BLL
{
    public class GISGPAService
    {
        public static GISGPAModel Create(GISGPAModel model)
        {
            using (Entities context = new Entities())
            {

                var entity = new GIS_GPA
                {
                    ClientId = model.ClientId,
                    IdGisGpa = model.IdGisGpa,
                    VehiculeGPAId = model.VehiculeGPAId,
                    VehiculeGis = model.VehiculeGISMat,
                    VehiculeGisId = model.VehiculeGISId
                };
                context.GIS_GPA.Add(entity);
                context.SaveChanges();
                model.IdGisGpa = entity.IdGisGpa;
                return model;
            }
        }

        public static List<GISGPAModel> Read(int departement, int client, DepartementsAccessRightsModel departementsAccess)
        {
            using (Entities context = new Entities())
            {
                List<GISGPAModel> list = new List<GISGPAModel>();
                var vehicules = context.Vehicules.Where(x => x.ClientId == client).ToList();
                if (departementsAccess != null)
                {
                    var d = departementsAccess.Visualisation;
                    vehicules = vehicules.Where(x => x.DepartementId != null && d.Contains((int)x.DepartementId)).ToList();
                }
                //if (client > 0)
                //{
                //    vehicules = vehicules.Where(x => x.ClientId == client).ToList();
                //}
                if (departement > 0)
                {
                    vehicules = vehicules.Where(x => x.DepartementId == departement).ToList();
                }


                foreach (var item in vehicules)
                {
                    var result = new GISGPAModel {
                        VehiculeGPAId = item.IdVehicule,
                        VehiculeGPAMat = item.MatriculeVehicule
                    };

                    var exist = context.GIS_GPA.FirstOrDefault(x => x.VehiculeGPAId == item.IdVehicule);
                    if (exist != null)
                    {
                        result.IdGisGpa = exist.IdGisGpa;
                        result.VehiculeGISMat = exist.VehiculeGis;
                        result.VehiculeGISId = exist.VehiculeGisId ?? 0;
                    }
                    list.Add(result);
                }

                return list;
            }
        }
        struct Vehicule
        {
            public int IdVeh { get; set; }
            public int? Id { get; set; }
            public string Cph { get; set; }
            public int? Client { get; set; }
            public int? ClientId { get; set; }
            public decimal VitesseMax { get; set; }
            public decimal? LastAtime { get; set; }
        }
        public static List<VehiculeModel> ReadFromGis(int clientId)
        {
            using (Entities context = new Entities())
            {
                if (clientId>0)
                {

               
                var emails = context.Clients.FirstOrDefault(e=>e.IdClient == clientId).GisParametres;
                             
                List<VehiculeModel> list = new List<VehiculeModel>();
                if (emails == null)
                {
                    return list;
                }
                string apiAddress = WebConfigurationManager.AppSettings["WebApiAddressGIS"];

              //  var parametres = "parbranchegabes@belive.tn;parsav@belive.tn";

                var requestURIString = apiAddress
                                       + "GisWebService/api/GPA/GetDevices?emails="
                                       + emails;

                var request = (HttpWebRequest)WebRequest.Create(requestURIString);

                var postData = "";
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                using (StreamReader r = new StreamReader(response.GetResponseStream()))
                {
                    string json = r.ReadToEnd();
                    System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    List<Vehicule> finalResult = jsSerializer.Deserialize<List<Vehicule>>(json);  // Deserialize JSON data into list of Vehicles.

                    foreach (var item in finalResult)
                    {
                        var result = new VehiculeModel
                        {
                            IdVehicule = item.IdVeh,
                            MatriculeVehicule = item.Cph //vehicule gis
                        };
                        list.Add(result);
                    }


                }
                return list;
                }
                else
                {
                    return null;
                }
            }
        }
        public static List<GISGPAModel> ReadfromGISGPA(int departement, int client, DepartementsAccessRightsModel departementsAccess)
        {
            using (Entities context = new Entities())
            {
                var list = context.GIS_GPA.Select(e => new GISGPAModel
                {
                    IdGisGpa = e.IdGisGpa,
                    ClientId = e.ClientId,
                    VehiculeGPAId = e.VehiculeGPAId,
                    VehiculeGISMat = e.VehiculeGis,
                    VehiculeGISId = e.VehiculeGisId,
                    VehiculeGPAMat = context.Vehicules.FirstOrDefault(x => x.IdVehicule ==e.VehiculeGPAId).MatriculeVehicule,
                    DepartementId = context.Vehicules.FirstOrDefault(x => x.IdVehicule == e.VehiculeGPAId).DepartementId
                }).ToList();

                if (departement > 0)
                {
                    list = list.Where(x => x.DepartementId == departement).ToList();
                }
                if (client > 0)
                {
                    list = list.Where(x => x.ClientId == client).ToList();
                }
                if (departementsAccess != null)
                {
                    var d = departementsAccess.Visualisation;
                    list = list.Where(x => x.DepartementId != null && d.Contains((int)x.DepartementId)).ToList();
                }
                return list;
            }
        }

        public static bool Update(GISGPAModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.GIS_GPA.FirstOrDefault(e => e.IdGisGpa == model.IdGisGpa);
                if (entity != null)
                {
                   // entity.ClientId = model.ClientId;
                   // entity.VehiculeGPAId = model.VehiculeGPAId;
                    entity.VehiculeGis = model.VehiculeGISMat;
                    entity.VehiculeGisId = model.VehiculeGISId;
                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }

        public static bool Delete(GISGPAModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.GIS_GPA.FirstOrDefault(e => e.IdGisGpa == model.IdGisGpa);

                if (entry != null)
                {
                    context.GIS_GPA.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }



    }
}
