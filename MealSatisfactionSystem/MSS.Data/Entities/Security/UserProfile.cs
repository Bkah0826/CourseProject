/// <summary>
/// This namespace contains all of the entities for the security tables in our database
/// </summary>
namespace MSS.Data.Entities.Security
{
    using System.Collections.Generic;

    /// <summary>
    /// UserProfile is a custom class that we use to store user information when we are attempting to add change or archive a user in the user manager
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// UserId is the user's id is the same as the Id of the Id of the user in the aspNetUsers table in the database
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// UserName is the user's username that this user is going to use to log into our system
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// SiteId is the Id of the site that the user belongs to (works in). Unless the user has a SuperUser role
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// FirstName is the user's First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// LastName is the user's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Active is a boolean which indicates whether the user is active or not (inactive users can no longer log in and do not have access to the system)
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// EmailConfirmation is the field contains information about whether the user has confirmed their email
        /// </summary>
        public bool EmailConfirmation { get; set; }

        /// <summary>
        /// RequestedPassword is the user's requested password (this will most likely end up as their password in the database although it is only the requested password)
        /// </summary>
        public string RequestedPassword { get; set; }

        /// <summary>
        /// RoleMemberships is a collection of the roles that the user has ()
        /// </summary>
        public IEnumerable<string> RoleMemberships { get; set; }

        /// <summary>
        /// Site is the description of the site that the user works in
        /// </summary>
        public string Site { get; set; }
    }
}
