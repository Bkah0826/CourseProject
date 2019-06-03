using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region AdditionalNamespaces
using System.ComponentModel;
using MSS.Data.Entities;
using MSSSystem.DAL;
#endregion

namespace MSSSystem.BLL
{
    /// <summary>
    /// Allows the website to access the response entity in the database
    /// </summary>
    [DataObject]
    public class ResponseController
    {
        /// <summary>
        /// Adds a new instance of a response in the database
        /// </summary>
        /// <param name="response">Contains the response item that is being added to the database. Item obtained from Survey</param>
        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public void Add_NewResponse(Response response)
        {
            using (var context = new MSSContext())
            {
                context.Responses.Add(response);
                context.SaveChanges();
            }
        }
        /// <summary>
        /// Finds the newest ResponseId
        /// </summary>
        /// <returns>A integer representing the newest ResponseId</returns>
        public int Get_NewestResponseID()
        {
            using (var context = new MSSContext())
            {
                var results = (from x in context.Responses
                               select x.ResponseId).Max();
                return results;
            }
        }
    }
}
