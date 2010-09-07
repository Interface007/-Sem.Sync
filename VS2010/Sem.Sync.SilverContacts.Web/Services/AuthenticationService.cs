// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthenticationService.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   RIA Services DomainService responsible for authenticating users when
//   they try to log on to the application.
//   Most of the functionality is already provided by the base class
//   AuthenticationBase
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SilverContacts.Web
{
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server.ApplicationServices;

    /// <summary>
    /// RIA Services DomainService responsible for authenticating users when
    /// they try to log on to the application.
    /// Most of the functionality is already provided by the base class
    /// AuthenticationBase
    /// </summary>
    [EnableClientAccess]
    public class AuthenticationService : AuthenticationBase<User>
    {
    }
}
