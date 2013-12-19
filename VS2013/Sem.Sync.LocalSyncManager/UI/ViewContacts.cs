// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewContacts.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Implements the form code
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.LocalSyncManager.UI
{
    using System.Windows.Forms;

    using Sem.Sync.LocalSyncManager.Business;

    /// <summary>
    /// Implements the form code
    /// </summary>
    public partial class ViewContacts : Form
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ViewContacts" /> class.
        /// </summary>
        public ViewContacts()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the contact folder including the connector.
        /// </summary>
        public ContactsFolder ContactsFolder { get; set; }

        #endregion
    }
}