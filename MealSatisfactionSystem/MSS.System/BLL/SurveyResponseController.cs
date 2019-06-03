using MSS.Data.Entities;
using MSS.Data.POCOs;
using MSSSystem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/// <summary>
/// Holds any business logic classes as well as classes that handle website calls to our entities.
/// </summary>
namespace MSSSystem.BLL
{
    /// <summary>
    /// Controls the "ViewResponses" webpage, including its access to the SurveyOverview and IndividualSurvey POCOs.
    /// </summary>
    [DataObject]
    public class SurveyResponseController
    {
        #region ResponseListViewMethods
        /// <summary>
        /// Searches for all the Responses that are associated with an active Site and Unit and stores them in a SurveyOverview list.
        /// </summary>
        /// <remarks>
        /// This method is used to populate the "ViewResponses" page before any filters have been selected by the user, if the user has a SuperUser role.
        /// </remarks>
        /// <returns>A list of SurveyOverview POCOs with an active Site and active Unit.</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SurveyOverview> Response_List_All()
        {
            using (MSSContext context = new MSSContext())
            {
                var results = (from survey in context.Responses
                               where !survey.Unit.Site.Disabled && !survey.Unit.Disabled
                               orderby survey.Date descending
                               select new SurveyOverview
                               {
                                   ResponseId = survey.ResponseId,
                                   SiteName = survey.Unit.Site.SiteName,
                                   UnitName = survey.Unit.UnitName,
                                   Age = (survey.Age != "Under 18" && survey.Age != "18-34" && survey.Age != "35-54" && survey.Age != "55-74" && survey.Age != "75+") ? "Not Provided" : survey.Age,
                                   Date = survey.Date,
                                   Gender = (survey.Gender == "M") ? "Male" : (survey.Gender == "F") ? "Female" : "Other / Not Provided",
                                   Comment = survey.Comment
                               });

                return results.ToList();
            }
        }

