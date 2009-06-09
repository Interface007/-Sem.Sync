//-----------------------------------------------------------------------
// <copyright file="Factory.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.Helpers
{
    using System;

    /// <summary>
    /// This implements a simple class factory that does support generic types
    /// </summary>
    public static class Factory
    {
        /// <summary>
        /// Create an object by using the class name.
        /// </summary>
        /// <typeparam name="T">the name of the type to cast to</typeparam>
        /// <param name="className">full class name: "namespace.classname, FilenameOfTheAssembly"</param>
        /// <returns>a new instance of the class specified with the class name</returns>
        public static T GetNewObject<T>(string className)
        {
            if (string.IsNullOrEmpty(className))
            {
                return default(T);
            }

            if (className.Contains(" of "))
            {
                var types = className.Split(new[] { " of " }, StringSplitOptions.None);
                return (T)GetNewObject(types[0], types[1]);
            }

            return (T)GetNewObject(className);
        }

        /// <summary>
        /// Create an object by using the class name.
        /// </summary>
        /// <param name="className">full class name: "namespace.classname, FilenameOfTheAssembly"</param>
        /// <returns>a new instance of the class specified with the class name</returns>
        public static object GetNewObject(string className)
        {
            return Activator.CreateInstance(Type.GetType(EnrichClassName(className), true, true));
        }

        /// <summary>
        /// Creates an instance of an generic type
        /// </summary>
        /// <param name="genericClassName">full class name of the generic type: "namespace.classname, FilenameOfTheAssembly"</param>
        /// <param name="className">full class name of the type parameter for the generic type: "namespace.classname, FilenameOfTheAssembly"</param>
        /// <returns>a new instance of the class specified with the class name</returns>
        public static object GetNewObject(string genericClassName, string className)
        {
            var genericClassType = Type.GetType(EnrichClassName(genericClassName));
            var classType = Type.GetType(EnrichClassName(className));
            var typeParams = new[] { classType };
            var constructedType = genericClassType.MakeGenericType(typeParams);

            return Activator.CreateInstance(constructedType);
        }

        /// <summary>
        /// processes a class name to make it full qualifies include in the assembly name
        /// </summary>
        /// <param name="className">The class name that may need processing.</param>
        /// <returns>the processed full qualified class name</returns>
        private static string EnrichClassName(string className)
        {
            if (!className.Contains(","))
            { 
                className = className + ", " + className.Substring(0, className.LastIndexOf(".", StringComparison.Ordinal));
            }

            return className;
        }
    }
}