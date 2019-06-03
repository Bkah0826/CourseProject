/// <summary>
/// This namespace holds the POCOs (Plain Old Common-Language-Runtime Objects) used in MSS.
/// </summary>
namespace MSS.Data.POCOs
{
    /// <summary>
    /// For charting lookup tables. Holds all types of queries (lookupAnswers, lookupQuestion, lookupSites)
    /// </summary>
    public class LookupValues
    {
        /// <summary>
        /// Id of the item being returned
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Description of the item pulled back
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Value of the item pulled back
        /// </summary>
        public string Value { get; set; }
        
        /// <summary>
        /// Type of the query that was originally passed in (lookupAnswers, lookupQuestion, lookupSites)
        /// </summary>
        public string Type { get; set; }
    }
}
