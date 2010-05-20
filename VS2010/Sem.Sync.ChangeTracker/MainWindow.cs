// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The main window.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.ChangeTracker
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// The main window.
    /// </summary>
    public partial class MainWindow : Form
    {
        #region Constants and Fields

        /// <summary>
        /// The agent.
        /// </summary>
        private readonly CheckAgent agent;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.agent = new CheckAgent();
            this.agent.DataChanged += this.agent_DataChanged;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The agent_ data changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void agent_DataChanged(object sender, EventArgs e)
        {
            this.Invoke(
                new MethodInvoker(
                    () =>
                        {
                            // refresh databinding
                            this.Sources.DataSource = null;
                            this.Sources.DataSource = this.agent.DetectedChanges;
                            this.Sources.DisplayMember = "DisplayName";

                            if (this.agent.DetectedChanges.Count <= 0 || Notification.Forms.Count != 0)
                            {
                                return;
                            }

                            var notification = new Notification();
                            notification.ShowChange(this.agent.DetectedChanges[0]);
                            this.agent.DetectedChanges.RemoveAt(0);
                        }));
        }

        #endregion
    }
}