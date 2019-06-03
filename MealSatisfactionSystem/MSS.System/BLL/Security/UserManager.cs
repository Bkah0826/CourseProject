using MSS.Data.Entities;
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

/// <summary>
/// BLL security contains all of the security controllers for the security tables in our database
/// </summary>
namespace MSSSystem.BLL.Security
{
    /// <summary>
    /// User manager class is the class that is responsible for methods involving the user such as inserting a user in the database, updating a user and deactivating a user
    /// </summary>
    [DataObject]
    public class UserManager : UserManager<ApplicationUser>
    {
        #region Constants
        /// <summary>
        /// STR_DEFAULT_PASSWORD is the default password for the webmaster
        /// </summary>
        private const string STR_DEFAULT_PASSWORD = "Password1";
        /// <summary>STR_USERNAME_FORMAT is the format of the username for a given user</summary>
        private const string STR_USERNAME_FORMAT = "{0}{1}";
        /// <summary>
        /// STR_WEBMASTER_USERNAME is the default webmaster username and it cannot be changed on our website
        /// </summary>
        private const string STR_WEBMASTER_USERNAME = "Webmaster";
        #endregion

        /// <summary>
        /// Gets called when the class is constructed
        /// </summary>
        public UserManager()
            : base(new UserStore<ApplicationUser>(new MSSContext()))
        {

        }

        /// <summary>
        /// Adds a default user with Superuser privileges to the database
        /// </summary>
        public void AddWebMaster()
        {
            // Checks to see if a user with the same username exists
            if (!Users.Any(u => u.UserName.Equals(STR_WEBMASTER_USERNAME)))
            {
                using (MSSContext context = new MSSContext())
                {
                    // create a new instance of an AspNetUser that will be used as the data to
                    //   add a new record to the AspNetUsers table
                    // dynamically fill two attributes of the instance while the user is being constructed
                    var webmasterAccount = new ApplicationUser()
                    {
                        UserName = STR_WEBMASTER_USERNAME,
                        // Gets the site Id of the TST site and assigns the webmaster to it
                        SiteId = context.Sites.Where(x => x.SiteName == "Administrator Site").FirstOrDefault().SiteId,
                        FirstName = "Web",
                        LastName = "Master",
                        Active = true
                    };

                    // place the webmaster account on the AspNetUsers table
                    this.Create(webmasterAccount, STR_DEFAULT_PASSWORD);

                    // Assign the webmaster account to the Superuser role
                    this.AddToRole(webmasterAccount.Id, SecurityRoles.SuperUser);
                }
            }
        }

        public void AddTestUsers()
        {
            var userOne = new { username = "MisUser", firstname = "Mis", lastname = "user", siteid = 2 };
            var userTwo = new { username = "GNUser", firstname = "GreyNun", lastname = "user", siteid = 3 };
            var userList = new[] { userOne, userTwo };
            // Checks to see if a user with the same username exists
            foreach(var i in userList)
            {
                if (!Users.Any(u => u.UserName.Equals(i.username)))
                {
                    using (MSSContext context = new MSSContext())
                    {
                        // create a new instance of an AspNetUser that will be used as the data to
                        //   add a new record to the AspNetUsers table                        
                        var userAccount = new ApplicationUser()
                        {
                            UserName = i.username,                            
                            SiteId = i.siteid,
                            FirstName = i.firstname,
                            LastName = i.lastname,
                            Active = true
                        };

                        // place the  account on the AspNetUsers table
                        this.Create(userAccount, "hunter1");

                        // Assign the account to a role
                        if (i.username == "MisUser")
                            this.AddToRole(userAccount.Id, SecurityRoles.AdminEdits);
                        else
                            this.AddToRole(userAccount.Id, SecurityRoles.AdminViews);
                    }
                }
            }            
        }

