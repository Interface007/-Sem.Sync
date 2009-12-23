// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncServiceHostFactory.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the SyncServiceHostFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.OnlineStorage
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Activation;

    public class SyncServiceHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return
                baseAddresses.Length > 1 ?
                new ServiceHost(serviceType, baseAddresses[1]) :
                new ServiceHost(serviceType, baseAddresses[0]);
        }
    }
}
