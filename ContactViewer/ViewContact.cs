namespace ContactViewer
{
    using System.Windows.Media.Imaging;

    public class ViewContact
    {
        public string FullName { get; set; }
        public BitmapImage Picture { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
    }
}