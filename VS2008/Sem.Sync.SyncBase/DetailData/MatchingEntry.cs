﻿//-----------------------------------------------------------------------
// <copyright file="MatchingEntry.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.DetailData
{
    using System;

    /// <summary>
    /// To save matching information only few information is needed. Each matching entry
    /// does contain the StdElements ID and a set of profile identifiers. So you can use 
    /// this as a lookup entry inside a list to lookup the std ID of a specific profile ID.
    /// </summary>
    public class MatchingEntry : StdElement
    {
        /// <summary>
        /// Gets or sets a set of profile identifiers.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "This class will be used in XML-Serialization, what means that a ReadOnly property will add a bunch of complexity.")]
        public ProfileIdentifiers ProfileId { get; set; }

        /// <summary>
        /// This method is not implemented and will trow a <see cref="NotImplementedException"/>.
        /// In a <see cref="MatchingEntry"/> there is nothing to normalize.
        /// </summary>
        public override void NormalizeContent()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a meaningful string representation for this object
        /// </summary>
        /// <returns>a meaningful string representation for this object</returns>
        public override string ToString()
        {
            return this.Id.ToString("B") + " matches " + this.ProfileId;
        }
    }
}