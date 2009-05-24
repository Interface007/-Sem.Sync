using System;
using System.Globalization;
using Sem.Sync.SyncBase.EventArgs;

namespace Sem.Sync.SyncBase
{
    public class SyncComponent
    {
        public event EventHandler<ProcessingEventArgs> ProcessingEvent;
        public event EventHandler<ProgressEventArgs> ProgressEvent;

        /// <summary>
        /// log the events 
        /// </summary>
        /// <param name="contactStdItem">the std-element this event corresponds to</param>
        /// <param name="message">the message for this even (should describe what's happening in this step of execution)</param>
        protected virtual void LogProcessingEvent(StdElement contactStdItem, string message)
        {
            if (ProcessingEvent != null)
            {
                ProcessingEvent(this, new ProcessingEventArgs { Item = contactStdItem, Message = message });
            }
        }
        
        /// <summary>
        /// log the events 
        /// </summary>
        /// <param name="message">the message for this even (should describe what's happening in this step of execution)</param>
        protected virtual void LogProcessingEvent(string message)
        {
            LogProcessingEvent(null, message);
        }

        /// <summary>
        /// log the events 
        /// </summary>
        /// <param name="message">the message for this even (should describe what's happening in this step of execution)</param>
        /// <param name="parameters">parameters that will be inserted into the log-message</param>
        protected virtual void LogProcessingEvent(string message, params object[] parameters)
        {
            LogProcessingEvent(null, string.Format(CultureInfo.CurrentCulture, message, parameters));
        }

        /// <summary>
        /// log the events 
        /// </summary>
        /// <param name="contactStdItem">element, this entry belongs to</param>
        /// <param name="message">the message for this even (should describe what's happening in this step of execution)</param>
        /// <param name="parameters">parameters that will be inserted into the log-message</param>
        protected void LogProcessingEvent(StdElement contactStdItem, string message, params object[] parameters)
        {
            LogProcessingEvent(contactStdItem, string.Format(CultureInfo.CurrentCulture, message, parameters));
        }

        /// <summary>
        /// log the events 
        /// </summary>
        protected void LogProcessingEvent(object sender, ProcessingEventArgs args)
        {
            LogProcessingEvent((StdElement)args.Item, args.Message);
        }

        protected void UpdateProgress(int percentage)
        {
            UpdateProgress(this, new ProgressEventArgs{PercentageDone = percentage});
        }

        private void UpdateProgress(object sender, ProgressEventArgs args)
        {
            if (ProgressEvent != null)
            {
                ProgressEvent(this, args);
            }
        }
    }
}
