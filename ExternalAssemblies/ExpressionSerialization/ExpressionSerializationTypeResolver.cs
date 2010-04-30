using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace ExpressionSerialization
{
    public abstract class CustomExpressionXmlConverter
    {
        public abstract Expression Deserialize(XElement expressionXml);
        public abstract XElement Serialize(Expression expression);
    }

    public class ExpressionSerializationTypeResolver
    {
        private readonly Dictionary<AnonTypeId, Type> anonymousTypes = new Dictionary<AnonTypeId, Type>();
        private readonly ModuleBuilder moduleBuilder;
        private int anonymousTypeIndex;

        //vsadov: hack to force loading of VB runtime.
#pragma warning disable 169
// ReSharper disable InconsistentNaming
        private int vb_hack = Microsoft.VisualBasic.CompilerServices.Operators.CompareString("qq","qq",true);
// ReSharper restore InconsistentNaming
#pragma warning restore 169


        public ExpressionSerializationTypeResolver()
        {
            var asmname = new AssemblyName { Name = "AnonymousTypes" };
            var assemblyBuilder = System.Threading.Thread.GetDomain().DefineDynamicAssembly(asmname, AssemblyBuilderAccess.RunAndSave);
            moduleBuilder = assemblyBuilder.DefineDynamicModule("AnonymousTypes");
        }

        protected virtual Type ResolveTypeFromString(string typeString) { return null; }
        protected virtual string ResolveStringFromType(Type type) { return null; }

        public Type GetType(string typeName, IEnumerable<Type> genericArgumentTypes)
        {
            return GetType(typeName).MakeGenericType(genericArgumentTypes.ToArray());
        }

        public Type GetType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                throw new ArgumentNullException("typeName");

            // First - try all replacers
            var type = this.ResolveTypeFromString(typeName);
            //type = typeReplacers.Select(f => f(typeName)).FirstOrDefault();
            if (type != null)
                return type;

            // If it's an array name - get the element type and wrap in the array type.
            if (typeName.EndsWith("[]"))
                return this.GetType(typeName.Substring(0, typeName.Length - 2)).MakeArrayType();

            // First - try all loaded types
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = assembly.GetType(typeName, false, true);
                if (type != null)
                    return type;
            }

            // Second - try just plain old Type.GetType()
            type = Type.GetType(typeName, false, true);
            if (type != null)
                return type;

            throw new ArgumentException("Could not find a matching type", typeName);
        }

        public class NameTypePair
        {
            public string Name { get; set; }
            public Type Type { get; set; }

            public override int GetHashCode()
            {
                return Name.GetHashCode() + Type.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                if (!(obj is NameTypePair))
                    return false;
                var other = obj as NameTypePair;
                return Name.Equals(other.Name) && Type.Equals(other.Type);
            }
        }

        private class AnonTypeId
        {
            public string Name { get; private set; }
            public IEnumerable<NameTypePair> Properties { get; private set; }

            public AnonTypeId(string name, IEnumerable<NameTypePair> properties)
            {
                this.Name = name;
                this.Properties = properties;
            }

            public override int GetHashCode()
            {
                return this.Name.GetHashCode() + this.Properties.Sum(ntpair => ntpair.GetHashCode());
            }

            public override bool Equals(object obj)
            {
                if (!(obj is AnonTypeId))
                    return false;
                var other = obj as AnonTypeId;
                return (Name.Equals(other.Name)
                    && Properties.SequenceEqual(other.Properties));

            }

        }


        public MethodInfo GetMethod(Type declaringType, string name, Type[] parameterTypes, Type[] genArgTypes)
        {
            var methods = from mi in declaringType.GetMethods()
                          where mi.Name == name
                          select mi;
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

        private static bool MatchPiecewise<T>(IEnumerable<T> first, IEnumerable<T> second)
        {
            var firstArray = first.ToArray();
            var secondArray = second.ToArray();
            if (firstArray.Length != secondArray.Length)
                return false;
            return !firstArray.Where((t, i) => !t.Equals(secondArray[i])).Any();
        }

        //vsadov: need to take ctor parameters too as they do not 
        //necessarily match properties order as returned by GetProperties
        public Type GetOrCreateAnonymousTypeFor(string name, NameTypePair[] properties, NameTypePair[] ctrParams)
        {
            var id = new AnonTypeId(name, properties.Concat(ctrParams));
            if (anonymousTypes.ContainsKey(id))
                return anonymousTypes[id];

            //vsadov: VB anon type. not necessary, just looks better
            var anonPrefix = name.StartsWith("<>") ? "<>f__AnonymousType" : "VB$AnonymousType_";
            var anonTypeBuilder = moduleBuilder.DefineType(anonPrefix + anonymousTypeIndex++, TypeAttributes.Public | TypeAttributes.Class);

            var fieldBuilders = new FieldBuilder[properties.Length];
            var propertyBuilders = new PropertyBuilder[properties.Length];

            for (int i = 0; i < properties.Length; i++)
            {
                fieldBuilders[i] = anonTypeBuilder.DefineField("_generatedfield_" + properties[i].Name, properties[i].Type, FieldAttributes.Private);
                propertyBuilders[i] = anonTypeBuilder.DefineProperty(properties[i].Name, PropertyAttributes.None, properties[i].Type, new Type[0]);
                var propertyGetterBuilder = anonTypeBuilder.DefineMethod("get_" + properties[i].Name, MethodAttributes.Public, properties[i].Type, new Type[0]);
                var getterIlGenerator = propertyGetterBuilder.GetILGenerator();
                getterIlGenerator.Emit(OpCodes.Ldarg_0);
                getterIlGenerator.Emit(OpCodes.Ldfld, fieldBuilders[i]);
                getterIlGenerator.Emit(OpCodes.Ret);
                propertyBuilders[i].SetGetMethod(propertyGetterBuilder);
            }

            var constructorBuilder = anonTypeBuilder.DefineConstructor(MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Public, CallingConventions.Standard, ctrParams.Select(prop => prop.Type).ToArray());
            var constructorIlGenerator = constructorBuilder.GetILGenerator();
            for (var i = 0; i < ctrParams.Length; i++)
            {
                constructorIlGenerator.Emit(OpCodes.Ldarg_0);
                constructorIlGenerator.Emit(OpCodes.Ldarg, i + 1);
                constructorIlGenerator.Emit(OpCodes.Stfld, fieldBuilders[i]);
                constructorBuilder.DefineParameter(i + 1, ParameterAttributes.None, ctrParams[i].Name);
            }
            constructorIlGenerator.Emit(OpCodes.Ret);

            //TODO - Define ToString() and GetHashCode implementations for our generated Anonymous Types
            //MethodBuilder toStringBuilder = anonTypeBuilder.DefineMethod();
            //MethodBuilder getHashCodeBuilder = anonTypeBuilder.DefineMethod();

            var anonType = anonTypeBuilder.CreateType();
            anonymousTypes.Add(id, anonType);
            return anonType;
        }

    }
}
