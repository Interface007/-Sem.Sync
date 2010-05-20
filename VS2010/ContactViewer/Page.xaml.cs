// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Page.xaml.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The page.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ContactViewer
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// The page.
    /// </summary>
    public partial class Page
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Page"/> class.
        /// </summary>
        public Page()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The button_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((ViewModel)this.DataContext).SearchForContact();
        }

        /// <summary>
        /// The list selection changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((ViewModel)this.DataContext).CurrentContact = (ViewContact)((ListBox)sender).SelectedItem;
        }

        #endregion
    }
}