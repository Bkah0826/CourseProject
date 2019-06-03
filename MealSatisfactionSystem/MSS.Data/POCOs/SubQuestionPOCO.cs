/// <summary>
/// This namespace holds the POCOs (Plain Old Common-Language-Runtime Objects) used in MSS.
/// </summary>
namespace MSS.Data.POCOs
{
    /// <summary>
    /// Handles the question Id and the subquestion text of a question entity
    /// </summary>
    public class SubQuestionPOCO
    {
        /// <summary>
        /// QuestionId is the unique identifier of the question entity
        /// </summary>
        public int QuestionId { get; set; }
        /// <summary>
        /// SubquestionText of the question entity
        /// </summary>
        public string SubQuestionText { get; set; }
    }
}
