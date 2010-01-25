//-----------------------------------------------------------------------
// <copyright file="Factory.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.Helpers
{
    using System;
    using System.Reflection;

    /// <summary>
    /// This class implements a simple class-factory that does support generic types.
    /// </summary>
    /// <remarks>
    /// The factory does support simple as well as generic types. Another feature is simply specifying
    /// the type name including the namespace if the namespace name matches the assembly name. In case of 
    /// generic types you may omit the "`1" specification as this will be added automatically when using 
    /// the " of "-string to specify using a generic type.
    /// </remarks>
    /// <example>
    /// creating a new source object using the simple type specification:
    /// <code>var sourceClient = Factory.GetNewObject&lt;IClientBase&gt;("Sem.Sync.FilesystemConnector.ContactClientCsv");</code>
    /// creating a new source object using the generic type specification by using the " of "-substring:
    /// <code>var sourceClient = Factory.GetNewObject&lt;IClientBase&gt;("Sem.Sync.FilesystemConnector.GenericClient of StdCalendarItem");</code>
    /// As you can see, you can omit the namespace if it is Sem.Sync.SyncBase.
    /// </example>
    public static class Factory
    {
        /// <summary>
        /// Create an object by using the class name.
        /// </summary>
        /// <typeparam name="T">the name of the type to cast to</typeparam>
        /// <param name="className">full class name: "namespace.classname, FilenameOfTheAssembly"; see <see cref="Factory"/> for information about the convinience features.</param>
        /// <returns>a new instance of the class specified with the class name</returns>
        /// <remarks>see the class definition <see cref="Factory"/> for an example</remarks>
        public static T GetNewObject<T>(string className)
        {
            if (string.IsNullOrEmpty(className))
            {
                return default(T);
            }

            if (className.Contains(" of "))
            {
                var types = className.Split(new[] { " of " }, StringSplitOptions.None);
                if (types[0].EndsWith("`1", StringComparison.Ordinal))
                {
                    types[0] = types[0].Substring(0, types[0].Length - 2);
                }

                return (T)GetNewObject(types[0] + "`1", types[1]);
            }

            return (T)GetNewObject(className);
        }

        /// <summary>
        /// Create an object by using the class name.
        /// </summary>
        /// <param name="className">full class name: "namespace.classname, FilenameOfTheAssembly"; see <see cref="Factory"/> for information about the convinience features.</param>
        /// <returns>a new instance of the class specified with the class name</returns>
        /// <remarks>see the class definition <see cref="Factory"/> for an example</remarks>
        public static object GetNewObject(string className)
        {
            return Activator.CreateInstance(Type.GetType(EnrichClassName(className), true, true));
        }

        /// <summary>
        /// Creates an instance of an generic type.
        /// </summary>
        /// <param name="genericClassName">full class name of the generic type: "namespace.classname, FilenameOfTheAssembly"; see <see cref="Factory"/> for information about the convinience features.</param>
        /// <param name="className">full class name of the type parameter for the generic type: "namespace.classname, FilenameOfTheAssembly"; see <see cref="Factory"/> for information about the convinience features.</param>
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
                if (!className.Contains("."))
                {
                    // ReSharper disable PossibleNullReferenceException
                    className =
                        Assembly.GetAssembly(typeof(Factory)).FullName.Split(
                            new[] { "," }, StringSplitOptions.RemoveEmptyEntries)[0] + "." + className;

                    // ReSharper restore PossibleNullReferenceException
                }

                className = className + ", " + className.Substring(0, className.LastIndexOf(".", StringComparison.Ordinal));
            }

            return className;
        }
    }
}