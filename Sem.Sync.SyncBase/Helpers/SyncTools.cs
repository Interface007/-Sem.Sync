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
    using System.Text.RegularExpressions;
    using System.Linq;
    using System.Xml.Serialization;

    using Microsoft.Win32;

    using Attributes;
    using DetailData;
    using Merging;

    public static class SyncTools
    {
        public static ReplacementLists Replacements { get; private set; }

        static SyncTools()
        {
            Replacements = LoadFromFile<ReplacementLists>("dictionary.xml");
        }

        public static string NormalizeFileName(string currentContactName)
        {
            var result = currentContactName;
            foreach (var character in Path.GetInvalidFileNameChars())
            {
                if (result.IndexOf(character) > -1)
                {
                    result = result.Replace(character, '_');
                }
            }
            return result;
        }

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

        public static string ExtractStreetNumberExtension(string street)
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
        /// <returns></returns>
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
                if (targetItem == null) continue;

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

                                       PropertyName = "",
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
                var sourceString = "";
                var targetString = "";
                var baselineString = "";

                if (container.SourceProperty == null && container.TargetProperty == null && container.BaselineProperty == null) continue;
                if (container.SourceProperty == null && container.TargetProperty == null && container.BaselineProperty != null) conflict = MergePropertyConflict.BothChangedIdentically;
                if (container.SourceProperty != null && container.BaselineProperty == null) conflict = conflict | MergePropertyConflict.SourceChanged;
                if (container.SourceProperty == null && container.BaselineProperty != null) conflict = conflict | MergePropertyConflict.SourceChanged;
                if (container.TargetProperty != null && container.BaselineProperty == null) conflict = conflict | MergePropertyConflict.TargetChanged;
                if (container.TargetProperty == null && container.BaselineProperty != null) conflict = conflict | MergePropertyConflict.TargetChanged;


                var typeName = item.PropertyType.Name;
                if (item.PropertyType.BaseType.FullName == "System.Enum")
                    typeName = "Enum";

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

                        sourceString = sourceValue == null ? "" : sourceValue.ToString();
                        targetString = targetValue == null ? "" : targetValue.ToString();
                        baselineString = baselineValue == null ? "" : baselineValue.ToString();


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

        public static bool ClearNulls(object item, Type testType)
        {
            if (item == null) return false;

            var isDefined = false;
            
            var typeName = testType.Name;
            if (testType.BaseType == typeof(Enum))
                typeName = "Enum";

            switch (typeName)
            {
                case "Enum":
                case "Int32":
                    isDefined = (Int32)item != 0;
                    break;

                case "String":
                    isDefined = !string.IsNullOrEmpty((string)item);
                    break;

                case "DateTime":
                    isDefined = (DateTime) item != new DateTime();
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

        public static void SaveToFile<T>(T source, string fileName)
        {

            var formatter = new XmlSerializer(typeof(T));

            EnsurePathExist(Path.GetDirectoryName(fileName));
            var file = new FileStream(fileName, FileMode.Create);

            try
            {
                formatter.Serialize(file, source);
            }
            finally
            {
                file.Close();
            }
        }

        public static T LoadFromFile<T>(string fileName)
        {
            var formatter = new XmlSerializer(typeof(T));
            var result = default(T);
            if (File.Exists(fileName))
            {
                var fileStream = new FileStream(fileName, FileMode.Open);
                try
                {
                    result = (T)formatter.Deserialize(fileStream);
                }
                catch (InvalidOperationException)
                { }
                catch (IOException)
                { }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    fileStream.Close();
                }
            }
            return result;
        }

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

        public static Gender GenderByText(string text)
        {
            return (text.IsOneOf("Herr", "Mr."))
                       ? Gender.Male
                       : (text.IsOneOf("Frau", "Mrs."))
                             ? Gender.Female
                             : Gender.Unspecified;
        }
    }
}