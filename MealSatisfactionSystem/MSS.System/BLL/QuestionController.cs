using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region AdditionalNamespaces
using MSS.Data.DTOs;
using MSS.Data.Entities;
using MSS.Data.POCOs;
using MSSSystem.DAL;
#endregion

/// <summary>
/// Holds any business logic classes and classes that handle website calls to our entities.
/// </summary>
namespace MSSSystem.BLL
{
    /// <summary>
    /// Allows the webpage to access the Question entity.
    /// </summary>
    [DataObject]
    public class QuestionController
    {
        /// <summary>
        /// Lists all questions
        /// </summary>
        /// <returns>A list of all questions</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Question> GetQuestionParameterList()
        {
            using (MSSContext context = new MSSContext())
            {
                return context.Questions.ToList();
            }
        }

        /// <summary>
        /// Counts number of questions
        /// </summary>
        /// <returns>An integer representing the total number of questions</returns>
        public int Question_Count()
        {
            using (MSSContext context = new MSSContext())
            {
                var questionList = from x in context.Questions select x;
                return questionList.Count(); 
            }
        }

        /// <summary>
        /// Gets the QuestionText and SubQuestionText of a question with the supplied Question ID
        /// </summary>
        /// <param name="questionId">Contains the Question ID for the question text</param>
        /// <returns>The QuestionText and SubQuestionText of the supplied Question ID</returns>
        public QuestionSubQuestion GetQuestionText(int questionId)
        {
            using (MSSContext context = new MSSContext())
            {
                var question = (from x in context.Questions
                               where x.QuestionId == questionId
                               select new QuestionSubQuestion
                               {
                                   id = x.QuestionId,
                                   QuestionText = x.QuestionText,
                                   SubquestionText = x.SubQuestionText
                               }).FirstOrDefault();
                return question;

            }
        }

        /// <summary>
        /// Lists questions by distinct question text
        /// </summary>
        /// <returns>A list of distinct question text</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<QuestionDTO> Questions_List()
        {
            using (MSSContext context = new MSSContext())
            {
                var questions = from x in context.Questions
                                group x by x.QuestionText into g
                                orderby g.Select(q => q).FirstOrDefault().QuestionId
                                select new QuestionDTO
                                {
                                    questionText = g.Key,
                                    questionIds = from y in context.Questions
                                                  where g.Key == y.QuestionText
                                                  select new QuestionPOCO
                                                  {
                                                      questionId = y.QuestionId
                                                  }
                                };

                return questions.ToList();
            }
        }

        /// <summary>
        /// Lists only questions in the survey that have subquestions
        /// </summary>
        /// <returns>A list of questions with subquestions</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<QuestionDTO> QuestionsWithSubQuestions_List()
        {
            using (MSSContext context = new MSSContext())
            {
                var questions = from x in context.Questions
                                where x.SubQuestionText != ""
                                group x by x.QuestionText into g
                                orderby g.Select(q => q).FirstOrDefault().QuestionId
                                select new QuestionDTO
                                {
                                    questionText = g.Key,
                                    questionIds = from y in context.Questions
                                                  where g.Key == y.QuestionText
                                                  select new QuestionPOCO
                                                  {
                                                      questionId = y.QuestionId
                                                  }
                                };

                return questions.ToList();
            }
        }

        /// <summary>
        /// Lists only questions in the survey that have associated answers, excluding questions that prompt respondents for comments
        /// </summary>
        /// <returns>A list of questions that have associated answers</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<QuestionDTO> QuestionsWithAnswers_List()
        {
            using (MSSContext context = new MSSContext())
            {
                var questions = from x in context.Questions
                                where x.Answers.Select(a => a.Description).FirstOrDefault() != ""
                                group x by x.QuestionText into g
                                orderby g.Select(q => q).FirstOrDefault().QuestionId
                                select new QuestionDTO
                                {
                                    questionText = g.Key,
                                    questionIds = from y in context.Questions
                                                  where g.Key == y.QuestionText
                                                  select new QuestionPOCO
                                                  {
                                                      questionId = y.QuestionId
                                                  }
                                };

                return questions.ToList();
            }
        }

        /// <summary>
        /// Updates the question in the database
        /// </summary>
        /// <param name="questionText">Contains the new question text that replaces the existing question text in the database</param>
        /// <param name="questionId">Contains the ID of the question to be updated</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void Question_Update(string questionText, int questionId)
        {
            using (MSSContext context = new MSSContext())
            {
                // Grabs the current site object the user wishes to update.
                var question = (from x in context.Questions
                            where x.QuestionId == questionId
                            select x).FirstOrDefault();

                // If the user makes no changes, show error message.
                if (question.QuestionText == questionText)
                {
                    throw new Exception("No changes were made to the question text before the \"Update\" button was clicked.");
                }
                //else, do the following actions:
                else
                {
                    //capture the new subquestion text that has been entered by the user
                    question.QuestionText = questionText;

                    //save the changes to the database
                    context.Entry(question).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Lists subquestions by question text
        /// </summary>
        /// <param name="questionText">Contains the question text of the question that is checked for any associated subquestions</param>
        /// <returns>A list of subquestions associated to the supplied question text</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SubQuestionPOCO> SubQuestionsByQuestion_List(string questionText)
        {
            using (MSSContext context = new MSSContext())
            {
                var subQuestions = from q in context.Questions
                                   where q.QuestionText == questionText
                                   select new SubQuestionPOCO
                                   {
                                       SubQuestionText = q.SubQuestionText,
                                       QuestionId = q.QuestionId,
                                   };

                return subQuestions.ToList();
            }
        }

        /// <summary>
        /// Updates a subquestion in the database
        /// </summary>
        /// <param name="questionId">Contains the ID of the question associated the subquestion to be updated</param>
        /// <param name="subQuestionText">Contains the new Subquestion text that replaces the existing subquestion text in the database</param>
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void SubQuestion_Update(int questionId, string subQuestionText)
        {
            using (MSSContext context = new MSSContext())
            {
                var question = (from x in context.Questions
                                where x.QuestionId == questionId
                                select x).FirstOrDefault();

                // If the user makes no changes, show error message.
                if (question.SubQuestionText == subQuestionText)
                {
                    throw new Exception("No changes were made to the subquestion text before the \"Update\" button was clicked.");
                }
                //else, do the following actions:
                else
                {
                    //capture the new subquestion text that has been entered by the user
                    question.SubQuestionText = subQuestionText;

                    //save the changes to the database
                    context.Entry(question).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }
    }
}
