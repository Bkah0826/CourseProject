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
    /// SecurityRoles class contains the default roles that will be entered into the database. It is purely a data storage class with no functionality and does not connect to the database in any way
    /// </summary>
    public static class SecurityRoles
    {
        /// <summary>
        /// SuperUser is the string that contains the name of the SuperUser role
        /// </summary>
        public const string SuperUser = "SuperUser";

        /// <summary>
        /// AdminViews is the string that contains the name of the AdminView role
        /// </summary>
        public const string AdminViews = "AdminView";

        /// <summary>
        /// AdminEdits is the string that contains the name of the AdminEdits role
        /// </summary>
        public const string AdminEdits = "AdminEdit";

        /// <summary>
        /// Returns all of the default roles in this class in a list
        /// </summary>
        public static List<string> DefaultSecurityRoles
        {
            get
            {
                List<string> value = new List<string>();
                value.Add(SuperUser);
                value.Add(AdminViews);
                value.Add(AdminEdits);
                return value;
            }
        }
    }
}
