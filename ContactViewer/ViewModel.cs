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
        private ViewContact currentContact;

        public ViewContact CurrentContact
        {
            get
            {
                return this.currentContact;
            }

            set
            {
                this.currentContact = value;
                this.RaisePropertyChanged("CurrentContact");
            }
        }

        public List<ViewContact> ResultList { get; set; }

        public void SearchForContact()
        {
            var service = new ContactViewServiceClient();
            service.GetAllCompleted += this.ServiceGetAllCompleted;
            service.GetAllAsync(string.Empty);
        }

        private static BitmapImage GetBitmapFromBytes(byte[] bytes)
        {
            var image = new BitmapImage();
            
            if (bytes != null && bytes.Length > 10)
            {
                using (var pictureStream = new MemoryStream(bytes) { Position = 0 })
                {
                    image.SetSource(pictureStream);
                }
            }
         
            return image;
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

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}