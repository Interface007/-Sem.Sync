namespace ContactViewer
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Windows.Media.Imaging;

    using ContactService;

    public class ViewModel : INotifyPropertyChanged
    {
        public ViewContact CurrentContact { get; set; }

        public List<ViewContact> ResultList { get; set; }

        public void SearchForContact()
        {
            var service = new ContactViewServiceClient();
            service.GetAllCompleted += ServiceGetAllCompleted;
            service.GetAllAsync("");
        }

        private void ServiceGetAllCompleted(object sender, GetAllCompletedEventArgs e)
        {
            this.ResultList =
                (from x in e.Result
                 select new ViewContact
                            {
                                FullName = x.FullName,
                                Street = x.Street,
                                City = x.City,
                                Picture = GetBitmapFromBytes(x.Picture),
                            }).ToList();

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("ResultList"));
        }

        private static BitmapImage GetBitmapFromBytes(byte[] bytes)
        {
            BitmapImage image = null;
            if (bytes != null && bytes.Length > 10)
            {
                var pictureStream = new MemoryStream(bytes) {Position = 0};
                image = new BitmapImage();
                image.SetSource(pictureStream);
            }
            return image;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    public class ViewContact
    {
        public string FullName { get; set; }
        public BitmapImage Picture { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
    }
}