using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;

namespace GPA.BLL
{
    public class AlerteEmailService
    {
        public static List<AlerteEmailModel> Read(int client)
        {
            using (Entities context = new Entities())
            {
                var list = context.AlerteEmails.Select(e => new AlerteEmailModel
                {
                    IdAlerteEmail = e.IdAlerteEmail,
                    ClientId = e.ClientId,
                    AlerteEmail = e.AlerteEmail1,
                    TypeAlerteId = e.TypeAlerteId
                }).ToList();
                if (client > 0)
                {
                    list = list.Where(x => x.ClientId == client).ToList();
                }
                return list;
            }
        }

        public static AlerteEmailModel Create(AlerteEmailModel model)
        {
            using (Entities context = new Entities())
            {
                var entity = new AlerteEmail
                {
                    ClientId = model.ClientId,
                    AlerteEmail1 = model.AlerteEmail,
                    TypeAlerteId = model.TypeAlerteId
                };
                context.AlerteEmails.Add(entity);
                context.SaveChanges();
                model.IdAlerteEmail = entity.IdAlerteEmail;
                return model;
            }
        }

        public static bool Update(AlerteEmailModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.AlerteEmails.FirstOrDefault(e => e.IdAlerteEmail == model.IdAlerteEmail);
                if (entity != null)
                {
                    entity.ClientId = model.ClientId;
                    entity.AlerteEmail1 = model.AlerteEmail;
                    entity.TypeAlerteId = model.TypeAlerteId;

                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }

        public static bool Delete(AlerteEmailModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.AlerteEmails.FirstOrDefault(e => e.IdAlerteEmail == model.IdAlerteEmail);

                if (entry != null)
                {
                    context.AlerteEmails.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }
    }
}
