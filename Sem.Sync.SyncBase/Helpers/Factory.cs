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
        /// <returns></returns>
        public static T GetNewObject<T>(string className)
        {
            if (string.IsNullOrEmpty(className))
                return default(T);

            if (className.Contains(" of "))
            {
                var types = className.Split(new []{" of "}, StringSplitOptions.None);
                return (T)GetNewObject(EnrichClassName(types[0]), EnrichClassName(types[1]));
            }
            return (T)GetNewObject(EnrichClassName(className));
        }

        /// <summary>
        /// Create an object by using the class name.
        /// </summary>
        /// <param name="className">full class name: "namespace.classname, FilenameOfTheAssembly"</param>
        /// <returns></returns>
        public static object GetNewObject(string className)
        {
            return Activator.CreateInstance(Type.GetType(EnrichClassName(className), true, true));
        }

        /// <summary>
        /// Creates an instance of an generic type
        /// </summary>
        /// <param name="className">full class name of the generic type: "namespace.classname, FilenameOfTheAssembly"</param>
        /// <param name="ofClassName">full class name of the type parameter for the generic type: "namespace.classname, FilenameOfTheAssembly"</param>
        /// <returns></returns>
        public static object GetNewObject(string className, string ofClassName)
        {
            var classType = Type.GetType(EnrichClassName(className));
            var ofType = Type.GetType(EnrichClassName(ofClassName));
            var typeParams = new [] { ofType };
            var constructedType = classType.MakeGenericType(typeParams);

            return Activator.CreateInstance(constructedType);
        }

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