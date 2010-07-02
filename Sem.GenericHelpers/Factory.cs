// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Factory.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This class implements a simple class-factory that does support generic types.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Text;

    /// <summary>
    /// This class implements a simple class-factory that does support generic types.
    /// </summary>
    /// <remarks>
    /// The factory does support simple as well as generic types. Another feature is simply specifying
    ///   the type name including the namespace if the namespace name matches the assembly name. In case of 
    ///   generic types you may omit the "`1" specification as this will be added automatically when using 
    ///   the " of "-string to specify using a generic type.
    /// </remarks>
    /// <example>
    /// creating a new source object using the simple type specification:
    ///   <code>
    /// var sourceClient = Factory.GetNewObject&lt;IClientBase&gt;("Sem.Sync.Connector.Filesystem.GenericClientCsv of StdContact");
    /// </code>
    /// creating a new source object using the generic type specification by using the " of "-substring:
    ///   <code>
    /// var sourceClient = Factory.GetNewObject&lt;IClientBase&gt;("Sem.Sync.Connector.Filesystem.GenericClient of StdCalendarItem");
    /// </code>
    /// As you can see, you can omit the namespace if it is Sem.Sync.SyncBase.
    /// </example>
    [Serializable]
    public class Factory
    {
        #region Constructors and Destructors

        static Factory()
        {
            Mocks = new SerializableDictionary<string, object>();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Factory" /> class.
        /// </summary>
        public Factory()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Factory"/> class and set the default name space.
        /// </summary>
        /// <param name="defaultNamespace">
        /// the default name space for class names that do not have a name space specified.
        /// </param>
        public Factory(string defaultNamespace)
        {
            this.DefaultNamespace = defaultNamespace;
        }

        #endregion

        #region Properties

        public static SerializableDictionary<string, object> Mocks { get; private set; }

        /// <summary>
        ///   Gets or sets the default name space for class names that do not have a name space specified.
        /// </summary>
        public string DefaultNamespace { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// processes a class name to make it full qualifies include in the assembly name
        /// </summary>
        /// <param name="className">
        /// The class name that may need processing.
        /// </param>
        /// <returns>
        /// the processed full qualified class name
        /// </returns>
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
                        if (className == "StdContact" || className == "StdContact")
                        {
                            returnValue.Append("DetailData.");
                        }
                    }
                    else
                    {
                        assemblyName = name.Substring(0, name.LastIndexOf(".", StringComparison.Ordinal)).Trim();
                    }

                    returnValue.Append(name.Trim()).Append(", ").Append(assemblyName);
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

        /// <summary>
        /// Create an object by using the class name.
        /// </summary>
        /// <typeparam name="T"> the name of the type to cast to </typeparam>
        /// <param name="className"> full class name: "namespace.classname, FilenameOfTheAssembly"; see <see cref="Factory"/> for information about the convinience features. </param>
        /// <returns> a new instance of the class specified with the class name </returns>
        /// <remarks> see the class definition <see cref="Factory"/> for an example </remarks>
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
        /// <param name="className">
        /// full class name: "namespace.classname, FilenameOfTheAssembly"; see <see cref="Factory"/> for information about the convinience features.
        /// </param>
        /// <returns>
        /// a new instance of the class specified with the class name
        /// </returns>
        /// <remarks>
        /// see the class definition <see cref="Factory"/> for an example
        /// </remarks>
        public object GetNewObject(string className)
        {
            var type = Type.GetType(this.EnrichClassName(className), false, true);
            if (type == null)
            {
                // todo: alternative version connectors should be supported via a configuration, not hard coded
                // search for an alternative object (e.g. compatible with an older version of the installed target)
                switch (className)
                {
                    case "Sem.Sync.Connector.Outlook.CalendarClient":

                        // the standard connector for outlook is for outlook 2008 - that
                        // one is not compatible with outlook 2003, so we do fall back
                        // to outlook 2003 if the standard one cannot be created.
                        className = "Sem.Sync.Connector.Outlook2003.CalendarClient";
                        break;

                    case "Sem.Sync.Connector.Outlook.ContactClient":

                        // the standard connector for outlook is for outlook 2008 - that
                        // one is not compatible with outlook 2003, so we do fall back
                        // to outlook 2003 if the standard one cannot be created.
                        className = "Sem.Sync.Connector.Outlook2003.ContactClient";
                        break;
                }

                // we do only have one try, so this one will throw an exception ;-)
                type = Type.GetType(this.EnrichClassName(className), true, true);
            }

            return CreateTypeInstance(type);
        }

        /// <summary>
        /// Creates an instance of an generic type.
        /// </summary>
        /// <param name="genericClassName">
        /// full class name of the generic type: "namespace.classname, FilenameOfTheAssembly"; see <see cref="Factory"/> for information about the convinience features.
        /// </param>
        /// <param name="className">
        /// full class name of the type parameter for the generic type: "namespace.classname, FilenameOfTheAssembly"; see <see cref="Factory"/> for information about the convinience features.
        /// </param>
        /// <returns>
        /// a new instance of the class specified with the class name
        /// </returns>
        public object GetNewObject(string genericClassName, string className)
        {
            var genericClassType = Type.GetType(this.EnrichClassName(genericClassName.Trim()));
            var classType = Type.GetType(this.EnrichClassName(className.Trim()));

            if (classType == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The class {0} cannot be found - check spelling.", className), "className");
            }

            var typeParams = new[] { classType };
            var constructedType = genericClassType.MakeGenericType(typeParams);

            return CreateTypeInstance(constructedType);
        }

        static object GetMock(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                if (Mocks.ContainsKey(name))
                {
                    return Mocks[name];
                }
            }

            return null;
        }

        public static object CreateTypeInstance(Type type)
        {
            return (GetMock(type.FullName) ?? Activator.CreateInstance(type));
        }

        public static object CreateTypeInstance<TCtorParam1>(Type type, TCtorParam1 param1)
        {
            return GetMock(string.Join(":", type.FullName , typeof(TCtorParam1).FullName))
                ?? Activator.CreateInstance(type, param1);
        }

        public static object CreateTypeInstance<TCtorParam1, TCtorParam2>(Type type, TCtorParam1 param1, TCtorParam2 param2)
        {
            return GetMock(string.Join(":", type.FullName, typeof(TCtorParam1).FullName, typeof(TCtorParam2).FullName))
                ?? Activator.CreateInstance(type, param1);
        }

        public static T CreateTypeInstance<T>() where T : class
        {
            return CreateTypeInstance(typeof(T)) as T;
        }

        public static T Invoke<T>(Expression<Func<T>> constructorExpression)
        {
            SerializableDictionary<Expression<Func<T>>, T> ExpressionMocks = new SerializableDictionary<Expression<Func<T>>, T>();
            if (ExpressionMocks.ContainsKey(constructorExpression))
            {
                return ExpressionMocks[constructorExpression];
            }

            return constructorExpression.Compile().Invoke();
        }

        #endregion
    }
}