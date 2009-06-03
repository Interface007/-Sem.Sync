namespace ContactViewer
{
    using System.Windows;

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
    }
}
