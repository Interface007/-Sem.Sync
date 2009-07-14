//-----------------------------------------------------------------------
// <copyright file="SyncDescription.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// Describes what to do (the action) for a synchronization command
    /// </summary>
    public enum SyncCommand
    {
        /// <summary>
        /// Copy all new entries from the source client to the destination client;
        /// Skip entries that do already exist.
        /// </summary>
        CopyNew,

        /// <summary>
        /// Copy all entries from the source client to the destination client;
        /// Overwrite existing entries
        /// </summary>
        CopyAll,

        /// <summary>
        /// Inserts only missing elements, existing elements will not be altered
        /// </summary>
        MergeMissing,

        /// <summary>
        /// Merge properties of entities from source to target if there is a very high propability that the
        /// source property is "better" than the target property - "better" is accepted if:
        /// - the target is NULL
        /// - the source datetime is between 1901 and 2200, but the target is not
        /// - the target string is empty, but the source is not
        /// - the target int is 0, but the source is not
        /// - the target Gender is Unspecified
        /// - the target CountryCode is unspecified
        /// - the target byte[] is of length = 0
        /// for complex types each property will be merged individually
        /// </summary>
        MergeHighEvidence,

        /// <summary>
        /// Removes duplicate entries. Calendar-Entries will be compared different to Contact entries.
        /// </summary>
        RemoveDuplicatesOnTarget,

        /// <summary>
        /// Merge entities (e.g. Contacts, Calendar-Items) using an external tool
        /// </summary>
        MergeExternal,

        /// <summary>
        /// Match internally without user interaction by comparing the names
        /// </summary>
        MatchByName,

        /// <summary>
        /// Match using the profile identifiers - each profile identifier is expected to assigned to a unique entity
        /// </summary>
        MatchByProfileId,

        /// <summary>
        /// Opens the matching window and matches using a baseline client
        /// </summary>
        MatchManually,

        /// <summary>
        /// Normalizes content by using lookup tables and removes unneeded whitespace
        /// </summary>
        NormalizeContent,

        /// <summary>
        /// Deletes a files by using a pattern string
        /// </summary>
        DeletePattern,

        /// <summary>
        /// Detects merge conflicts and resolves them using user interaction
        /// </summary>
        DetectConflicts,

        /// <summary>
        /// Asks the user if the processing should continue - the command parameter is the text to be displayed
        /// </summary>
        AskForContinue,

        /// <summary>
        /// Performs a shell execute to open the document specified as the command parameter
        /// </summary>
        OpenDocument,
    }

    /// <summary>
    /// This class represents a job description for the sync engine. It provides the class names of the three 
    /// connectors (source, target and baseline) as well as Parameter information for each connector and
    /// for the command.
    /// </summary>
    [Serializable]
    public class SyncDescription
    {
        /// <summary>
        /// Gets or sets the human readable name of this command. The name is persisted as an XmlAttribute instead 
        /// of an element.
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the command to execute.
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Gets or sets the parameter for the command to be executed.
        /// </summary>
        public string CommandParameter { get; set; }

        /// <summary>
        /// Gets or sets the name of the source connector. This name can be incomplete (without the 
        /// assembly name), if the assembly name is equal to the name space for the connector class.
        /// This can also be a generic class specified as <c>classname of detailclass</c>
        /// </summary>
        public string SourceConnector { get; set; }

        /// <summary>
        /// Gets or sets the parameter for the source connector. This is normally a specification for 
        /// the location/filter inside the storage this connector connects to.
        /// </summary>
        public string SourceStorePath { get; set; }

        /// <summary>
        /// Gets or sets the name of the target connector. This name can be incomplete (without the 
        /// assembly name), if the assembly name is equal to the name space for the connector class.
        /// This can also be a generic class specified as <c>classname of detailclass</c>
        /// </summary>
        public string TargetConnector { get; set; }

        /// <summary>
        /// Gets or sets the parameter for the target connector. This is normally a specification for 
        /// the location/filter inside the storage this connector connects to.
        /// </summary>
        public string TargetStorePath { get; set; }

        /// <summary>
        /// Gets or sets the name of the base line connector. This name can be incomplete (without the 
        /// assembly name), if the assembly name is equal to the name space for the connector class.
        /// This can also be a generic class specified as <c>classname of detailclass</c>
        /// </summary>
        public string BaselineConnector { get; set; }

        /// <summary>
        /// Gets or sets the parameter for the base line connector. This is normally a specification for 
        /// the location/filter inside the storage this connector connects to.
        /// </summary>
        public string BaselineStorePath { get; set; }
    }
}
