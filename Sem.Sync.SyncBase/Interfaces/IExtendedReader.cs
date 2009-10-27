// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExtendedReader.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the IExtendedReader type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Interfaces
{
    using System.Collections.Generic;

    using DetailData;

    /// <summary>
    /// Provides access to additional data collection methods
    /// </summary>
    public interface IExtendedReader
    {
        /// <summary>
        /// Reads additional contact relations with known contacts.
        /// </summary>
        /// <param name="contactToFill"> The contact to fill. </param>
        /// <param name="baseline"> The baseline to lookup the contact id. </param>
        /// <returns> the "enriched" element </returns>
        StdElement FillContacts(StdElement contactToFill, List<MatchingEntry> baseline);
    }
}