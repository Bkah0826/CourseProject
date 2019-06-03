/// <summary>
/// This namespace holds the POCOs (Plain Old Common-Language-Runtime Objects) used in MSS.
/// </summary>
namespace MSS.Data.POCOs
{
    /// <summary>
    /// This object holds all relevant data for generating the analytics on the home page. It is a list of surveys.
    /// </summary>
    public class ChartingHomeResponse
    {
        /// <summary>
        /// questions contains the survey's questions as a string value
        /// </summary>
        public string questions { get; set; }

        /// <summary>
        /// subQuestions contains questions that belong under a branach of a larger overarching question as a string value.
        /// </summary>
        public string subQuestions { get; set; }

        /// <summary>
        /// answers contains all answers' text of the survey.
        /// </summary>
        public string answers { get; set; }

        /// <summary>
        /// Stores all ID's of the pulled response.
        /// </summary>
        public int answerID { get; set; }

        /// <summary>
        /// Stores all ID's of pulled response.
        /// </summary>
        public int questionID { get; set; }

        /// <summary>
        /// Stores the associated scored value of each answer.
        /// </summary>
        public int value { get; set; }

        /// <summary>
        /// unitId is the unit of the response being shown.
        /// </summary>
        public int unitID { get; set; }

        /// <summary>
        /// questionParam stores the associated search parameter of each question.
        /// </summary>
        public string questionParam { get; set; }

        /// <summary>
        /// maxValue stores the maximum point value that can be scored for a each question.
        /// </summary>
        public int maxValue { get; set; }

        /// <summary>
        /// colour stores the colour associated with each question
        /// </summary>
        public string colour { get; set; }

        /// <summary>
        /// removed stores the unit's deactivated status.
        /// </summary>
        public bool removed { get; set; }

        /// <summary>
        /// unitName stores the unit name.
        /// </summary>
        public string unitName { get; set; }
    }
}
