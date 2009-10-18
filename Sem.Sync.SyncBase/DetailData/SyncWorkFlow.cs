// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncWorkFlow.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines a concrete workflow based on a template.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.DetailData
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using GenericHelpers.Entities;

    /// <summary>
    /// Defines a concrete workflow based on a template.
    /// </summary>
    [XmlInclude(typeof(Credentials))]
    [XmlInclude(typeof(KeyValuePair<string, string>))]
    public class SyncWorkFlow
    {
        /// <summary>
        /// Gets or sets the human readable name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the connector information for the source.
        /// </summary>
        public ConnectorInformation Source { get; set; }

        /// <summary>
        /// Gets or sets the connector information for the source.
        /// </summary>
        public ConnectorInformation Target { get; set; }

        /// <summary>
        /// Gets or sets the Template to use.
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        /// replaces some internal token to insert the workflow data into a workflow template.a
        /// </summary>
        /// <param name="value"> The value that may contain tokens. </param>
        /// <returns> The value with replaced tokens. </returns>
        public string ReplaceToken(string value)
        {
            var returnvalue = (value ?? string.Empty)
                .Replace("{source}", this.Source.Name)
                .Replace("{target}", this.Target.Name)
                .Replace("{sourcepath}", this.Source.Path)
                .Replace("{targetpath}", this.Target.Path);

            if (this.Source.ConnectorDescription != null)
            {
                returnvalue = returnvalue.Replace(
                    "{sourcepersonalidentifier}",
                    Enum.GetName(typeof(ProfileIdentifierType), this.Source.ConnectorDescription.MatchingIdentifier));
            }

            if (this.Target.ConnectorDescription != null)
            {
                returnvalue = returnvalue.Replace(
                    "{targetpersonalidentifier}",
                    Enum.GetName(typeof(ProfileIdentifierType), this.Target.ConnectorDescription.MatchingIdentifier));
            }

            return returnvalue;
        }
    }
}