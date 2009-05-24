namespace Sem.Sync.OnlineStorage
{
    using System.ServiceModel;

    public class SyncServiceAuthorizationManager : ServiceAuthorizationManager 
    {
        /// <summary>
        /// This method implements the access security of the service
        /// </summary>
        /// <param name="operationContext">the context information about the current request</param>
        /// <returns></returns>
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            // todo: we need to implement some kind of security here
            return true;
        }
    }
}
