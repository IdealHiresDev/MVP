using System;

namespace IdealHires.API.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    /// Describes a type model.
    /// </summary>
    public abstract class ModelDescription
    {
        /// <summary>
        /// Repersents the Documentation properties is string  data type
        /// </summary>
        public string Documentation { get; set; }

        /// <summary>
        /// Repersents the ModelType properties is Type class refrence  type
        /// </summary>
        public Type ModelType { get; set; }

        /// <summary>
        /// Repersents the Name properties is string  data type
        /// </summary>
        public string Name { get; set; }
    }
}