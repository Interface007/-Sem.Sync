//-----------------------------------------------------------------------
// <copyright file="IClientBase.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.Interfaces
{
    using System;
    using System.Collections.Generic;

    using EventArgs;

    public interface IClientBase : ICredentialAware
    {
        event EventHandler<ProcessingEventArgs> ProcessingEvent;
        event EventHandler<QueryForLogOnCredentialsEventArgs> QueryForLoginCredentialsEvent;

        string FriendlyClientName { get; }
        
        void RemoveDuplicates(string clientFolderName);

        List<StdElement> GetAll(string clientFolderName);

        void AddItem(StdElement element, string clientFolderName);
        void AddRange(List<StdElement> elements, string clientFolderName);

        void MergeMissingItem(StdElement element, string clientFolderName);
        void MergeMissingRange(List<StdElement> elements, string clientFolderName);

        void WriteRange(List<StdElement> elements, string clientFolderName);

        List<StdElement> Normalize(List<StdElement> elements);
    }
}