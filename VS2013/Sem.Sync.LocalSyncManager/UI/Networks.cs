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
        /// The height of the buttons for the connectors
        /// </summary>
        private const int ButtonWidthConnectors = 90;

        /// <summary>
        /// The height of the buttons for the connectors
        /// </summary>
        private const int ButtonHeightConnectors = 60;

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
                        Size = new Size(ButtonWidthConnectors, ButtonHeightConnectors),
                        UseVisualStyleBackColor = true,
                        Text = source.Value,
                        Visible = true,
                    };
                
                this.Network.Add(button);
                this.Controls.Add(button);
            }

            this.ArrangeElements();
        }

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        public SyncWizardContext DataContext { get; set; }

        /// <summary>
        /// Gets or sets the list of network.
        /// </summary>
        private List<Button> Network { get; set; }

        /// <summary>
        /// Handels the ResizeEnd event.
        /// </summary>
        /// <param name="e"> Empty event argument. </param>
        protected override void OnResizeEnd(EventArgs e)
        {
            this.ArrangeElements();
            base.OnResizeEnd(e);
        }

        /// <summary>
        /// Arranges element around the local store
        /// </summary>
        private void ArrangeElements()
        {
            // ReSharper disable PossibleLossOfFraction
            var midX = (int)(((this.Width - ButtonWidthConnectors) / 2) * 0.8);
            var midY = (int)(((this.Height - ButtonHeightConnectors) / 2) * 0.8);
            var count = this.Network.Count;

            // ReSharper restore PossibleLossOfFraction
            var i = 0;
            foreach (var button in this.Network)
            {
                var index = 360 * i * Math.PI / 180 / count;
                button.Location = new Point
                    {
                        X = (int)((midX * 1.2) + (midX * Math.Cos(index))),
                        Y = (int)((midY * 1.2) + (midY * Math.Sin(index)))
                    };
                i++;
            }

            btnLocalStore.Location = new Point((this.Width - btnLocalStore.Width) / 2, (this.Height - btnLocalStore.Height) / 2);
        }
    }
}
