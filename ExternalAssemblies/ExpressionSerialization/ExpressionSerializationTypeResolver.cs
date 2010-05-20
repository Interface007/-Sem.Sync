// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionSerializationTypeResolver.cs" company="">
//   
// </copyright>
// <summary>
//   The custom expression xml converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExpressionSerialization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Xml.Linq;

    /// <summary>
    /// The custom expression xml converter.
    /// </summary>
    public abstract class CustomExpressionXmlConverter
    {
        #region Public Methods

        /// <summary>
        /// The deserialize.
        /// </summary>
        /// <param name="expressionXml">
        /// The expression xml.
        /// </param>
        /// <returns>
        /// </returns>
        public abstract Expression Deserialize(XElement expressionXml);

        /// <summary>
        /// The serialize.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <returns>
        /// </returns>
        public abstract XElement Serialize(Expression expression);

        #endregion
    }

    /// <summary>
    /// The expression serialization type resolver.
    /// </summary>
    public class ExpressionSerializationTypeResolver
    {
        /// <summary>
        /// The anonymous types.
        /// </summary>
        private readonly Dictionary<AnonTypeId, Type> anonymousTypes = new Dictionary<AnonTypeId, Type>();

        /// <summary>
        /// The module builder.
        /// </summary>
        private readonly ModuleBuilder moduleBuilder;

        /// <summary>
        /// The anonymous type index.
        /// </summary>
        private int anonymousTypeIndex;

        // vsadov: hack to force loading of VB runtime.
#pragma warning disable 169

        // ReSharper disable InconsistentNaming
        /// <summary>
        /// The vb_hack.
        /// </summary>
        private int vb_hack = Microsoft.VisualBasic.CompilerServices.Operators.CompareString("qq", "qq", true);

        // ReSharper restore InconsistentNaming
#pragma warning restore 169

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionSerializationTypeResolver"/> class.
        /// </summary>
        public ExpressionSerializationTypeResolver()
        {
            var asmname = new AssemblyName { Name = "AnonymousTypes" };
            var assemblyBuilder = System.Threading.Thread.GetDomain().DefineDynamicAssembly(
                asmname, AssemblyBuilderAccess.RunAndSave);
            this.moduleBuilder = assemblyBuilder.DefineDynamicModule("AnonymousTypes");
        }

        /// <summary>
        /// The resolve type from string.
        /// </summary>
        /// <param name="typeString">
        /// The type string.
        /// </param>
        /// <returns>
        /// </returns>
        protected virtual Type ResolveTypeFromString(string typeString)
        {
            return null;
        }

        /// <summary>
        /// The resolve string from type.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The resolve string from type.
        /// </returns>
        protected virtual string ResolveStringFromType(Type type)
        {
            return null;
        }

        /// <summary>
        /// The get type.
        /// </summary>
        /// <param name="typeName">
        /// The type name.
        /// </param>
        /// <param name="genericArgumentTypes">
        /// The generic argument types.
        /// </param>
        /// <returns>
        /// </returns>
        public Type GetType(string typeName, IEnumerable<Type> genericArgumentTypes)
        {
            return this.GetType(typeName).MakeGenericType(genericArgumentTypes.ToArray());
        }

        /// <summary>
        /// The get type.
        /// </summary>
        /// <param name="typeName">
        /// The type name.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public Type GetType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentNullException("typeName");
            }

            // First - try all replacers
            var type = this.ResolveTypeFromString(typeName);

            // type = typeReplacers.Select(f => f(typeName)).FirstOrDefault();
            if (type != null)
            {
                return type;
            }

            // If it's an array name - get the element type and wrap in the array type.
            if (typeName.EndsWith("[]"))
            {
                return this.GetType(typeName.Substring(0, typeName.Length - 2)).MakeArrayType();
            }

            // First - try all loaded types
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = assembly.GetType(typeName, false, true);
                if (type != null)
                {
                    return type;
                }
            }

            // Second - try just plain old Type.GetType()
            type = Type.GetType(typeName, false, true);
            if (type != null)
            {
                return type;
            }

            throw new ArgumentException("Could not find a matching type", typeName);
        }

        /// <summary>
        /// The name type pair.
        /// </summary>
        public class NameTypePair
        {
            #region Properties

            /// <summary>
            /// Gets or sets Name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets Type.
            /// </summary>
            public Type Type { get; set; }

            #endregion

            #region Public Methods

            /// <summary>
            /// The equals.
            /// </summary>
            /// <param name="obj">
            /// The obj.
            /// </param>
            /// <returns>
            /// The equals.
            /// </returns>
            public override bool Equals(object obj)
            {
                if (!(obj is NameTypePair))
                {
                    return false;
                }

                var other = obj as NameTypePair;
                return this.Name.Equals(other.Name) && this.Type.Equals(other.Type);
            }

            /// <summary>
            /// The get hash code.
            /// </summary>
            /// <returns>
            /// The get hash code.
            /// </returns>
            public override int GetHashCode()
            {
                return this.Name.GetHashCode() + this.Type.GetHashCode();
            }

            #endregion
        }

        /// <summary>
        /// The anon type id.
        /// </summary>
        private class AnonTypeId
        {
            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="AnonTypeId"/> class.
            /// </summary>
            /// <param name="name">
            /// The name.
            /// </param>
            /// <param name="properties">
            /// The properties.
            /// </param>
            public AnonTypeId(string name, IEnumerable<NameTypePair> properties)
            {
                this.Name = name;
                this.Properties = properties;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets Name.
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// Gets Properties.
            /// </summary>
            public IEnumerable<NameTypePair> Properties { get; private set; }

            #endregion

            #region Public Methods

            /// <summary>
            /// The equals.
            /// </summary>
            /// <param name="obj">
            /// The obj.
            /// </param>
            /// <returns>
            /// The equals.
            /// </returns>
            public override bool Equals(object obj)
            {
                if (!(obj is AnonTypeId))
                {
                    return false;
                }

                var other = obj as AnonTypeId;
                return this.Name.Equals(other.Name) && this.Properties.SequenceEqual(other.Properties);
            }

            /// <summary>
            /// The get hash code.
            /// </summary>
            /// <returns>
            /// The get hash code.
            /// </returns>
            public override int GetHashCode()
            {
                return this.Name.GetHashCode() + this.Properties.Sum(ntpair => ntpair.GetHashCode());
            }

            #endregion
        }

        /// <summary>
        /// The get method.
        /// </summary>
        /// <param name="declaringType">
        /// The declaring type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="parameterTypes">
        /// The parameter types.
        /// </param>
        /// <param name="genArgTypes">
        /// The gen arg types.
        /// </param>
        /// <returns>
        /// </returns>
        public MethodInfo GetMethod(Type declaringType, string name, Type[] parameterTypes, Type[] genArgTypes)
        {
            var methods = from mi in declaringType.GetMethods() where mi.Name == name select mi;
            foreach (var method in methods)
            {
                // Would be nice to remvoe the try/catch
                try
                {
                    MethodInfo realMethod = method;
                    if (method.IsGenericMethod)
                    {
                        realMethod = method.MakeGenericMethod(genArgTypes);
                    }

                    var methodParameterTypes = realMethod.GetParameters().Select(p => p.ParameterType);
                    if (MatchPiecewise(parameterTypes, methodParameterTypes))
                    {
                        return realMethod;
                    }
                }
                catch (ArgumentException)
                {
                    continue;
                }
            }

            return null;
        }

        /// <summary>
        /// The match piecewise.
        /// </summary>
        /// <param name="first">
        /// The first.
        /// </param>
        /// <param name="second">
        /// The second.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The match piecewise.
        /// </returns>
        private static bool MatchPiecewise<T>(IEnumerable<T> first, IEnumerable<T> second)
        {
            var firstArray = first.ToArray();
            var secondArray = second.ToArray();
            if (firstArray.Length != secondArray.Length)
            {
                return false;
            }

            return !firstArray.Where((t, i) => !t.Equals(secondArray[i])).Any();
        }

        // vsadov: need to take ctor parameters too as they do not 
        // necessarily match properties order as returned by GetProperties
        /// <summary>
        /// The get or create anonymous type for.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="properties">
        /// The properties.
        /// </param>
        /// <param name="ctrParams">
        /// The ctr params.
        /// </param>
        /// <returns>
        /// </returns>
        public Type GetOrCreateAnonymousTypeFor(string name, NameTypePair[] properties, NameTypePair[] ctrParams)
        {
            var id = new AnonTypeId(name, properties.Concat(ctrParams));
            if (this.anonymousTypes.ContainsKey(id))
            {
                return this.anonymousTypes[id];
            }

            // vsadov: VB anon type. not necessary, just looks better
            var anonPrefix = name.StartsWith("<>") ? "<>f__AnonymousType" : "VB$AnonymousType_";
            var anonTypeBuilder = this.moduleBuilder.DefineType(
                anonPrefix + this.anonymousTypeIndex++, TypeAttributes.Public | TypeAttributes.Class);

            var fieldBuilders = new FieldBuilder[properties.Length];
            var propertyBuilders = new PropertyBuilder[properties.Length];

            for (int i = 0; i < properties.Length; i++)
            {
                fieldBuilders[i] = anonTypeBuilder.DefineField(
                    "_generatedfield_" + properties[i].Name, properties[i].Type, FieldAttributes.Private);
                propertyBuilders[i] = anonTypeBuilder.DefineProperty(
                    properties[i].Name, PropertyAttributes.None, properties[i].Type, new Type[0]);
                var propertyGetterBuilder = anonTypeBuilder.DefineMethod(
                    "get_" + properties[i].Name, MethodAttributes.Public, properties[i].Type, new Type[0]);
                var getterIlGenerator = propertyGetterBuilder.GetILGenerator();
                getterIlGenerator.Emit(OpCodes.Ldarg_0);
                getterIlGenerator.Emit(OpCodes.Ldfld, fieldBuilders[i]);
                getterIlGenerator.Emit(OpCodes.Ret);
                propertyBuilders[i].SetGetMethod(propertyGetterBuilder);
            }

            var constructorBuilder =
                anonTypeBuilder.DefineConstructor(
                    MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Public, 
                    CallingConventions.Standard, 
                    ctrParams.Select(prop => prop.Type).ToArray());
            var constructorIlGenerator = constructorBuilder.GetILGenerator();
            for (var i = 0; i < ctrParams.Length; i++)
            {
                constructorIlGenerator.Emit(OpCodes.Ldarg_0);
                constructorIlGenerator.Emit(OpCodes.Ldarg, i + 1);
                constructorIlGenerator.Emit(OpCodes.Stfld, fieldBuilders[i]);
                constructorBuilder.DefineParameter(i + 1, ParameterAttributes.None, ctrParams[i].Name);
            }

            constructorIlGenerator.Emit(OpCodes.Ret);

            // TODO - Define ToString() and GetHashCode implementations for our generated Anonymous Types
            // MethodBuilder toStringBuilder = anonTypeBuilder.DefineMethod();
            // MethodBuilder getHashCodeBuilder = anonTypeBuilder.DefineMethod();
            var anonType = anonTypeBuilder.CreateType();
            this.anonymousTypes.Add(id, anonType);
            return anonType;
        }
    }
}