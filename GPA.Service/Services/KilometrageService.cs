using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.Configuration;
using Newtonsoft.Json.Linq;
using GPA.DAL;
using GPA.Models;
using GPA.Service.Utilities;
using System.Net;
using System.IO;
using System.Configuration;
using System.Data.Entity;

namespace GPA.Service.Services
{
    public class KilometrageService
    {
        public static bool KilometrageUpdate()
        {
            using (Entities context = new Entities())
            {
                var clientsGis = context.Clients.Where(c => c.GisParametres != null).ToList();
                string emails = "";
                var inc = 0;
                foreach (var item in clientsGis)
                {
                    if (inc == 0)
                    {
                        emails += item.GisParametres;
                        inc++;
                    }else
                    {
                        emails += ";";
                        emails += item.GisParametres;
                        inc++;
                    }
                   

                }

                var lastUpdate = context.GISLastUpdates.FirstOrDefault().DateUpdate;
                var date = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
                 var nbrJour = (System.DateTime.Now - lastUpdate).Days;
                //var nbrJour = 1;
                var response = GetKilometers(emails, date, nbrJour);

                if (response != null)
                {
                    try
                    {
                        using (StreamReader r = new StreamReader(response.GetResponseStream()))
                        {
                            var objectAffected = 0;
                            string json = r.ReadToEnd();
                            Logger.LogInformation("Received data: "+ json);
                            System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                            List<Vehicule> finalResult = jsSerializer.Deserialize<List<Vehicule>>(json);  // Deserialize JSON data into list of Vehicles.
                            Logger.LogInformation("Total data: " + finalResult.Count());
                            foreach (var item in finalResult)
                            {
                                Logger.LogInformation("--------Received GIS Item: " + item.Cph);
                                var vehicule = context.GIS_GPA.Where(i => i.VehiculeGis == item.Cph || i.VehiculeGisId == item.IdVeh ).FirstOrDefault();
                                int kmAjoute = 0;
                                if (vehicule != null)
                                {
                                    Logger.LogInformation("vehicule GPA: " + vehicule.VehiculeGPAId);
                                    var listLectureparvehicule = context.LectureKilometrages.Where(i => i.ClientId == vehicule.ClientId && i.VehiculeId == vehicule.VehiculeGPAId).ToList();
                                    if (listLectureparvehicule.Count() > 0)
                                    {
                                        var derniereLecture = listLectureparvehicule.OrderByDescending(x => x.DateLectureKilometrage).FirstOrDefault();
                                        kmAjoute = derniereLecture.Kilometrage;

                                            // var hier = DateTime.Now.AddDays(-1).Date;
                                        /**
                                         * plus besoin de table Last update! GPA ajoute le kilometrage reçu à la derniere valeur dans la table qque soit son origine 
                                         * 
                                         *    var lastKmEnregistrement = context.LectureKilometrages.Where(x => x.ClientId == vehicule.ClientId && x.VehiculeId == vehicule.VehiculeGPAId && DbFunctions.TruncateTime(x.DateLectureKilometrage) == lastUpdate.Date).FirstOrDefault();
                                            if (lastKmEnregistrement != null)
                                            {
                                                kmAjoute = lastKmEnregistrement.Kilometrage;
                                            }
                                        **/
                                     }

                                    /*Pour tester le service plusieurs fois le meme jour et ne pas comptabiliser les donnees plusieurs fois
                                     * 
                                     * var auj = DateTime.Now.Date;
                                    var aujKmEnregistrement = context.LectureKilometrages.Where(i => i.ClientId == vehicule.ClientId && i.VehiculeId == vehicule.VehiculeGPAId && DbFunctions.TruncateTime(i.DateLectureKilometrage) == auj).ToList();

                                    if (aujKmEnregistrement.Count() != 0)
                                    {
                                        foreach (var record in aujKmEnregistrement)
                                        {
                                            if (record.Note == "Lors de lecture par GPS")
                                            {
                                                context.LectureKilometrages.Remove(record);
                                            }
                                            
                                        }
                                    }**/

                                    var entity = new LectureKilometrage
                                    {
                                        ClientId = vehicule.ClientId,
                                        VehiculeId = vehicule.VehiculeGPAId, //vehicule gis
                                        DateLectureKilometrage = System.DateTime.Now,
                                        Kilometrage = (int)Math.Round(item.Km, MidpointRounding.ToEven) + kmAjoute,
                                        Note = "Lors de lecture par GPS"
                                    };
                                    context.LectureKilometrages.Add(entity);
                                    Logger.LogInformation("Nouvelle Lecture vehicule GIS :" + vehicule.VehiculeGis);
                                    objectAffected += context.SaveChanges();

                                }

                            }
                            if (objectAffected > 0)
                            {
                                var updateEntity = context.GISLastUpdates.FirstOrDefault();
                                if (updateEntity != null)
                                {
                                    updateEntity.DateUpdate = System.DateTime.Now;
                                }
                                context.SaveChanges();
                                Logger.LogInformation("Database mise à jour; "+ objectAffected +" lectures effectuées.");
                            }
                            
                            
                        }
                        return false;
                    }
                    catch (Exception e)
                    {
                        //Server not responding
                        Logger.LogException(e);
                        return true;
                    }
                }
                else
                {
                    return true;
                }                 
            }
        }

        private static HttpWebResponse GetKilometers(string emails, string date, double nbJour)
        {

            string apiAddress = System.Configuration.ConfigurationManager.AppSettings["WebApiAddressGIS"];// WebConfigurationManager.AppSettings["WebApiAddressGIS"];
            var requestURIString = apiAddress
                                   + "GisWebService/api/GPA/GetDevicesKm?mode=1&emails="
                                   + emails
                                   + "&date="
                                   + date
                                   + "&days="
                                   + nbJour;

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
            try{
                Logger.LogInformation("Connexion au serveur... emails: " + emails);
                var response = (HttpWebResponse)request.GetResponse();
                Logger.LogInformation("Connexion au serveur réussie ");
                return response;
            } 
            catch (Exception ex)
            {
                //Server not responding
                Logger.LogException(ex);
                return null;
            }
        }

        struct Vehicule
        {
            public int IdVeh { get; set; }
            public string Cph { get; set; }
            public decimal Km { get; set; }
            }    }
}
