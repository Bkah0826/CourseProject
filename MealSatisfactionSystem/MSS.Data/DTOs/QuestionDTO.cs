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
    /// Handles the question text and the collection of the question IDs of question entities under the same question text
    /// </summary>
    public class QuestionDTO
    {
        /// <summary>
        /// quetionText is the text of the of the question entities
        /// </summary>
        public string questionText { get; set; }

        /// <summary>
        /// questionIds is a Collection of question IDs with the same question text
        /// </summary>
        public IEnumerable<QuestionPOCO> questionIds { get; set; }
    }
}
