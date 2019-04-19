using System;

namespace IdealHires.API.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    /// That class handle the annotation of parameter
    /// </summary>
    public class ParameterAnnotation
    {
        /// <summary>
        /// AnnotationAttribute propertis of Attribute class type.
        /// </summary>
        public Attribute AnnotationAttribute { get; set; }

        /// <summary>
        /// Documentation propertis of string data type.
        /// </summary>
        public string Documentation { get; set; }
    }
}