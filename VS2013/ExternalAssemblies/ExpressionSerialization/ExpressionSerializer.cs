// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionSerializer.cs" company="">
//   
// </copyright>
// <summary>
//   The expression serializer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExpressionSerialization
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Xml.Linq;

    /// <summary>
    /// The expression serializer.
    /// </summary>
    public class ExpressionSerializer
    {
        #region Constants and Fields

        /// <summary>
        /// The attribute types.
        /// </summary>
        private static readonly Type[] AttributeTypes = new[]
            {
               typeof(string), typeof(int), typeof(bool), typeof(ExpressionType) 
            };

        /// <summary>
        /// The parameters.
        /// </summary>
        private readonly Dictionary<string, ParameterExpression> parameters =
            new Dictionary<string, ParameterExpression>();

        /// <summary>
        /// The resolver.
        /// </summary>
        private readonly ExpressionSerializationTypeResolver resolver;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionSerializer"/> class.
        /// </summary>
        /// <param name="resolver">
        /// The resolver.
        /// </param>
        public ExpressionSerializer(ExpressionSerializationTypeResolver resolver)
        {
            this.resolver = resolver;
            this.Converters = new List<CustomExpressionXmlConverter>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionSerializer"/> class.
        /// </summary>
        public ExpressionSerializer()
        {
            this.resolver = new ExpressionSerializationTypeResolver();
            this.Converters = new List<CustomExpressionXmlConverter>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets Converters.
        /// </summary>
        public List<CustomExpressionXmlConverter> Converters { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The deserialize.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        public Expression Deserialize(XElement xml)
        {
            this.parameters.Clear();
            return this.ParseExpressionFromXmlNonNull(xml);
        }

        /// <summary>
        /// The deserialize.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <typeparam name="TDelegate">
        /// </typeparam>
        /// <returns>
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public Expression<TDelegate> Deserialize<TDelegate>(XElement xml)
        {
            Expression e = this.Deserialize(xml);
            if (e is Expression<TDelegate>)
            {
                return e as Expression<TDelegate>;
            }

            throw new Exception("xml must represent an Expression<TDelegate>");
        }

        /*
         * SERIALIZATION 
         */

        /// <summary>
        /// The serialize.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <returns>
        /// </returns>
        public XElement Serialize(Expression e)
        {
            return this.GenerateXmlFromExpressionCore(e);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The apply custom converters.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <returns>
        /// </returns>
        private XElement ApplyCustomConverters(Expression e)
        {
            foreach (var converter in this.Converters)
            {
                XElement result = converter.Serialize(e);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// The apply custom deserializers.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        private Expression ApplyCustomDeserializers(XElement xml)
        {
            foreach (var converter in this.Converters)
            {
                Expression result = converter.Deserialize(xml);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// The as i enumerable of.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        private IEnumerable<T> AsIEnumerableOf<T>(object value)
        {
            if (value == null)
            {
                return null;
            }

            return (value as IEnumerable).Cast<T>();
        }

        /// <summary>
        /// The generate xml from assignment.
        /// </summary>
        /// <param name="memberAssignment">
        /// The member assignment.
        /// </param>
        /// <returns>
        /// The generate xml from assignment.
        /// </returns>
        private object GenerateXmlFromAssignment(MemberAssignment memberAssignment)
        {
            return new XElement(
                "MemberAssignment", 
                this.GenerateXmlFromProperty(memberAssignment.Member.GetType(), "Member", memberAssignment.Member), 
                this.GenerateXmlFromProperty(
                    memberAssignment.Expression.GetType(), "Expression", memberAssignment.Expression));
        }

        /// <summary>
        /// The generate xml from binding.
        /// </summary>
        /// <param name="binding">
        /// The binding.
        /// </param>
        /// <returns>
        /// The generate xml from binding.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// </exception>
        private object GenerateXmlFromBinding(MemberBinding binding)
        {
            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    return this.GenerateXmlFromAssignment(binding as MemberAssignment);
                case MemberBindingType.ListBinding:
                    return this.GenerateXmlFromListBinding(binding as MemberListBinding);
                case MemberBindingType.MemberBinding:
                    return this.GenerateXmlFromMemberBinding(binding as MemberMemberBinding);
                default:
                    throw new NotSupportedException(
                        string.Format("Binding type {0} not supported.", binding.BindingType));
            }
        }

        /// <summary>
        /// The generate xml from binding list.
        /// </summary>
        /// <param name="propName">
        /// The prop name.
        /// </param>
        /// <param name="bindings">
        /// The bindings.
        /// </param>
        /// <returns>
        /// The generate xml from binding list.
        /// </returns>
        private object GenerateXmlFromBindingList(string propName, IEnumerable<MemberBinding> bindings)
        {
            if (bindings == null)
            {
                bindings = new MemberBinding[] { };
            }

            return new XElement(propName, from binding in bindings select this.GenerateXmlFromBinding(binding));
        }

        /// <summary>
        /// The generate xml from constructor info.
        /// </summary>
        /// <param name="propName">
        /// The prop name.
        /// </param>
        /// <param name="constructorInfo">
        /// The constructor info.
        /// </param>
        /// <returns>
        /// The generate xml from constructor info.
        /// </returns>
        private object GenerateXmlFromConstructorInfo(string propName, ConstructorInfo constructorInfo)
        {
            if (constructorInfo == null)
            {
                return new XElement(propName);
            }

            return new XElement(
                propName, 
                new XAttribute("MemberType", constructorInfo.MemberType), 
                new XAttribute("MethodName", constructorInfo.Name), 
                this.GenerateXmlFromType("DeclaringType", constructorInfo.DeclaringType), 
                new XElement(
                    "Parameters", 
                    from param in constructorInfo.GetParameters()
                    select
                        new XElement(
                        "Parameter", 
                        new XAttribute("Name", param.Name), 
                        this.GenerateXmlFromType("Type", param.ParameterType))));
        }

        /// <summary>
        /// The generate xml from element init list.
        /// </summary>
        /// <param name="propName">
        /// The prop name.
        /// </param>
        /// <param name="initializers">
        /// The initializers.
        /// </param>
        /// <returns>
        /// The generate xml from element init list.
        /// </returns>
        private object GenerateXmlFromElementInitList(string propName, IEnumerable<ElementInit> initializers)
        {
            if (initializers == null)
            {
                initializers = new ElementInit[] { };
            }

            return new XElement(
                propName, from elementInit in initializers select this.GenerateXmlFromElementInitializer(elementInit));
        }

        /// <summary>
        /// The generate xml from element initializer.
        /// </summary>
        /// <param name="elementInit">
        /// The element init.
        /// </param>
        /// <returns>
        /// The generate xml from element initializer.
        /// </returns>
        private object GenerateXmlFromElementInitializer(ElementInit elementInit)
        {
            return new XElement(
                "ElementInit", 
                this.GenerateXmlFromMethodInfo("AddMethod", elementInit.AddMethod), 
                this.GenerateXmlFromExpressionList("Arguments", elementInit.Arguments));
        }

        /// <summary>
        /// The generate xml from expression.
        /// </summary>
        /// <param name="propName">
        /// The prop name.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <returns>
        /// </returns>
        private XElement GenerateXmlFromExpression(string propName, Expression e)
        {
            return new XElement(propName, this.GenerateXmlFromExpressionCore(e));
        }

        /// <summary>
        /// The generate xml from expression core.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <returns>
        /// </returns>
        private XElement GenerateXmlFromExpressionCore(Expression e)
        {
            if (e == null)
            {
                return null;
            }

            XElement replace = this.ApplyCustomConverters(e);
            if (replace != null)
            {
                return replace;
            }

            return new XElement(
                this.GetNameOfExpression(e), 
                from prop in e.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                select this.GenerateXmlFromProperty(prop.PropertyType, prop.Name, prop.GetValue(e, null)));
        }

        /// <summary>
        /// The generate xml from expression list.
        /// </summary>
        /// <param name="propName">
        /// The prop name.
        /// </param>
        /// <param name="expressions">
        /// The expressions.
        /// </param>
        /// <returns>
        /// The generate xml from expression list.
        /// </returns>
        private object GenerateXmlFromExpressionList(string propName, IEnumerable<Expression> expressions)
        {
            return new XElement(
                propName, from expression in expressions select this.GenerateXmlFromExpressionCore(expression));
        }

        /// <summary>
        /// The generate xml from field info.
        /// </summary>
        /// <param name="propName">
        /// The prop name.
        /// </param>
        /// <param name="fieldInfo">
        /// The field info.
        /// </param>
        /// <returns>
        /// The generate xml from field info.
        /// </returns>
        private object GenerateXmlFromFieldInfo(string propName, FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
            {
                return new XElement(propName);
            }

            return new XElement(
                propName, 
                new XAttribute("MemberType", fieldInfo.MemberType), 
                new XAttribute("FieldName", fieldInfo.Name), 
                this.GenerateXmlFromType("DeclaringType", fieldInfo.DeclaringType));
        }

        /// <summary>
        /// The generate xml from list binding.
        /// </summary>
        /// <param name="memberListBinding">
        /// The member list binding.
        /// </param>
        /// <returns>
        /// The generate xml from list binding.
        /// </returns>
        private object GenerateXmlFromListBinding(MemberListBinding memberListBinding)
        {
            return new XElement(
                "MemberListBinding", 
                this.GenerateXmlFromProperty(memberListBinding.Member.GetType(), "Member", memberListBinding.Member), 
                this.GenerateXmlFromProperty(
                    memberListBinding.Initializers.GetType(), "Initializers", memberListBinding.Initializers));
        }

        /// <summary>
        /// The generate xml from member binding.
        /// </summary>
        /// <param name="memberMemberBinding">
        /// The member member binding.
        /// </param>
        /// <returns>
        /// The generate xml from member binding.
        /// </returns>
        private object GenerateXmlFromMemberBinding(MemberMemberBinding memberMemberBinding)
        {
            return new XElement(
                "MemberMemberBinding", 
                this.GenerateXmlFromProperty(memberMemberBinding.Member.GetType(), "Member", memberMemberBinding.Member), 
                this.GenerateXmlFromBindingList("Bindings", memberMemberBinding.Bindings));
        }

        /// <summary>
        /// The generate xml from member info list.
        /// </summary>
        /// <param name="propName">
        /// The prop name.
        /// </param>
        /// <param name="members">
        /// The members.
        /// </param>
        /// <returns>
        /// The generate xml from member info list.
        /// </returns>
        private object GenerateXmlFromMemberInfoList(string propName, IEnumerable<MemberInfo> members)
        {
            if (members == null)
            {
                members = new MemberInfo[] { };
            }

            return new XElement(
                propName, from member in members select this.GenerateXmlFromProperty(member.GetType(), "Info", member));
        }

        /// <summary>
        /// The generate xml from method info.
        /// </summary>
        /// <param name="propName">
        /// The prop name.
        /// </param>
        /// <param name="methodInfo">
        /// The method info.
        /// </param>
        /// <returns>
        /// The generate xml from method info.
        /// </returns>
        private object GenerateXmlFromMethodInfo(string propName, MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                return new XElement(propName);
            }

            return new XElement(
                propName, 
                new XAttribute("MemberType", methodInfo.MemberType), 
                new XAttribute("MethodName", methodInfo.Name), 
                this.GenerateXmlFromType("DeclaringType", methodInfo.DeclaringType), 
                new XElement(
                    "Parameters", 
                    from param in methodInfo.GetParameters()
                    select this.GenerateXmlFromType("Type", param.ParameterType)), 
                new XElement(
                    "GenericArgTypes", 
                    from argType in methodInfo.GetGenericArguments() select this.GenerateXmlFromType("Type", argType)));
        }

        /// <summary>
        /// The generate xml from object.
        /// </summary>
        /// <param name="propName">
        /// The prop name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The generate xml from object.
        /// </returns>
        private object GenerateXmlFromObject(string propName, object value)
        {
            object result = null;
            if (value is Type)
            {
                result = this.GenerateXmlFromTypeCore((Type)value);
            }

            if (result == null)
            {
                result = value.ToString();
            }

            return new XElement(propName, result);
        }

        /// <summary>
        /// The generate xml from primitive.
        /// </summary>
        /// <param name="propName">
        /// The prop name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The generate xml from primitive.
        /// </returns>
        private object GenerateXmlFromPrimitive(string propName, object value)
        {
            return new XAttribute(propName, value);
        }

        /// <summary>
        /// The generate xml from property.
        /// </summary>
        /// <param name="propType">
        /// The prop type.
        /// </param>
        /// <param name="propName">
        /// The prop name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The generate xml from property.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// </exception>
        private object GenerateXmlFromProperty(Type propType, string propName, object value)
        {
            if (AttributeTypes.Contains(propType))
            {
                return this.GenerateXmlFromPrimitive(propName, value);
            }

            if (propType.Equals(typeof(object)))
            {
                return this.GenerateXmlFromObject(propName, value);
            }

            if (typeof(Expression).IsAssignableFrom(propType))
            {
                return this.GenerateXmlFromExpression(propName, value as Expression);
            }

            if (value is MethodInfo || propType.Equals(typeof(MethodInfo)))
            {
                return this.GenerateXmlFromMethodInfo(propName, value as MethodInfo);
            }

            if (value is PropertyInfo || propType.Equals(typeof(PropertyInfo)))
            {
                return this.GenerateXmlFromPropertyInfo(propName, value as PropertyInfo);
            }

            if (value is FieldInfo || propType.Equals(typeof(FieldInfo)))
            {
                return this.GenerateXmlFromFieldInfo(propName, value as FieldInfo);
            }

            if (value is ConstructorInfo || propType.Equals(typeof(ConstructorInfo)))
            {
                return this.GenerateXmlFromConstructorInfo(propName, value as ConstructorInfo);
            }

            if (propType.Equals(typeof(Type)))
            {
                return this.GenerateXmlFromType(propName, value as Type);
            }

            if (this.IsIEnumerableOf<Expression>(propType))
            {
                return this.GenerateXmlFromExpressionList(propName, this.AsIEnumerableOf<Expression>(value));
            }

            if (this.IsIEnumerableOf<MemberInfo>(propType))
            {
                return this.GenerateXmlFromMemberInfoList(propName, this.AsIEnumerableOf<MemberInfo>(value));
            }

            if (this.IsIEnumerableOf<ElementInit>(propType))
            {
                return this.GenerateXmlFromElementInitList(propName, this.AsIEnumerableOf<ElementInit>(value));
            }

            if (this.IsIEnumerableOf<MemberBinding>(propType))
            {
                return this.GenerateXmlFromBindingList(propName, this.AsIEnumerableOf<MemberBinding>(value));
            }

            throw new NotSupportedException(propName);
        }

        /// <summary>
        /// The generate xml from property info.
        /// </summary>
        /// <param name="propName">
        /// The prop name.
        /// </param>
        /// <param name="propertyInfo">
        /// The property info.
        /// </param>
        /// <returns>
        /// The generate xml from property info.
        /// </returns>
        private object GenerateXmlFromPropertyInfo(string propName, PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                return new XElement(propName);
            }

            return new XElement(
                propName, 
                new XAttribute("MemberType", propertyInfo.MemberType), 
                new XAttribute("PropertyName", propertyInfo.Name), 
                this.GenerateXmlFromType("DeclaringType", propertyInfo.DeclaringType), 
                new XElement(
                    "IndexParameters", 
                    from param in propertyInfo.GetIndexParameters()
                    select this.GenerateXmlFromType("Type", param.ParameterType)));
        }

        /// <summary>
        /// The generate xml from type.
        /// </summary>
        /// <param name="propName">
        /// The prop name.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The generate xml from type.
        /// </returns>
        private object GenerateXmlFromType(string propName, Type type)
        {
            return new XElement(propName, this.GenerateXmlFromTypeCore(type));
        }

        /// <summary>
        /// The generate xml from type core.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// </returns>
        private XElement GenerateXmlFromTypeCore(Type type)
        {
            // vsadov: add detection of VB anon types
            if (type.Name.StartsWith("<>f__") || type.Name.StartsWith("VB$AnonymousType"))
            {
                return new XElement(
                    "AnonymousType", 
                    new XAttribute("Name", type.FullName), 
                    from property in type.GetProperties()
                    select
                        new XElement(
                        "Property", 
                        new XAttribute("Name", property.Name), 
                        this.GenerateXmlFromTypeCore(property.PropertyType)), 
                    new XElement(
                        "Constructor", 
                        from parameter in type.GetConstructors().First().GetParameters()
                        select
                            new XElement(
                            "Parameter", 
                            new XAttribute("Name", parameter.Name), 
                            this.GenerateXmlFromTypeCore(parameter.ParameterType))));
            }
            else
            {
                // vsadov: GetGenericArguments returns args for nongeneric types 
                // like arrays no need to save them.
                if (type.IsGenericType)
                {
                    return new XElement(
                        "Type", 
                        new XAttribute("Name", type.GetGenericTypeDefinition().FullName), 
                        from genArgType in type.GetGenericArguments() select this.GenerateXmlFromTypeCore(genArgType));
                }
                else
                {
                    return new XElement("Type", new XAttribute("Name", type.FullName));
                }
            }
        }

        /// <summary>
        /// The get name of expression.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <returns>
        /// The get name of expression.
        /// </returns>
        private string GetNameOfExpression(Expression e)
        {
            if (e is LambdaExpression)
            {
                return "LambdaExpression";
            }

            return e.GetType().Name;
        }

        /// <summary>
        /// The is i enumerable of.
        /// </summary>
        /// <param name="propType">
        /// The prop type.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The is i enumerable of.
        /// </returns>
        private bool IsIEnumerableOf<T>(Type propType)
        {
            if (!propType.IsGenericType)
            {
                return false;
            }

            Type[] typeArgs = propType.GetGenericArguments();
            if (typeArgs.Length != 1)
            {
                return false;
            }

            if (!typeof(T).IsAssignableFrom(typeArgs[0]))
            {
                return false;
            }

            if (!typeof(IEnumerable<>).MakeGenericType(typeArgs).IsAssignableFrom(propType))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The parse anonymous type from xml core.
        /// </summary>
        /// <param name="xElement">
        /// The x element.
        /// </param>
        /// <returns>
        /// </returns>
        private Type ParseAnonymousTypeFromXmlCore(XElement xElement)
        {
            string name = xElement.Attribute("Name").Value;
            var properties = from propXml in xElement.Elements("Property")
                             select
                                 new ExpressionSerializationTypeResolver.NameTypePair
                                     {
                                        Name = propXml.Attribute("Name").Value, Type = this.ParseTypeFromXml(propXml) 
                                     };
            var ctr_params = from propXml in xElement.Elements("Constructor").Elements("Parameter")
                             select
                                 new ExpressionSerializationTypeResolver.NameTypePair
                                     {
                                        Name = propXml.Attribute("Name").Value, Type = this.ParseTypeFromXml(propXml) 
                                     };

            return this.resolver.GetOrCreateAnonymousTypeFor(name, properties.ToArray(), ctr_params.ToArray());
        }

        /// <summary>
        /// The parse binary expresssion from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        private Expression ParseBinaryExpresssionFromXml(XElement xml)
        {
            var expressionType = (ExpressionType)this.ParseConstantFromAttribute<ExpressionType>(xml, "NodeType");
            
            var left = this.ParseExpressionFromXml(xml.Element("Left"));
            var right = this.ParseExpressionFromXml(xml.Element("Right"));
            var isLifted = (bool)this.ParseConstantFromAttribute<bool>(xml, "IsLifted");
            var isLiftedToNull = (bool)this.ParseConstantFromAttribute<bool>(xml, "IsLiftedToNull");
            var type = this.ParseTypeFromXml(xml.Element("Type"));
            var method = this.ParseMethodInfoFromXml(xml.Element("Method"));
            var conversion = this.ParseExpressionFromXml(xml.Element("Conversion")) as LambdaExpression;
            if (expressionType == ExpressionType.Coalesce)
            {
                return Expression.Coalesce(left, right, conversion);
            }

            return Expression.MakeBinary(expressionType, left, right, isLiftedToNull, method);
        }

        /// <summary>
        /// The parse binding from xml.
        /// </summary>
        /// <param name="tXml">
        /// The t xml.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        private MemberBinding ParseBindingFromXml(XElement tXml)
        {
            MemberInfo member = this.ParseMemberInfoFromXml(tXml.Element("Member"));
            switch (tXml.Name.LocalName)
            {
                case "MemberAssignment":
                    Expression expression = this.ParseExpressionFromXml(tXml.Element("Expression"));
                    return Expression.Bind(member, expression);
                case "MemberMemberBinding":
                    var bindings = this.ParseBindingListFromXml(tXml, "Bindings");
                    return Expression.MemberBind(member, bindings);
                case "MemberListBinding":
                    var initializers = this.ParseElementInitListFromXml(tXml, "Initializers");
                    return Expression.ListBind(member, initializers);
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// The parse binding list from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <param name="elemName">
        /// The elem name.
        /// </param>
        /// <returns>
        /// </returns>
        private IEnumerable<MemberBinding> ParseBindingListFromXml(XElement xml, string elemName)
        {
            return from tXml in xml.Element(elemName).Elements() select this.ParseBindingFromXml(tXml);
        }

        /// <summary>
        /// The parse conditional expression from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        private Expression ParseConditionalExpressionFromXml(XElement xml)
        {
            Expression test = this.ParseExpressionFromXml(xml.Element("Test"));
            Expression ifTrue = this.ParseExpressionFromXml(xml.Element("IfTrue"));
            Expression ifFalse = this.ParseExpressionFromXml(xml.Element("IfFalse"));
            return Expression.Condition(test, ifTrue, ifFalse);
        }

        /// <summary>
        /// The parse constant from attribute.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <param name="attrName">
        /// The attr name.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The parse constant from attribute.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        private object ParseConstantFromAttribute<T>(XElement xml, string attrName)
        {
            string objectStringValue = xml.Attribute(attrName).Value;
            if (typeof(Type).IsAssignableFrom(typeof(T)))
            {
                throw new Exception("We should never be encoding Types in attributes now.");
            }

            if (typeof(Enum).IsAssignableFrom(typeof(T)))
            {
                return Enum.Parse(typeof(T), objectStringValue);
            }

            return Convert.ChangeType(objectStringValue, typeof(T));
        }

        /// <summary>
        /// The parse constant from attribute.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <param name="attrName">
        /// The attr name.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The parse constant from attribute.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        private object ParseConstantFromAttribute(XElement xml, string attrName, Type type)
        {
            string objectStringValue = xml.Attribute(attrName).Value;
            if (typeof(Type).IsAssignableFrom(type))
            {
                throw new Exception("We should never be encoding Types in attributes now.");
            }

            if (typeof(Enum).IsAssignableFrom(type))
            {
                return Enum.Parse(type, objectStringValue);
            }

            return Convert.ChangeType(objectStringValue, type);
        }

        /// <summary>
        /// The parse constant from element.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <param name="elemName">
        /// The elem name.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The parse constant from element.
        /// </returns>
        private object ParseConstantFromElement(XElement xml, string elemName, Type type)
        {
            string objectStringValue = xml.Element(elemName).Value;
            if (typeof(Type).IsAssignableFrom(type))
            {
                return this.ParseTypeFromXml(xml.Element("Value"));
            }

            if (typeof(Enum).IsAssignableFrom(type))
            {
                return Enum.Parse(type, objectStringValue);
            }

            return Convert.ChangeType(objectStringValue, type);
        }

        /// <summary>
        /// The parse constat expression from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        private Expression ParseConstatExpressionFromXml(XElement xml)
        {
            Type type = this.ParseTypeFromXml(xml.Element("Type"));
            return Expression.Constant(this.ParseConstantFromElement(xml, "Value", type), type);
        }

        /// <summary>
        /// The parse constructor info from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        private ConstructorInfo ParseConstructorInfoFromXml(XElement xml)
        {
            if (xml.IsEmpty)
            {
                return null;
            }

            Type declaringType = this.ParseTypeFromXml(xml.Element("DeclaringType"));
            var ps = from paramXml in xml.Element("Parameters").Elements() select this.ParseParameterFromXml(paramXml);
            ConstructorInfo ci = declaringType.GetConstructor(ps.ToArray());
            return ci;
        }

        /// <summary>
        /// The parse element init from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        private ElementInit ParseElementInitFromXml(XElement xml)
        {
            MethodInfo addMethod = this.ParseMethodInfoFromXml(xml.Element("AddMethod"));
            var arguments = this.ParseExpressionListFromXml<Expression>(xml, "Arguments");
            return Expression.ElementInit(addMethod, arguments);
        }

        /// <summary>
        /// The parse element init list from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <param name="elemName">
        /// The elem name.
        /// </param>
        /// <returns>
        /// </returns>
        private IEnumerable<ElementInit> ParseElementInitListFromXml(XElement xml, string elemName)
        {
            return from tXml in xml.Element(elemName).Elements() select this.ParseElementInitFromXml(tXml);
        }

        /// <summary>
        /// The parse expression from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        private Expression ParseExpressionFromXml(XElement xml)
        {
            if (xml.IsEmpty)
            {
                return null;
            }

            return this.ParseExpressionFromXmlNonNull(xml.Elements().First());
        }

        /// <summary>
        /// The parse expression from xml non null.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// </exception>
        private Expression ParseExpressionFromXmlNonNull(XElement xml)
        {
            Expression expression = this.ApplyCustomDeserializers(xml);
            if (expression != null)
            {
                return expression;
            }

            switch (xml.Name.LocalName)
            {
                case "BinaryExpression":
                    return this.ParseBinaryExpresssionFromXml(xml);
                case "ConstantExpression":
                    return this.ParseConstatExpressionFromXml(xml);
                case "ParameterExpression":
                    return this.ParseParameterExpressionFromXml(xml);
                case "LambdaExpression":
                    return this.ParseLambdaExpressionFromXml(xml);
                case "MethodCallExpression":
                    return this.ParseMethodCallExpressionFromXml(xml);
                case "UnaryExpression":
                    return this.ParseUnaryExpressionFromXml(xml);
                case "MemberExpression":
                    return this.ParseMemberExpressionFromXml(xml);
                case "NewExpression":
                    return this.ParseNewExpressionFromXml(xml);
                case "ListInitExpression":
                    return this.ParseListInitExpressionFromXml(xml);
                case "MemberInitExpression":
                    return this.ParseMemberInitExpressionFromXml(xml);
                case "ConditionalExpression":
                    return this.ParseConditionalExpressionFromXml(xml);
                case "NewArrayExpression":
                    return this.ParseNewArrayExpressionFromXml(xml);
                case "TypeBinaryExpression":
                    return this.ParseTypeBinaryExpressionFromXml(xml);
                case "InvocationExpression":
                    return this.ParseInvocationExpressionFromXml(xml);
                default:
                    throw new NotSupportedException(xml.Name.LocalName);
            }
        }

        /// <summary>
        /// The parse expression list from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <param name="elemName">
        /// The elem name.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        private IEnumerable<T> ParseExpressionListFromXml<T>(XElement xml, string elemName) where T : Expression
        {
            return from tXml in xml.Element(elemName).Elements() select (T)this.ParseExpressionFromXmlNonNull(tXml);
        }

        /// <summary>
        /// The parse field info from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        private MemberInfo ParseFieldInfoFromXml(XElement xml)
        {
            var fieldName = (string)this.ParseConstantFromAttribute<string>(xml, "FieldName");
            Type declaringType = this.ParseTypeFromXml(xml.Element("DeclaringType"));
            return declaringType.GetField(fieldName);
        }

        /// <summary>
        /// The parse invocation expression from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        private Expression ParseInvocationExpressionFromXml(XElement xml)
        {
            Expression expression = this.ParseExpressionFromXml(xml.Element("Expression"));
            var arguments = this.ParseExpressionListFromXml<Expression>(xml, "Arguments");
            return Expression.Invoke(expression, arguments);
        }

        /// <summary>
        /// The parse lambda expression from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        private Expression ParseLambdaExpressionFromXml(XElement xml)
        {
            var body = this.ParseExpressionFromXml(xml.Element("Body"));
            var parameters = this.ParseExpressionListFromXml<ParameterExpression>(xml, "Parameters");
            var type = this.ParseTypeFromXml(xml.Element("Type"));

            // We may need to 
            // var lambdaExpressionReturnType = type.GetMethod("Invoke").ReturnType;
            // if (lambdaExpressionReturnType.IsArray)
            // {

            // type = typeof(IEnumerable<>).MakeGenericType(type.GetElementType());
            // }
            return Expression.Lambda(type, body, parameters);
        }

        /// <summary>
        /// The parse list init expression from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        private Expression ParseListInitExpressionFromXml(XElement xml)
        {
            var newExpression = this.ParseExpressionFromXml(xml.Element("NewExpression")) as NewExpression;
            if (newExpression == null)
            {
                throw new Exception("Expceted a NewExpression");
            }

            var initializers = this.ParseElementInitListFromXml(xml, "Initializers").ToArray();
            return Expression.ListInit(newExpression, initializers);
        }

        /// <summary>
        /// The parse member expression from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        private Expression ParseMemberExpressionFromXml(XElement xml)
        {
            Expression expression = this.ParseExpressionFromXml(xml.Element("Expression"));
            MemberInfo member = this.ParseMemberInfoFromXml(xml.Element("Member"));
            return Expression.MakeMemberAccess(expression, member);
        }

        /// <summary>
        /// The parse member info from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// </exception>
        private MemberInfo ParseMemberInfoFromXml(XElement xml)
        {
            var memberType = (MemberTypes)this.ParseConstantFromAttribute<MemberTypes>(xml, "MemberType");
            switch (memberType)
            {
                case MemberTypes.Field:
                    return this.ParseFieldInfoFromXml(xml);
                case MemberTypes.Property:
                    return this.ParsePropertyInfoFromXml(xml);
                case MemberTypes.Method:
                    return this.ParseMethodInfoFromXml(xml);
                case MemberTypes.Constructor:
                    return this.ParseConstructorInfoFromXml(xml);
                case MemberTypes.Custom:
                case MemberTypes.Event:
                case MemberTypes.NestedType:
                case MemberTypes.TypeInfo:
                default:
                    throw new NotSupportedException(string.Format("MEmberType {0} not supported", memberType));
            }
        }

        /// <summary>
        /// The parse member info list from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <param name="elemName">
        /// The elem name.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        private IEnumerable<T> ParseMemberInfoListFromXml<T>(XElement xml, string elemName) where T : MemberInfo
        {
            return from tXml in xml.Element(elemName).Elements() select (T)this.ParseMemberInfoFromXml(tXml);
        }

        /// <summary>
        /// The parse member init expression from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        private Expression ParseMemberInitExpressionFromXml(XElement xml)
        {
            var newExpression =
                this.ParseNewExpressionFromXml(xml.Element("NewExpression").Element("NewExpression")) as NewExpression;
            var bindings = this.ParseBindingListFromXml(xml, "Bindings").ToArray();
            return Expression.MemberInit(newExpression, bindings);
        }

        /// <summary>
        /// The parse method call expression from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        private Expression ParseMethodCallExpressionFromXml(XElement xml)
        {
            Expression instance = this.ParseExpressionFromXml(xml.Element("Object"));
            MethodInfo method = this.ParseMethodInfoFromXml(xml.Element("Method"));
            var arguments = this.ParseExpressionListFromXml<Expression>(xml, "Arguments").ToArray();
            return Expression.Call(instance, method, arguments);
        }

        /// <summary>
        /// The parse method info from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        private MethodInfo ParseMethodInfoFromXml(XElement xml)
        {
            if (xml.IsEmpty)
            {
                return null;
            }

            var name = (string)this.ParseConstantFromAttribute<string>(xml, "MethodName");
            Type declaringType = this.ParseTypeFromXml(xml.Element("DeclaringType"));
            var ps = from paramXml in xml.Element("Parameters").Elements() select this.ParseTypeFromXml(paramXml);
            var genArgs = from argXml in xml.Element("GenericArgTypes").Elements() select this.ParseTypeFromXml(argXml);
            return this.resolver.GetMethod(declaringType, name, ps.ToArray(), genArgs.ToArray());
        }

        /// <summary>
        /// The parse new array expression from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        private Expression ParseNewArrayExpressionFromXml(XElement xml)
        {
            Type type = this.ParseTypeFromXml(xml.Element("Type"));
            if (!type.IsArray)
            {
                throw new Exception("Expected array type");
            }

            Type elemType = type.GetElementType();
            var expressions = this.ParseExpressionListFromXml<Expression>(xml, "Expressions");
            switch (xml.Attribute("NodeType").Value)
            {
                case "NewArrayInit":
                    return Expression.NewArrayInit(elemType, expressions);
                case "NewArrayBounds":
                    return Expression.NewArrayBounds(elemType, expressions);
                default:
                    throw new Exception("Expected NewArrayInit or NewArrayBounds");
            }
        }

        /// <summary>
        /// The parse new expression from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        private Expression ParseNewExpressionFromXml(XElement xml)
        {
            ConstructorInfo constructor = this.ParseConstructorInfoFromXml(xml.Element("Constructor"));
            var arguments = this.ParseExpressionListFromXml<Expression>(xml, "Arguments").ToArray();
            var members = this.ParseMemberInfoListFromXml<MemberInfo>(xml, "Members").ToArray();
            if (members.Length == 0)
            {
                return Expression.New(constructor, arguments);
            }

            return Expression.New(constructor, arguments, members);
        }

        /// <summary>
        /// The parse normal type from xml core.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        private Type ParseNormalTypeFromXmlCore(XElement xml)
        {
            if (!xml.HasElements)
            {
                return this.resolver.GetType(xml.Attribute("Name").Value);
            }

            var genericArgumentTypes = from genArgXml in xml.Elements() select this.ParseTypeFromXmlCore(genArgXml);
            return this.resolver.GetType(xml.Attribute("Name").Value, genericArgumentTypes);
        }

        /// <summary>
        /// The parse parameter expression from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        private Expression ParseParameterExpressionFromXml(XElement xml)
        {
            Type type = this.ParseTypeFromXml(xml.Element("Type"));
            var name = (string)this.ParseConstantFromAttribute<string>(xml, "Name");

            // vs: hack
            string id = name + type.FullName;
            if (!this.parameters.ContainsKey(id))
            {
                this.parameters.Add(id, Expression.Parameter(type, name));
            }

            return this.parameters[id];
        }

        /// <summary>
        /// The parse parameter from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        private Type ParseParameterFromXml(XElement xml)
        {
            var name = (string)this.ParseConstantFromAttribute<string>(xml, "Name");
            Type type = this.ParseTypeFromXml(xml.Element("Type"));
            return type;
        }

        /// <summary>
        /// The parse property info from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        private MemberInfo ParsePropertyInfoFromXml(XElement xml)
        {
            var propertyName = (string)this.ParseConstantFromAttribute<string>(xml, "PropertyName");
            Type declaringType = this.ParseTypeFromXml(xml.Element("DeclaringType"));
            var ps = from paramXml in xml.Element("IndexParameters").Elements() select this.ParseTypeFromXml(paramXml);
            return declaringType.GetProperty(propertyName, ps.ToArray());
        }

        /// <summary>
        /// The parse type binary expression from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        private Expression ParseTypeBinaryExpressionFromXml(XElement xml)
        {
            Expression expression = this.ParseExpressionFromXml(xml.Element("Expression"));
            Type typeOperand = this.ParseTypeFromXml(xml.Element("TypeOperand"));
            return Expression.TypeIs(expression, typeOperand);
        }

        /// <summary>
        /// The parse type from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        private Type ParseTypeFromXml(XElement xml)
        {
            Debug.Assert(xml.Elements().Count() == 1);
            return this.ParseTypeFromXmlCore(xml.Elements().First());
        }

        /// <summary>
        /// The parse type from xml core.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        private Type ParseTypeFromXmlCore(XElement xml)
        {
            switch (xml.Name.ToString())
            {
                case "Type":
                    return this.ParseNormalTypeFromXmlCore(xml);
                case "AnonymousType":
                    return this.ParseAnonymousTypeFromXmlCore(xml);
                default:
                    throw new ArgumentException("Expected 'Type' or 'AnonymousType'");
            }
        }

        /// <summary>
        /// The parse unary expression from xml.
        /// </summary>
        /// <param name="xml">
        /// The xml.
        /// </param>
        /// <returns>
        /// </returns>
        private Expression ParseUnaryExpressionFromXml(XElement xml)
        {
            Expression operand = this.ParseExpressionFromXml(xml.Element("Operand"));
            MethodInfo method = this.ParseMethodInfoFromXml(xml.Element("Method"));
            var isLifted = (bool)this.ParseConstantFromAttribute<bool>(xml, "IsLifted");
            var isLiftedToNull = (bool)this.ParseConstantFromAttribute<bool>(xml, "IsLiftedToNull");
            var expressionType = (ExpressionType)this.ParseConstantFromAttribute<ExpressionType>(xml, "NodeType");
            var type = this.ParseTypeFromXml(xml.Element("Type"));

            // TODO: Why can't we use IsLifted and IsLiftedToNull here?  
            // May need to special case a nodeType if it needs them.
            return Expression.MakeUnary(expressionType, operand, type, method);
        }

        #endregion
    }
}