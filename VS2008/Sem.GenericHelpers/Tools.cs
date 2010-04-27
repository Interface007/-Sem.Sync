// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tools.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This class contains some "misc" tools
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using System.Xml.Serialization;

    using Microsoft.Win32;

    using Sem.GenericHelpers.Attributes;
    using Sem.GenericHelpers.Entities;

    /// <summary>
    /// This class contains some "misc" tools
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// Calculates a SHA1-Checksum
        /// </summary>
        private static readonly HashAlgorithm Sha1HashProvider = new SHA1CryptoServiceProvider();

        /// <summary>
        /// caches the allowed Ascii characters for the quoted printable encoding
        /// </summary>
        private static string cachedAllowedAscii = string.Empty;

        /// <summary>
        /// Tests for the existence of the specified path and tries to create the path if it's missing
        /// </summary>
        /// <param name="filePath">The path to check</param>
        public static void EnsurePathExist(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            if (!filePath.EndsWith("\\", StringComparison.Ordinal))
            {
                filePath += "\\";
            }

            var n = filePath.IndexOf("\\", StringComparison.Ordinal);
            while (n > 0)
            {
                if (!Directory.Exists(filePath.Substring(0, n)))
                {
                    Directory.CreateDirectory(filePath.Substring(0, n));
                }

                n = filePath.IndexOf("\\", n + 1, StringComparison.Ordinal);
            }
        }

        /// <summary>
        /// Encodes a not QP-Encoded string.
        /// Alle nicht im Ascii-Zeichnsatz enthaltenen Zeichen werden ersetzt durch die hexadezimale 
        /// Darstellung mit einem vorangestellten =
        /// Bsp.: aus "ü" wird "=FC"
        /// </summary>
        /// <param name="value">The string which should be encoded.</param>
        /// <returns>The encoded string</returns>
        public static string DecodeFromQuotedPrintable(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            for (var i = 0; i < value.Length; i++)
            {
                var s = value[i];

                if (s != '=')
                {
                    sb.Append(s);
                }
                else
                {
                    if (i > value.Length - 2)
                    {
                        break;
                    }

                    if (value[i + 1] == '\n')
                    {
                        i++;
                        continue;
                    }

                    sb.Append(
                        Encoding.GetEncoding("iso-8859-2").GetString(
                            new[] { Convert.ToByte(value.Substring(i + 1, 2), 16) }));
                    i = i + 2;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Encodes a not QP-Encoded string.
        /// Alle nicht im Ascii-Zeichnsatz enthaltenen Zeichen werden ersetzt durch die hexadezimale 
        /// Darstellung mit einem vorangestellten =
        /// Bsp.: aus "ü" wird "=FC"
        /// </summary>
        /// <param name="value">The string which should be encoded.</param>
        /// <returns>The encoded string</returns>
        public static string EncodeToQuotedPrintable(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var ascii7Bit = GetAllowedAscii();
            var sb = new StringBuilder();
            foreach (var s in value)
            {
                if (ascii7Bit.LastIndexOf(s) > -1)
                {
                    sb.Append(s);
                }
                else
                {
                    sb.Append("=");
                    sb.Append(Convert.ToString(s, 16));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Saves the entity to the file system
        /// </summary>
        /// <typeparam name="T">the type that should be serialized</typeparam>
        /// <param name="source">the source object that should be serialized</param>
        /// <param name="fileName">the destination file name that should get the serialized entity</param>
        public static void SaveToFile<T>(T source, string fileName)
        {
            SaveToFile(source, fileName, typeof(KeyValuePair));
        }

        /// <summary>
        /// Saves the entity to the file system
        /// </summary>
        /// <typeparam name="T">the type that should be serialized</typeparam>
        /// <param name="source">the source object that should be serialized</param>
        /// <param name="fileName">the destination file name that should get the serialized entity</param>
        /// <param name="additionalTypes">specifies additional types for serialization</param>
        public static void SaveToFile<T>(T source, string fileName, params Type[] additionalTypes)
        {
            var formatter = new XmlSerializer(typeof(T), additionalTypes);

            EnsurePathExist(Path.GetDirectoryName(fileName));
            using (var file = new FileStream(fileName, FileMode.Create))
            {
                formatter.Serialize(file, source);
            }
        }

        /// <summary>
        /// Serializes an object to a string
        /// </summary>
        /// <param name="source"> The source object to be serialized. </param>
        /// <typeparam name="T"> The type that will be serialized (automatically detected by type invariance)</typeparam>
        /// <returns>
        /// The serialized object as a string
        /// </returns>
        public static string SaveToString<T>(T source) where T : class
        {
            var formatter = new XmlSerializer(typeof(T));
            var result = new StringBuilder();
            if (source != null)
            {
                try
                {
                    var writer = new StringWriter(result, CultureInfo.CurrentCulture);
                    formatter.Serialize(writer, source);
                    writer.Flush();
                    writer.Close();
                }
                catch (InvalidOperationException)
                {
                }
                catch (Exception ex)
                {
                    DebugWriteLine(ex.Message);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Loads an entity from the file system.
        /// </summary>
        /// <typeparam name="T">the type of the entity that should be loaded</typeparam>
        /// <param name="fileName">the source file that should be deserialized</param>
        /// <returns>the deserialized entity - null if there was nothing valid to deserialize</returns>
        public static T LoadFromFile<T>(string fileName)
        {
            var formatter = new XmlSerializer(typeof(T));
            var result = default(T);
            if (File.Exists(fileName))
            {
                using (var fileStream = new FileStream(fileName, FileMode.Open))
                {
                    try
                    {
                        result = (T)formatter.Deserialize(fileStream);
                    }
                    catch (InvalidOperationException)
                    {
                    }
                    catch (IOException)
                    {
                    }
                    catch (Exception ex)
                    {
                        DebugWriteLine(ex.Message);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Reads a value from the registry and also ensures that the registry key does exist
        /// </summary>
        /// <param name="pathToValue">the path inside the registry to the value</param>
        /// <param name="keyName">the name of the value</param>
        /// <param name="defaultValue">this value will be used (and set) if the value is missing - the value is not missing if it's empty</param>
        /// <returns>the string value of the registry key</returns>
        public static string GetRegValue(string pathToValue, string keyName, string defaultValue)
        {
            using (
                var regKey = Registry.CurrentUser.OpenSubKey(pathToValue, true) ??
                             Registry.CurrentUser.CreateSubKey(pathToValue))
            {
                if (regKey != null)
                {
                    var regValue = regKey.GetValue(keyName);
                    if (regValue == null)
                    {
                        if (defaultValue != null)
                        {
                            regKey.SetValue(keyName, defaultValue);
                        }

                        return defaultValue;
                    }

                    return regValue.ToString();
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets serialized credentials from the registry.
        /// </summary>
        /// <param name="name"> The name of the credentials. </param>
        /// <returns> A deserialized <see cref="Credentials"/> object containing a decrypted password </returns>
        public static Credentials GetCredentials(string name)
        {
            var serialized = GetRegValue("Software\\Sem.Sync\\Credentials", name, string.Empty);
            return Tools.LoadFromString<Credentials>(serialized);
        }

        /// <summary>
        /// Reads a value from the registry and also ensures that the registry key does exist
        /// </summary>
        /// <param name="pathToValue">the path inside the registry to the value</param>
        /// <param name="keyName">the name of the value</param>
        /// <param name="value">this value will be set</param>
        public static void SetRegValue(string pathToValue, string keyName, string value)
        {
            using (
                var regKey = Registry.CurrentUser.OpenSubKey(pathToValue, true) ??
                             Registry.CurrentUser.CreateSubKey(pathToValue))
            {
                if (regKey != null)
                {
                    regKey.SetValue(keyName, value);
                }
            }
        }

        /// <summary>
        /// Reads a property by its path. E.g. you might specify the path "PersonalAddressPrimary.Phone.AreaCode"
        /// to access the AreaCode property of the Phone property inside the PersonalAddressPrimary property of 
        /// the entity
        /// </summary>
        /// <typeparam name="T">the type of the object to read from</typeparam>
        /// <param name="objectToReadFrom">the object to read from</param>
        /// <param name="pathToProperty">the path to the property</param>
        /// <returns>the value of the property rendered as a string</returns>
        public static object GetPropertyValue<T>(T objectToReadFrom, string pathToProperty)
        {
            if (!Equals(objectToReadFrom, default(T)))
            {
                var type = objectToReadFrom.GetType();
                var propName = GetInvokePartFromPath(ref pathToProperty);

                var isMethod = propName.EndsWith(")", StringComparison.Ordinal) && propName.Contains("(");
                var isIndexed = propName.EndsWith("]", StringComparison.Ordinal) && propName.Contains("[");

                var parameterStart = propName.IndexOfAny(new[] { '[', '(' });
                var parameter = parameterStart > -1
                                    ? propName.Substring(parameterStart + 1, propName.Length - parameterStart - 2)
                                    : string.Empty;
                propName = parameterStart > -1 ? propName.Substring(0, parameterStart) : propName;

                // for method invocation parameters are not allowed!
                if (isMethod)
                {
                    var methods = type.GetMethods();
                    if (methods.Length == 0)
                    {
                        return null;
                    }

                    if (string.IsNullOrEmpty(parameter))
                    {
                        var method =
                            (from x in methods where x.GetParameters().Length == 0 && x.Name == propName select x).
                                FirstOrDefault();
                        return method == null ? null : method.Invoke(objectToReadFrom, null);
                    }
                    else
                    {
                        var method =
                            (from x in methods where x.GetParameters().Length == 1 && x.Name == propName select x).
                                FirstOrDefault();

                        if (method == null)
                        {
                            return null;
                        }

                        var parameters = method.GetParameters();
                        if (parameters.Count() == 1)
                        {
                            var parameterType = parameters[0].ParameterType;

                            if (parameterType.BaseType == typeof(Enum))
                            {
                                return method.Invoke(objectToReadFrom, new[] { Enum.Parse(parameterType, parameter) });
                            }
                        }

                        return method.Invoke(objectToReadFrom, new[] { parameter });
                    }
                }

                object value;
                if (string.IsNullOrEmpty(propName))
                {
                    value = objectToReadFrom;
                }
                else
                {
                    if (type.Name == "SerializableDictionary`2" && propName == "Item")
                    {
                        return null;
                    }
                    var propertyInfo = type.GetProperty(propName);
                    value = propertyInfo == null ? null : propertyInfo.GetValue(objectToReadFrom, null);
                }

                if (value == null)
                {
                    return null;
                }

                var valueType = value.GetType();

                if (isIndexed)
                {
                    int numIndex;
                    if (parameter.EndsWith("?", StringComparison.Ordinal))
                    {
                        parameter = parameter.Substring(0, parameter.Length - 1);
                    }

                    var indexerObject = valueType.GetCustomAttributes(typeof(DefaultMemberAttribute), true);
                    if (indexerObject.Length == 0)
                    {
                        if (!int.TryParse(parameter, out numIndex))
                        {
                            return null;
                        }

                        value = ((object[])value)[numIndex];
                    }
                    else
                    {
                        var indexerName = ((DefaultMemberAttribute)indexerObject[0]).MemberName;
                        var indexerPropertyInfo = valueType.GetProperty(indexerName);

                        var valueDictionary = value as IDictionary;
                        if (valueDictionary != null)
                        {
                            var getter = valueType.GetMethod("get_Item");

                            object parameterObject = parameter;
                            if (getter.GetParameters()[0].ParameterType.BaseType == typeof(Enum))
                            {
                                parameterObject = Enum.Parse(getter.GetParameters()[0].ParameterType, parameter);
                            }

                            var exists = (bool)GetPropertyValue(value, "ContainsKey(" + parameter + ")");
                            if (!exists)
                            {
                                return null;
                            }

                            value = indexerPropertyInfo.GetValue(value, new[] { parameterObject });
                        }
                        else
                        {
                            if (!int.TryParse(parameter, out numIndex))
                            {
                                return null;
                            }

                            var maxIndex = ((int)GetPropertyValue(value, "Count")) - 1;
                            if (maxIndex < numIndex)
                            {
                                return null;
                            }

                            value = indexerPropertyInfo.GetValue(value, new object[] { numIndex });
                        }
                    }
                }

                return pathToProperty.Contains('.') ? GetPropertyValue(value, pathToProperty) : value;
            }

            return null;
        }

        /// <summary>
        /// Reads a property by its path. E.g. you might specify the path "PersonalAddressPrimary.Phone.AreaCode"
        /// to access the AreaCode property of the Phone property inside the PersonalAddressPrimary property of 
        /// the entity
        /// </summary>
        /// <typeparam name="T">the type of the object to read from</typeparam>
        /// <param name="objectToReadFrom">the object to read from</param>
        /// <param name="pathToProperty">the path to the property</param>
        /// <returns>the value of the property rendered as a string</returns>
        public static string GetPropertyValueString<T>(T objectToReadFrom, string pathToProperty)
        {
            var value = GetPropertyValue(objectToReadFrom, pathToProperty) ?? string.Empty;
            if (value.GetType() == typeof(List<string>))
            {
                value = ((List<string>)value).ConcatElementsToString("|");
            }

            return value.ToString();
        }

        /// <summary>
        /// Reads a property by its path. E.g. you might specify the path "PersonalAddressPrimary.Phone.AreaCode"
        /// to access the AreaCode property of the Phone property inside the PersonalAddressPrimary property of 
        /// the entity
        /// </summary>
        /// <typeparam name="T">the type of the object to read from</typeparam>
        /// <param name="objectToReadFrom">the object to read from</param>
        /// <param name="pathToProperty">the path to the property</param>
        /// <returns>the value of the property rendered as a boolean</returns>
        public static bool GetPropertyValueBoolean<T>(T objectToReadFrom, string pathToProperty)
        {
            var value = GetPropertyValue(objectToReadFrom, pathToProperty);
            if (value != null && value.GetType() == typeof(bool))
            {
                return (bool)value;
            }

            return false;
        }

        /// <summary>
        /// replaces all invalid chars from a file name with a hyphen ("-")
        /// </summary>
        /// <param name="fileName"> The file name. </param>
        /// <returns> the file name without any invalid character </returns>
        public static string ReplaceInvalidFileCharacters(string fileName)
        {
            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(invalidChar.ToString(), "-");
            }

            return fileName;
        }

        /// <summary>
        /// Sets a property by the complete path specified as a string (like "PersonalAddressPrimary.Phone.AreaCode").
        /// If one of the objects inside the path is null, it will be initialized with a default instance. If the type
        /// of that object does not contain a default constructor, an exception will be thrown.
        /// </summary>
        /// <typeparam name="T">the type of object to write the property to</typeparam>
        /// <param name="objectToWriteTo">the object that is the root of the path</param>
        /// <param name="pathToProperty">the path to the property to be set</param>
        /// <param name="valueString">a string representation of the value to be set</param>
        public static void SetPropertyValue<T>(T objectToWriteTo, string pathToProperty, string valueString)
        {
            if (pathToProperty.StartsWith(".", StringComparison.Ordinal))
            {
                pathToProperty = pathToProperty.Substring(1);
            }

            var propName = GetInvokePartFromPath(ref pathToProperty);

            var isMethod = propName.EndsWith(")", StringComparison.Ordinal) && propName.Contains("(");
            if (isMethod)
            {
                return;
            }

            var isIndexed = propName.EndsWith("]", StringComparison.Ordinal) && propName.Contains("[");

            var parameterStart = propName.IndexOfAny(new[] { '[', '(' });
            var parameter = parameterStart > -1
                                ? propName.Substring(parameterStart + 1, propName.Length - parameterStart - 2)
                                : string.Empty;
            propName = parameterStart > -1 ? propName.Substring(0, parameterStart) : propName;

            var type = objectToWriteTo.GetType();
            var propInfo = type.GetProperty(propName);
            if (propInfo == null)
            {
                return;
            }

            // if we need to navigate down the object path
            if (pathToProperty.Contains('.'))
            {
                // generate a new instance for this property in case of NULL
                if (propInfo.GetValue(objectToWriteTo, null) == null)
                {
                    var constructor = propInfo.PropertyType.GetConstructor(new Type[] { });
                    if (constructor == null)
                    {
                        return;
                    }

                    propInfo.SetValue(objectToWriteTo, constructor.Invoke(null), null);
                }

                // recursively call this method
                SetPropertyValue(
                    propInfo.GetValue(objectToWriteTo, null),
                    pathToProperty.Substring(pathToProperty.IndexOf('.') + 1),
                    valueString);
                return;
            }

            // we have to deal with special type data (int, datetime) that need to be
            // converted back from string - there is no automated cast in SetValue.
            if (string.IsNullOrEmpty(valueString))
            {
                return;
            }

            var propType = propInfo.PropertyType;
            var destinationType = (propType.BaseType == typeof(Enum)) ? "Enum" : propInfo.PropertyType.Name;

            switch (destinationType)
            {
                case "TimeSpan":
                    propInfo.SetValue(objectToWriteTo, TimeSpan.Parse(valueString), null);
                    break;

                case "DateTime":
                    propInfo.SetValue(objectToWriteTo, DateTime.Parse(valueString, CultureInfo.CurrentCulture), null);
                    break;

                case "Int32":
                    int theValue;
                    if (int.TryParse(valueString, NumberStyles.Any, CultureInfo.CurrentCulture, out theValue))
                    {
                        propInfo.SetValue(objectToWriteTo, theValue, null);
                    }
                    else
                    {
                        DebugWriteLine(string.Format(CultureInfo.CurrentCulture, "non-parsable int: {0}", valueString));
                    }

                    break;

                case "Enum":
                    if (Enum.IsDefined(propType, valueString))
                    {
                        propInfo.SetValue(objectToWriteTo, Enum.Parse(propType, valueString), null);
                    }
                    break;

                case "Boolean":
                    propInfo.SetValue(objectToWriteTo, bool.Parse(valueString), null);
                    break;

                case "Guid":
                    propInfo.SetValue(objectToWriteTo, new Guid(valueString), null);
                    break;

                case "String":
                    if (valueString == "{emptystring}")
                    {
                        propInfo.SetValue(objectToWriteTo, string.Empty, null);
                        break;
                    }

                    propInfo.SetValue(objectToWriteTo, valueString, null);
                    break;

                case "SerializableDictionary`2":
                    var sdic = propInfo.GetValue(objectToWriteTo, null) as SerializableDictionary<string, string>;
                    if (sdic == null)
                    {
                        sdic = propType.GetConstructor(new Type[] { }).Invoke(null) as SerializableDictionary<string, string>;
                        propInfo.SetValue(objectToWriteTo, sdic, null);
                    }

                    if (sdic != null)
                    {
                        sdic.Add(parameter, valueString);
                    }

                    break;

                case "List`1":

                    var list = propType.GetConstructor(new Type[] { }).Invoke(null) as List<string>;
                    if (list != null)
                    {
                        list.AddRange(valueString.Split('|'));
                        propInfo.SetValue(objectToWriteTo, list, null);
                    }

                    break;

                case "ProfileIdInformation":
                    var myObject = propType.GetConstructor(new[] { typeof(string) }).Invoke(new[] { valueString });
                    propInfo.SetValue(objectToWriteTo, myObject, null);
                    break;

                case "queryType":
                    var myQueryType = propType.GetConstructor(new Type[] { }).Invoke(new object[] { });
                    SetPropertyValue(myQueryType, "Value", valueString);
                    propInfo.SetValue(objectToWriteTo, myQueryType, null);
                    break;

                case "PhoneNumber":
                    var myPhoneNumber = propType.GetConstructor(new[] {typeof (string)}).Invoke(new[] {valueString});
                    propInfo.SetValue(objectToWriteTo, myPhoneNumber, null);
                    break;

                default:
                    if (isIndexed)
                    {
                        propInfo.SetValue(objectToWriteTo, valueString, CreateIndexerValue(parameter));
                    }
                    else
                    {
                        propInfo.SetValue(objectToWriteTo, valueString, null);
                    }

                    break;
            }
        }

        /// <summary>
        /// Loads a serialized object from a string.
        /// </summary>
        /// <param name="serialized"> The serialized object as a string. </param>
        /// <typeparam name="T"> the type to be deserialized </typeparam>
        /// <returns> the deserialized object </returns>
        public static T LoadFromString<T>(string serialized)
        {
            var formatter = new XmlSerializer(typeof(T));
            var result = default(T);
            if (!string.IsNullOrEmpty(serialized))
            {
                try
                {
                    var reader = new StringReader(serialized);
                    result = (T)formatter.Deserialize(reader);
                }
                catch (InvalidOperationException)
                {
                }
                catch (IOException)
                {
                }
                catch (Exception ex)
                {
                    DebugWriteLine(ex.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the SH a1 hash.
        /// </summary>
        /// <param name="text">The text to be hashed.</param>
        /// <returns>the hash value of the text</returns>
        public static string GetSha1Hash(string text)
        {
            var sha1 = Sha1HashProvider;

            string result = null;
            string temp;

            var arrayData = Encoding.ASCII.GetBytes(text ?? string.Empty);
            var arrayResult = sha1.ComputeHash(arrayData);
            for (var i = 0; i < arrayResult.Length; i++)
            {
                temp = Convert.ToString(arrayResult[i], 16);
                if (temp.Length == 1)
                {
                    temp = "0" + temp;
                }

                result += temp;
            }

            return result;
        }

        /// <summary>
        /// Logging method that will be removed in the release build
        /// </summary>
        /// <param name="message"> The message to be logged.  </param>
        /// <param name="parameters"> The parameters. </param>
        [Conditional("DEBUG")]
        public static void DebugWriteLine(string message, params object[] parameters)
        {
            Console.WriteLine(string.Format(CultureInfo.InvariantCulture, message, parameters));
        }

        /// <summary>
        /// Logging method that will be removed in the release build
        /// </summary>
        /// <param name="message"> The message to be logged. </param>
        [Conditional("DEBUG")]
        public static void DebugWriteLine(string message)
        {
            DebugWriteLine(true, message);
        }

        /// <summary>
        /// Logging method that will be removed in the release build
        /// </summary>
        /// <param name="condition"> The condition (must be true in order to write the line). </param>
        /// <param name="message"> The message to be logged.  </param>
        /// <param name="parameters"> The parameters to be inserted into the message. </param>
        [Conditional("DEBUG")]
        public static void DebugWriteLine(bool condition, string message, params object[] parameters)
        {
            if (condition)
            {
                DebugWriteLine(message, parameters);
            }
        }

        /// <summary>
        /// Gets a list of paths to public properties of the object and its sub objects. It also includes
        /// paths to methods tagged with the <see cref="AddAsPropertyAttribute"/> attribute.
        /// </summary>
        /// <param name="parentName">the path that should be used as a root path for the string specification of the path</param>
        /// <param name="type">the type to be inspected</param>
        /// <returns>a list of strings with the paths of properties detected</returns>
        public static List<string> GetPropertyList(string parentName, Type type)
        {
            if (parentName == null)
            {
                throw new ArgumentNullException("parentName");
            }

            if (parentName.Length > 0)
            {
                parentName += ".";
            }

            var resultList = new List<string>();

            if (type.Name == "ProfileIdentifiers")
            {
                return resultList;
            }

            var methodsToAdd = from x in type.GetMethods()
                               where
                                   x.GetParameters().Length == 0 &&
                                   x.GetCustomAttributes(false).Contains(new AddAsPropertyAttribute())
                               select parentName + x.Name + "()";

            resultList.AddRange(methodsToAdd.ToList());

            var members = type.GetProperties();

            foreach (var item in members)
            {
                var typeName = item.PropertyType.Name;
                if (item.PropertyType.BaseType != null && item.PropertyType.BaseType.FullName == "System.Enum")
                {
                    typeName = "Enum";
                }

                switch (typeName)
                {
                    case "Enum":
                    case "Guid":
                    case "String":
                    case "DateTime":
                    case "TimeSpan":
                    case "Boolean":
                    case "Int32":
                        resultList.Add(parentName + item.Name);
                        break;

                    case "Byte[]":
                        // we don't want to save byte arrays to the CSV file (may be some time)
                        break;

                    case "List`1":
                        resultList.Add(parentName + item.Name);
                        break;

                    default:
                        resultList.AddRange(GetPropertyList(parentName + item.Name, item.PropertyType));
                        break;
                }
            }

            return resultList;
        }

        /// <summary>
        /// Returns an enumeration of all elements that are provided and are not null or empty
        /// </summary>
        /// <param name="elements"> The elements to combine as an enumeration. </param>
        /// <returns> the enumeration of elements </returns>
        public static IEnumerable<string> CombineNonEmpty(params string[] elements)
        {
            foreach (var element in elements)
            {
                if (!string.IsNullOrEmpty(element))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Assembels an array of indexer parameters for a property get/set invocation.
        /// </summary>
        /// <param name="parameter"> The parameter to extract the indexers from. </param>
        /// <returns> an array of call arguments </returns>
        private static object[] CreateIndexerValue(string parameter)
        {
            int integerIndexer;
            if (int.TryParse(parameter, out integerIndexer))
            {
                return new object[] { integerIndexer };
            }

            if (parameter.StartsWith(@"""", StringComparison.Ordinal) && parameter.EndsWith(@"""", StringComparison.Ordinal))
            {
                return new object[] { parameter.Substring(1, parameter.Length - 2) };
            }

            var lastDot = parameter.LastIndexOf('.');
            var type = Type.GetType(new Factory().EnrichClassName(parameter.Substring(0, lastDot)), false, true);

            if (type != null && type.BaseType == typeof(Enum))
            {
                return new[] { Enum.Parse(type, parameter.Substring(lastDot + 1)) };
            }

            return null;
        }

        /// <summary>
        /// Gets the next path of the invocation path - will cut off this part from the path.
        /// </summary>
        /// <param name="pathToProperty"> The path to property - the part wil be removed from that string. </param>
        /// <returns> next part of the invocation path to get the property </returns>
        private static string GetInvokePartFromPath(ref string pathToProperty)
        {
            if (pathToProperty.StartsWith(".", StringComparison.Ordinal))
            {
                pathToProperty = pathToProperty.Substring(1);
            }

            string propName;
            var indexOfOpeningBracket = pathToProperty.IndexOfAny(new[] { '[', '(' });
            var indexOfClosingBracket = pathToProperty.IndexOfAny(new[] { ']', ')' });
            var indexOfNextPart = pathToProperty.IndexOf('.');
            if (indexOfOpeningBracket > 0 &&
                indexOfOpeningBracket < indexOfNextPart &&
                indexOfClosingBracket > indexOfNextPart)
            {
                indexOfNextPart = indexOfClosingBracket + 1;
            }

            if (indexOfNextPart > 0)
            {
                propName = pathToProperty.Substring(0, indexOfNextPart);
                pathToProperty = pathToProperty.Substring(indexOfNextPart);
            }
            else
            {
                propName = pathToProperty;
                pathToProperty = string.Empty;
            }

            return propName;
        }

        /// <summary>
        /// Gets a string which contains the first 128-characters (ANSII 7 bit).
        /// </summary>
        /// <returns> The allowed ascii characters. </returns>
        private static string GetAllowedAscii()
        {
            if (string.IsNullOrEmpty(cachedAllowedAscii))
            {
                var sb = new StringBuilder(128);
                for (var i = 0; i < 127; i++)
                {
                    sb.Append(Convert.ToChar(i));
                }

                cachedAllowedAscii = sb.ToString();
            }

            return cachedAllowedAscii;
        }
    }
}