//-----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Sven Erik Matzen">
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
    using System.Xml.Serialization;

    using DetailData;

    using GenericHelpers;

    /// <summary>
    /// This static class defines extension methods for variuos types. The extension methods
    /// are helper methods that do ease the reading (and writing) of the main code.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Determines if a string is equal to one of the parameter strings.
        /// </summary>
        /// <param name="theString">the string to test</param>
        /// <param name="candidates">the candidate string this string is compared to</param>
        /// <returns>true if the string is equal to one of the candidates</returns>
        public static bool IsOneOf(this string theString, params string[] candidates)
        {
            if (candidates == null)
            {
                throw new ArgumentNullException("candidates");
            }

            return candidates.Contains(theString);
        }

        /// <summary>
        /// Performs a "high evidence merge" between two lists. This mean that missing property values at the target will be
        /// written from the source. Also some clearly wrong data in context of contact data will be overwritten by the values
        /// from the source, if that does contain a "better" quality (e.g. dates out of reasonable range will be
        /// overwritten if more accurate data is available in the source)
        /// </summary>
        /// <param name="target">the list that will be returned in an "enriched" state</param>
        /// <param name="source">the list that contains the elements that might contribute content</param>
        /// <returns>the merged list</returns>
        public static List<StdElement> MergeHighEvidence(this List<StdElement> target, List<StdElement> source)
        {
            foreach (var targetItem in target)
            {
                if (targetItem == null)
                {
                    continue;
                }

                // ReSharper disable AccessToModifiedClosure
                var sourceItem = (from item in source
                                  where item.Id == targetItem.Id
                                  select item).FirstOrDefault();

                // ReSharper restore AccessToModifiedClosure
                if (targetItem is StdContact)
                {
                    targetItem.MergeHighEvidence(sourceItem, typeof(StdContact));
                }
                else
                {
                    targetItem.MergeHighEvidence(sourceItem, null);
                }
            }

            return target;
        }

        /// <summary>
        /// Performs a "high evidence merge" between two objects. This mean that missing property values at the target will be
        /// written from the source. Also some clearly wrong data in context of contact data will be overwritten by the values
        /// from the source, if that does contain a "better" quality (e.g. dates out of reasonable range will be
        /// overwritten if more accurate data is available in the source)
        /// </summary>
        /// <typeparam name="T">the type of the objects</typeparam>
        /// <param name="target">the list that will be returned in an "enriched" state</param>
        /// <param name="source">the list that contains the elements that might contribute content</param>
        /// <param name="typeToUse">if this is not null, this type will be used instead of the type parameter</param>
        /// <returns>the merged list</returns>
        public static T MergeHighEvidence<T>(this T target, T source, Type typeToUse)
        {
            typeToUse = typeToUse ?? typeof(T);

            var members = typeToUse.GetProperties();
            foreach (var item in members)
            {
                var setValue = false;
                if (Equals(source, default(T))
                    || item.GetValue(source, null) == null)
                {
                    continue;
                }

                if (item.GetValue(target, null) == null)
                {
                    setValue = item.GetValue(source, null) != null;
                }
                else
                {
                    var sourceValue = item.GetValue(source, null);
                    var targetValue = item.GetValue(target, null);
                    switch (item.PropertyType.Name)
                    {
                        case "Guid":
                            setValue = ((Guid)targetValue == new Guid())
                                && ((Guid)sourceValue != new Guid());
                            break;

                        case "String":
                            setValue = string.IsNullOrEmpty((string)targetValue)
                                && !string.IsNullOrEmpty((string)sourceValue);
                            break;

                        case "TimeSpan":
                            setValue = (((TimeSpan)targetValue).TotalMinutes > 0)
                                && !(((TimeSpan)sourceValue).TotalMinutes > 0);
                            break;

                        case "DateTime":
                            setValue = (((DateTime)targetValue).Year < 1901 || ((DateTime)targetValue).Year > 2200)
                                && !(((DateTime)sourceValue).Year < 1901 || ((DateTime)sourceValue).Year > 2200);
                            break;

                        case "Int32":
                            setValue = (int)targetValue == 0
                                && (int)sourceValue != 0;
                            break;

                        case "Gender":
                            setValue = (Gender)targetValue == Gender.Unspecified
                                && (Gender)sourceValue != Gender.Unspecified;
                            break;

                        case "CountryCode":
                            setValue = (CountryCode)targetValue == CountryCode.unspecified
                                && (CountryCode)sourceValue != CountryCode.unspecified;
                            break;

                        case "Byte[]":
                            setValue = ((byte[])sourceValue).Length > ((byte[])targetValue).Length;
                            break;

                        case "List`1":
                            (sourceValue as List<string>).MergeList(targetValue as List<string>);
                            (sourceValue as List<KeyValuePair<string, ProfileIdInformation>>).MergeList(targetValue as List<KeyValuePair<string, ProfileIdInformation>>);
                            break;

                        case "ProfileIdentifiers":
                            var targetProfiles = targetValue as ProfileIdentifiers;
                            if (targetProfiles != null)
                            {
                                (sourceValue as ProfileIdentifiers)
                                    .ForEach(x => targetProfiles.SetProfileId(x.Key, x.Value, true));
                            }
                            
                            break;

                        case "SerializableDictionary`2":
                            var sourcedic = sourceValue as SerializableDictionary<string, string>;
                            var targetdic = targetValue as SerializableDictionary<string, string>;

                            if (sourcedic != null && sourcedic != targetdic)
                            {
                                if (targetdic == null)
                                {
                                    targetdic = new SerializableDictionary<string, string>();
                                    //item.SetValue(targetValue, targetdic, null);
                                }

                                foreach (var sourceDicItem in sourcedic)
                                {
                                    if (targetdic.Keys.Contains(sourceDicItem.Key))
                                    {
                                        targetdic[sourceDicItem.Key] = sourceDicItem.Value;
                                    }
                                    else
                                    {
                                        targetdic.Add(sourceDicItem.Key, sourceDicItem.Value);
                                    }
                                }
                            }
                            break;


                        case "SyncData":
                        case "PersonName":
                        case "AddressDetail":
                        case "PhoneNumber":
                        case "InstantMessengerAddresses":
                            targetValue.MergeHighEvidence(sourceValue, item.PropertyType);
                            break;

                        default:
                            targetValue.MergeHighEvidence(sourceValue, item.PropertyType);
                            break;
                    }
                }

                if (setValue)
                {
                    item.SetValue(target, item.GetValue(source, null), null);
                }
            }

            return target;
        }

        /// <summary>
        /// merges two lists of strings by omitting the strings that are already present in the target list
        /// </summary>
        /// <param name="targetValue">the list into which should be merged (will be modified!)</param>
        /// <param name="sourceValue">the list with items to be inserted (will not be modified)</param>
        /// <returns>the <paramref name="targetValue"/></returns>
        public static List<string> MergeList(this List<string> targetValue, List<string> sourceValue)
        {
            if (targetValue == null)
            {
                targetValue = new List<string>();
            }

            if (sourceValue != null)
            {
                foreach (var sourceString in sourceValue)
                {
                    var currentString = sourceString;
                    if (!targetValue.Exists(x => x.Equals(currentString, StringComparison.OrdinalIgnoreCase)))
                    {
                        targetValue.Add(currentString);
                    }
                }
            }

            return targetValue;
        }

        /// <summary>
        /// merges two lists of strings by omitting the strings that are already present in the target list
        /// </summary>
        /// <param name="targetValue">the list into which should be merged (will be modified!)</param>
        /// <param name="sourceValue">the list with items to be inserted (will not be modified)</param>
        /// <returns>the <paramref name="targetValue"/></returns>
        public static List<KeyValuePair<string, ProfileIdInformation>> MergeList(this List<KeyValuePair<string, ProfileIdInformation>> targetValue, List<KeyValuePair<string, ProfileIdInformation>> sourceValue)
        {
            if (targetValue == null)
            {
                targetValue = new List<KeyValuePair<string, ProfileIdInformation>>();
            }

            if (sourceValue != null)
            {
                foreach (var sourceItem in sourceValue)
                {
                    var currentItem = sourceItem;
                    if (!targetValue.Exists(x => x.Key == currentItem.Key && x.Value == currentItem.Value))
                    {
                        targetValue.Add(currentItem);
                    }
                }
            }

            return targetValue;
        }

        /// <summary>
        /// Expands the methods of a List my adding a serialization to disk
        /// </summary>
        /// <typeparam name="T">the type of elements in this list</typeparam>
        /// <param name="elementList">the list instance that should be sezialized</param>
        /// <param name="destinationFile">the target file for the serialization</param>
        /// <param name="extraTypes">you need to add types that are used in the list here</param>
        /// <returns>The saved list.</returns>
        public static List<T> SaveTo<T>(this List<T> elementList, string destinationFile, Type[] extraTypes)
        {
            if (destinationFile == null)
            {
                throw new ArgumentNullException("destinationFile");
            }

            // the xml serializer needs the additional type of the element
            var formatter = new XmlSerializer(typeof(List<T>), extraTypes);
            Tools.EnsurePathExist(Path.GetDirectoryName(destinationFile));
            using (var file = new FileStream(destinationFile, FileMode.Create))
            {
                formatter.Serialize(file, elementList);
            }

            return elementList;
        }

        /// <summary>
        /// Expands the methods of a List my adding a serialization to a string
        /// </summary>
        /// <typeparam name="T">the type of elements in this list</typeparam>
        /// <param name="elementList">the list instance that should be sezialized</param>
        /// <param name="extraTypes">you need to add types that are used in the list here</param>
        /// <returns>The saved list as a string.</returns>
        public static string SaveToString<T>(this List<T> elementList, params Type[] extraTypes)
        {
            // the xml serializer needs the additional type of the element
            var formatter = new XmlSerializer(typeof(List<T>), extraTypes);
            var writer = new StringWriter(CultureInfo.CurrentCulture);
            formatter.Serialize(writer, elementList);
            return writer.ToString();
        }

        /// <summary>
        /// Expands the methods of a List by adding a de-serialization to disk
        /// </summary>
        /// <typeparam name="T">the type of elements in this list</typeparam>
        /// <param name="elementList">the list instance that should be deserialized to</param>
        /// <param name="sourceFile">the source file for the de-serialization</param>
        /// <param name="extraTypes">you need to add types that are used in the list here</param>
        /// <returns>The list loaded from the file system.</returns>
        public static List<T> LoadFrom<T>(this List<T> elementList, string sourceFile, Type[] extraTypes)
        {
            if (sourceFile == null)
            {
                throw new ArgumentNullException("sourceFile");
            }

            // the xml serializer needs the additional type of the element
            var formatter = new XmlSerializer(typeof(List<T>), extraTypes);

            Tools.EnsurePathExist(Path.GetDirectoryName(sourceFile));
            if (File.Exists(sourceFile))
            {
                using (var file = new FileStream(sourceFile, FileMode.OpenOrCreate))
                {
                    if (file.Length > 0)
                    {
                        elementList = (List<T>)formatter.Deserialize(file);
                    }
                }
            }

            return elementList;
        }

        /// <summary>
        /// Expands the methods of a List by adding a de-serialization from string
        /// </summary>
        /// <typeparam name="T">the type of elements in this list</typeparam>
        /// <param name="elementList">the list instance that should be deserialized to</param>
        /// <param name="sourceString">the source string for the de-serialization</param>
        /// <param name="extraTypes">you need to add types that are used in the list here</param>
        /// <returns>The list loaded from the string.</returns>
