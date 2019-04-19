using System.Collections.ObjectModel;

namespace IdealHires.API.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    /// This class is used to Complex type of model description
    /// </summary>
    public class ComplexTypeModelDescription : ModelDescription
    {
        /// <summary>
        /// This Constructor is used to initialize the Propoerties
        /// </summary>
        public ComplexTypeModelDescription()
        {
            Properties = new Collection<ParameterDescription>();
        }

        /// <summary>
        /// This propertis is used as collection of Parameter Description type
        /// </summary>
        public Collection<ParameterDescription> Properties { get; private set; }
    }
}