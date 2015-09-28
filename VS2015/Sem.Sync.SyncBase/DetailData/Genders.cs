// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Genders.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Represents a specification about the gender/sex of a person
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.DetailData
{
    /// <summary>
    /// Represents a specification about the gender/sex of a person
    /// </summary>
    public enum Gender
    {
        /// <summary>
        ///   there is no information about the gender of the person
        /// </summary>
        Unspecified = 0, 

        /// <summary>
        ///   The person is definitely a male
        /// </summary>
        Male = 1, 

        /// <summary>
        ///   The person is definitely a female
        /// </summary>
        Female = 2
    }
}