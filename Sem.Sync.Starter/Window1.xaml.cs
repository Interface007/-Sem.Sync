namespace Sem.Sync.Starter
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    
    using Sem.Sync.SharedUI.Common;

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        /// <summary>
        /// The height of the buttons for the connectors
        /// </summary>
        private const int ButtonWidthConnectors = 90;

        /// <summary>
        /// The height of the buttons for the connectors
        /// </summary>
        private const int ButtonHeightConnectors = 60;

        public Window1()
        {
            InitializeComponent();

            this.Network = new List<Button>();
            this.DataContext = new SyncWizardContext();

            foreach (var source in ((SyncWizardContext)this.DataContext).ClientsSource)
            {
                var button = new Button
                {
                    Width = ButtonWidthConnectors,
                    Height = ButtonHeightConnectors,
                    Content = source.Value,
                };

                button.Click += ButtonClickHandler;

                this.Network.Add(button);
                LayoutRoot.Children.Add(button);
            }

            this.ArrangeElements();
        }

        /// <summary>
        /// Gets or sets the list of network.
        /// </summary>
        private List<Button> Network { get; set; }

        /// <summary>
        /// Handels the click event for the buttons
        /// </summary>
        /// <param name="sender"> The sender object. </param>
        /// <param name="e"> The event arguments. </param>
        private static void ButtonClickHandler(object sender, EventArgs e)
        {
          var clicked = (Button) sender;
          MessageBox.Show("Button's name is: " + clicked.Name);
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
                button.Margin = new Thickness
                {
                    Left = (int)((midX * 1.2) + (midX * Math.Cos(index))),
                    Top = (int)((midY * 1.2) + (midY * Math.Sin(index))),
                };
                i++;
            }

            ////btnLocalStore.Location = new Point((this.Width - btnLocalStore.Width) / 2, (this.Height - btnLocalStore.Height) / 2);
        }
    }
}
