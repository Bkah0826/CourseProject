/// <summary>
/// This namespace holds the entities that are mapped to the database.
/// </summary>
namespace MSS.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    ///  QuestionResponse is the QuestionResponse Entity. It has all the basic information that the table requires in the database, as well as virtual attributes for navigation.
    /// </summary>
    [Table("QuestionResponse")]
    public partial class QuestionResponse
    {
        /// <summary>
        /// ResponseId is the unique identifier for a specific response. In the QuestionResponse Entity the ResponseId along with the QuestionId create a unique identifier for this entity. 
        /// </summary>
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ResponseId { get; set; }

        /// <summary>
        /// QuestionId is the unique identifier for a question. In the QuestionResponse Entity the QuestionId along with the ResponseId create a unique identifier for this entity.
        ///</summary>
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int QuestionId { get; set; }
        /// <summary>
        /// AnswerId is a unique identifier that identifies the specific Answer attached to a QuestionResponse.
        /// </summary>
        [Required(ErrorMessage = "AnswerId is Required")]
        public int AnswerId { get; set; }

        #region VirtualAttributes
        /// <summary>
        /// Response is the answer connected to the QuestionResponse via it's ResponseId
        /// </summary>
        public virtual Response Response { get; set; }

        /// <summary>
        /// Answer is the answer connected to the QuestionResponse via it's AnswerId
        /// </summary>
        public virtual Answer Answer { get; set; }

        /// <summary>
        /// Question is the answer connected to the QuestionResponse via it's QuestionId
        /// </summary>
        public virtual Question Question { get; set; }  
        #endregion
    }
}
