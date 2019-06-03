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
/// <summary>
/// MSSSystem.BLL holds any business logic classes as well as classes that handle website calls to our entities.
/// </summary>
namespace MSSSystem.BLL
{
    /// <summary>
    /// QuestionResponseController allows the webpage to access the QuestionResponse entity.
    /// </summary>
    [DataObject]
    public class QuestionResponseController
    {
        /// <summary>
        /// Method for adding question response to database
        /// </summary>
        /// <param name="responseId">Contians the response unique identifier</param>
        /// <param name="questionId">Contains the question unique identifier</param>
        /// <param name="answerId">Contains the answer unique identifier</param>
        public void Add_QuestionResponse(int responseId, int questionId, int answerId)
        {
            using (MSSContext context = new MSSContext())
            {
                QuestionResponse NewQuestionResponse = new QuestionResponse();
                NewQuestionResponse.QuestionId = questionId;
                NewQuestionResponse.ResponseId = responseId;
                NewQuestionResponse.AnswerId = answerId;

                context.QuestionResponses.Add(NewQuestionResponse);
                context.SaveChanges();
            }
        }
    }
}