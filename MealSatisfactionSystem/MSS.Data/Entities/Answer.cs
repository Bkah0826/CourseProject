/// <summary>
/// This namespace holds the entities that are mapped to the database.
/// </summary>
namespace MSS.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    ///  Answer is the Answer Entity. It has all the basic information that the table requires in the database, as well as virtual attributes for navigation.
    /// </summary>
    [Table("Answer")]
    public partial class Answer
    {
        /// <summary>
        /// Answer() is the default constructor for the Answer class that allows the creation of empty Answer objects.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Answer()
        {
            QuestionResponses = new HashSet<QuestionResponse>();
            Questions = new HashSet<Question>();
        }

        /// <summary>
        /// AnswerId is a unique identifier that identifies a specific Answer.
        /// </summary>
        [Key]
        public int AnswerId { get; set; }
        
        /// <summary>
        /// Value is a quantifiable field that indicates the points the Answer is worth
        /// </summary>
        [Required(ErrorMessage = "Value is required")]
        public int Value { get; set; }

        /// <summary>
        /// MaxValue is a quantifiable field that indicates the maximum amount of points this Answer can be worth
        /// </summary>
        [Required(ErrorMessage = "MaxValue is required")]
        public int MaxValue { get; set; }

        /// <summary>
        /// Description is a short description of the Answer.
        /// </summary>
        [StringLength(50, ErrorMessage = "Description cannot exceed 50 characters")]
        public string Description { get; set; }
        
        /// <summary>
        /// Colour holds the colour for the Answer, this is for reporting purposes.
        /// </summary>
        public string Colour { get; set; }

        #region Virtual Attributes
        /// <summary>
        /// QuestionResponses is a collection of questionResponses linked to the Answer via its AnswerId.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionResponse> QuestionResponses { get; set; }

        /// <summary>
        /// Questions is a collection of questions linked to the Answer via its AnswerId.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Question> Questions { get; set; } 
        #endregion
    }
}
