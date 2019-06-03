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
    ///  Site is the Site Entity. It has all the basic information that the table requires in the database, as well as virtual attributes for navigation.
    /// </summary>
    [Table("Site")]
    public partial class Site
    {
        /// <summary>
        /// Allows the instantiation of empty Site objects.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Site()
        {
            Units = new HashSet<Unit>();
        }

        /// <summary>
        /// SiteId is a unique identifier that identifies a specific Site.
        /// </summary>
        [Key]
        public int SiteId { get; set; }

        /// <summary>
        /// SiteName is the name of the Site.
        /// </summary>
        [Required(ErrorMessage = "Site Name is required")]        
        [StringLength(50, ErrorMessage = "Site Name cannot exceed 50 characters")]
        public string SiteName { get; set; }

        /// <summary>
        /// Description is a short description of the Site.
        /// </summary>
        [Required(ErrorMessage = "Description is required")]
        [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters")]
        public string Description { get; set; }

        /// <summary>
        /// Passcode is the code that is required for patients to access the survey for each specific Site.
        /// </summary>
        [Required(ErrorMessage = "Passcode is required")]        
        [StringLength(15, ErrorMessage = "Passcode cannot exceed 15 characters")]
        public string Passcode { get; set; }

        /// <summary>
        /// Disabled is a field that determines whether or not a Site is currently active. false = active, true = inactive.
        /// </summary>
        [Required(ErrorMessage = "Disabled must be true or false")]
        public bool Disabled { get; set; }


        #region VirtualAttributes
        /// <summary>
        /// Units is a collection of units linked to the Site via its SiteId.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Unit> Units { get; set; } 
        #endregion
    }
}
