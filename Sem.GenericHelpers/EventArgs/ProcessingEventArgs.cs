//-----------------------------------------------------------------------
// <copyright file="ProcessingEventArgs.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.GenericHelpers.EventArgs
{
    /// <summary>
    /// Event argument while processing some work
    /// </summary>
    public class ProcessingEventArgs : System.EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessingEventArgs"/> class.
        /// </summary>
        public ProcessingEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessingEventArgs"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public ProcessingEventArgs(string message)
        {
            this.Message = message;
        }
        
        /// <summary>
        /// Gets or sets the item that is related to the current work.
        /// </summary>
        public object Item { get; set; }

        /// <summary>
        /// Gets or sets the message describing the event.
        /// </summary>
        public string Message { get; set; }
    }
}