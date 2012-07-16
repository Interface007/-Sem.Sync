namespace Sem.Sync.Connector.ActiveDirectory
{
    using System;
    using System.Collections;
    using System.DirectoryServices;
    using System.Linq;

    public static class Extension
    {
        public static bool SetPropertyValue(this DirectoryEntry user, string propertyName, string newValue)
        {
            var properties = (IDictionary)user.Properties;
            if (properties.Contains(propertyName))
            {
                var property = properties[propertyName];
                var propertyCollection = properties[propertyName] as PropertyValueCollection;
                if (propertyCollection != null && (string)propertyCollection[0] == newValue)
                {
                    return false;
                }

                if (propertyCollection == null || propertyCollection.Count != 1)
                {
                    throw new NotImplementedException("handling of " + property.GetType());
                }

                propertyCollection.Clear();
                propertyCollection.Add(newValue);
                return true;
            }

            if (newValue != null)
            { 
                ((PropertyValueCollection)properties[propertyName]).Add(newValue); 
            }

            return true;
        }

        /// <summary>
        /// extracts the first element of a property collection as string
        /// </summary>
        /// <param name="thePropertyCollection"> the result property collection to search </param>
        /// <param name="propName"> the name of the property to extract </param>
        /// <returns> the date that has been extracted </returns>
        public static DateTime GetPropDate(this ResultPropertyCollection thePropertyCollection, string propName)
        {
            if (thePropertyCollection != null
                && thePropertyCollection.Count > 0
                && thePropertyCollection[propName].Count > 0)
            {
                return (DateTime)thePropertyCollection[propName][0];
            }

            return new DateTime();
        }

        /// <summary>
        /// extracts the first element of a property collection as string
        /// </summary>
        /// <param name="thePropertyCollection"> the result property collection to search </param>
        /// <param name="propName"> the name of the property to extract </param>
        /// <returns> the date that has been extracted </returns>
        public static DateTime GetPropDate(this PropertyCollection thePropertyCollection, string propName)
        {
            if (thePropertyCollection != null
                && thePropertyCollection.Count > 0
                && thePropertyCollection[propName].Count > 0)
            {
                return (DateTime)thePropertyCollection[propName][0];
            }

            return new DateTime();
        }

        /// <summary>
        /// extracts the first element of a property collection as string
        /// </summary>
        /// <param name="thePropertyCollection"> the result property collection to search </param>
        /// <param name="propName"> the name of the property to extract </param>
        /// <returns> the string that has been extracted </returns>
        public static string GetPropString(this ResultPropertyCollection thePropertyCollection, string propName)
        {
            if (thePropertyCollection != null
                && thePropertyCollection.Count > 0
                && thePropertyCollection[propName].Count > 0)
            {
                return thePropertyCollection[propName][0].ToString();
            }

            return null;
        }

        /// <summary>
        /// extracts the first element of a property collection as string
        /// </summary>
        /// <param name="thePropertyCollection"> the result property collection to search </param>
        /// <param name="propName"> the name of the property to extract </param>
        /// <returns> the string that has been extracted </returns>
        public static byte[] GetPropBytes(this ResultPropertyCollection thePropertyCollection, string propName)
        {
            if (thePropertyCollection != null
                && thePropertyCollection.Count > 0
                && thePropertyCollection[propName].Count > 0)
            {
                return (byte[])thePropertyCollection[propName][0];
            }

            return null;
        }

        /// <summary>
        /// extracts the first element of a property collection as string
        /// </summary>
        /// <param name="thePropertyCollection"> the result property collection to search </param>
        /// <param name="propName"> the name of the property to extract </param>
        /// <returns> the string that has been extracted </returns>
        public static byte[] GetPropBytes(this PropertyCollection thePropertyCollection, string propName)
        {
            if (thePropertyCollection != null
                && thePropertyCollection.Count > 0
                && thePropertyCollection[propName].Count > 0)
            {
                return (byte[])thePropertyCollection[propName][0];
            }

            return null;
        }

        /// <summary>
        /// extracts the first element of a property collection as string
        /// </summary>
        /// <param name="thePropertyCollection"> the result property collection to search </param>
        /// <param name="propName"> the name of the property to extract </param>
        /// <returns> the string that has been extracted </returns>
        public static string GetPropString(this PropertyCollection thePropertyCollection, string propName)
        {
            if (thePropertyCollection != null
                && thePropertyCollection.Count > 0
                && thePropertyCollection[propName].Count > 0)
            {
                return thePropertyCollection[propName][0].ToString();
            }

            return null;
        }

        /// <summary>
        /// extracts the first element of a property collection as string
        /// </summary>
        /// <param name="thePropertyCollection"> the result property collection to search </param>
        /// <param name="propNamesByPriotity"> the name of the property to extract </param>
        /// <returns> the string that has been extracted </returns>
        public static string GetPropString(this ResultPropertyCollection thePropertyCollection, params string[] propNamesByPriotity)
        {
            if (thePropertyCollection != null && thePropertyCollection.Count > 0)
            {
                return (from name in propNamesByPriotity
                        where thePropertyCollection[name].Count > 0
                        select thePropertyCollection[name][0].ToString()).FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// extracts the first element of a property collection as string
        /// </summary>
        /// <param name="thePropertyCollection"> the result property collection to search </param>
        /// <param name="propNamesByPriotity"> the name of the property to extract </param>
        /// <returns> the string that has been extracted </returns>
        public static string GetPropString(this PropertyCollection thePropertyCollection, params string[] propNamesByPriotity)
        {
            if (thePropertyCollection != null && thePropertyCollection.Count > 0)
            {
                return (from name in propNamesByPriotity
                        where thePropertyCollection[name].Count > 0
                        select thePropertyCollection[name][0].ToString()).FirstOrDefault();
            }

            return null;
        }
    }
}