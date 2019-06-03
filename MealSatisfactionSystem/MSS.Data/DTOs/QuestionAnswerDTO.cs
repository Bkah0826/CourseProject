using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region AdditionalNamespaces
using MSS.Data.POCOs;
#endregion
/// <summary>
/// This namespace holds the DTO (Data Transfer Object) entities that are mapped to the database.
/// </summary>
namespace MSS.Data.DTOs
{
    /// <summary>
    /// Handles the question text and the collection of the associated answers of a question entity
    /// </summary>
    public class QuestionAnswerDTO
    {
        /// <summary>
        /// questionAnswers is a collection of answers associated to the question entity
        /// </summary>
        public IEnumerable<AnswerPOCO> questionAnswers { get; set; }
        /// <summary>
        /// questionText is the text of the question entity
        /// </summary>
        public string questionText { get; set; }
    }
}
