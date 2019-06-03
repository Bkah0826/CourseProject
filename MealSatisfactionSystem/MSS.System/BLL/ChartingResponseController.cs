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
using System.Linq.Dynamic;
using System.Data.Entity;


namespace MSSSystem.BLL
{
    /// <summary>
    /// Controls the request and return of the charting data
    /// </summary>
    [DataObject]
    public class ChartingResponseController
    {
        /// <summary>
        /// For getting data from the webservice.
        /// Uses dynamic Linq to create query strings. Allows for custom building.        
        /// </summary>
        /// <param name="Parameter">Contains a single Question Parameter to filter by</param>
        /// <param name="FromDate">Contains the start date to filter by</param>
        /// <param name="ToDate">Contains the date to filter to</param>
        /// <param name="Units">Contains an array of units to filter by</param>
        /// <param name="Genders">Contains an array of genders to filter by</param>
        /// <param name="Ages">Contains an array of ages(ints) to filter by</param>
        /// <param name="Site">Contains the SiteId of the user generating the reports</param>
        /// <returns>ChartingResponse with gender,age,and data(value, desc, date)</returns>
        public ChartingResponse GetChartingResponseData(string Parameter, DateTime FromDate, DateTime ToDate, List<string> Units, List<string> Genders, List<string> Ages, int Site)
        {
            // Init as 1==1 so if that clause isn't used it has no effect on the query
            string paramClauseList = "1==1";
            string unitsClauseList = "1==1";
            string genderClauseList = "1==1";
            string ageClauseList = "1==1";
            string toDateClauseList = "1==1";
            string fromDateClauseList = "1==1";
            string siteClauseList = "1==1";

            string paramClause;
            paramClause = $"Response.Unit.SiteId == {Site}";
            siteClauseList = paramClause;

            if (Parameter != null) //Will always be present
            {
                paramClause = $"Question.QuestionParameter == \"{Parameter}\"";
                paramClauseList = paramClause;
            }

            if (Units != null) // No query needed for all units
            {
                List<string> unitsList = new List<string>();
                foreach (string unit in Units)
                {
                    if (unit == "All Units")
                        break;
                    unitsList.Add($"Response.Unit.UnitName == \"{unit}\"");
                }
                if (unitsList.Count() > 0)
                    unitsClauseList = (unitsList.Aggregate((current, next) => current + " OR " + next)); //Builds the query strings for the dynamic Linq
            }

            if (Ages != null) //recieved as a number value
            {
                List<string> ageList = new List<string>();
                foreach (string age in Ages)
                {
                    if (age == "-1")//Not specified
                        break;
                    string ageWord = "";
                    switch (age)
                    {
                        case "1":
                            ageWord = "Under 18";
                            break;
                        case "2":
                            ageWord = "18-34";
                            break;
                        case "3":
                            ageWord = "35-54";
                            break;
                        case "4":
                            ageWord = "55-74";
                            break;
                        case "5":
                            ageWord = "75+";
                            break;
                    }
                    ageList.Add($"Response.Age == \"{ageWord}\"");
                }
                if (ageList.Count() > 0)
                    ageClauseList = (ageList.Aggregate((current, next) => current + " OR " + next));
            }

            if (Genders != null)
            {
                List<string> genderList = new List<string>();
                foreach (string gender in Genders)
                {
                    if (gender == "-1")//Not specified
                        break;
                    genderList.Add($"Response.Gender == \"{gender}\"");
                }
                if (genderList.Count() > 0)
                    genderClauseList = (genderList.Aggregate((current, next) => current + " OR " + next));
            }

            if (ToDate != null & ToDate.Year != 1)
            {
                toDateClauseList = ($"Response.Date <= DateTime({ToDate.Year},{ToDate.Month},{ToDate.Day})");
            }

            if (FromDate != null & FromDate.Year != 1)
            {
                fromDateClauseList = ($"Response.Date >= DateTime({FromDate.Year},{FromDate.Month},{FromDate.Day})");
            }

            using (MSSContext context = new MSSContext())
            {
                //Have to specify each Where() clause seperatly or else the &/|| gets mixed up in the complicated filterings.
                IQueryable<DateData> result = context.QuestionResponses
                                        .Include(a => a.Answer)
                                        .Include(q => q.Question)
                                        .Include(r => r.Response)
                                        .Include(u => u.Response.Unit)
                                        .Where("Response.Unit.Disabled == false")
                                        .Where(paramClauseList)
                                        .Where(ageClauseList)
                                        .Where(unitsClauseList)
                                        .Where(genderClauseList)
                                        .Where(toDateClauseList)
                                        .Where(fromDateClauseList)
                                        .Where(siteClauseList)
                                        .Select(qr => new DateData { Date = qr.Response.Date, Data = qr.Answer.Value, AnswerText = qr.Answer.Description });

                Question questionObj = context.Questions.Where(q => q.QuestionParameter == Parameter).FirstOrDefault();

                ChartingResponse response = new ChartingResponse
                {
                    Question = questionObj.QuestionText,
                    Subtext = questionObj.SubQuestionText,
                    Parameter = Parameter,
                    Gender = Genders,
                    Age = Ages,
                    Data = result.ToList()
                };

                return response;
            }
        }
    }
}
