// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the MainWindow type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.ChangeTracker
{
    using System.Windows.Forms;

    public partial class MainWindow : Form
    {
        private CheckAgent agent;
        
        public MainWindow()
        {
            InitializeComponent();
            this.agent = new CheckAgent();
            this.agent.DataChanged += this.agent_DataChanged;
        }

        void agent_DataChanged(object sender, System.EventArgs e)
        {
            this.Invoke(
                new MethodInvoker(
                    () =>
                    {
                        // refresh databinding
                        this.Sources.DataSource = null;
                        this.Sources.DataSource = this.agent.DetectedChanges;
                        this.Sources.DisplayMember = "DisplayName";
                    }));
        }
    }
}
