// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncServiceHostFactory.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Enables running at a hosting provider that does host multiple roots in the same iis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.OnlineStorage
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Activation;

    /// <summary>
    /// Enables running at a hosting provider that does host multiple roots in the same iis.
    /// </summary>
    public class SyncServiceHostFactory : ServiceHostFactory
    {
        #region Methods

        /// <summary>
        /// Selects the correct service host instance.
        /// </summary>
        /// <param name="serviceType">
        /// The service type. 
        /// </param>
        /// <param name="baseAddresses">
        /// The base addresses. 
        /// </param>
        /// <returns>
        /// The selected service host. 
        /// </returns>
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return baseAddresses.Length > 1
                       ? new ServiceHost(serviceType, baseAddresses[1])
                       : new ServiceHost(serviceType, baseAddresses[0]);
        }

        #endregion
    }
}