using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;

namespace GPA.BLL
{
    public class PieceService
    {
        public static List<PieceModel> Read(int client)
        {
            using (Entities context = new Entities())
            {
                var list = context.Pieces.Select(e => new PieceModel
                {
                    IdPiece = e.IdPiece,
                    ClientId = e.ClientId,
                    NumeroPiece = e.NumeroPiece,
                    NomPiece = e.NomPiece,
                    PrixPiece = e.PrixPiece,
                    DatePrixPiece = e.DatePrixPiece,
                    FournisseurId = e.FournisseurId,
                    Constructeur = e.Constructeur
                }).ToList();
                if (client > 0)
                {
                    list = list.Where(x => x.ClientId == client).ToList();
                }
                return list;
            }
        }

        public static PieceModel Create(PieceModel model)
        {
            using (Entities context = new Entities())
            {
                var entity = new Piece
                {
                    ClientId = model.ClientId,
                    NumeroPiece = model.NumeroPiece,
                    NomPiece = model.NomPiece,
                    PrixPiece = model.PrixPiece,
                    DatePrixPiece = model.DatePrixPiece,
                    FournisseurId = model.FournisseurId,
                    Constructeur = model.Constructeur
                };
                context.Pieces.Add(entity);
                context.SaveChanges();
                model.IdPiece = entity.IdPiece;
                return model;
            }
        }

        public static bool Update(PieceModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.Pieces.FirstOrDefault(e => e.IdPiece == model.IdPiece);
                if (entity != null)
                {
                    entity.ClientId = model.ClientId;
                    entity.NumeroPiece = model.NumeroPiece;
                    entity.NomPiece = model.NomPiece;
                    entity.PrixPiece = model.PrixPiece;
                    entity.DatePrixPiece = model.DatePrixPiece;
                    entity.FournisseurId = model.FournisseurId;
                    entity.Constructeur = model.Constructeur;

                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }

        public static bool Delete(PieceModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.Pieces.FirstOrDefault(e => e.IdPiece == model.IdPiece);

                if (entry != null)
                {
                    context.Pieces.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }
    }
}
