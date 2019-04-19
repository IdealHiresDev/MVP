using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IdealHires.API.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    /// That class handle the Enum type model description for help page
    /// </summary>
    public class EnumTypeModelDescription : ModelDescription
    {
        /// <summary>
        /// Initialize the value by using Enum type of  model description
        /// </summary>
        public EnumTypeModelDescription()
        {
            Values = new Collection<EnumValueDescription>();
        }

        /// <summary>
        ///  Declration   collection  value properties of Enum value Desciption type
        /// </summary>
        public Collection<EnumValueDescription> Values { get; private set; }
    }
}