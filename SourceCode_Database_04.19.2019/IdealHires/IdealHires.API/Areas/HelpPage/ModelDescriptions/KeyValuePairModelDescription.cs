namespace IdealHires.API.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    /// That class handle the  Key value pair Model Description for help page
    /// </summary>
    public class KeyValuePairModelDescription : ModelDescription
    {
        /// <summary>
        /// Declaration of KeyModelDecription properties of ModelDescription type
        /// </summary>
        public ModelDescription KeyModelDescription { get; set; }

        /// <summary>
        /// Declaration of ValueModelDescription properties of ModelDescription type
        /// </summary>
        public ModelDescription ValueModelDescription { get; set; }
    }
}