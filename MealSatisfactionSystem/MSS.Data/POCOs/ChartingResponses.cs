/// <summary>
/// This namespace holds the POCOs (Plain Old Common-Language-Runtime Objects) used in MSS.
/// </summary>
namespace MSS.Data.POCOs
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The parent class to hold the data for each charting request
    /// </summary>
    public class ChartingResponse
    {
        /// <summary>
        /// question contains Question Text
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Subtext contains Question subtext
        /// </summary>
        public string Subtext { get; set; }

        /// <summary>
        /// Parameter contains the shorthand question parameter
        /// </summary>
        public string Parameter { get; set; }

        /// <summary>
        /// Gender contains array of genders
        /// </summary>
        public List<string> Gender { get; set; }

        /// <summary>
        /// Age contains array of ages
        /// </summary>
        public List<string> Age { get; set; }

        /// <summary>
        /// Data contains the array of data orginized by date
        /// </summary>
        public List<DateData> Data { get; set; }
    }

    /// <summary>
    /// Holds answers with thir data value and date
    /// </summary>
    public class DateData
    {
        /// <summary>
        /// AnswerText contains the descriptive text of the answer selected
        /// </summary>
        public string AnswerText { get; set; }
        /// <summary>
        /// Date contains the date the response was submitted
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Data contains the value associated with the answer
        /// </summary>
        public double Data { get; set; }
    }
}
