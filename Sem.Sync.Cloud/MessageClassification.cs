// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageClassification.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the MessageClassification type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Cloud
{
    /// <summary>
    /// Classifies the messages so that you can react in code
    /// </summary>
    public enum MessageClassification
    {
        /// <summary>
        /// The default message classifivation is something like an information 
        /// - don't care in code, but you may show this to the user.
        /// </summary>
        Default = 0,

        /// <summary>
        /// A warning does not impact data consistency, but the consequences might be unintended.
        /// </summary>
        Warning = 1,

        /// <summary>
        /// An error is an exception that should not happen and does impact the flow of the program.
        /// </summary>
        Error = 2,

        /// <summary>
        /// Something really did went wrong in a way we cannot recover - we might lost some data.
        /// </summary>
        Critical = 3,
    }
}