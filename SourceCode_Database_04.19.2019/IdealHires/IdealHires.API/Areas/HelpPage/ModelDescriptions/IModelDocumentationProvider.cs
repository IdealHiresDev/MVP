using System;
using System.Reflection;

namespace IdealHires.API.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    /// That Interface definiotion   of GetDocumentation and  GetDocumentation abstract. 
    /// </summary>
    public interface IModelDocumentationProvider
    {
        /// <summary>
        /// obtains the documentation about the attribute of a member  of  MemberInfo class.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        string GetDocumentation(MemberInfo member);

        /// <summary>
        /// obtains the documentation about the attribute of Type class.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetDocumentation(Type type);
    }
}