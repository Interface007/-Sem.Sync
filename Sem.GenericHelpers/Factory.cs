//-----------------------------------------------------------------------
// <copyright file="Factory.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.GenericHelpers
{
    using System;
    using System.Configuration;
    using System.Text;

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
    /// <code>var sourceClient = Factory.GetNewObject&lt;IClientBase&gt;("Sem.Sync.FilesystemConnector.GenericClientCsv of StdContact");</code>
    /// creating a new source object using the generic type specification by using the " of "-substring:
    /// <code>var sourceClient = Factory.GetNewObject&lt;IClientBase&gt;("Sem.Sync.FilesystemConnector.GenericClient of StdCalendarItem");</code>
    /// As you can see, you can omit the namespace if it is Sem.Sync.SyncBase.
    /// </example>
    public class Factory
    {
        /// <summary>
        /// Gets or sets the default name space for class names that do not have a name space specified.
        /// </summary>
        public string DefaultNamespace { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Factory"/> class.
        /// </summary>
        public Factory()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Factory"/> class and set the default name space.
        /// </summary>
        /// <param name="defaultNamespace">the default name space for class names that do not have a name space specified.</param>
        public Factory(string defaultNamespace)
        {
            this.DefaultNamespace = defaultNamespace;
        }

        /// <summary>
        /// Create an object by using the class name.
        /// </summary>
        /// <typeparam name="T">the name of the type to cast to</typeparam>
        /// <param name="className">full class name: "namespace.classname, FilenameOfTheAssembly"; see <see cref="Factory"/> for information about the convinience features.</param>
        /// <returns>a new instance of the class specified with the class name</returns>
        /// <remarks>see the class definition <see cref="Factory"/> for an example</remarks>
        public T GetNewObject<T>(string className)
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

                return (T)this.GetNewObject(types[0] + "`1", types[1]);
            }

            return (T)this.GetNewObject(className);
        }

        /// <summary>
        /// Create an object by using the class name.
        /// </summary>
        /// <param name="className">full class name: "namespace.classname, FilenameOfTheAssembly"; see <see cref="Factory"/> for information about the convinience features.</param>
        /// <returns>a new instance of the class specified with the class name</returns>
        /// <remarks>see the class definition <see cref="Factory"/> for an example</remarks>
        public object GetNewObject(string className)
        {
            return Activator.CreateInstance(Type.GetType(this.EnrichClassName(className), true, true));
        }

        /// <summary>
        /// Creates an instance of an generic type.
        /// </summary>
        /// <param name="genericClassName">full class name of the generic type: "namespace.classname, FilenameOfTheAssembly"; see <see cref="Factory"/> for information about the convinience features.</param>
        /// <param name="className">full class name of the type parameter for the generic type: "namespace.classname, FilenameOfTheAssembly"; see <see cref="Factory"/> for information about the convinience features.</param>
        /// <returns>a new instance of the class specified with the class name</returns>
        public object GetNewObject(string genericClassName, string className)
        {
            var genericClassType = Type.GetType(this.EnrichClassName(genericClassName.Trim()));
            var classType = Type.GetType(this.EnrichClassName(className.Trim()));
            var typeParams = new[] { classType };
            var constructedType = genericClassType.MakeGenericType(typeParams);

            return Activator.CreateInstance(constructedType);
        }

        /// <summary>
        /// processes a class name to make it full qualifies include in the assembly name
        /// </summary>
        /// <param name="className">The class name that may need processing.</param>
        /// <returns>the processed full qualified class name</returns>
        public string EnrichClassName(string className)
        {
            var returnValue = new StringBuilder();
            var isFirstFragement = true;
            var names = className.Split(new[] { " of " }, StringSplitOptions.None);
            foreach (var name in names)
            {
                if (!isFirstFragement)
                {
                    returnValue.Append(" of ");
                }

                if (!name.Contains(","))
                {
                    string assemblyName;
                    if (!name.Contains("."))
                    {
                        if (string.IsNullOrEmpty(this.DefaultNamespace))
                        {
                            throw new ConfigurationErrorsException("This factory class needs a DefaultNamespace set by the constructor of the DefaultNamespace property to add the default namespace to class names.");
                        }
                        assemblyName = this.DefaultNamespace.Trim();
                        returnValue.Append(assemblyName).Append(".");
                    }
                    else
                    {
                        assemblyName = name.Substring(0, name.LastIndexOf(".", StringComparison.Ordinal)).Trim();
                    }

                    returnValue
                        .Append(name.Trim())
                        .Append(", ")
                        .Append(assemblyName);
                }
                else
                {
                    returnValue.Append(name.Trim());
                }

                isFirstFragement = false;
            }

            var result = returnValue.ToString();
            if (result.StartsWith("{", StringComparison.Ordinal) && result.EndsWith("}", StringComparison.Ordinal))
            {
                result = ConfigurationManager.AppSettings["factoryName-" + result.Substring(1, result.Length - 2)];
            }

            return result;
        }
    }
}