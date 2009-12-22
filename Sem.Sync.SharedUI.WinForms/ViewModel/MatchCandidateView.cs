// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MatchCandidateView.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the MatchCandidateView type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SharedUI.WinForms.ViewModel
{
    using SyncBase;

    /// <summary>
    /// Implements a view entity for a match-candidate
    /// </summary>
    public class MatchCandidateView
    {
        /// <summary>
        /// Gets or sets the name of the contact.
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// Gets or sets the contact element.
        /// </summary>
        public StdContact Element { get; set; }

        public override string ToString()
        {
            return this.ContactName + " - profiles: " + this.Element.PersonalProfileIdentifiers;
        }
    }
}