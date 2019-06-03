using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSS.Data.POCOs;
using MSSSystem.DAL;

namespace MSSSystem.BLL
{
    /// <summary>
    /// Handles the return of lookup table values
    /// </summary>
    public class LookupController
    {
        /// <summary>
        /// Based on the passed in type, gets the required information and stores
        /// in LookupValues object
        /// </summary>
        /// <param name="type">Contains the type of lookup table needed</param>
        /// <returns>List of LookupValues, or an empty list</returns>
        public List<LookupValues> getValues(string type)
        {
            using (MSSContext context = new MSSContext())
            {
                List<LookupValues> result;

                if (type == "lookupSites")
                {
                    result = (from site in context.Sites
                              select new LookupValues
                              {
                                  Id = site.SiteId,
                                  Description = site.SiteName,
                                  Type = type
                              }).ToList();
                    return result;
                }

                if (type == "lookupQuestions")
                {
                    result = (from q in context.Questions
                              select new LookupValues
                              {
                                  Id = q.QuestionId,
                                  Description = q.QuestionParameter,
                                  Type = type,
                                  Value = q.Colour
                              }).ToList();
                    return result;
                }
                if (type == "lookupAnswers")
                {
                    result = (from a in context.Answers
                              select new LookupValues
                              {
                                  Id = a.AnswerId,
                                  Description = a.Description,
                                  Type = type,
                                  Value = a.Colour
                              }).ToList();
                    return result;
                }
                else
                {
                    return new List<LookupValues>();
                }
            }
        }
    }
}

