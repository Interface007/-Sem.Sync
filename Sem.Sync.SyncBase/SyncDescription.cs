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

    [Serializable]
    public class SyncDescription
    {
        [XmlAttribute]
        public string Name { get; set; }
        public SyncCommand Command { get; set; }
        public string CommandParameter { get; set; }
        public string SourceConnector { get; set; }
        public string SourceStorePath { get; set; }
        public string TargetConnector { get; set; }
        public string TargetStorePath { get; set; }
        public string BaselineConnector { get; set; }
        public string BaselineStorePath { get; set; }
    }

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
        /// 
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
}
