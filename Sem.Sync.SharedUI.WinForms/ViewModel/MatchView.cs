// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MatchView.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Binding entity for matches.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SharedUI.WinForms.ViewModel
{
    using System;

    using Sem.Sync.SyncBase;

    /// <summary>
    /// Binding entity for matches.
    /// </summary>
    public class MatchView : IComparable<MatchView>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchView"/> class.
        /// </summary>
        public MatchView()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchView"/> class.
        /// </summary>
        /// <param name="sourceElement">
        /// The source element.
        /// </param>
        /// <param name="targetElement">
        /// The target element.
        /// </param>
        public MatchView(StdElement sourceElement, StdElement targetElement)
        {
            var sourceContact = sourceElement as StdContact;
            var targetContact = targetElement as StdContact;

            this.ContactName = sourceContact != null ? sourceContact.GetFullName() : sourceElement.ToString();
            this.ContactNameMatch = targetContact != null ? targetContact.GetFullName() : targetElement.ToString();

            this.BaselineId = targetElement.Id;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the BaselineId (global contact ID of this entity).
        /// </summary>
        public Guid BaselineId { get; set; }

        /// <summary>
        ///   Gets or sets ContactName of this matches source entity.
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        ///   Gets or sets ContactName of this matches target entity.
        /// </summary>
        public string ContactNameMatch { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a meaningful string representation for this object
        /// </summary>
        /// <returns>
        /// a meaningful string representation for this object
        /// </returns>
        public override string ToString()
        {
            return this.ContactName + " is matched to " + this.ContactNameMatch;
        }

        #endregion

        #region Implemented Interfaces

        #region IComparable<MatchView>

        /// <summary>
        /// Implements the <see cref="IComparable{T}"/> interface for <see cref="MatchView"/>
        /// </summary>
        /// <param name="other">
        /// The other instance to compare to. 
        /// </param>
        /// <returns>
        /// an integer representing the comparison result (<see cref="IComparable{T}"/>) 
        /// </returns>
        public int CompareTo(MatchView other)
        {
            return string.CompareOrdinal(this.ToString(), other.ToString());
        }

        #endregion

        #endregion
    }
}