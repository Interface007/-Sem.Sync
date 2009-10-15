//-----------------------------------------------------------------------
// <copyright file="SyncComponent.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase
{
    using System;
    using System.Globalization;

    using GenericHelpers.EventArgs;
    using GenericHelpers.Exceptions;

    /// <summary>
    /// Base class for Sync classes that do provide UI feedback like logging and progress.
    /// </summary>
    public class SyncComponent
    {
        /// <summary>
        /// Event that informs the subscriber about a processing event (e.g. change of current element)
        /// </summary>
        public event EventHandler<ProcessingEventArgs> ProcessingEvent;

        /// <summary>
        /// Event that informs the subscriber about the current work progress relative to the complete 
        /// current method call inside this object. The percentage of work does not match to the time 
        /// for this method execution: 50% after one minute done does not mean that there is still work
        /// for 1 minute left.
        /// </summary>
        public event EventHandler<ProgressEventArgs> ProgressEvent;

        /// <summary>
        /// logs an event by specifying the current element that is related to the event and a message about the current event
        /// </summary>
        /// <param name="stdItem">the std-element this event corresponds to</param>
        /// <param name="message">the message for this even (should describe what's happening in this step of execution)</param>
        protected void LogProcessingEvent(StdElement stdItem, string message)
        {
            if (this.ProcessingEvent != null)
            {
                this.ProcessingEvent((object)stdItem ?? (object)this, new ProcessingEventArgs { Item = stdItem, Message = message });
            }
        }

        /// <summary>
        /// log an event by only specifying a message - in some situations a current element is not in context.
        /// </summary>
        /// <param name="message">the message for this even (should describe what's happening in this step of execution)</param>
        protected void LogProcessingEvent(string message)
        {
            this.LogProcessingEvent(null, message);
        }

        /// <summary>
        /// log an event by only specifying a message and string.format parameters - in some situations a current element is not in context.
        /// use this overload to prevent preformatting the parameters for the message.
        /// </summary>
        /// <param name="message">the message for this even (should describe what's happening in this step of execution)</param>
        /// <param name="parameters">parameters that will be inserted into the log-message</param>
        protected void LogProcessingEvent(string message, params object[] parameters)
        {
            this.LogProcessingEvent(null, string.Format(CultureInfo.CurrentCulture, message, parameters));
        }

        /// <summary>
        /// log the events 
        /// </summary>
        /// <param name="contactStdItem">element, this entry belongs to</param>
        /// <param name="message">the message for this even (should describe what's happening in this step of execution)</param>
        /// <param name="parameters">parameters that will be inserted into the log-message</param>
        protected void LogProcessingEvent(StdElement contactStdItem, string message, params object[] parameters)
        {
            this.LogProcessingEvent(contactStdItem, string.Format(CultureInfo.CurrentCulture, message, parameters));
        }

        /// <summary>
        /// logs an event by specifying the current element that is related to the event and a message about the current event
        /// use this overload to prevent preformatting the parameters for the message.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="args">The processing arguments.</param>
        protected void LogProcessingEvent(object sender, ProcessingEventArgs args)
        {
            this.LogProcessingEvent((StdElement)args.Item, args.Message);
        }

        /// <summary>
        /// log an exception.
        /// use this overload if you don't have any context information
        /// </summary>
        /// <param name="exception"> The exception. </param>
        protected void LogException(Exception exception)
        {
            this.LogException(exception, "Error while executing client: {0}");
        }

        /// <summary>
        /// log an exception.
        /// use this overload to use a defined message with parameters
        /// </summary>
        /// <param name="exception"> The exception.  </param>
        /// <param name="message"> The message. </param>
        /// <param name="parameterStrings"> The parameter Strings. </param>
        protected void LogException(Exception exception, string message, params string[] parameterStrings)
        {
            // throw an exception if this one is a ProcessAbortException
            ThrowOnException(exception as ProcessAbortException);

            // combine the specified parameters with the message of the exception to 
            // build a param array for the format function
            var parameters = new string[parameterStrings.Length + 1];
            parameters[0] = message;
            for (var i = 0; i < parameterStrings.Length - 1; i++)
            {
                parameters[i + 1] = parameterStrings[i];
            }
            
            this.LogProcessingEvent(message, parameters);
        }

        /// <summary>
        /// Informs the subscriber of the <see cref="ProgressEvent"/> about the percentage of the current 
        /// execution. This percentage is relative to the complete method call.
        /// </summary>
        /// <param name="percentage">Specifies the percentage of work already done.</param>
        protected void UpdateProgress(int percentage)
        {
            this.UpdateProgress(this, new ProgressEventArgs { PercentageDone = percentage });
        }

        /// <summary>
        /// Throws an <see cref="Exception"/> if one is specified
        /// </summary>
        /// <param name="exception"> The process abort exception to be thrown. </param>
        /// <exception cref="ProcessAbortException"> if the parameter is not null </exception>
        private static void ThrowOnException(Exception exception)
        {
            if (exception != null)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Informs the subscriber of the <see cref="ProgressEvent"/> about the percentage of the current 
        /// execution. This percentage is relative to the complete method call.
        /// </summary>
        /// <param name="sender">The reference to the sender of this event (this object)</param>
        /// <param name="args">The progress arguments about the current work</param>
        private void UpdateProgress(object sender, ProgressEventArgs args)
        {
            if (this.ProgressEvent != null)
            {
                this.ProgressEvent(sender, args);
            }
        }
    }
}
