using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// This namespace contains all of the entities for the security tables in our database
/// </summary>
namespace MSS.Data.Entities.Security
{
    /// <summary>
    /// RoleProfile is the code first entity for the role table. It has all the basic information that the role table requires in the database.
    /// </summary>
    public class RoleProfile
    {
        /// <summary>
        /// RoleId is a unique hashed text identifier for the tole
        /// </summary>
        public string RoleId { get; set; }
        /// <summary>
        /// RoleName is the name of the role in the database(eg. AdminView).
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// UserNames is a collection that contains the username(s) of the user(s) the role belongs to
        /// </summary>
        public IEnumerable<string> UserNames { get; set; }
    }
}
