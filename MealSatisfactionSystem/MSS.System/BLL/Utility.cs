using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Holds any business logic classes as well as classes that handle website calls to our entities.
/// </summary>
namespace MSSSystem.BLL
{
    /// <summary>
    /// Used to store utility methods that are used throughout the project.
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// Checks a string to see if it contains any invalid characters and throws an exception if it does.
        /// </summary>
        /// <param name="check">Contains the string to be validated.</param>
        public void checkValidString(string check)
        {
            List<string> InvalidStringList = new List<string>();

            //InvalidStringList.Add("!");
            InvalidStringList.Add("-");
            InvalidStringList.Add("_");
            //InvalidStringList.Add("?");
            InvalidStringList.Add("*");
            InvalidStringList.Add(";");
            InvalidStringList.Add(":");
            InvalidStringList.Add("<");
            InvalidStringList.Add(">");
            InvalidStringList.Add("/");
            InvalidStringList.Add("\\");
            InvalidStringList.Add("#");
            InvalidStringList.Add("@");
            InvalidStringList.Add("#");
            InvalidStringList.Add("$");
            InvalidStringList.Add("%");
            InvalidStringList.Add("^");
            InvalidStringList.Add("&");
            //InvalidStringList.Add("(");
            //InvalidStringList.Add(")");
            InvalidStringList.Add("{");
            InvalidStringList.Add("}");
            InvalidStringList.Add("[");
            InvalidStringList.Add("]");
            InvalidStringList.Add("|");
            InvalidStringList.Add("+");
            InvalidStringList.Add("=");
            InvalidStringList.Add("\'");
            InvalidStringList.Add("\"");
            if (InvalidStringList.Any(check.Contains))
            {
                throw new Exception("The following characters are not allowed: @ # $ % ^ & * _ - + = { } [ ] : ; \\ / | < > \" \' ");
            }
        }
    }
}
