using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using GPA.DAL;
using GPA.Models;
using System.Web;

namespace GPA.BLL
{
    public class RoleService
    {
        public static bool Create(RoleModel role)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;

                var myEntity = new Role
                {
                    NomRole = role.NomRole,
                    DescriptionRole = role.DescriptionRole,
                    ProfileRole = role.ProfileRole
                };

                context.Roles.Add(myEntity);
                objectAffected = context.SaveChanges();
                role.IdRole = myEntity.IdRole;

                return objectAffected > 0;
            }
        }

        public static bool Update(RoleModel role)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;
                var myEntity = context.Roles.FirstOrDefault(o => o.IdRole == role.IdRole);
                if (myEntity != null)
                {
                    myEntity.NomRole = role.NomRole;
                    myEntity.DescriptionRole = role.DescriptionRole;
                    myEntity.ProfileRole = role.ProfileRole;

                    objectAffected = context.SaveChanges();

                }
                return objectAffected > 0;
            }
        }

        public static bool Delete(RoleModel role)
        {
            using (Entities context = new Entities())
            {
                int objectAffected = 0;
                var myEntity = context.Roles.FirstOrDefault(r => r.IdRole == role.IdRole);
                if (myEntity != null)
                {
                    context.Roles.Remove(myEntity);
                    objectAffected = context.SaveChanges();
                }
                return objectAffected > 0;
            }
        }

        public static IEnumerable<RoleModel> GetAll()
        {
            using (Entities context = new Entities())
            {
                return context.Roles.Select(role => new RoleModel
                {
                    NomRole = role.NomRole,
                    DescriptionRole = role.DescriptionRole,
                    IdRole = role.IdRole,
                    ProfileRole = role.ProfileRole
                }).ToList();
            }
        }

        public static IEnumerable<string> GetLabelsForMenu()
        {
            using (Entities context = new Entities())
            {
                return context.Roles.Select(e => e.NomRole).Distinct().ToList();
            }
        }

        public static RoleModel GetById(int id)
        {
            using (Entities context = new Entities())
            {
                var myModel = new RoleModel();
                var myEntity = context.Roles.FirstOrDefault(role => role.IdRole == id);
                if (myEntity != null)
                {
                    myModel.NomRole = myEntity.NomRole;
                    myModel.DescriptionRole = myEntity.DescriptionRole;
                    myModel.IdRole = myEntity.IdRole;
                    myModel.ProfileRole = myEntity.ProfileRole;
                    myModel.AccessRights = AccessRightsService.GetByRoleId(id);
                }
                return myModel;
            }
        }

        public static RoleModel GetByProfile(int profile)
        {
            using (Entities context = new Entities())
            {
                var myModel = new RoleModel();
                var myEntity = context.Roles.FirstOrDefault(role => role.ProfileRole == profile);
                if (myEntity != null)
                {
                    myModel.NomRole = myEntity.NomRole;
                    myModel.DescriptionRole = myEntity.DescriptionRole;
                    myModel.IdRole = myEntity.IdRole;
                    myModel.ProfileRole = myEntity.ProfileRole;
                }
                return myModel;
            }
        }

        public static string Update(string roleID, string roleName, string roleDescription, string rights)
        {
            int[] arrAccessRights = rights != null ? rights.Split(',')
                .Select(item => int.Parse(item)).ToArray() : new int[] { };

            return UpdateRole(roleID, roleName, roleDescription, arrAccessRights);
        }

        private static string UpdateRole(string roleID, string roleName, string roleDescription, int[] rights)
        {
            int key = 0;
            int.TryParse(roleID, out key);
            if (key == 0)
                return "rôle n'existe pas dans la base";
            var myRole = GetById(key);
            if (myRole == null)
                throw new Exception("The role id cannot be empty");

            myRole.NomRole = roleName;
            myRole.DescriptionRole = roleDescription;
            Update(myRole);

            var isUpdated = AccessRightsService.SetAccessRightsToRole(key, rights.Select(item => (Enumerators.AccessRights)item).ToArray());
            if (isUpdated)
            {
                return string.Empty;
            }
            else
            {
                return "La mise à jour a échoué";
            }
        }

        public static string Create(string roleName, string roleDescription, string rights)
        {
            int[] arrAccessRights = rights != null ? rights.Split(',')
                .Select(item => int.Parse(item)).ToArray() : new int[] { };

            return CreateRole(roleName, roleDescription, arrAccessRights);
        }

        private static string CreateRole(string roleName, string roleDescription, int[] rights)
        {
            var myRole = GetByLabel(roleName);
            if (myRole != null)
                return "role existe déjà";

            var myCreatedRole = new RoleModel { NomRole = roleName, DescriptionRole = roleDescription, IdRole = 0};
            if(!Create(myCreatedRole))
            //skips method if role not created
            if (myCreatedRole == null)
                return "l'enregistrement a échoué";

            //update RightsInRoles
            var isUpdated = AccessRightsService.SetAccessRightsToRole(myCreatedRole.IdRole, rights.Select(item => (Enumerators.AccessRights)item).ToArray());
            if (isUpdated)
            {
                return string.Empty;
            }
            else
            {
                return "L'insertion des droits d'accès a échoué";
            }
        }

        private static Role GetByLabel(string roleName)
        {
            using (Entities context = new Entities())
            {
                if (string.IsNullOrEmpty(roleName))
                {
                    return null;
                }
                return context.Roles.FirstOrDefault(item => item.NomRole.ToLower() == roleName.ToLower());
            }
        }

        public static RoleModel GetNew()
        {
            var myModel = new RoleModel();
            myModel.IdRole = 0;
            myModel.AccessRights = new Enumerators.AccessRights[] { };
            return myModel;
        }
    }
}
