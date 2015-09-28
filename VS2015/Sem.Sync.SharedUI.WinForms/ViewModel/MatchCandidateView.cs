// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MatchCandidateView.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Implements a view entity for a match-candidate
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SharedUI.WinForms.ViewModel
{
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.DetailData;

    /// <summary>
    /// Implements a view entity for a match-candidate
    /// </summary>
    public class MatchCandidateView
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchCandidateView"/> class.
        /// </summary>
        /// <param name="stdElement">
        /// The std element.
        /// </param>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchCandidateView"/> class.
        /// </summary>
        public MatchCandidateView()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the name of the contact.
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        ///   Gets or sets the contact element.
        /// </summary>
        public StdElement Element { get; set; }

        #endregion

        #region Public Methods

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

        #endregion
    }
}