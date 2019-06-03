using MSS.Data.Entities.Security;
using MSSSystem.DAL;
//using LS.System.DAL.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSS.Data.Entities;

/// <summary>
/// BLL security contains all of the security controllers for the security tables in our database
/// </summary>
namespace MSSSystem.BLL.Security
{
    /// <summary>
    /// Role manager is the class that is responsible for methods involving the role such as inserting the default roles on
    /// </summary>
    [DataObject]
    public class RoleManager : RoleManager<IdentityRole>
    {
        /// <summary>
        /// Gets called when the class is constructed
        /// </summary>
        public RoleManager() : base(new RoleStore<IdentityRole>
            (new MSSContext()))
        {
        }

        /// <summary>
        /// Adds all 3 of the default roles(AdministratorView, AdministratorEdit, SuperUser) to the database
        /// </summary>
        public void AddDefaultRoles()
        {
            foreach (string roleName in SecurityRoles.DefaultSecurityRoles)
            {
                // Check if the role exists
                if (!Roles.Any(r => r.Name == roleName))
                {
                    this.Create(new IdentityRole(roleName));
                }
            }
        }


        /// <summary>
        /// Lists the names of all of the roles in the database
        /// </summary>
        /// <returns>A list of strings containing the names of the roles in the database</returns>
        #region UserRole Administration
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<string> ListAllRoleNames()
        {
            return this.Roles.Select(r => r.Name).ToList();
        }

        /// <summary>
        /// Lists all roles as classes from the database
        /// </summary>
        /// <returns>A list of Role profile classes which contains all information about all of the roles in the database</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<RoleProfile> ListAllRoles()
        {
            var um = new UserManager();
            var results = from role in Roles.ToList()
                          select new RoleProfile
                          {
                              RoleId = role.Id,
                              RoleName = role.Name,
                              UserNames = role.Users.Select(r => um.FindById(r.UserId).UserName)
                          };
            return results.ToList();
        }

        /// <summary>
        /// Lists the names of all of the roles in the database except for the SuperUser role
        /// </summary>
        /// <returns>A list of strings containing the names of the roles in the database except for the SuperUser role</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<string> ListAllAdminRoles()
        {
            return this.Roles.Where(x => x.Name != "SuperUser").Select(r => r.Name).ToList(); ;
        }
        #endregion
    }
}
