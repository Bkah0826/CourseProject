using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region AdditionalNamespaces
using MSS.Data.Entities;
using MSS.Data.POCOs;
using MSSSystem.DAL;
#endregion
/// <summary>
/// MSSSystem.BLL holds any business logic classes as well as classes that handle website calls to our entities.
/// </summary>
namespace MSSSystem.BLL
{
/// <summary>
/// AnswerController allows the webpage to access the Answer entity.
/// </summary>
    [DataObject]
    public class AnswerController
    {
        /// <summary>
        /// Lists all  the answers associated to the supplied Question ID
        /// </summary>
        /// <param name="questionId">Contains the question's unique identifier</param>
        /// <returns>A list of answers associated to the question ID</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Answer> AnswerList(int questionId)
        {
            using (MSSContext context = new MSSContext())
            {
                var results =
                   (from n in context.Questions
                    where n.QuestionId == questionId
                    select n.Answers).FirstOrDefault();

                return results.ToList();
            }
        }

        /// <summary>
        /// Lists answers by question text
        /// </summary>
        /// <param name="questionText">Contains the question text of the question that is associated to the answers</param>
        /// <returns>A list of answers by question text</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<AnswerPOCO> AnswersByQuestion_List(string questionText)
        {
            using (MSSContext context = new MSSContext())
            {
                var answers = from x in context.Answers
                                where x.Questions.Select(y => y.QuestionText).FirstOrDefault() == questionText
                                select new AnswerPOCO
                                {
                                    description = x.Description,
                                    answerId = x.AnswerId
                                };

                return answers.ToList();
            }
        }

        /// <summary>
        /// Updates the description of the answer
        /// </summary>
        /// <param name="description">Contains the new answer description that replaces the existing answer description in the database</param>
        /// <param name="answerId">Contains the unique identifier of the answer to be updated</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void Answer_Update(string description, int answerId)
        {
            using (MSSContext context = new MSSContext())
            {
                var answer = (from x in context.Answers
                              where x.AnswerId == answerId
                              select x).FirstOrDefault();

                // If the user makes no changes, show error message.
                if (answer.Description == description)
                {
                    throw new Exception("No changes were made to the answer text before the \"Update\" button was clicked.");
                }
                //else, do the following actions:
                else
                {
                    //capture the new answer text / description that has been entered by the user
                    answer.Description = description;

                    //save the changes to the database
                    context.Entry(answer).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }
    }
}