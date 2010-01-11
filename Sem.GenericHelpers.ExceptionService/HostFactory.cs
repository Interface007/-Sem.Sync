// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HostFactory.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The host factory will enable this WCF to run at GoDaddy (my current hoster)
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.ExceptionService
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Activation;

    /// <summary>
    /// The host factory will enable this WCF to run at GoDaddy (my current hoster)
    /// </summary>
    public class HostFactory : ServiceHostFactory
    {
        /// <summary>
        /// Selects the correct service host implementation from the provided list
        /// </summary>
        /// <param name="serviceType"> The service type. </param>
        /// <param name="baseAddresses"> The base addresses. </param>
        /// <returns> the selected service host from the list </returns>
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return 
                baseAddresses.Length > 1 
                ? new ServiceHost(serviceType, baseAddresses[1]) 
                : new ServiceHost(serviceType, baseAddresses[0]);
        }
    }
}
