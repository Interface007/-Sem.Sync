namespace Sem.Sync.Cloud
{
    using System.Collections.Generic;

    public class ResultBase
    {
        /// <summary>
        /// Gets or sets a list of messages to be exchanged in conjunction with the contact item list.
        /// </summary>
        public List<TechnicalMessage> Messages { get; set; }
    }
}
