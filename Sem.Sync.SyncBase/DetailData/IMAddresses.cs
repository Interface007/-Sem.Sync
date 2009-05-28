//-----------------------------------------------------------------------
// <copyright file="InstantMessengerAddresses.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.DetailData
{
    using System;

    public class InstantMessengerAddresses
    {
        public InstantMessengerAddresses()
        {
        }

        public InstantMessengerAddresses(string imAddresses)
        {
            if ((imAddresses != null) && imAddresses.StartsWith("msn:", StringComparison.OrdinalIgnoreCase))
            {
                this.MsnMessenger = imAddresses;
            }
        }

        public string MsnMessenger { get; set; }

        public string GoogleTalk { get; set; }

        public string YahooMessenger { get; set; }

        public string Skype { get; set; }
    }
}