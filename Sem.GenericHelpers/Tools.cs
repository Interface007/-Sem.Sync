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
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Xml.Serialization;

    using Entities;

    using Microsoft.Win32;

    /// <summary>
    /// This class contains some "misc" tools
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// Tests for the existence of the specified path and tries to create the path if it's missing
        /// </summary>
        /// <param name="filePath">The path to check</param>
        public static void EnsurePathExist(string filePath)
        {
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

                    sb.Append(Encoding.ASCII.GetString(new[] { Convert.ToByte(value.Substring(i + 1, 2), 16) }));
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
                        Console.WriteLine(ex.Message);
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
            using (var regKey = Registry.CurrentUser.OpenSubKey(pathToValue, true)
                   ?? Registry.CurrentUser.CreateSubKey(pathToValue))
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
        /// Reads a value from the registry and also ensures that the registry key does exist
        /// </summary>
        /// <param name="pathToValue">the path inside the registry to the value</param>
        /// <param name="keyName">the name of the value</param>
        /// <param name="value">this value will be set</param>
        public static void SetRegValue(string pathToValue, string keyName, string value)
        {
            using (var regKey = Registry.CurrentUser.OpenSubKey(pathToValue, true)
                   ?? Registry.CurrentUser.CreateSubKey(pathToValue))
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
                if (pathToProperty.StartsWith(".", StringComparison.Ordinal))
                {
                    pathToProperty = pathToProperty.Substring(1);
                }

                var type = objectToReadFrom.GetType();
                var propName = pathToProperty.Contains('.') ? pathToProperty.Substring(0, pathToProperty.IndexOf('.')) : pathToProperty;

                if (propName.EndsWith("()", StringComparison.Ordinal))
                {
                    return type.GetMethod(propName.Substring(0, propName.Length - 2)).Invoke(objectToReadFrom, null);
                }

                if (type.GetProperty(propName) != null)
                {
                    if (pathToProperty.Contains('.'))
                    {
                        return
                            GetPropertyValueString(
                                type
                                    .GetProperty(pathToProperty.Substring(0, pathToProperty.IndexOf('.')))
                                    .GetValue(objectToReadFrom, null),
                                pathToProperty.Substring(pathToProperty.IndexOf('.') + 1));
                    }

                    if (type.Name == "List`1")
                    {
                        return type.GetProperty(pathToProperty).GetValue(objectToReadFrom, null);
                    }

                    return
                        type
                            .GetProperty(pathToProperty)
                            .GetValue(objectToReadFrom, null);
                }
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
            return (GetPropertyValue(objectToReadFrom, pathToProperty) ?? string.Empty).ToString();
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

            var type = objectToWriteTo.GetType();
            var propName = pathToProperty.Contains('.') ? pathToProperty.Substring(0, pathToProperty.IndexOf('.')) : pathToProperty;

            if (propName.EndsWith("()", StringComparison.Ordinal))
            {
                return;
            }

            var propInfo = type.GetProperty(propName);
            if (propInfo != null)
            {
                if (pathToProperty.Contains('.'))
                {
                    if (propInfo.GetValue(objectToWriteTo, null) == null)
                    {
                        propInfo.SetValue(objectToWriteTo, propInfo.PropertyType.GetConstructor(new Type[] { }).Invoke(null), null);
                    }

                    SetPropertyValue(
                        propInfo.GetValue(objectToWriteTo, null),
                        pathToProperty.Substring(pathToProperty.IndexOf('.') + 1),
                        valueString);
                    return;
                }

                // we have to deal with special type data (int, datetime) that need to be
                // converted back from string - there is no automated cast in SetValue.
                if (!string.IsNullOrEmpty(valueString))
                {
                    var propType = propInfo.PropertyType;
                    var destinationType = (propType.BaseType == typeof(Enum)) ? "Enum" : propInfo.PropertyType.Name;

                    switch (destinationType)
                    {
                        case "DateTime":
                            propInfo.SetValue(
                                objectToWriteTo, DateTime.Parse(valueString, CultureInfo.CurrentCulture), null);
                            break;

                        case "Int32":
                            propInfo.SetValue(
                                objectToWriteTo, Int32.Parse(valueString, CultureInfo.CurrentCulture), null);
                            break;

                        case "Enum":
                            propInfo.SetValue(objectToWriteTo, Enum.Parse(propType, valueString), null);
                            break;

                        case "Guid":
                            propInfo.SetValue(objectToWriteTo, new Guid(valueString), null);
                            break;

                        default:
                            propInfo.SetValue(objectToWriteTo, valueString, null);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the SH a1 hash.
        /// </summary>
        /// <param name="text">The text to be hashed.</param>
        /// <returns>the hash value of the text</returns>
        public static string GetSha1Hash(string text)
        {
            var sha1 = new SHA1CryptoServiceProvider();

            string result = null;
            string temp;

            var arrayData = Encoding.ASCII.GetBytes(text);
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
        /// Gets a string which contains the first 128-characters (ANSII 7 bit).
        /// </summary>
        /// <returns> The allowed ascii characters. </returns>
        private static string GetAllowedAscii()
        {
            var sb = new StringBuilder(128);
            for (var i = 0; i < 127; i++)
            {
                sb.Append(Convert.ToChar(i));
            }

            return sb.ToString();
        }
    }
}
