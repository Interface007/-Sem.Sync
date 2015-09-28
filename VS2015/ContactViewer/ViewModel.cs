// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewModel.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ContactViewer
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Windows.Media.Imaging;

    using ContactViewer.ContactService;

    /// <summary>
    /// The view model.
    /// </summary>
    public class ViewModel : INotifyPropertyChanged
    {
        #region Constants and Fields

        /// <summary>
        /// The current contact.
        /// </summary>
        private ViewContact currentContact;

        #endregion

        #region Events

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets CurrentContact.
        /// </summary>
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

        /// <summary>
        /// Gets or sets ResultList.
        /// </summary>
        public List<ViewContact> ResultList { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The search for contact.
        /// </summary>
        public void SearchForContact()
        {
            var service = new ContactViewServiceClient();
            service.GetAllCompleted += this.ServiceGetAllCompleted;
            service.GetAllAsync(string.Empty);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get bitmap from bytes.
        /// </summary>
        /// <param name="bytes">
        /// The bytes.
        /// </param>
        /// <returns>
        /// </returns>
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

        /// <summary>
        /// The raise property changed.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// The service get all completed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ServiceGetAllCompleted(object sender, GetAllCompletedEventArgs e)
        {
            this.ResultList = (from x in e.Result
                               select
                                   new ViewContact
                                       {
                                           FullName = x.FullName, 
                                           Street = x.Street, 
                                           City = x.City, 
                                           Picture = GetBitmapFromBytes(x.Picture), 
                                       }).ToList();

            this.RaisePropertyChanged("ResultList");
        }

        #endregion
    }
}