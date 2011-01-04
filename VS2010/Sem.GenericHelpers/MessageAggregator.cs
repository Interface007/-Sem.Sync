// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageAggregator.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the MessageAggregator type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers
{
    /// <summary>
    /// Message agregator for distributing and subscribing to messages.
    /// </summary>
    public class MessageAggregator
    {
        /// <summary>
        /// Initializes static members of the <see cref="MessageAggregator"/> class.
        /// </summary>
        static MessageAggregator()
        {
            Aggregator = new MessageAggregator();
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="MessageAggregator"/> class from being created.
        /// </summary>
        private MessageAggregator()
        {
        }

        /// <summary>
        /// Gets the Aggregator singleton instance.
        /// </summary>
        public static MessageAggregator Aggregator { get; private set; }
    }
}
