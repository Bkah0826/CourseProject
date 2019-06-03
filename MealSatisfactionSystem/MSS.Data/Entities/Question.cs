/// <summary>
/// This namespace holds the entities that are mapped to the database.
/// </summary>
namespace MSS.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    ///  Question is the Question Entity. It has all the basic information that the table requires in the database, as well as virtual attributes for navigation.
    /// </summary>
    [Table("Question")]
    public partial class Question
    {
        /// <summary>
        /// Question() is the default constructor for the Question class that allows the creation of empty Question objects.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Question()
        {
            QuestionResponses = new HashSet<QuestionResponse>();
            Answers = new HashSet<Answer>();
        }

        /// <summary>
        /// QuestionId is a unique identifier that identifies a specific Question.
        /// </summary>
        [Key]
        public int QuestionId { get; set; }

        /// <summary>
        /// QuestionText is a string of text for a question.
        /// </summary>
        [Required(ErrorMessage = "Question Text is required")]
        [StringLength(300, ErrorMessage = "Question Text cannot exceed 300 characters")]
        public string QuestionText { get; set; }

        /// <summary>
        /// SubQuestionText is a string of text for the subquestion if it exist. 
        /// </summary>
        [StringLength(150, ErrorMessage = "Sub Question Text cannot exceed 150 characters")]
        public string SubQuestionText { get; set; }

        /// <summary>
        /// QuestionParameter is a string of text that defines the category of the question.
        /// </summary>
        [StringLength(20, ErrorMessage = "Question Parameter cannot exceed 20 characters")]
        public string QuestionParameter { get; set; }

        /// <summary>
        /// DateAdded is the date the Question was added
        /// </summary>
        [Required(ErrorMessage = "DateAdded is required")]
        public DateTime DateAdded { get; set; }

        /// <summary>
        /// Colour is a property used by reporting to determine the colour used to signify this object in graphs.
        /// </summary>
        public string Colour { get; set; }

        #region Virtual Attributes
        /// <summary>
        /// QuestionResponses is a collection of question responses connected to the question via questionId 
        /// </summary>
        /// 
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionResponse> QuestionResponses { get; set; }

        /// <summary>
        /// Answers is a collection of answers linked to the question via questionId
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Answer> Answers { get; set; } 
        #endregion
    }
}
