// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUiInteraction.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the IUiInteraction type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Interfaces
{
    public interface IUiInteraction
    {
        bool AskForLogOnCredentials(IClientBase client, string messageForUser, string logOnUserId, string logOnPassword);
        bool AskForConfirm(string messageForUser, string title);
    }
}
