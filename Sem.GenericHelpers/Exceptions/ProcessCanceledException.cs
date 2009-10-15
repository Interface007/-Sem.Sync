// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessCanceledException.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the ProcessCanceledException type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Sem.GenericHelpers.Exceptions
{
    using Sync.LocalSyncManager.Business;

    /// <summary>
    /// signals the cancelation of the process through all layers and components
    /// </summary>
    public class ProcessAbortException : TechnicalException
    {
    }
}