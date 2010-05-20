// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DLinqSerializer.cs" company="">
//   
// </copyright>
// <summary>
//   The d linq serializer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExpressionSerialization
{
    using System;
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Xml.Linq;

    /// <summary>
    /// The d linq serializer.
    /// </summary>
    public static class DLinqSerializer
    {
        #region Public Methods

        /// <summary>
        /// The deserialize query.
        /// </summary>
        /// <param name="dc">
        /// The dc.
        /// </param>
        /// <param name="rootXml">
        /// The root xml.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public static IQueryable DeserializeQuery(this DataContext dc, XElement rootXml)
        {
            var resolver = new DLinqSerializationTypeResolver(dc);
            var customConverter = new DLinqCustomExpressionXmlConverter(dc, resolver);
            var serializer = new ExpressionSerializer(resolver) { Converters = { customConverter } };
            Expression queryExpr = serializer.Deserialize(rootXml);

            // Query kind is populated by the ResolveXmlFromExpression method
            if (customConverter.QueryKind == null)
            {
                throw new Exception(
                    string.Format("CAnnot deserialize into DLinq query for datacontext {0} - no Table found", dc));
            }

            return customConverter.QueryKind.Provider.CreateQuery(queryExpr);
        }

        /// <summary>
        /// The serialize query.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <returns>
        /// </returns>
        public static XElement SerializeQuery(this IQueryable query)
        {
            var resolver = new DLinqSerializationTypeResolver(null);
            var serializer = new ExpressionSerializer(resolver)
                {
                   Converters = {
                                     new DLinqCustomExpressionXmlConverter(null, resolver) 
                                 } 
                };
            return serializer.Serialize(query.Expression);
        }

        #endregion

        /// <summary>
        /// The d linq custom expression xml converter.
        /// </summary>
        private class DLinqCustomExpressionXmlConverter : CustomExpressionXmlConverter
        {
            #region Constants and Fields

            /// <summary>
            /// The dc.
            /// </summary>
            private readonly DataContext dc;

            /// <summary>
            /// The resolver.
            /// </summary>
            private readonly ExpressionSerializationTypeResolver resolver;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="DLinqCustomExpressionXmlConverter"/> class.
            /// </summary>
            /// <param name="dc">
            /// The dc.
            /// </param>
            /// <param name="resolver">
            /// The resolver.
            /// </param>
            public DLinqCustomExpressionXmlConverter(DataContext dc, ExpressionSerializationTypeResolver resolver)
            {
                this.dc = dc;
                this.resolver = resolver;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets QueryKind.
            /// </summary>
            public IQueryable QueryKind { get; private set; }

            #endregion

            #region Public Methods

            /// <summary>
            /// The deserialize.
            /// </summary>
            /// <param name="expressionXml">
            /// The expression xml.
            /// </param>
            /// <returns>
            /// </returns>
            public override Expression Deserialize(XElement expressionXml)
            {
                if (expressionXml.Name.LocalName == "Table")
                {
                    Type type = this.resolver.GetType(expressionXml.Attribute("Type").Value);
                    ITable table = this.dc.GetTable(type);

                    // REturning a random IQueryable of the right kind so that we can re-create the IQueryable
                    // instance at the end of this method...
                    this.QueryKind = table;
                    return Expression.Constant(table);
                }

                return null;
            }

            /// <summary>
            /// The serialize.
            /// </summary>
            /// <param name="expression">
            /// The expression.
            /// </param>
            /// <returns>
            /// </returns>
            public override XElement Serialize(Expression expression)
            {
                if (typeof(IQueryService).IsAssignableFrom(expression.Type))
                {
                    return new XElement(
                        "Table", new XAttribute("Type", expression.Type.GetGenericArguments()[0].FullName));
                }

                return null;
            }

            #endregion
        }

        /// <summary>
        /// The d linq serialization type resolver.
        /// </summary>
        private class DLinqSerializationTypeResolver : ExpressionSerializationTypeResolver
        {
            #region Constants and Fields

            /// <summary>
            /// The dc.
            /// </summary>
            private readonly DataContext dc;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="DLinqSerializationTypeResolver"/> class.
            /// </summary>
            /// <param name="dc">
            /// The dc.
            /// </param>
            public DLinqSerializationTypeResolver(DataContext dc)
            {
                this.dc = dc;
            }

            #endregion

            #region Methods

            /// <summary>
            /// The resolve type from string.
            /// </summary>
            /// <param name="typeString">
            /// The type string.
            /// </param>
            /// <returns>
            /// </returns>
            protected override Type ResolveTypeFromString(string typeString)
            {
                var dataContextTableTypes = new HashSet<Type>(this.dc.Mapping.GetTables().Select(mt => mt.RowType.Type));
                if (typeString.Contains('`'))
                {
                    return null;
                }

                if (typeString.Contains(','))
                {
                    typeString.Substring(0, typeString.IndexOf(','));
                }

                foreach (var tableType in dataContextTableTypes)
                {
                    if (typeString.EndsWith(tableType.Name))
                    {
                        return tableType;
                    }

                    if (typeString.EndsWith(tableType.Name + "[]"))
                    {
                        return typeof(EntitySet<>).MakeGenericType(tableType);
                    }
                }

                return null;
            }

            #endregion
        }
    }
}