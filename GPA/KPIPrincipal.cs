using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using GPA.BLL;
using GPA.Models;
 
public class KPIPrincipal
{
    private UtilisateurModel _user;  
    private Enumerators.AccessRights[] rights = new Enumerators.AccessRights[] {};
    private DepartementsAccessRightsModel departementsAccessRights = new DepartementsAccessRightsModel();

    public KPIPrincipal()
    {
        var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
        string username = cookie != null ? FormsAuthentication.Decrypt(cookie.Value).Name : string.Empty;
        this._user = UtilisateurService.GetByUsername(username);
        if (this._user != null)
        {
            this.Fullname = this._user.NomUtilisateur + " " + this._user.PrenomUtilisateur;
            var userName = this._user != null ? this._user.LoginUtilisateur : String.Empty;
            this.Identity = new GenericIdentity(userName);


            this.Client = this._user.ClientId;
            var role = RoleService.GetById(this._user.RoleId);
            if (role.ProfileRole != null && role.ProfileRole.HasValue)
            {
                this.Profile = role.ProfileRole.Value;
            }
            else
            {
                this.Profile = 0;
            }
            this.rights = AccessRightsService.GetByRoleId(this._user != null ? this._user.RoleId : 0);
            this.departementsAccessRights = AccessRightsService.GetDepartementsAccessRights(this._user != null ? this._user.IdUtilisateur : 0);
        }
    }

    public string Fullname { get; private set; }

    public IIdentity Identity { get; private set; }

    public Enumerators.AccessRights[] Rights
    {
        get { return this.rights; }
    }

    public DepartementsAccessRightsModel DepartementsAccessRights
    {
        get { return this.departementsAccessRights; }
    }

    public int Profile { get; private set; }

    public int Client { get; private set; }

    public bool IsInRights(Enumerators.AccessRights right)
    {
        bool isInRights = Rights.Contains(right);
        return isInRights;
    }

    public bool IsInRights(Enumerators.AccessRights[] rights)
    {
        bool isInRights = Rights.Any(item => rights.Contains(item));
        return isInRights;
    }  
}
