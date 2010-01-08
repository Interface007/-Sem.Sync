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
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public partial class Notification : Form
    {
        private static List<Notification> ExistingForms = new List<Notification>();

        private ChangeInfo information;

        private BackgroundWorker worker = new BackgroundWorker();

        public Notification()
        {
            InitializeComponent();
        }

        public void ShowChange(ChangeInfo info)
        {
            this.information = info;
            this.worker.DoWork += this.worker_DoWork;
            ExistingForms.Add(this);
            this.worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Show();
            this.Opacity = 0;
            this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            this.Top = Screen.PrimaryScreen.WorkingArea.Height - this.Height;
            
            this.Text = this.information.TargetSystemName;
            this.label1.Text = this.information.DisplayName;

            for (var i = 0; i < 100; i++)
            {
                this.Opacity = (double)i / 100;
                this.Refresh();
                Thread.Sleep(10);
            }
            
            Thread.Sleep(2000);

            for (var i = 100; i > 0; i--)
            {
                this.Opacity = (double)i / 100;
                this.Refresh();
                Thread.Sleep(10);
            }

            ExistingForms.Remove(this);
            this.Close();
        }
    }
}
