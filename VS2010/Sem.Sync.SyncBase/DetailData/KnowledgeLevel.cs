// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KnowledgeLevel.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This enumeration describes a level of knowledge
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.DetailData
{
    /// <summary>
    /// This enumeration describes a level of knowledge
    /// </summary>
    public enum KnowledgeLevel
    {
        /// <summary>
        ///   The knowledge is not known
        /// </summary>
        Unknown = 0, 

        /// <summary>
        ///   The person is a beginner in this field - not able to work in this field without a more expirienced person
        /// </summary>
        Beginner = 300, 

        /// <summary>
        ///   The person is able to work simple tasks in this field without further assistance
        /// </summary>
        Intermediate = 500, 

        /// <summary>
        ///   The person is able to deal with advanced problems in this field
        /// </summary>
        Professional = 600, 

        /// <summary>
        ///   The person is known to be an expert in this field and can serve as a methor
        /// </summary>
        Expert = 900, 
    }
}