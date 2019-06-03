/// <summary>
/// This namespace holds the POCOs (Plain Old Common-Language-Runtime Objects) used in MSS.
/// </summary>
namespace MSS.Data.POCOs
{
    /// <summary>
    /// Handles only the question Id of a question entity
    /// </summary>
    public class QuestionPOCO
    {
        /// <summary>
        /// questionId is the unique identifier of the question
        /// </summary>
        public int questionId { get; set; }
    }
}
