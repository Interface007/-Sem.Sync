// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageAggregator.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The MessageAggregator class contains a singleton (<see cref="instance" />) to subsribe to or publish messages.
//   Subscribers need to implement the interface <see cref="IHandleThis{T}" />, where T is the message to subscribe to.
//   Multiple subscriptions of a single object instance to the same message will be suppressed.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.MessageAggregation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    /// The MessageAggregator class contains a singleton (<see cref="instance"/>) to subsribe to or publish messages.
    /// Subscribers need to implement the interface <see cref="IHandleThis{T}"/>, where T is the message to subscribe to.
    /// Multiple subscriptions of a single object instance to the same message will be suppressed.
    /// </summary>
    public class MessageAggregator
    {
        /// <summary>
        /// Singleton instance of <see cref="MessageAggregator"/>.
        /// </summary>
        private static volatile MessageAggregator instance = new MessageAggregator();

        /// <summary>
        /// Threading object instance for lock statements.
        /// </summary>
        private static readonly object _SyncRoot = new Object();

        /// <summary>
        /// Internal list of subscibers as weak references, so they will be 
        /// garbage-collected if this is the only reference.
        /// </summary>
        public List<WeakReference> Subscriptions { get; set; }

        /// <summary>
        /// Gets the singleton instance of the <see cref="MessageAggregator"/>.
        /// </summary>
        public static MessageAggregator Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the <see cref="MessageAggregator"/>.
        /// </summary>
        static MessageAggregator()
        {
            instance = new MessageAggregator();
        }

        /// <summary>
        /// Initializes the new instance of the <see cref="MessageAggregator"/> with a 
        /// new list of weak references.
        /// </summary>
        private MessageAggregator()
        {
            this.Subscriptions = new List<WeakReference>();
        }

        /// <summary>
        /// Subscribes an object to all messages for which <see cref="IHandleThis{T}"/> is implemented. You cannot 
        /// subscibe for a specific message, the object will be added to the list of subscibers and will receive all
        /// messages for which the interface <see cref="IHandleThis{T}"/> is implemented.
        /// </summary>
        /// <param name="subscriber">The object that implements <see cref="IHandleThis{T}"/> and wants to subscribe 
        /// to messages of type T.</param>
        public void Subscribe(object subscriber)
        {
            Bouncer.ForCheckData(subscriber, "subscriber")
                .Assert(new IsNotNullRule<object>())
                .Assert(x => x.ToString() != "hallo")
                .Assert(new ImplementsInterfaceRule<object>(), typeof(IHandleThis<>));
            
            if (this.GetWeakReference(subscriber) != null)
            {
                return;
            }

            lock (_SyncRoot)
            {
                if (this.GetWeakReference(subscriber) == null)
                {
                    this.Subscriptions.Add(new WeakReference(subscriber));
                }
            }
        }

        /// <summary>
        /// Unsubscibes an object - you cannot unsubscibe for a specific message, the object will be removed 
        /// completely from the list of subscibers.
        /// </summary>
        /// <param name="subscriber"></param>
        public void UnSubscribe(object subscriber)
        {
            if (this.GetWeakReference(subscriber) == null)
            {
                return;
            }

            lock (_SyncRoot)
            {
                if (this.GetWeakReference(subscriber) != null)
                {
                    this.Subscriptions.Remove(GetWeakReference(subscriber));
                }
            }
        }

        /// <summary>
        /// Publishes a message to all subscribers that do implement the <see cref="IHandleThis{T}"/> interface
        /// for type T is equal to or inherits from <typeparamref name="TMessage"/>. The method
        /// <see cref="IHandleThis{T}.Handle"/> will be called for each subscriber.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to be published.</typeparam>
        /// <param name="message">The message object to be published.</param>
        /// <returns>The message object.</returns>
        public TMessage Publish<TMessage>(TMessage message)
        {
            foreach (var x in this.GetSubscriber<TMessage>())
            {
                x.Handle(message);
            }

            return message;
        }

        /// <summary>
        /// Clears the subsciber list.
        /// </summary>
        public void Reset()
        {
            lock (_SyncRoot)
            {
                this.Subscriptions.Clear();
            } 
        }

        /// <summary>
        /// Gets the <see cref="WeakReference"/> object for a given subsciber object.
        /// </summary>
        /// <param name="subscriber">the subsciber object to search for</param>
        /// <returns>The waek reference object containing the subsciver as a <see cref="WeakReference.Target"/> - 
        /// or null if the object is not found.</returns>
        private WeakReference GetWeakReference(object subscriber)
        {
            var weakReferences = this.Subscriptions.Where(x => x.Target == subscriber);
            return weakReferences.FirstOrDefault();
        }

        /// <summary>
        /// Searches a list of subscibers that did subscibe to the message <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The message type to search for.</typeparam>
        /// <returns>The list of subscribers for this message type.</returns>
        private IEnumerable<IHandleThis<T>> GetSubscriber<T>()
        {
            return from x in this.Subscriptions where x.Target is IHandleThis<T> select (IHandleThis<T>)x.Target;
        }
    }
}
