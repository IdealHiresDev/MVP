using System;

namespace IdealHires.API.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    /// Use this attribute to change the name of the <see cref="ModelDescription"/> generated for a type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
    public sealed class ModelNameAttribute : Attribute
    {
        /// <summary>
        /// This is a ModelNameAttribute class connstructor. That constructor handel the initialization of name propoertis
        /// </summary>
        /// <param name="name"></param>
        public ModelNameAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// string type propertis of ModelNameAttribute class.
        /// </summary>
        public string Name { get; private set; }
    }
}