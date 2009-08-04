// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Networks.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the Networks type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.LocalSyncManager.UI
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    using Business;

    /// <summary>
    /// User interface for wizard-like interaction
    /// </summary>
    public partial class Networks : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Networks"/> class.
        /// </summary>
        public Networks()
        {
            InitializeComponent();
            this.DataContext = new SyncWizardContext();
            
            this.Network = new List<Button>();
            foreach (var source in this.DataContext.ClientsSource)
            {
                var button = new Button
                    {
                        Size = new Size(120, 90),
                        UseVisualStyleBackColor = true,
                        Text = source.Value,
                        Visible = true,
                    };
                
                this.Network.Add(button);
                this.Controls.Add(button);
            }

            var midX = this.Width / 2;
            var midY = this.Height / 2;
            var count = this.Network.Count;

            var i = 0;
            foreach (var button in this.Network)
            {
                var index = 360 * i * Math.PI / 180 / count;
                button.Location = new Point
                    {
                        X = (int)(midX + ((midX * Math.Cos(index)) / 2)),
                        Y = (int)(midY + ((midY * Math.Sin(index)) / 2))
                    };
                i++;
            }
        }

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        public SyncWizardContext DataContext { get; set; }

        /// <summary>
        /// Gets or sets the list of network.
        /// </summary>
        private List<Button> Network { get; set; }
    }
}
