//-----------------------------------------------------------------------
// <copyright file="InstantMessengerAddresses.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.DetailData
{
    using System;

    /// <summary>
    /// Implements a generic representation of the addressing information for instant
    /// messaging clients for a specific user.
    /// </summary>
    [Serializable]
    public class InstantMessengerAddresses
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstantMessengerAddresses"/> class 
        /// that is "empty" (does not contain addressing information).
        /// </summary>
        public InstantMessengerAddresses()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstantMessengerAddresses"/> class that 
        /// contains already addressing information.
        /// </summary>
        /// <param name="imAddresses">A string representation of the address. E.g. MSN-Messenger addresses 
        /// start with the sequence "msn:".</param>
        public InstantMessengerAddresses(string imAddresses)
        {
            if (!string.IsNullOrWhiteSpace(imAddresses) && imAddresses.StartsWith("msn:", StringComparison.OrdinalIgnoreCase))
            {
                this.MsnMessenger = imAddresses;
            }
        }

        /// <summary>
        /// Gets or sets the MSN-Messenger address.
        /// </summary>
        public string MsnMessenger { get; set; }

        /// <summary>
        /// Gets or sets the Google-Talk address.
        /// </summary>
        public string GoogleTalk { get; set; }

        /// <summary>
        /// Gets or sets the Yahoo-Messenger address.
        /// </summary>
        public string YahooMessenger { get; set; }

        /// <summary>
        /// Gets or sets the Skype name.
        /// </summary>
        public string Skype { get; set; }

        /// <summary>
        /// Gets or sets the ICQ name.
        /// </summary>
        public string Icq { get; set; }
    }
}