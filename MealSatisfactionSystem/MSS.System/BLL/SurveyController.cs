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
    /// Allows the webpage to access the Response entity.
    /// </summary>
    [DataObject]
    class SurveyController
    {
        /// <summary>
        /// Response_List_All() will search for all responses. This method is used to populate the ViewResponses page before filters have been selected.
        /// </summary>
        /// <returns>List of all responses</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Response> Response_List_All()
        {
            using (MSSContext context = new MSSContext())
            {
                var results = (from x in context.Responses
                               select x);

                return results.ToList();
            }
        }
    }
}
