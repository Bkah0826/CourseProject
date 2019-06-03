/// <summary>
/// This namespace holds the entities that are mapped to the database.
/// </summary>
namespace MSS.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    ///  Unit is the Unit Entity. It has all the basic information that the table requires in the database, as well as virtual attributes for navigation.
    /// </summary>
    [Table("Unit")]
    public partial class Unit
    {
        /// <summary>
        /// Unit() is a default constructor for the Unit class that allows the creation of empty Unit objects.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Unit()
        {
            Responses = new HashSet<Response>();
        }

        /// <summary>
        /// UnitId is a unique identifier that identifies a specific Unit.
        /// </summary>
        [Key]
        public int UnitId { get; set; }

        /// <summary>
        /// SiteId is a unique identifier that identifies the specific site attached to a Unit
        /// </summary>
        [Required(ErrorMessage = "SiteId is required")]
        public int SiteId { get; set; }

        /// <summary>
        /// UnitName is the name of the Unit.
        /// </summary>
        [Required(ErrorMessage = "Unit Name is required")]
        [StringLength(8, ErrorMessage = "Unit Name cannot exceed 8 characters")]
        public string UnitName { get; set; }

        /// <summary>
        /// Description is a short description of the Unit.
        /// </summary>
        [Required(ErrorMessage = "Description is required")]
        [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters")]
        public string Description { get; set; }

        /// <summary>
        /// Disabled is a field that determines whether or not a Unit is currently active. false = active, true = inactive.
        /// </summary>
        [Required(ErrorMessage ="Disabled must be true or false")]
        public bool Disabled { get; set; }

        #region Virtual Attributes
        // These attributes are purely for navigational purposes. These can be called using Linq queries to help navigate through the database.

        /// <summary>
        /// Responses is a collection of responses linked to the Unit via its UnitId.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Response> Responses { get; set; }

        /// <summary>
        /// Site is the site connected to the Unit via it's SiteId.
        /// </summary>
        public virtual Site Site { get; set; }

        #endregion
    }
}
