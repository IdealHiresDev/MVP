using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IdealHires.API.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    /// that class handel the declaration of parameter and intialize the annonation by using constructor
    /// </summary>
    public class ParameterDescription
    {
        /// <summary>
        ///  intialize the annonation by using constructor
        /// </summary>
        public ParameterDescription()
        {
            Annotations = new Collection<ParameterAnnotation>();
        }

        /// <summary>
        /// declration of Collection of ParameterAnnotation type of properties
        /// </summary> 
        public Collection<ParameterAnnotation> Annotations { get; private set; }

        /// <summary>
        /// string data type properties
        /// </summary>
        public string Documentation { get; set; }

        /// <summary>
        /// string data type properties
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ModelDescription class type properties
        /// </summary>
        public ModelDescription TypeDescription { get; set; }
    }
}