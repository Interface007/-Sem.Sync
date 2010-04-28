// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MatchCandidateView.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the MatchCandidateView type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;

namespace Sem.Sync.SharedUI.WinForms.ViewModel
{
    using SyncBase;

    /// <summary>
    /// Implements a view entity for a match-candidate
    /// </summary>
    public class MatchCandidateView
    {
        public MatchCandidateView(StdElement stdElement)
        {
            this.Element = stdElement;
            var contact = stdElement as StdContact;
            if (contact != null)
            {
                this.ContactName = contact.GetFullName();
                return;
            }
            
            this.ContactName = stdElement.ToString();
        }

        public MatchCandidateView()
        {
        }

        /// <summary>
        /// Gets or sets the name of the contact.
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// Gets or sets the contact element.
        /// </summary>
        public StdElement Element { get; set; }

        /// <summary>
        /// Overrides the <see cref="object.ToString"/> by returning a meaningful string representation of the data.
        /// </summary>
        /// <returns>
        /// ContactName + " - profiles: " + ExternalIdentifier
        /// </returns>
        public override string ToString()
        {
            return this.ContactName + " - profiles: " + this.Element.ExternalIdentifier;
        }
    }
}