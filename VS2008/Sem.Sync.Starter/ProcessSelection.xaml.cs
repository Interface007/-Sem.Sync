// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessSelection.xaml.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Interaction logic for Window1.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Sem.GenericHelpers.Exceptions;
using Sem.Sync.SyncBase;

namespace Sem.Sync.Starter
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    
    using SharedUI.Common;

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ProcessSelection
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
        /// Initializes a new instance of the <see cref="ProcessSelection"/> class.
        /// </summary>
        public ProcessSelection()
        {
            InitializeComponent();
            this.DataContext = new SyncWizardContext(typeof(StdContact), ExceptionHandler.UserInterface);

            this.Network = new List<Button>();
            foreach (var source in ((SyncWizardContext)this.DataContext).SyncWorkflowData)
            {
                var button = new Button
                {
                    Width = ButtonWidthConnectors,
                    Height = ButtonHeightConnectors,
                    Content = source.Value,
                    Tag = source,
                };

                button.Click += this.ButtonClickHandler;

                this.Network.Add(button);
                LayoutRoot.Children.Add(button);
            }
        }

        /// <summary>
        /// Gets or sets the list of network.
        /// </summary>
        private List<Button> Network { get; set; }

        /// <summary>
        /// arranges the buttons on resizing the window
        /// </summary>
        /// <param name="sizeInfo"> The size info. </param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            this.ArrangeElements();
            base.OnRenderSizeChanged(sizeInfo);
        }

        /// <summary>
        /// Handels the click event for the buttons
        /// </summary>
        /// <param name="sender"> The sender object. </param>
        /// <param name="e"> The event arguments. </param>
        private void ButtonClickHandler(object sender, EventArgs e)
        {
          var clicked = (Button) sender;
          var process = (KeyValuePair<string, string>) clicked.Tag;
          if (MessageBox.Show("Starting process: " + process.Value, "Sync-Process", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
          {
              ((SyncWizardContext)this.DataContext).Run(process.Key);
          }
        }

        /// <summary>
        /// Arranges element around the local store
        /// </summary>
        private void ArrangeElements()
        {
            // ReSharper disable PossibleLossOfFraction
            var midX = (int)((this.LayoutRoot.ActualWidth - ButtonWidthConnectors) / 1.3);
            var midY = (int)((this.LayoutRoot.ActualHeight - ButtonHeightConnectors) / 1.3);

            var count = this.Network.Count;

            // ReSharper restore PossibleLossOfFraction
            var i = 0;
            foreach (var button in this.Network)
            {
                var index = 360 * i * Math.PI / 180 / count;
                button.Margin = new Thickness
                {
                    Left = (int)(midX * Math.Cos(index)),
                    Top = (int)(midY * Math.Sin(index)),
                };
                i++;
            }
        }
    }
}