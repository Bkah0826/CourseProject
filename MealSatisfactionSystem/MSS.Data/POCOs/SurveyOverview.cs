/// <summary>
/// This namespace holds the POCOs (Plain Old Common-Language-Runtime Objects) used in MSS.
/// </summary>
namespace MSS.Data.POCOs
{
    using System;

    /// <summary>
    ///  Holds the all data associated with a survey overview, taken from the Response, Site and Unit entities.
    /// </summary>
    public class SurveyOverview
    {
        /// <summary>
        /// ResponseId contains the ResponseId field from the Response entity.
        /// </summary>
        public int ResponseId { get; set; }

        /// <summary>
        /// SiteName contains the SiteName field from the Site entity.
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// UnitName contains the UnitName field from the Unit entity.
        /// </summary>
        public String UnitName { get; set; }

        /// <summary>
        /// Gender contains the Gender field (as a string) from the Response entity.
        /// </summary>
        public String Gender { get; set; }

        /// <summary>
        /// Age contains the Age field from the Response entity.
        /// </summary>
        public String Age { get; set; }

        /// <summary>
        /// Date contains the Date field from the Response entity.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Comment contains the Comment field from the Response entity.
        /// </summary>
        public String Comment { get; set; }
    }
}
