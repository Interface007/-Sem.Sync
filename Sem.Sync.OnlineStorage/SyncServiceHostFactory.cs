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
