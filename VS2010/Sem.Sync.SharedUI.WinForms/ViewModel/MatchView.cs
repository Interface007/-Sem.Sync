// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MatchView.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the MatchView type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Sem.Sync.SyncBase;

namespace Sem.Sync.SharedUI.WinForms.ViewModel
{
    using System;

    /// <summary>
    /// Binding entity for matches.
    /// </summary>
    public class MatchView : IComparable<MatchView>
    {
        public MatchView()
        {
        }

        public MatchView(StdElement sourceElement, StdElement targetElement)
        {
            var sourceContact = sourceElement as StdContact;
            var targetContact = targetElement as StdContact;

            ContactName = sourceContact != null ? sourceContact.GetFullName() : sourceElement.ToString();
            ContactNameMatch = targetContact != null ? targetContact.GetFullName() : targetElement.ToString();

            BaselineId = targetElement.Id;
        }

        /// <summary>
        /// Gets or sets ContactName of this matches source entity.
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// Gets or sets ContactName of this matches target entity.
        /// </summary>
        public string ContactNameMatch { get; set; }

        /// <summary>
        /// Gets or sets the BaselineId (global contact ID of this entity).
        /// </summary>
        public Guid BaselineId { get; set; }

        /// <summary>
        /// Returns a meaningful string representation for this object
        /// </summary>
        /// <returns>a meaningful string representation for this object</returns>
        public override string ToString()
        {
            return this.ContactName + " is matched to " + this.ContactNameMatch;
        }

        /// <summary>
        /// Implements the <see cref="IComparable{T}"/> interface for <see cref="MatchView"/>
        /// </summary>
        /// <param name="other"> The other instance to compare to. </param>
        /// <returns> an integer representing the comparison result (<see cref="IComparable{T}"/>) </returns>
        public int CompareTo(MatchView other)
        {
            return string.CompareOrdinal(this.ToString(), other.ToString());
        }
    }
}