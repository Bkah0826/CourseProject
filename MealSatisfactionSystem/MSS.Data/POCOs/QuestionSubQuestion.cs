/// <summary>
/// This namespace holds the POCOs (Plain Old Common-Language-Runtime Objects) used in MSS.
/// </summary>
namespace MSS.Data.POCOs
{
    /// <summary>
    /// QuestionSubQuestion is a POCO class that contains the id and text for question.
    /// </summary>
    public class QuestionSubQuestion
    {
        /// <summary>
        /// QuestionText is used to store the QuestionText from database when used.
        /// </summary>
        public string QuestionText { get; set; }

        /// <summary>
        /// SubquestionText is used to store the SubQuestionText from database when used.
        /// </summary>
        public string SubquestionText { get; set; }

        /// <summary>
        /// id is used to store the question id from database when used.
        /// </summary>
        public int id { get; set; }
    }
}
