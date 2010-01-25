// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Page.xaml.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Routes the page control events to the ViewModel ... I know that this does not 100% match 
//   to the MVVM-Pattern - but who cares ;-)
//   This impelementation does definitely have problems with huge masses of contacts bacause 
//   all contacts are transferred at once - non-iterative. This should be changed for use with 
//   many contacts or when using slow connections.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ContactViewer
{
    using System.Windows;
    using System.Windows.Controls;

    public partial class Page
    {
        public Page()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((ViewModel) this.DataContext).SearchForContact();
        }

        private void ListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((ViewModel)this.DataContext).CurrentContact = (ViewContact)((ListBox)sender).SelectedItem;
        }
    }
}
