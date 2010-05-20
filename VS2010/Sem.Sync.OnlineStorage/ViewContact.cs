// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewContact.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   view entity class for a contact
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.OnlineStorage
{
    using System.Runtime.Serialization;

    /// <summary>
    /// view entity class for a contact
    /// </summary>
    [DataContract(Name = "ViewContact")]
    public class ViewContact
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the city name of the address.
        /// </summary>
        [DataMember(Name = "City")]
        public string City { get; set; }

        /// <summary>
        ///   Gets or sets the full nay for a diaplay.
        /// </summary>
        [DataMember(Name = "FullName")]
        public string FullName { get; set; }

        /// <summary>
        ///   Gets or sets the image data.
        /// </summary>
        [DataMember(Name = "Picture")]
        public byte[] Picture { get; set; }

        /// <summary>
        ///   Gets or sets the street of the address.
        /// </summary>
        [DataMember(Name = "Street")]
        public string Street { get; set; }

        #endregion
    }
}