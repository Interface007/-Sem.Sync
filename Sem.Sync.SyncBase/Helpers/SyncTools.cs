//-----------------------------------------------------------------------
// <copyright file="SyncTools.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    using Attributes;
    using DetailData;
    using Merging;

    using Microsoft.Win32;

    /// <summary>
    /// The <see cref="SyncTools"/> class does contain helper methods for working with the library functions.
    /// </summary>
    public static class SyncTools
    {
        /// <summary>
        /// Initializes static members of the <see cref="SyncTools"/> class.
        /// </summary>
        static SyncTools()
        {
            Replacements = LoadFromFile<ReplacementLists>("dictionary.xml");
        }

        /// <summary>
        /// Gets the list of replacement lists. Using these lists you can specify 
        /// a collection of key value pairs that will replace the key with the value
        /// in specific areas of the entities while normalizing
        /// </summary>
        public static ReplacementLists Replacements { get; private set; }

        /// <summary>
        /// Replaces forbidden file name characters with an underscore.
        /// </summary>
        /// <param name="fileName">file name to normalize</param>
        /// <returns>the noramlized file name</returns>
        public static string NormalizeFileName(string fileName)
        {
            var result = fileName;
            foreach (var character in Path.GetInvalidFileNameChars())
            {
                if (result.IndexOf(character) > -1)
                {
                    result = result.Replace(character, '_');
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts the street number from a complete street specification.
        /// </summary>
        /// <param name="streetDescription">A text representation of the street specification.</param>
        /// <returns>The street number</returns>
        public static int ExtractStreetNumber(string streetDescription)
        {
            var result = 0;

            if (!string.IsNullOrEmpty(streetDescription))
            {
                var streetNumberExtract = new Regex("[0-9]+");
                var match = streetNumberExtract.Match(streetDescription);
                if (match.Captures.Count > 0)
                {
                    if (!int.TryParse(match.Captures[0].ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out result))
                    {
                        result = 0;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts the street number extension from a complete street specification. E.g. in germany the 
        /// street number can be extended by a character like "Sesamstreet 21a". In this case "a" is the 
        /// extension and describes the fact that after numbering all houses in a stree there has been
        /// built another one.
        /// </summary>
        /// <returns>The street number extension</returns>
        public static string ExtractStreetNumberExtension()
        {
            // TODO: implement a way to extract the street number extension
            return null;
        }

        /// <summary>
        /// Match source to target and baseline (if provided) and build up a list of elements that can be tested 
        /// for conflicts. This list does contain information about possible merge conflicts with references to
        /// the real objects from the source lists.
        /// </summary>
        /// <param name="source">a list of standard elements that should be merged into a target list</param>
        /// <param name="target">a list of standard elements that should get the changes from the source list</param>
        /// <param name="baseline">a baseline both other lists will be compared to</param>
        /// <param name="type">the type that should serve as a pattern for detecting the properties to compare</param>
        /// <returns>A list of conflicts that have been detected</returns>
        public static List<ConflictTestContainer> BuildConflictTestContainerList(List<StdElement> source, List<StdElement> target, List<StdElement> baseline, Type type)
        {
            var resultList = new List<ConflictTestContainer>();

            // we iterate through the source list and check for each element
            foreach (var sourceItem in source)
            {
                // find the corresponding target element
                var targetItem =
                    (from x in target
                     where sourceItem.Id == x.Id
                     select x).FirstOrDefault();

                // if we have no target element, we (by definition) do not have a conflict - 
                // in this case the source element can simply be copied to the target list
                if (targetItem == null)
                {
                    continue;
                }

                // lookup the base element if we have one
                var baselineItem = (baseline == null) ? null :
                                                                 (from x in baseline
                                                                  where sourceItem.Id == x.Id
                                                                  select x).FirstOrDefault();

                // add the matched entries to the list of potential conflicts
                resultList.Add(new ConflictTestContainer
                                   {
                                       SourceObject = sourceItem,
                                       BaselineObject = baselineItem,
                                       TargetObject = targetItem,

                                       SourceProperty = sourceItem,
                                       BaselineProperty = baselineItem,
                                       TargetProperty = targetItem,

                                       PropertyName = string.Empty,
                                       PropertyType = type
                                   });
            }

            return resultList;
        }

        /// <summary>
        /// Detect conflicts by comparing the elements of the list via reflection (public properties only!)
        /// </summary>
        /// <param name="containers">a list of matched element that should be compared</param>
        /// <param name="skipIdenticalChanges">set ths to true to suppress changed where source and target differ from baseline, but with identical changes</param>
        /// <returns>a list of canflicts to be solved</returns>
        public static List<MergeConflict> DetectConflicts(List<ConflictTestContainer> containers, bool skipIdenticalChanges)
        {
            var resultList = new List<MergeConflict>();

            foreach (var container in containers)
            {
                resultList.AddRange(DetectConflicts(container, skipIdenticalChanges));
            }

            return resultList;
        }

        /// <summary>
        /// Detects standard values in sub entities and replaces them with a NULL reference.
        /// </summary>
        /// <param name="item">the item to check (checks will be done including sub entities)</param>
        /// <param name="testType">the type of the item to test</param>
        /// <returns>a processed item that contains NULL references instead of defaults</returns>
        /// <remarks>
        /// </remarks>
        public static bool ClearNulls(object item, Type testType)
        {
            if (item == null)
            {
                return false;
            }

            var isDefined = false;

            var typeName = testType.Name;
            if (testType.BaseType == typeof(Enum))
            {
                typeName = "Enum";
            }

            switch (typeName)
            {
                case "Enum":
                case "Int32":
                    isDefined = (int)item != 0;
                    break;

                case "String":
                    isDefined = !string.IsNullOrEmpty((string)item);
                    break;

                case "DateTime":
                    isDefined = (DateTime)item != new DateTime();
                    break;

                case "Date":
                    isDefined = (DateTime)item != new DateTime();
                    break;

                case "Guid":
                    isDefined = (Guid)item != new Guid();
                    break;

                case "Byte[]":
                    isDefined = true;
                    break;

                default:
                    // check if we have a new type for that we need to identify the default value
                    switch (typeName)
                    {
                        case "SyncData":
                        case "PhoneNumber":
                        case "AddressDetail":
                        case "PersonName":
                        case "StdContact":
                            break;
                        default:
                            Console.WriteLine("type name not explicitly supported in ClearNulls: " + typeName);
                            break;
                    }

                    var members = testType.GetProperties();

                    foreach (var member in members)
                    {
                        if (ClearNulls(member.GetValue(item, null), member.PropertyType))
                        {
                            isDefined = true;
                        }
                        else
                        {
                            member.SetValue(item, null, null);
                        }
                    }

                    break;
            }

            return isDefined;
        }

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
        /// Saves the entity to the file system
        /// </summary>
        /// <typeparam name="T">the type that should be serialized</typeparam>
        /// <param name="source">the source object that should be serialized</param>
        /// <param name="fileName">the destination file name that should get the serialized entity</param>
        public static void SaveToFile<T>(T source, string fileName)
        {
            var formatter = new XmlSerializer(typeof(T), new Type[]{typeof(KeyValuePair)});

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
        /// <param name="defaultValue">thsi value will be used (and set) if the value is missing - the value is not missing if it's empty</param>
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
                        regKey.SetValue(keyName, defaultValue);
                        return defaultValue;
                    }

                    return regValue.ToString();
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Interprets the gender from a text representation (e.g. "Mr." or "Frau")
        /// </summary>
        /// <param name="text"> The text to be interpreted. </param>
        /// <returns>The interpreted gender.</returns>
        public static Gender GenderByText(string text)
        {
            return text.IsOneOf("Herr", "Mr.")
                       ? Gender.Male
                       : text.IsOneOf("Frau", "Mrs.")
                             ? Gender.Female
                             : Gender.Unspecified;
        }

        /// <summary>
        /// Detects merge conflicts and stored the information about them in a <see cref="MergeConflict"/>.
        /// </summary>
        /// <param name="container"> The container of the attribute description. </param>
        /// <param name="skipIdenticalChanges">Skips identical changes on both sides.</param>
        /// <returns>A list of <see cref="MergeConflict"/> that have been detected.</returns>
        private static List<MergeConflict> DetectConflicts(ConflictTestContainer container, bool skipIdenticalChanges)
        {
            var result = new List<MergeConflict>();
            var members = container.PropertyType.GetProperties();

            foreach (var item in members)
            {
                var comparison = (from x in Attribute.GetCustomAttributes(item)
                                  where x.GetType() == typeof(ComparisonModifierAttribute)
                                  select x).FirstOrDefault() as ComparisonModifierAttribute ?? new ComparisonModifierAttribute();

                if (comparison.SkipMerge)
                {
                    break;
                }

                var conflict = MergePropertyConflict.None;
                var sourceString = string.Empty;
                var targetString = string.Empty;
                var baselineString = string.Empty;

                if (container.SourceProperty == null && container.TargetProperty == null && container.BaselineProperty == null) continue;
                if (container.SourceProperty == null && container.TargetProperty == null && container.BaselineProperty != null) conflict = MergePropertyConflict.BothChangedIdentically;
                if (container.SourceProperty != null && container.BaselineProperty == null) conflict = conflict | MergePropertyConflict.SourceChanged;
                if (container.SourceProperty == null && container.BaselineProperty != null) conflict = conflict | MergePropertyConflict.SourceChanged;
                if (container.TargetProperty != null && container.BaselineProperty == null) conflict = conflict | MergePropertyConflict.TargetChanged;
                if (container.TargetProperty == null && container.BaselineProperty != null) conflict = conflict | MergePropertyConflict.TargetChanged;


                var typeName = item.PropertyType.Name;
                if (item.PropertyType.BaseType.FullName == "System.Enum")
                {
                    typeName = "Enum";
                }

                switch (typeName)
                {
                    case "Enum":
                    case "Guid":
                    case "String":
                    case "DateTime":
                    case "Int32":
                        var sourceValue = container.SourceProperty == null ? null : item.GetValue(container.SourceProperty, null);
                        var targetValue = container.TargetProperty == null ? null : item.GetValue(container.TargetProperty, null);
                        var baselineValue = container.BaselineProperty == null ? null : item.GetValue(container.BaselineProperty, null);

                        sourceString = sourceValue == null ? string.Empty : sourceValue.ToString();
                        targetString = targetValue == null ? string.Empty : targetValue.ToString();
                        baselineString = baselineValue == null ? string.Empty : baselineValue.ToString();


                        if (sourceString.Equals(targetString, comparison.CaseInsensitive ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture) &&
                            sourceString.Equals(baselineString, comparison.CaseInsensitive ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture)) continue;

                        if (sourceString.Equals(targetString, comparison.CaseInsensitive ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture) &&
                            !sourceString.Equals(baselineString, comparison.CaseInsensitive ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture)) conflict = MergePropertyConflict.BothChangedIdentically;

                        if (!sourceString.Equals(baselineString, comparison.CaseInsensitive ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture)) conflict = conflict | MergePropertyConflict.SourceChanged;
                        if (!targetString.Equals(baselineString, comparison.CaseInsensitive ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture)) conflict = conflict | MergePropertyConflict.TargetChanged;
                        break;

                    case "Byte[]":
                        break;

                    default:
                        result.AddRange(
                            DetectConflicts(
                                new ConflictTestContainer
                                {
                                    SourceObject = container.SourceObject,
                                    TargetObject = container.TargetObject,
                                    BaselineObject = container.BaselineObject,
                                    PropertyName = container.PropertyName + "." + item.Name,
                                    PropertyType = item.PropertyType,
                                    SourceProperty =
                                        (container.SourceProperty == null)
                                            ? null
                                            : item.GetValue(container.SourceProperty, null),
                                    TargetProperty =
                                        (container.TargetProperty == null)
                                            ? null
                                            : item.GetValue(container.TargetProperty, null),
                                    BaselineProperty =
                                        (container.BaselineProperty == null)
                                            ? null
                                            : item.GetValue(container.BaselineProperty, null)
                                },
                                skipIdenticalChanges));

                        // we did already add the results for this property, so skip it now
                        conflict = MergePropertyConflict.None;
                        break;
                }

                // add to result, if it's not "Default" and not an ignorable indentical change
                if (conflict != MergePropertyConflict.None
                    && (!skipIdenticalChanges || conflict != MergePropertyConflict.BothChangedIdentically))
                {
                    result.Add(new MergeConflict
                    {
                        PropertyConflict = conflict,

                        BaselineElement = container.BaselineObject,
                        SourceElement = container.SourceObject,
                        TargetElement = container.TargetObject,

                        PathToProperty = container.PropertyName + "." + item.Name,
                        BaselinePropertyValue = baselineString,
                        SourcePropertyValue = sourceString,
                        TargetPropertyValue = targetString
                    });
                }
            }

            return result;
        }
    }
}