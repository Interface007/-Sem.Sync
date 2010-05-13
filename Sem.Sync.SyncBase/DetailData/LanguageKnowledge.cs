// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LanguageKnowledge.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Describes the know how about reading, talking and writing languages
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.DetailData
{
    using System;

    /// <summary>
    /// Describes the know how about reading, talking and writing languages
    /// </summary>
    [Serializable]
    public class LanguageKnowledge
    {
        /// <summary>
        /// Gets or sets the name of the language.
        /// </summary>
        public string LanguageName { get; set; }

        /// <summary>
        /// Gets or sets the overall level of knowledge about using the language.
        /// </summary>
        public KnowledgeLevel Level { get; set; }
    }
}