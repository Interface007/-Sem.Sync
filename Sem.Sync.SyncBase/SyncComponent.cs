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

    using EventArgs;

    public class SyncComponent
    {
        public event EventHandler<ProcessingEventArgs> ProcessingEvent;
        public event EventHandler<ProgressEventArgs> ProgressEvent;

        /// <summary>
        /// log the events 
        /// </summary>
        /// <param name="contactStdItem">the std-element this event corresponds to</param>
        /// <param name="message">the message for this even (should describe what's happening in this step of execution)</param>
        protected void LogProcessingEvent(StdElement contactStdItem, string message)
        {
            if (this.ProcessingEvent != null)
            {
                this.ProcessingEvent(this, new ProcessingEventArgs { Item = contactStdItem, Message = message });
            }
        }
        
        /// <summary>
        /// log the events 
        /// </summary>
        /// <param name="message">the message for this even (should describe what's happening in this step of execution)</param>
        protected void LogProcessingEvent(string message)
        {
            this.LogProcessingEvent(null, message);
        }

        /// <summary>
        /// log the events 
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
        /// log the events 
        /// </summary>
        protected void LogProcessingEvent(object sender, ProcessingEventArgs args)
        {
            this.LogProcessingEvent((StdElement)args.Item, args.Message);
        }

        protected void UpdateProgress(int percentage)
        {
            UpdateProgress(this, new ProgressEventArgs{PercentageDone = percentage});
        }

        private void UpdateProgress(object sender, ProgressEventArgs args)
        {
            if (this.ProgressEvent != null)
            {
                this.ProgressEvent(sender, args);
            }
        }
    }
}
