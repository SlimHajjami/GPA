using System;
using System.Collections.Generic;
using System.Linq;
using GPA.DAL;
using GPA.Models;
using Excel;
using System.Data;
using System.IO;

namespace GPA.BLL
{
    public class VehiculeService
    {
        public static List<VehiculeModel> Read(int departement, int client, DepartementsAccessRightsModel departementsAccess)
        {
            using (Entities context = new Entities())
            {
                var list = context.Vehicules.Select(e => new VehiculeModel
                {
                    IdVehicule = e.IdVehicule,
                    ClientId = e.ClientId,
                    MatriculeVehicule = e.MatriculeVehicule,
                    codeRefMarque = e.codeRefMarque,
                    MarqueVehicule = context.REFERENTIELs.FirstOrDefault(x => x.KEY_REF == "MARQUE_VEHICULE" && x.CODE_REF == e.codeRefMarque).VALUE,
                    IdModele = e.IdModele,
                    ModeleVehicule = context.VehiculeModeles.FirstOrDefault(x => x.IdModele == e.IdModele).NomModele ?? "",
                    CouleurVehicule = e.CouleurVehicule,
                    NumeroSerieVehicule = e.NumeroSerieVehicule,
                    MiseCirculation = e.MiseCirculation,
                    TypeCarburantId = e.TypeCarburantId,
                    TypeCarburant = context.REFERENTIELs.FirstOrDefault(x => x.KEY_REF == "TYPE_CARBURANT" && x.CODE_REF == e.TypeCarburantId).VALUE,
                    TypeVehiculeId = e.TypeVehiculeId,
                    TypeVehicule = context.REFERENTIELs.FirstOrDefault(x => x.KEY_REF == "TYPE_VEHICULE" && x.CODE_REF == e.TypeCarburantId).VALUE,
                    DepartementId = e.DepartementId,
                    Departement = context.VehiculeDepartements.FirstOrDefault(x => x.IdDepartement == e.DepartementId).NomDepartement,
                    ConducteurId = e.ConducteurId,
                    Conducteur = context.VehiculeConducteurs.FirstOrDefault(x => x.IdConducteur == e.ConducteurId).NomConducteur + " " + context.VehiculeConducteurs.FirstOrDefault(x => x.IdConducteur == e.ConducteurId).PrenomConducteur,
                    IsActif = e.IsActif
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

        public static VehiculeModel Get(int id)
        {
            using (Entities context = new Entities())
            {
                var list = context.Vehicules.Select(e => new VehiculeModel
                {
                    IdVehicule = e.IdVehicule,
                    ClientId = e.ClientId,
                    MatriculeVehicule = e.MatriculeVehicule,
                    codeRefMarque = e.codeRefMarque,
                    MarqueVehicule = context.REFERENTIELs.FirstOrDefault(x => x.KEY_REF == "MARQUE_VEHICULE" && x.CODE_REF == e.codeRefMarque).VALUE,
                    IdModele = e.IdModele,
                    ModeleVehicule = context.VehiculeModeles.FirstOrDefault(x => x.IdModele == e.IdModele).NomModele ?? "",
                    CouleurVehicule = e.CouleurVehicule,
                    NumeroSerieVehicule = e.NumeroSerieVehicule,
                    MiseCirculation = e.MiseCirculation,
                    TypeCarburantId = e.TypeCarburantId,
                    TypeCarburant = context.REFERENTIELs.FirstOrDefault(x => x.KEY_REF == "TYPE_CARBURANT" && x.CODE_REF == e.TypeCarburantId).VALUE,
                    TypeVehiculeId = e.TypeVehiculeId,
                    TypeVehicule = context.REFERENTIELs.FirstOrDefault(x => x.KEY_REF == "TYPE_VEHICULE" && x.CODE_REF == e.TypeCarburantId).VALUE,
                    DepartementId = e.DepartementId,
                    Departement = context.VehiculeDepartements.FirstOrDefault(x => x.IdDepartement == e.DepartementId).NomDepartement,
                    ConducteurId = e.ConducteurId,
                    Conducteur = context.VehiculeConducteurs.FirstOrDefault(x => x.IdConducteur == e.ConducteurId).NomConducteur + " " + context.VehiculeConducteurs.FirstOrDefault(x => x.IdConducteur == e.ConducteurId).PrenomConducteur,
                    IsActif = e.IsActif
                }).FirstOrDefault(x => x.IdVehicule == id);

                return list;
            }
        }

        public static VehiculeModel Exist(string matricule)
        {
            return Read(0, 0, null).FirstOrDefault(x => x.MatriculeVehicule == matricule);
        }

        public static VehiculeModel Create(VehiculeModel model)
        {
            using (Entities context = new Entities())
            {
                if (model.TypeCarburantId == 0 || model.TypeCarburantId == null)
                {
                    var typeCarburant = context.REFERENTIELs.FirstOrDefault(x => x.VALUE.Contains("Essence")).CODE_REF;
                    model.TypeCarburantId = typeCarburant;
                }
                var entity = new Vehicule
                {
                    ClientId = model.ClientId,
                    MatriculeVehicule = model.MatriculeVehicule,
                    codeRefMarque = model.codeRefMarque,
                    IdModele = model.IdModele,
                    CouleurVehicule = model.CouleurVehicule,
                    NumeroSerieVehicule = model.NumeroSerieVehicule,
                    MiseCirculation = model.MiseCirculation,
                    TypeCarburantId = model.TypeCarburantId,
                    TypeVehiculeId = model.TypeVehiculeId,
                    DepartementId = model.DepartementId,
                    ConducteurId = model.ConducteurId,
                    IsActif = model.IsActif
                };
                context.Vehicules.Add(entity);
                context.SaveChanges();
                model.IdVehicule = entity.IdVehicule;
                return model;
            }
        }

        public static bool Update(VehiculeModel model)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var entity = context.Vehicules.FirstOrDefault(e => e.IdVehicule == model.IdVehicule);
                if (entity != null)
                {
                    entity.ClientId = model.ClientId;
                    entity.MatriculeVehicule = model.MatriculeVehicule;
                    entity.codeRefMarque = model.codeRefMarque;
                    entity.IdModele = model.IdModele;
                    entity.CouleurVehicule = model.CouleurVehicule;
                    entity.NumeroSerieVehicule = model.NumeroSerieVehicule;
                    entity.MiseCirculation = model.MiseCirculation;
                    entity.TypeCarburantId = model.TypeCarburantId;
                    entity.TypeVehiculeId = model.TypeVehiculeId;
                    entity.DepartementId = model.DepartementId;
                    entity.ConducteurId = model.ConducteurId;
                    entity.IsActif = model.IsActif;

                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }

        public static bool Delete(VehiculeModel model)
        {
            using (Entities context = new Entities())
            {
                var entry = context.Vehicules.FirstOrDefault(e => e.IdVehicule == model.IdVehicule);

                if (entry != null)
                {
                    context.Vehicules.Remove(entry);
                }

                var objectAffedted = context.SaveChanges();
                return objectAffedted > 0;
            }
        }

        public static List<string> getImportedFileColumns(string filePath, string fileFormat)
        {
            List<string> columns = new List<string>();
            if (fileFormat == ".xls" || fileFormat == ".xlsx")
            {
                IExcelDataReader excelReader = GetExcelReader(filePath, fileFormat);
                if (excelReader != null)
                {
                    excelReader.IsFirstRowAsColumnNames = true;
                    DataSet result = excelReader.AsDataSet();
                    DataTable articles = result.Tables[0];

                    foreach (var col in articles.Columns)
                    {
                        columns.Add(col.ToString());
                    }

                    excelReader.Close();
                }
            }
            return columns;
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

        public static int Import(string filePath, string fileFormat, VehiculeColumns columns, int client)
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

                    var conducteurs = VehiculeConducteurService.Read(client);
                    var departements = VehiculeDepartementService.Read(client, null);
                    var types = ReferentielService.GetReferentiel("TYPE_VEHICULE");
                    var carburants = ReferentielService.GetReferentiel("TYPE_CARBURANT");

                    foreach (DataRow row in articles.Rows)
                    {
                        string matricule = "";
                        if (!string.IsNullOrEmpty(columns.Matricule))
                        {
                            matricule = row[columns.Matricule].ToString();
                            //check if the first character of matricule is number
                            if (!Char.IsNumber(matricule.FirstOrDefault()))
                            {
                                continue;
                            }
                            //if (String.IsNullOrEmpty(matricule) || !Char.IsNumber(matricule[0])) { continue; }
                        }

                        string marque = "";
                        string modele = "";
                        int codeRefMarque = 0;
                        int idModele = 0;
                        using (Entities context = new Entities())
                        {
                            if (!string.IsNullOrEmpty(columns.Marque))
                            {
                                marque = row[columns.Marque].ToString();
                            }


                            if (!string.IsNullOrEmpty(columns.Modele))
                            {
                                modele = row[columns.Modele].ToString();
                                var entry = context.VehiculeModeles.FirstOrDefault(x => x.NomModele == modele);
                                if (entry != null)
                                {
                                    codeRefMarque = entry.CodeRefMarque;
                                    idModele = entry.IdModele;
                                }
                            }
                        }

                        string couleur = "";
                        if (!string.IsNullOrEmpty(columns.Couleur))
                        {
                            couleur = row[columns.Couleur].ToString();
                        }

                        string numeroSerie = "";
                        if (!string.IsNullOrEmpty(columns.NumeroSerie))
                        {
                            numeroSerie = row[columns.NumeroSerie].ToString();
                        }

                        DateTime? miseCirculation = null;
                        if (!string.IsNullOrEmpty(columns.MiseCirculation))
                        {
                            double d = 0;
                            if (double.TryParse(row[columns.MiseCirculation].ToString(), out d))
                            {
                                miseCirculation = DateTime.FromOADate(d);
                            }
                        }

                        int carburant = 0;
                        if (!string.IsNullOrEmpty(columns.Carburant))
                        {
                            var c = carburants.FirstOrDefault(x => x.Text == row[columns.Carburant].ToString());
                            if (c != null)
                            {
                                carburant = int.Parse(c.Value);
                            }
                        }

                        int type = 0;
                        if (!string.IsNullOrEmpty(columns.Type))
                        {
                            var t = types.FirstOrDefault(x => x.Text == row[columns.Type].ToString());
                            if (t != null)
                            {
                                type = int.Parse(t.Value);
                            }
                        }

                        int departement = 0;
                        if (!string.IsNullOrEmpty(columns.Departement))
                        {
                            var d = departements.FirstOrDefault(x => x.NomDepartement == row[columns.Departement].ToString());
                            if (d != null)
                            {
                                departement = d.IdDepartement;
                            }
                        }

                        int conducteur = 0;
                        if (!string.IsNullOrEmpty(columns.Conducteur))
                        {
                            var c = conducteurs.FirstOrDefault(x => (x.NomConducteur + x.PrenomConducteur).Replace(" ","") == row[columns.Conducteur].ToString().Replace(" ",""));
                            if (c != null)
                            {
                                conducteur = c.IdConducteur;
                            }
                        }


                        VehiculeModel vehicule = new VehiculeModel
                        {
                            ClientId = client,
                            MatriculeVehicule = matricule,
                            MarqueVehicule = marque,
                            codeRefMarque = codeRefMarque,
                            ModeleVehicule = modele,
                            IdModele = idModele,
                            CouleurVehicule = couleur,
                            NumeroSerieVehicule = numeroSerie,
                            MiseCirculation = miseCirculation,
                            TypeCarburantId = carburant,
                            TypeVehiculeId = type,
                            ConducteurId = conducteur,
                            DepartementId = departement,
                            IsActif = true
                        };

                        var v =  Exist(matricule);
                        if (v != null)
                        {
                            vehicule.IdVehicule = v.IdVehicule;
                            var model = Update(vehicule);
                        }
                        else
                        {
                            var model = Create(vehicule);
                            VehiculeAcquisitionService.Create(model.IdVehicule);
                            VehiculeDocumentsService.Create(model.IdVehicule);
                        }
                        importedRows++;
                    }

                    excelReader.Close();
                }
                File.Delete(filePath);
            }
            return importedRows;
        }
    }
}
