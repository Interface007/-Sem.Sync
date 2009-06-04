// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewModel.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Implements data processing on the client side like retrieving the contact data and serves as a binding source
//   for the xaml.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
        private ViewContact _CurrentContact;

        public ViewContact CurrentContact
        {
            get
            {
                return this._CurrentContact;
            }
            set
            {
                this._CurrentContact = value;
                this.RaisePropertyChanged("CurrentContact");
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

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

            this.RaisePropertyChanged("ResultList");
        }

        private static BitmapImage GetBitmapFromBytes(byte[] bytes)
        {
            var image = new BitmapImage();
            
            if (bytes != null && bytes.Length > 10)
            {
                var pictureStream = new MemoryStream(bytes) {Position = 0};
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