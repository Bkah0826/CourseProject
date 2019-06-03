using MSS.Data.Entities;
using MSS.Data.POCOs;
using MSSSystem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

/// <summary>
/// Holds any business logic classes as well as classes that handle website calls to our entities.
/// </summary>
namespace MSSSystem.BLL
{
    /// <summary>
    /// Holds all business layer classes associated with the homepage.
    /// </summary>
    [DataObject]
    public class HomePageController
    {
        /// <summary>
        /// Queries and returns the site ID the current user is affiliated with.
        /// </summary>
        /// <param name="userId">The ID of the user logged in</param>
        /// <returns>The site ID</returns>
        public int getSite(string userId)
        {
            using (MSSContext context = new MSSContext())
            {
                var result = (from x in context.Users
                              where x.Id == userId
                              select x.SiteId).FirstOrDefault();
                return result;
            }
        }

        /// <summary>
        /// Queries and returns the site's daily passcode based on site which the user is part of.
        /// </summary>
        /// <param name="userId">The ID of the user logged in</param>
        /// <returns>The site's Passcode</returns>
        public string getPass(string userId)
        {
            using (MSSContext context = new MSSContext())
            {               
                try
                {
                    var result = (from x in context.Users
                                  where x.Id == userId
                                  select x.Site.Passcode).FirstOrDefault();
                    var r = result.ToCharArray();
                    r[0] = Char.ToUpper(r[0]);
                    return new string(r);
                }
                catch
                {
                    return "***";
                }
            }
        }
        
        /// <summary>
        /// Queries and returns all QuestionResponses and thier associated values that are dated within the current quarter.
        /// </summary>
        /// <param name="siteId">This is the site ID that the current user is affiliated with.</param>
        /// <returns>A list of QuestionResponses and thier relevatn associated data.</returns>
        public List<ChartingHomeResponse> grabData(int siteId)
        {
            using (var context = new MSSContext())
            {
                var result = from x in context.QuestionResponses                             
                             where (x.Response.Unit.SiteId == siteId && Math.Floor(1 + (((double)x.Response.Date.Month - 1) / 3)) == Math.Floor(1 + (double)(DateTime.Now.Month - 1) / 3) && DateTime.Now.Year == x.Response.Date.Year)

                             select new ChartingHomeResponse
                             {
                                 questions = x.Question.QuestionText,
                                 subQuestions = x.Question.SubQuestionText,
                                 answers = x.Answer.Description,
                                 answerID = x.AnswerId,
                                 value = x.Answer.Value,
                                 unitID = x.Response.UnitId,
                                 questionID = x.QuestionId,
                                 questionParam = x.Question.QuestionParameter,
                                 maxValue = x.Answer.MaxValue,
                                 colour = x.Answer.Colour,
                                 removed = x.Response.Unit.Disabled,
                                 unitName = x.Response.Unit.UnitName

                             };
                return result.ToList();

            }
        }

        /// <summary>
        /// Overloaded grabData() for filtering based on date.
        /// </summary>
        /// <param name="siteId">Id of site to query</param>
        /// <param name="fromDate">From date to query</param>
        /// <param name="toDate">To date to query</param>
        /// <returns></returns>
        public List<ChartingHomeResponse> grabData(int siteId, DateTime fromDate, DateTime toDate)
        {
            using (var context = new MSSContext())
            {
                var result = from x in context.QuestionResponses
                             where x.Response.Unit.SiteId == siteId
                                    && x.Response.Date >= fromDate 
                                    && x.Response.Date <= toDate                                    

                             select new ChartingHomeResponse
                             {
                                 questions = x.Question.QuestionText,
                                 subQuestions = x.Question.SubQuestionText,
                                 answers = x.Answer.Description,
                                 answerID = x.AnswerId,
                                 value = x.Answer.Value,
                                 unitID = x.Response.UnitId,
                                 questionID = x.QuestionId,
                                 questionParam = x.Question.QuestionParameter,
                                 maxValue = x.Answer.MaxValue,
                                 colour = x.Answer.Colour,
                                 removed = x.Response.Unit.Disabled,
                                 unitName = x.Response.Unit.UnitName

                             };
                return result.ToList();

            }
        }
    }
}
