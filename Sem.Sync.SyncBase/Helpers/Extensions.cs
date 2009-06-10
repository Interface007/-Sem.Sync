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
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    using DetailData;

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
        /// <returns></returns>
        public static List<StdElement> MergeHighEvidence(this List<StdElement> target, List<StdElement> source)
        {
            foreach (var targetItem in target)
            {
                var sourceItem = (from item in source
                                  where item.Id == targetItem.Id
                                  select item).FirstOrDefault();

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
        /// <returns></returns>
        public static T MergeHighEvidence<T>(this T target, T source, Type typeToUse)
        {
            if (typeToUse == null) typeToUse = typeof(T);
            var members = typeToUse.GetProperties();
            foreach (var item in members)
            {
                var setValue = false;
                if (Equals(source, default(T)) || item.GetValue(source, null) == null)
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

                        case "DateTime":
                            setValue = (((DateTime)targetValue).Year < 1901 || ((DateTime)targetValue).Year > 2200)
                                && !(((DateTime)sourceValue).Year < 1901 || ((DateTime)sourceValue).Year > 2200);
                            break;

                        case "Int32":
                            setValue = (Int32)targetValue == 0
                                && (Int32)sourceValue != 0;
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
                            setValue = ((byte[])sourceValue).Length > 0;
                            break;

                        case "SyncData":
                        case "PersonName":
                        case "AddressDetail":
                        case "PhoneNumber":
                        case "InstantMessengerAddresses":
                        case "ProfileIdentifiers":
                            targetValue.MergeHighEvidence(sourceValue, item.PropertyType);
                            break;

                        default:
                            targetValue.MergeHighEvidence(sourceValue, item.PropertyType);
                            break;
                    }
                }

                if (setValue)
                    item.SetValue(target, item.GetValue(source, null), null);

            }
            return target;
        }


        /// <summary>
        /// Expands the methods of a List my adding a serialization to disk
        /// </summary>
        /// <typeparam name="T">the type of elements in this list</typeparam>
        /// <param name="elementList">the list instance that should be sezialized</param>
        /// <param name="destinationFile">the target file for the serialization</param>
        /// <param name="extraTypes">you need to add types that are used in the list here</param>
        public static List<T> SaveTo<T>(this List<T> elementList, string destinationFile, Type[] extraTypes)
        {
            if (destinationFile == null) throw new ArgumentNullException("destinationFile");

            // the xml serializer needs the additional type of the element
            try
            {
                var formatter = new XmlSerializer(typeof(List<T>), extraTypes);
                SyncTools.EnsurePathExist(Path.GetDirectoryName(destinationFile));
                var file = new FileStream(destinationFile, FileMode.Create);

                try
                {
                    formatter.Serialize(file, elementList);
                }
                finally
                {
                    file.Close();
                }

            }
            catch
            {
            }

            return elementList;
        }

        /// <summary>
        /// Expands the methods of a List my adding a serialization to disk
        /// </summary>
        /// <typeparam name="T">the type of elements in this list</typeparam>
        /// <param name="elementList">the list instance that should be sezialized</param>
        /// <param name="sourceFile">the target file for the serialization</param>
        /// <param name="extraTypes">you need to add types that are used in the list here</param>
        public static List<T> LoadFrom<T>(this List<T> elementList, string sourceFile, Type[] extraTypes)
        {
            if (sourceFile == null) throw new ArgumentNullException("sourceFile");

            // the xml serializer needs the additional type of the element
            var formatter = new XmlSerializer(typeof(List<T>), extraTypes);

            SyncTools.EnsurePathExist(Path.GetDirectoryName(sourceFile));
            if (File.Exists(sourceFile))
            {
                var file = new FileStream(sourceFile, FileMode.Create);
                try
                {
                    if (file.Length > 0)
                    {
                        elementList = (List<T>)formatter.Deserialize(file);
                    }
                }
                catch
                {
                }
                finally
                {
                    file.Close();
                }
            }
            return elementList;
        }

        /// <summary>
        /// Converts a list of standard contacts to a list of standard elements
        /// </summary>
        /// <param name="list">a list of standard contacts to cast</param>
        /// <returns>a list of casted elements</returns>
        public static List<StdContact> ToContacts(this List<StdElement> list)
        {
            var result = new List<StdContact>();
            foreach (var element in list)
            {
                result.Add((StdContact)element);
            }
            return result;
        }

        public static List<MatchingEntry> ToMatchingEntries(this List<StdElement> list)
        {
            var result = new List<MatchingEntry>();
            foreach (var element in list)
            {
                result.Add((MatchingEntry)element);
            }
            return result;
        }

        public static List<StdElement> ToStdElement<T>(this List<T> list) where T : StdElement
        {
            var result = new List<StdElement>();
            foreach (var element in list)
            {
                result.Add(element);
            }
            return result;
        }

        public static List<StdCalendarItem> ToCalendatItems(this List<StdElement> list)
        {
            var result = new List<StdCalendarItem>();
            foreach (var element in list)
            {
                result.Add((StdCalendarItem)element);
            }
            return result;
        }
    }
}