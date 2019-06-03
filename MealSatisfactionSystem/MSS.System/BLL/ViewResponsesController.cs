using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region AdditionalNamespaces
using System.ComponentModel;
using MSS.Data.Entities;
using MSS.Data.POCOs;
using MSSSystem.DAL;
#endregion

namespace MSSSystem.BLL
{
    /// <summary>
    /// Allows the webpage to access the Response entity.
    /// </summary>
    [DataObject]
    public class ViewResponsesController
    {
        /// <summary>
        /// Response_List_All() will search for all responses. This method is used to populate the ViewResponses page before filters have been selected.
        /// </summary>
        /// <returns>List of all responses</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SurveyResponses> Response_List_All()
        {
            using (MSSContext context = new MSSContext())
            {
                var results = (from x in context.Responses
                               select new SurveyResponses
                               {
                                   ResponseId = x.ResponseId,
                                   SiteName = x.Unit.Site.Description,
                                   UnitName = x.Unit.UnitName,
                                   Overall = (from questionresponse in x.QuestionResponses
                                              where questionresponse.QuestionId == 8
                                              select questionresponse.Answer.Description).FirstOrDefault(),
                                   Age = x.Age,
                                   Date = x.Date,
                                   Gender = (x.Gender == "M") ? "Male" : (x.Gender == "F") ? "Female" : "Not Provided",
                                   Comment = x.Comment
                               });

                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<OneSurveyResponse> Response_List_Individual(int responseId)
        {
            using (var context = new MSSContext())
            {
                var response = (from x in context.QuestionResponses
                                where x.ResponseId == responseId
                                select new OneSurveyResponse
                                {
                                    Question = x.Question.QuestionText,
                                    SubQuestion = x.Question.SubQuestionText,
                                    Answer = x.Answer.Description
                                });

                return response.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SurveyResponses> Response_List_Filters(int siteId, int unitId, List<string> gender)
        {
            if (siteId == 0)
            {
               return Response_List_All();
            } else if (unitId == 0)
            {
                using (var context = new MSSContext())
                {
                    var filteredList = (from x in context.Responses
                                        where x.Unit.SiteId == siteId && x.Gender.Equals(gender) 
                                        select new SurveyResponses
                                        {
                                            ResponseId = x.ResponseId,
                                            SiteName = x.Unit.Site.Description,
                                            UnitName = x.Unit.UnitName,
                                            Overall = (from questionresponse in x.QuestionResponses
                                                       where questionresponse.QuestionId == 8 
                                                       select questionresponse.Answer.Description).FirstOrDefault(),
                                            Age = x.Age,
                                            Date = x.Date,
                                            Gender = (x.Gender == "M") ? "Male" : (x.Gender == "F") ? "Female" : "Not Provided",
                                            Comment = x.Comment
                                        });
                    return filteredList.ToList();
                }
            } else
            {
                using (var context = new MSSContext())
                {
                    var filteredList = (from x in context.Responses
                                        where x.Unit.SiteId == siteId && x.UnitId == unitId && x.Gender.Equals(gender)
                                        select new SurveyResponses
                                        {
                                            ResponseId = x.ResponseId,
                                            SiteName = x.Unit.Site.Description,
                                            UnitName = x.Unit.UnitName,
                                            Overall = (from questionresponse in x.QuestionResponses
                                                       where questionresponse.QuestionId == 8
                                                       select questionresponse.Answer.Description).FirstOrDefault(),
                                            Age = x.Age,
                                            Date = x.Date,
                                            Gender = (x.Gender == "M") ? "Male" : (x.Gender == "F") ? "Female" : "Not Provided",
                                            Comment = x.Comment
                                        });

                    var filteredGender = filteredList.Where(s => gender.Contains(s.Gender)).ToList();

                    return filteredGender.ToList();
           
                }
            }
            //stringList.Where(s => s.Contains("Tutorials"));

            //using (ServiceContext svcContext = new ServiceContext(_serviceProxy))
            //{
            //    var methodResults = svcContext.ContactSet
            //     .Where(a => a.LastName == "Smith");
            //    var methodResults2 = svcContext.ContactSet
            //     .Where(a => a.LastName.StartsWith("Smi"));
            //    Console.WriteLine();
            //    Console.WriteLine("Method query using Lambda expression");
            //    Console.WriteLine("---------------------------------------");
            //    foreach (var a in methodResults)
            //    {
            //        Console.WriteLine("Name: " + a.FirstName + " " + a.LastName);
            //    }
            //    Console.WriteLine("---------------------------------------");
            //    Console.WriteLine("Method query 2 using Lambda expression");
            //    Console.WriteLine("---------------------------------------");
            //    foreach (var a in methodResults2)
            //    {
            //        Console.WriteLine("Name: " + a.Attributes["firstname"] +
            //         " " + a.Attributes["lastname"]);
            //    }
            //}
        }

        public OneSurveyResponse Response_Get_One_Answer(int responseId, int questionId)
        {
            using (var context = new MSSContext())
            {
                var answer = (from x in context.QuestionResponses
                              where x.ResponseId == responseId && x.QuestionId == questionId
                              select new OneSurveyResponse
                              {
                                  Answer = x.Answer.Description
                              });
                return answer.Single();
            }
        }
    }

}

