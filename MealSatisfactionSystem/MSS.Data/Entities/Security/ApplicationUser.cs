using Microsoft.AspNet.Identity.EntityFramework;
using MSS.Data.Entities;
using System;
/// <summary>
/// This namespace contains all of the entities for the security tables in our database
/// </summary>
namespace MSS.Data.Entities.Security
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary> 
    /// ApplicationUser is the code first entity for the user table. It has all the basic information that the user table requires in the database.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// SiteId is the unique Id of the site that the user belongs to (works in). Unless the user has a SuperUser role
        /// </summary>
        public int SiteId { get; set; }
        /// <summary>
        /// FirstName is the user's First name
        /// </summary>
        [StringLength(35, ErrorMessage = "Please enter a first name with 35 or less characters")]
        public string FirstName { get; set; }
        /// <summary>
        /// LastName is the user's last name
        /// </summary>
        [StringLength(40, ErrorMessage = "Please enter a last name with 40 or less characters")]
        public string LastName { get; set; }
        /// <summary>
        /// Active is a boolean which indicates whether the user is active or not (inactive users can no longer log in and do not have access to the system)
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        ///Site is a navigation property used to look at the attributes of the site that the user is connected to
        /// </summary>
        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }
    }
}
