// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Home.xaml.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Home page for the application.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SilverContacts
{
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// Home page for the application.
    /// </summary>
    public partial class ContactsView : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Home"/> class. 
        /// </summary>
        public ContactsView()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.HomePageTitle;
        }

        /// <summary>
        /// Executes when the user navigates to this page.
        /// </summary>
        /// <param name="e"> The navigation event argumenmts. </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}