        /// <summary>
        /// Queries the database for usernames that contains the provided username and then adds
        /// the number of usernames found to the end of the supplied username making the username unique
        /// </summary>
        /// <param name="suggestedUserName">The username to be verified</param>
        /// <returns>Returns a unique username</returns>
        public string VerifyNewUserName(string suggestedUserName)
        {
            // get a list of all current usernames (customers and employees)
            //  that start with the suggestusername
            // list of strings will be in memory
            var allUserNames = from x in Users.ToList()
                               where x.UserName.StartsWith(suggestedUserName)
                               orderby x.UserName
                               select x.UserName;

            // set the verified username to be equals to the suggested username
            var verifiedUserName = suggestedUserName;

            // this for loop counts the number of names in the "allUserNames" variable that are the same as the suggested user name and the adds that number at the end of 
            // suggested user name and puts the result in the verified username
            for (int i = 1; allUserNames.Any(x => x.Equals(verifiedUserName,
                         StringComparison.OrdinalIgnoreCase)); i++)
            {
                verifiedUserName = suggestedUserName + i.ToString();
            }

            // return the finalized new verified user name
            return verifiedUserName;
        }

        #region UserRole Adminstration
        /// <summary>
        /// Returns a list of all of the users in our system
        /// </summary>
        /// <returns>A list of userprofile classes</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<UserProfile> ListAllUsers()
        {
            using (MSSContext context = new MSSContext())
            {
                var rm = new RoleManager();
                var tempresults = from person in Users.ToList()
                                  select new UserProfile
                                  {
                                      UserId = person.Id,
                                      UserName = person.UserName,
                                      FirstName = person.FirstName,
                                      LastName = person.LastName,
                                      SiteId = person.SiteId,
                                      Site = context.Sites.Find(person.SiteId).SiteName,
                                      EmailConfirmation = person.EmailConfirmed,
                                      RoleMemberships = person.Roles.Select(r => rm.FindById(r.RoleId).Name),
                                      Active = person.Active
                                  };
                return tempresults.ToList();
            }            
        }

        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="userinfo">The user to be added to the database</param>
        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public void AddUser(UserProfile userinfo)
        {
            using (var context = new MSSContext())
            {
                // Checks if the admin has enetered a site that exists in our database for the new user
                if (string.IsNullOrEmpty(userinfo.SiteId.ToString()) || context.Sites.Find(userinfo.SiteId) == null)
                {
                    // If the site is invalid it gives the admin an error message
                    throw new Exception("Site ID is missing or invalid please enter a valid Site initialism");
                }
                else
                {
                    // If the site is valid then a new user gets created
                    var userAccount = new ApplicationUser()
                    {
                        UserName = userinfo.UserName,
                        SiteId = userinfo.SiteId,
                        FirstName = userinfo.FirstName,
                        LastName = userinfo.LastName,
                        Active = userinfo.Active
                    };
                    // This code adds the new user to the database with the requested password
                    IdentityResult result = this.Create(userAccount,
                        string.IsNullOrEmpty(userinfo.RequestedPassword) ? STR_DEFAULT_PASSWORD
                        : userinfo.RequestedPassword);
                    // Checks to see if the insert failed
                    if (!result.Succeeded)
                    {
                        // Verifies the username
                        userAccount.UserName = VerifyNewUserName(userinfo.UserName);
                        // Attempts to create a user with a new username but the same password
                        result = this.Create(userAccount, userinfo.RequestedPassword);

                    }
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            throw new Exception(error);
                        }
                    }
                    // Goes through all of the roles provided by the admin
                    foreach (var roleName in userinfo.RoleMemberships)
                    {
                        // adds the user to the roles that the admin selected
                        this.AddToRole(userAccount.Id, roleName);
                        AddUserToRole(userAccount, roleName);
                    }
                }
            }

        }

        /// <summary>
        /// Adds the user to the role with the name provided
        /// </summary>
        /// <param name="userAccount">The applicationUser class with information about the user</param>
        /// <param name="roleName">The role name the user should be added to</param>
        public void AddUserToRole(ApplicationUser userAccount, string roleName)
        {
            this.AddToRole(userAccount.Id, roleName);
        }

        /// <summary>
        /// Updates the user with the same Id as the Id in the userProfile passed into the method
        /// </summary>
        /// <param name="userinfo">The information about which user should be updated as well as the new information for the updated user</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void UpdateUser(UserProfile userinfo)
        {
            ApplicationUser user = new ApplicationUser();
            using (MSSContext context = new MSSContext())
            {
                user = context.Users.Where(x => x.UserName == userinfo.UserName).FirstOrDefault();
                // Hashes the password and adds it to the Password hash field in the database
                if (!string.IsNullOrEmpty(userinfo.RequestedPassword))
                {
                    user.PasswordHash = PasswordHasher.HashPassword(userinfo.RequestedPassword);
                }
                user.FirstName = userinfo.FirstName;
                user.LastName = userinfo.LastName;
                user.Site = context.Sites.Find(userinfo.SiteId);
                user.Active = userinfo.Active;
                context.Entry(user).State = System.Data.Entity.EntityState.Modified;
                if (userinfo.UserName != "Webmaster")
                {
                    user.Roles.Clear();
                }
                context.SaveChanges();
            }
            if (userinfo.UserName != "Webmaster")
            {
                foreach (var role in userinfo.RoleMemberships)
                {
                    this.AddToRole(user.Id, role);
                }
            }
        }


        /// <summary>
        /// Gets the user's site Id based on the user's username
        /// </summary>
        /// <param name="userName">The user's username</param>
        /// <returns>The user's siteId</returns>
        public int GetUserSiteId(string userName)
        {
            var users = ListAllUsers();
            var user = users.Where(u => u.UserName == userName).Select(u => u).FirstOrDefault();

            int siteId = 0;

            if (user != null)
            {
                using (MSSContext context = new MSSContext())
                {
                    siteId = (from x in context.Users
                              where x.UserName == userName
                              select x.SiteId).FirstOrDefault();
                }
            }
            else
            {
                throw new Exception("No User identified, please ensure you are logged in.");
            }

            return siteId;
        }

        /// <summary>
        /// Looks up all users and filters by the search parameters provided
        /// </summary>
        /// <param name="partialName">Partial username, firstname or last name of the users that will be displayed</param>
        /// <param name="sites">The site(s) of the users that will be displayed</param>
        /// <param name="roleName">The role(s) of the users that will be displayed</param>
        /// <param name="status">The status of the users that will be displayed. 1 is active, 2 is inactive 3 is either active or inactive users.</param>
        /// <returns>List of users stored in a list of UserProfile</returns>
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<UserProfile> ListUser_BySearchParams(string partialName, List<int> sites, List<string> roleName, int status)
        {
            using (var context = new MSSContext())
            {
                string FName = "";
                string LName = "";
                if (partialName.Contains(" "))
                {
                    FName = partialName.Split(' ')[0];
                    LName = partialName.Split(' ')[1];
                }
                var rm = new RoleManager();
                var tempresults = from x in Users.ToList()
                                  where (x.UserName.ToLower().Contains(partialName.ToLower()) || x.FirstName.ToLower().Contains(partialName.ToLower()) ||
                                  x.LastName.ToLower().Contains(partialName.ToLower()) ||
                                  (x.FirstName.ToLower().Contains(FName.ToLower()) && x.LastName.ToLower().Contains(LName.ToLower()) && partialName.Contains(" "))) &&
                                  (sites.Count == 0 || sites.Contains(x.SiteId)) &&
                                  (roleName.Count == 0 || roleName.Contains(context.Roles.Find(x.Roles.FirstOrDefault().RoleId).Name)) &&
                                  (status == 3 || (status == 1 && x.Active) || (status == 2 && !x.Active))
                                  orderby x.FirstName, x.LastName, x.SiteId, x.UserName
                                  select new UserProfile
                                  {
                                      UserId = x.Id,
                                      UserName = x.UserName,
                                      FirstName = x.FirstName,
                                      LastName = x.LastName,
                                      SiteId = x.SiteId,
                                      Site = context.Sites.Find(x.SiteId).SiteName,
                                      EmailConfirmation = x.EmailConfirmed,
                                      RoleMemberships = x.Roles.Select(r => rm.FindById(r.RoleId).Name),
                                      Active = x.Active
                                  };
                var returnList = tempresults.ToList();
                return returnList;
            }
        }

        /// <summary>
        /// Gets the role of the user with the provided username
        /// </summary>
        /// <param name="username">Username of the user whose role to look for</param>
        /// <returns></returns>
        public string getUserRole(string username)
        {
            using (MSSContext context = new MSSContext())
            {
                var user = context.Users.Where(u => u.UserName == username).Select(u => u).FirstOrDefault();
                return context.Roles.Find(user.Roles.FirstOrDefault().RoleId).Name;
            }
        }
        #endregion
    }
}