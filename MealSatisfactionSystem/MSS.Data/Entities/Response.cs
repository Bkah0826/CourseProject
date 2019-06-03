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
    ///  Response is the Response Entity. It has all the basic information that the table requires in the database, as well as virtual attributes for navigation.
    /// </summary>
    [Table("Response")]
    public partial class Response
    {
        /// <summary>
        /// ResponseId is a unique identifier that identifies a specific Response.
        /// </summary>
        [Key]
        public int ResponseId { get; set; }

        /// <summary>
        /// UnitId is a unique identifier that identifies the specific Unit attached to a Response
        /// </summary>
        [Required(ErrorMessage = "Unit Id is required")]
        public int UnitId { get; set; }

        /// <summary>
        /// Age is the age of the person who completed this Response.
        /// </summary>
        [StringLength(30, ErrorMessage = "Age cannot exceed 30 characters")]
        public string Age { get; set; }

        /// <summary>
        /// Gender is the gender of the person who completed this Response.
        /// </summary>
        [Column(TypeName ="char")]
        [StringLength(1, ErrorMessage = "Gender cannot exceed 1 character")]
        public string Gender { get; set; }

        /// <summary>
        /// Date is the date the Response was completed
        /// </summary>
        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Comment is for the comment field at the end of the survey
        /// </summary>
        [StringLength(300, ErrorMessage = "Comment cannot exceed 300 characters")]
        public string Comment { get; set; }
        
        #region Virtual Attributes
        /// <summary>
        /// Unit is the unit connected to the Response via UnitId
        /// </summary>
        public virtual Unit Unit { get; set; }

        /// <summary>
        /// QuestionResponses is a collection of question responses connected to the Response via ResponseId 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionResponse> QuestionResponses { get; set; } 
        #endregion
    }
}
