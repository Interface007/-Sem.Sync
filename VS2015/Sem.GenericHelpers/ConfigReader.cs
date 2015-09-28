// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigReader.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers
{
    using System;
    using System.Configuration;
    using System.Xml.Serialization;

    /// <summary>
    /// This generic reader allows to store information inside the *.config
    /// by just defining a new type.
    /// </summary>
    public class ConfigReader : ConfigurationSection
    {
        private static object current; 
        private static Type currentType; 
        private static object sync = new object();
        
        /// <summary>
        /// Reads a config section into a new instance of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">the type that matches the structure of the config section.</typeparam>
        /// <returns>an instance of the config type filled with the values from the configuration section</returns>
        public static TResult GetConfig<TResult>() where TResult : new()
        {
            lock (sync)
            {
                currentType = typeof(TResult);
                ConfigurationManager.GetSection(currentType.Name);
                return (TResult)(current ?? new TResult());
            }
        }

        /// <summary>
        /// Reads the values.
        /// </summary>
        /// <param name="reader"> The config section xml reader. </param>
        protected override void DeserializeSection(System.Xml.XmlReader reader)
        {
            var serializer = new XmlSerializer(currentType);
            current = serializer.Deserialize(reader);
        }
    }
}
