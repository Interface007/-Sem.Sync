// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncServiceAuthorizationManager.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the SyncServiceAuthorizationManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.OnlineStorage
{
    using System.ServiceModel;

    /// <summary>
    /// Implements an authorization manager that checks the autorization to use a specific resource/functionality
    /// </summary>
    public class SyncServiceAuthorizationManager : ServiceAuthorizationManager 
    {
        /// <summary>
        /// This method implements the access security of the service
        /// </summary>
        /// <param name="operationContext">the context information about the current request</param>
        /// <returns>true if access is granted</returns>
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            // todo: we need to implement some kind of security here
            return true;
        }
    }
}
