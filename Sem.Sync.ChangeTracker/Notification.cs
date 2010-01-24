// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Notification.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the Notification type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.ChangeTracker
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    /// <summary>
    /// Form that will show the change information
    /// </summary>
    public partial class Notification : Form
    {
        /// <summary>
        /// List of currently "active" forms that will fade in/fade out.
        /// </summary>
        private static readonly List<Notification> ExistingForms = new List<Notification>();

        /// <summary>
        /// state if this form has already been shown with 100% opacity
        /// </summary>
        private bool fadeInCompleted;

        /// <summary>
        /// Information to be shown within this form.
        /// </summary>
        private ChangeInfo information;

        /// <summary>
        /// Initializes a new instance of the <see cref="Notification"/> class.
        /// </summary>
        public Notification()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the List of currently "active" forms that will fade in/fade out.
        /// </summary>
        public static IList<Notification> Forms
        {
            get
            {
                return ExistingForms;
            }
        }

        /// <summary>
        /// Method to fill this form and fade in.
        /// </summary>
        /// <param name="info"> The info to show. </param>
        public void ShowChange(ChangeInfo info)
        {
            this.information = info;
            ExistingForms.Add(this);

            this.Show();
            
            this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            this.Top = Screen.PrimaryScreen.WorkingArea.Height - this.Height;

            this.Text = this.information.TargetSystemName;
            this.label1.Text = this.information.DisplayName;
            
            this.FadeOutTimer.Enabled = true;
        }

        /// <summary>
        /// Tick event of the timer to increase/decrease the opacity of the form.
        /// </summary>
        /// <param name="sender"> The sender control (timer) of this event. </param>
        /// <param name="e"> The event argument. </param>
        private void FadeOutTimer_Tick(object sender, EventArgs e)
        {
            if (!this.fadeInCompleted)
            {
                if (this.Opacity > 0.99)
                {
                    this.fadeInCompleted = true;
                    return;
                }

                this.Opacity = this.Opacity + 0.05;
                return;   
            }

            if (!acknowledged.Checked)
            {
                return;
            }

            this.Opacity = this.Opacity - 0.3;
            
            if (this.Opacity != 0)
            {
                return;
            }

            this.FadeOutTimer.Enabled = false;
            ExistingForms.Remove(this);
            this.Close();
        }

        /// <summary>
        /// The checkbox controls whether the fade out process will be started.
        /// </summary>
        /// <param name="sender"> The sender of this event. </param> 
        /// <param name="e"> The event argument. </param>
        private void Acknowledged_CheckedChanged(object sender, EventArgs e)
        {
            acknowledged.Enabled = false;
        }
    }
}
