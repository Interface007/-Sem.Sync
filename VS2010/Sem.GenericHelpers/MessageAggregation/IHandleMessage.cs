// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHandleMessage.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the IHandleThis type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.MessageAggregation
{
    public interface IHandleThis<in T> 
    {
        void Handle(T message);
    }
}