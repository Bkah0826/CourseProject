/// <summary>
/// This namespace holds the POCOs (Plain Old Common-Language-Runtime Objects) used in MSS.
/// </summary>
namespace MSS.Data.POCOs
{
    /// <summary>
    /// Handles the description and answer Id of an answer entity
    /// </summary>
    public class AnswerPOCO
    {
        /// <summary>
        /// Description is a short description of the answer. May also be referred to as the answer text.
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// answerId is the unique identifier of the answer
        /// </summary>
        public int answerId { get; set; }
    }
}
