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
    /// <summary>
    /// Interface that must be implemented for classes that should participate in the message aggregator 
    /// handling as a subscriber.
    /// </summary>
    /// <typeparam name="T">The type of message this class wants to subscribe to.</typeparam>
    public interface IHandleThis<in T> 
    {
        /// <summary>
        /// Method that handels the arrival of a message of a certain type.
        /// </summary>
        /// <param name="message">The message to handle</param>
        void Handle(T message);
    }
}