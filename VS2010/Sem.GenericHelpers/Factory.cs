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
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Text;
    using System.Text.RegularExpressions;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    /// This class implements a simple class-factory that does support generic types.
    /// </summary>
    /// <remarks>cks for the 
    /// The factory does support simple as well as generic types. Another feature is simply specifying
    ///   the type name including the namespace if the namespace name matches the assembly name. In case of 
    ///   generic types you may omit the "`1" specification as this will be added automatically when using 
    ///   the " of "-string to specify using a generic type.
    /// </remarks>
    /// <example>
    /// creating a new source object using the simple type specification:
    /// <code>var sourceClient = Factory.GetNewObject&lt;IClientBase&gt;("Sem.Sync.Connector.Filesystem.GenericClientCsv of StdContact");</code>
    /// creating a new source object using the generic type specification by using the " of "-substring:
    /// <code>var sourceClient = Factory.GetNewObject&lt;IClientBase&gt;("Sem.Sync.Connector.Filesystem.GenericClient of StdCalendarItem");</code>
    /// As you can see, you can omit the namespace if it is Sem.Sync.SyncBase.
    /// </example>
    [Serializable]
    public class Factory
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="Factory"/> class.
        /// </summary>
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

        /// <summary>
        /// Gets the mocks dictionary for registered type surrogates.
        /// </summary>
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
        /// <param name="className">The class name that may need processing.</param>
        /// <returns>The processed full qualified class name.</returns>
        public string EnrichClassName(string className)
        {
            Bouncer.ForCheckData(() => className)
                .Assert(new StringNotNullOrEmptyRule());

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

            Bouncer
                .ForCheckData(() => classType)
                .Assert(new IsNotNullRule<Type>());

            Bouncer
                .ForCheckData(() => genericClassType)
                .Assert(new IsNotNullRule<Type>());

            var typeParams = new[] { classType };
            Debug.Assert(genericClassType != null, "genericClassType != null");
            var constructedType = genericClassType.MakeGenericType(typeParams);

            return CreateTypeInstance(constructedType);
        }

        /// <summary>
        /// Determines the mock for a specific type name.
        /// </summary>
        /// <param name="name"> The name of the original type. </param>
        /// <returns>The mock object.</returns>
        private static object GetMock(string name)
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

        /// <summary>
        /// Creates an instance for a specific type
        /// </summary>
        /// <param name="type">The type the instance should be created for</param>
        /// <returns>the instance</returns>
        public static object CreateTypeInstance(Type type)
        {
            Bouncer
                .ForCheckData(() => type)
                .Assert(new IsNotNullRule<Type>());

            return GetMock(type.FullName) ?? Activator.CreateInstance(type);
        }

        /// <summary>
        /// Creates an instance for a specific type
        /// </summary>
        /// <param name="type">The type the instance should be created for</param>
        /// <param name="param1">The type of the 1st ctor parameter for instanciating the class</param>
        /// <typeparam name="TCtorParam1">The parameter value for instanciating the class</typeparam>
        /// <returns>the instance</returns>
        public static object CreateTypeInstance<TCtorParam1>(Type type, TCtorParam1 param1)
        {
            Bouncer
                .ForCheckData(() => type)
                .Assert(new IsNotNullRule<Type>());

            return GetMock(string.Join(":", type.FullName, typeof(TCtorParam1).FullName))
                ?? Activator.CreateInstance(type, param1);
        }

        /// <summary>
        /// Creates an instance for a specific type
        /// </summary>
        /// <param name="type">The type the instance should be created for</param>
        /// <param name="param1">The type of the 1st ctor parameter for instanciating the class</param>
        /// <param name="param2">The type of the 2nd ctor parameter for instanciating the class</param>
        /// <typeparam name="TCtorParam1">The 1st parameter value for instanciating the class</typeparam>
        /// <typeparam name="TCtorParam2">The 2nd parameter value for instanciating the class</typeparam>
        /// <returns>the instance </returns>
        public static object CreateTypeInstance<TCtorParam1, TCtorParam2>(Type type, TCtorParam1 param1, TCtorParam2 param2)
        {
            return GetMock(string.Join(":", type.FullName, typeof(TCtorParam1).FullName, typeof(TCtorParam2).FullName))
                ?? Activator.CreateInstance(type, param1, param2);
        }

        /// <summary>
        /// Creates an instance for a specific type
        /// </summary>
        /// <typeparam name="T">The type of the class that should be created.</typeparam>
        /// <returns>The instance </returns>
        public static T CreateTypeInstance<T>() where T : class
        {
            return CreateTypeInstance(typeof(T)) as T;
        }

        /// <summary>
        /// Mapping of expressions that should be replaced
        /// </summary>
        static readonly Dictionary<string, Func<object>> ExpressionMap = new Dictionary<string, Func<object>>();

        private static readonly Regex AnonymousClassReplacer = new Regex(@"\<>c__DisplayClass[0-9]+", RegexOptions.Compiled);

        public static void Register<T>(Expression<Func<T>> toBeReplaced, Expression<Func<T>> replacement)
        {
            var genToBeReplaced = GenerateNameFromExpression(toBeReplaced);
            Register(genToBeReplaced, replacement);
        }

        public static void Register<T>(string genToBeReplaced, Expression<Func<T>> replacement)
        {
            Func<object> genReplacement = () => replacement.Compile();
            if (ExpressionMap.ContainsKey(genToBeReplaced))
            {
                ExpressionMap.Remove(genToBeReplaced);
            }

            ExpressionMap.Add(genToBeReplaced, genReplacement);
        }

        public static T Invoke<T>(Expression<Func<T>> toBeReplaced)
            where T : class
        {
            var genToBeReplaced = GenerateNameFromExpression(toBeReplaced);
            if (ExpressionMap.ContainsKey(genToBeReplaced))
            {
                var result = ExpressionMap[genToBeReplaced].Invoke();
                return ((Func<object>)result).Invoke() as T;
            }

            var compiledExpression = toBeReplaced.Compile();
            if (compiledExpression == null)
            {
                throw new ArgumentException(@"The expression did not compile properly and no replacement has been registered.", "toBeReplaced");
            }

            return compiledExpression.Invoke();
        }

        private static string GenerateNameFromExpression<T>(Expression<Func<T>> expression)
        {
            var nameFromExpression = expression.ToString();
            if (nameFromExpression.Contains("c__DisplayClass"))
            {
                nameFromExpression = AnonymousClassReplacer.Replace(nameFromExpression, "<>c__DisplayClass");
            }

            return nameFromExpression;
        }

        #endregion
    }
}