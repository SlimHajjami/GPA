using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;

namespace GPA.BLL
{
    public class ClientService
    {
        public static List<ClientModel> Read()
        {
            using (Entities context = new Entities())
            {
                var list = context.Clients.Select(e => new ClientModel
                {
                    IdClient = e.IdClient,
                    NomClient = e.NomClient,
                    AdresseClient = e.AdresseClient,
                    VilleClient = e.VilleClient,
                    CodePostalClient = e.CodePostalClient,
                    ContactClient = e.ContactClient,
                    Tel1Client = e.Tel1Client,
                    Tel2Client = e.Tel2Client,
                    FaxClient = e.FaxClient,
                    SiteWebClient = e.SiteWebClient,
                    EmailClient = e.EmailClient,
                    ParametresGis = e.GisParametres
                }).ToList();
                return list;
            }
        }

        public static ClientModel Get(int id)
        {
            using (Entities context = new Entities())
            {
                var list = context.Clients.Select(e => new ClientModel
                {
                    IdClient = e.IdClient,
                    NomClient = e.NomClient,
                    AdresseClient = e.AdresseClient,
                    VilleClient = e.VilleClient,
                    CodePostalClient = e.CodePostalClient,
                    ContactClient = e.ContactClient,
                    Tel1Client = e.Tel1Client,
                    Tel2Client = e.Tel2Client,
                    FaxClient = e.FaxClient,
                    SiteWebClient = e.SiteWebClient,
                    EmailClient = e.EmailClient,
                    ParametresGis = e.GisParametres
                }).FirstOrDefault(x => x.IdClient == id);
                return list;
            }
        }

        public static ClientModel Create(ClientModel model)
        {
            using (Entities context = new Entities())
            {
                var entity = new Client
                {
                    NomClient = model.NomClient,
                    AdresseClient = model.AdresseClient,
                    VilleClient = model.VilleClient,
                    CodePostalClient = model.CodePostalClient,
                    ContactClient = model.ContactClient,
                    Tel1Client = model.Tel1Client,
                    Tel2Client = model.Tel2Client,
                    FaxClient = model.FaxClient,
                    SiteWebClient = model.SiteWebClient,
                    EmailClient = model.EmailClient,
                    GisParametres = model.ParametresGis 
                };
                context.Clients.Add(entity);
                context.SaveChanges();
                model.IdClient = entity.IdClient;
                return model;
            }
        }

        public static bool Update(ClientModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.Clients.FirstOrDefault(e => e.IdClient == model.IdClient);
                if (entity != null)
                {
                    entity.NomClient = model.NomClient;
                    entity.AdresseClient = model.AdresseClient;
                    entity.VilleClient = model.VilleClient;
                    entity.CodePostalClient = model.CodePostalClient;
                    entity.ContactClient = model.ContactClient;
                    entity.Tel1Client = model.Tel1Client;
                    entity.Tel2Client = model.Tel2Client;
                    entity.FaxClient = model.FaxClient;
                    entity.SiteWebClient = model.SiteWebClient;
                    entity.EmailClient = model.EmailClient;
                    entity.GisParametres = model.ParametresGis;

                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }

        public static bool Delete(ClientModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.Clients.FirstOrDefault(e => e.IdClient == model.IdClient);

                if (entry != null)
                {
                    context.Clients.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }
    }
}