        /// <summary>
        /// Searches for all the Responses that match the filters inputted by the user and stores them in a SurveyOverview list.
        /// </summary>
        /// <remarks>
        /// This method is used to narrow down the displayed Responses based on inputted criteria.
        /// In addition, this method is used to populate the "ViewResponses" page before any filters have been selected by the user, if the user does not have a SuperUser role.
        /// </remarks>
        /// <param name="sites">Contains a list of SiteName strings.</param>
        /// <param name="units">Contains a list of UnitName strings.</param>
        /// <param name="gender">Contains a list of Gender strings. </param>
        /// <param name="startDate">Contains a DateTime? value of the text input in the FromDate control.</param>
        /// <param name="endDate">Contains a DateTime? value of the text input in the ToDate control.</param>
        /// <param name="age">Contains a list of Age strings.</param>
        /// <returns>A list of SurveyOverview POCOs that match filters.</returns>
        public List<SurveyOverview> Response_List_Filters(List<string> sites, List<string> units, List<string> gender, DateTime? startDate, DateTime? endDate, List<string> age)
        {
            using (var context = new MSSContext())
            {
                //Ensure that a valid date combination was entered. If not, flag the user's error.

                //Due to the difference between the DateTime? and List<> filters, the process has been broken into two parts:
                //1. Determine if the startDate and/or endDate were inputted and store the matching results into a list.
                //2. Take the list from #1 and cycle through the inputted lists of sites, units, genders, and age, to find all matching results and store these in a second list, to be returned.

                //Create new list, per #1 above. 
                var filteredList = new List<SurveyOverview>();

                //If no dates were entered, call the Response_List_All method.
                if ((startDate == null) && (endDate == null))
                {
                    filteredList = Response_List_All();
                }
                //If the startDate was entered but the endDate was not, query for the surveys that were submitted after the startDate.
                else if ((startDate != null) && (endDate == null))
                {
                    filteredList = (from survey in context.Responses
                                    where !survey.Unit.Site.Disabled && !survey.Unit.Disabled && survey.Date >= startDate
                                    orderby survey.Date descending
                                    select new SurveyOverview
                                    {
                                        ResponseId = survey.ResponseId,
                                        SiteName = survey.Unit.Site.SiteName,
                                        UnitName = survey.Unit.UnitName,
                                        Age = (survey.Age != "Under 18" && survey.Age != "18-34" && survey.Age != "35-54" && survey.Age != "55-74" && survey.Age != "75+") ? "Not Provided" : survey.Age,
                                        Date = survey.Date,
                                        Gender = (survey.Gender == "M") ? "Male" : (survey.Gender == "F") ? "Female" : "Other / Not Provided",
                                        Comment = survey.Comment
                                    }).ToList();
                }
                //If the startDate was not entered but the endDate was entered, query for the surveys that were submitted before the endDate.
                else if ((startDate == null) && (endDate != null))
                {
                    filteredList = (from survey in context.Responses
                                    where !survey.Unit.Site.Disabled && !survey.Unit.Disabled && survey.Date <= endDate
                                    orderby survey.Date descending
                                    select new SurveyOverview
                                    {
                                        ResponseId = survey.ResponseId,
                                        SiteName = survey.Unit.Site.SiteName,
                                        UnitName = survey.Unit.UnitName,
                                        Age = (survey.Age != "Under 18" && survey.Age != "18-34" && survey.Age != "35-54" && survey.Age != "55-74" && survey.Age != "75+") ? "Not Provided" : survey.Age,
                                        Date = survey.Date,
                                        Gender = (survey.Gender == "M") ? "Male" : (survey.Gender == "F") ? "Female" : "Other / Not Provided",
                                        Comment = survey.Comment
                                    }).ToList();
                }
                //If both dates were entered, query for the surveys that were submitted after the startDate and before the endDate.
                else
                {
                    filteredList = (from survey in context.Responses
                                    where !survey.Unit.Site.Disabled && !survey.Unit.Disabled && survey.Date >= startDate && survey.Date <= endDate
                                    orderby survey.Date descending
                                    select new SurveyOverview
                                    {
                                        ResponseId = survey.ResponseId,
                                        SiteName = survey.Unit.Site.SiteName,
                                        UnitName = survey.Unit.UnitName,
                                        Age = (survey.Age != "Under 18" && survey.Age != "18-34" && survey.Age != "35-54" && survey.Age != "55-74" && survey.Age != "75+") ? "Not Provided" : survey.Age,
                                        Date = survey.Date,
                                        Gender = (survey.Gender == "M") ? "Male" : (survey.Gender == "F") ? "Female" : "Other / Not Provided",
                                        Comment = survey.Comment
                                    }).ToList();
                }
                //Take the filteredList and filter it again using the inputted lists.
                var checkboxFilters = filteredList.Where(s => sites.Contains(s.SiteName) && units.Contains(s.UnitName) && gender.Contains(s.Gender) && age.Contains(s.Age));
                //Return the final filteredList.
                return checkboxFilters.ToList();
            }
        }
        #endregion

        #region FilterMethods
        /// <summary>
        /// Finds all active Sites and stores the active SiteName in a string list.
        /// </summary>
        /// <returns>A list of SiteName strings.</returns>
        public List<String> Active_Site_List()
        {
            using (MSSContext context = new MSSContext())
            {
                var results =
                   (from site in context.Sites
                    where !site.Disabled
                    select site.SiteName);
                return results.ToList();
            }
        }

        /// <summary>
        /// Finds all active Units and stores the active UnitNames in a string list.
        /// </summary>
        /// <returns>A list of UnitName strings.</returns>
        public List<String> Active_Unit_List()
        {
            using (MSSContext context = new MSSContext())
            {
                var results =
                   (from unit in context.Units
                    where !unit.Disabled
                    select unit.UnitName);
                return results.ToList();
            }
        }