// ReSharper disable RedundantAssignment
        public static T LoadFromString<T>(this T elementList, string sourceString, params Type[] extraTypes)
// ReSharper restore RedundantAssignment
        {
            if (string.IsNullOrEmpty(sourceString))
            {
                throw new ArgumentNullException("sourceString");
            }

            // the xml serializer needs the additional type of the element
            var formatter = new XmlSerializer(typeof(T), extraTypes);
            var reader = new StringReader(sourceString);
            elementList = (T)formatter.Deserialize(reader);
            
            return elementList;
        }

        /// <summary>
        /// Converts a list of standard contacts to a list of standard elements
        /// </summary>
        /// <param name="list">a list of standard contacts to cast</param>
        /// <returns>a list of casted elements</returns>
        public static List<StdContact> ToStdContacts(this IEnumerable<StdElement> list)
        {
            var result = new List<StdContact>();
            foreach (var element in list)
            {
                var e = element as StdContact;

                if (e == null)
                {
                    var m = element as MatchingEntry;
                    if (m != null)
                    {
                        e = new StdContact
                            {
                                Id = element.Id,
                                ExternalIdentifier = m.ProfileId
                            };
                    }
                }

                if (e != null)
                {
                    result.Add(e);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts a list of standard contacts to a list of standard elements
        /// </summary>
        /// <param name="list">a list of standard contacts to cast</param>
        /// <returns>a list of casted elements</returns>
        public static List<StdCalendarItem> ToStdCalendarItems(this IEnumerable<StdElement> list)
        {
            var result = new List<StdCalendarItem>();
            foreach (var element in list)
            {
                var e = element as StdCalendarItem;

                if (e == null)
                {
                    var m = element as MatchingEntry;
                    if (m != null)
                    {
                        e = new StdCalendarItem
                            {
                                Id = element.Id,
                                ExternalIdentifier = m.ProfileId
                            };
                    }
                }

                if (e != null)
                {
                    result.Add(e);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts a list of typed elements to a list of different typed elements. This is usefull to
        /// cast a list of type 1 into a list of type 2.
        /// </summary>
        /// <typeparam name="TSource"> The type of elements in the source </typeparam>
        /// <typeparam name="TDestination"> the type of elements to cast to. </typeparam>
        /// <param name="list"> a list of elements to cast </param>
        /// <returns> a list of casted elements </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "There is no design to accieve type inference in fluent interfaces for this method.")]
        public static List<TDestination> ToOtherType<TSource, TDestination>(this List<TSource> list) 
            where TDestination : class
        {
            var result = new List<TDestination>();
            foreach (var element in list)
            {
                var e = element as TDestination;

                if (e != null)
                {
                    result.Add(e);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts a list of <see cref="StdElement"/> to a list of <see cref="MatchingEntry"/> by omitting all
        /// entries that cannot be casted to a <see cref="MatchingEntry"/> .
        /// </summary>
        /// <param name="list">The list of <see cref="StdElement"/> to be converted</param>
        /// <returns>The resulting list of <see cref="MatchingEntry"/></returns>
        public static List<MatchingEntry> ToMatchingEntries(this List<StdElement> list)
        {
            var result = new List<MatchingEntry>();
            foreach (var element in list)
            {
                var e = element as MatchingEntry;
                if (e != null)
                {
                    result.Add(e);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts a list of some type to a list of <see cref="StdElement"/> by omitting all
        /// entries that cannot be casted to a <see cref="StdElement"/>.
        /// </summary>
        /// <typeparam name="T"> the type of conversion. </typeparam>
        /// <param name="list"> The list of something to be converted </param>
        /// <returns> The resulting list of <see cref="StdElement"/> </returns>
        public static List<StdElement> ToStdElements<T>(this IEnumerable<T> list) where T : StdElement
        {
            return list.Cast<StdElement>().ToList();
        }

        /// <summary>
        /// Performs a lookup of a <see cref="StdContact"/> by the <see cref="StdElement.Id"/>.
        /// </summary>
        /// <param name="list">the list to be searched</param>
        /// <param name="uid">the id of the contect to be returned</param>
        /// <returns>the contact with that id or null if there is no such contact inside the list</returns>
        public static StdContact GetContactById(this List<StdContact> list, string uid)
        {
            var idToSearch = new Guid(uid);
            var result = (from x in list where x.Id == idToSearch select x).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Performs a lookup of a <see cref="StdElement"/> by the <see cref="StdElement.Id"/>.
        /// </summary>
        /// <param name="list">the list to be searched</param>
        /// <param name="uid">the id of the contect to be returned</param>
        /// <returns>the contact with that id or null if there is no such contact inside the list</returns>
        public static StdContact GetContactById(this List<StdElement> list, string uid)
        {
            var idToSearch = new Guid(uid);
            var result = (from x in list where x.Id == idToSearch select x).FirstOrDefault();
            return result as StdContact;
        }
    }
}