        /// <summary>
        /// Finds a SiteName, based on the SiteId that is associated with the logged in user's account.
        /// </summary>
        /// <remarks>
        /// This method is used when the logged in user does not have a SuperUser role and, therefore, does not have access to the survey data from all sites.
        /// The user does not input the siteId for this method. Instead, the siteId is determined in the code behind, based on the user's account information.
        /// </remarks>
        /// <param name="siteId">Contains an integer equal to the SiteId of the logged in UserProfile.</param>
        /// <returns>A string of the Site's SiteName.</returns>
        public string Get_SiteName(int siteId)
        {
            using (MSSContext context = new MSSContext())
            {
                var result = (from site in context.Sites
                              where site.SiteId == siteId
                              select site.SiteName);
                return result.Single();
            }
        }

        /// <summary>
        /// Determines if a Site has been deactivated.
        /// </summary>
        /// <param name="siteId">Contains an integer equal to the SiteId of the logged in UserProfile.</param>
        /// <returns>A true/false value indicating whether the Site is disabled.</returns>
        public bool Get_SiteStatus(int siteId)
        {
            using (MSSContext context = new MSSContext())
            {
                var result = (from site in context.Sites
                              where site.SiteId == siteId
                              select site.Disabled);

                return result.Single();
            }
        }
        #endregion

        #region IndividualSurveyGenerationMethod
        /// <summary>
        /// Searches for the QuestionText of the Question, based on the inputted questionId.
        /// </summary>
        /// <param name="questionId">Contains an integer equal to the QuestionId of the requested Question.</param>
        /// <returns>A string with the Question's QuestionText.</returns>
        public String Get_Survey_Question(int questionId)
        {
            using (var context = new MSSContext())
            {
                var question = from survey in context.Questions
                               where survey.QuestionId == questionId
                               select survey.QuestionText;
                return question.Single();
            }
        }

        /// <summary>
        /// Searches for the SubQuestionText of the Question, based on the inputted questionId.
        /// </summary>
        /// <param name="questionId">Contains an integer equal to the QuestionId of the requested Question.</param>
        /// <returns>A string with the Question's SubQuestionText.</returns>
        public String Get_Survey_SubQuestion(int questionId)
        {
            using (var context = new MSSContext())
            {
                var question = from survey in context.Questions
                               where survey.QuestionId == questionId
                               select survey.SubQuestionText;
                return question.Single();
            }
        }

        /// <summary>
        /// Searches for the Answer Description of the Answer, based on the inputted responseId and questionId.
        /// </summary>
        /// <param name="responseId">Contains an integer equal to the ResponseId of the requested Resposne.</param>
        /// <param name="questionId">Contains an integer equal to the QuestionId of the requested Question.</param>
        /// <returns>A string with the Answer's Description.</returns>
        public String Get_Survey_Answer(int responseId, int questionId)
        {
            using (var context = new MSSContext())
            {
                var answer = from survey in context.QuestionResponses
                             where survey.ResponseId == responseId && survey.QuestionId == questionId
                             select survey.Answer.Description == "" ? "No Response" : survey.Answer.Description;
                return answer.Single();
            }
        }

        /// <summary>
        /// Searches for the Response and stores the result in a SurveyOverview instance.
        /// </summary>
        /// <param name="responseId">Contains an integer equal to the ResponseId of the selected Response.</param>
        /// <returns>A single SurveyOverview instance.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public SurveyOverview Response_List_Individual(int responseId)
        {
            using (var context = new MSSContext())
            {
                var response = from survey in context.Responses
                               where survey.ResponseId == responseId
                               select new SurveyOverview
                               {
                                   SiteName = survey.Unit.Site.SiteName,
                                   UnitName = survey.Unit.UnitName,
                                   Date = survey.Date,
                                   Age = (survey.Age != "Under 18" && survey.Age != "18-34" && survey.Age != "35-54" && survey.Age != "55-74" && survey.Age != "75+") ? "Not Provided" : survey.Age,
                                   Gender = (survey.Gender == "M") ? "Male" : (survey.Gender == "F") ? "Female" : "Other / Not Provided",
                                   Comment = (survey.Comment == "") ? "No Response" : (survey.Comment == null) ? "No Response" : survey.Comment
                               };
                return response.Single();
            }
        }
        #endregion
    }
}