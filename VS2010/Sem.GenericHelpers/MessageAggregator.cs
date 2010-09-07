using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sem.GenericHelpers
{
    public class MessageAggregator
    {
        public static MessageAggregator Aggregator { get; private set; }

        static MessageAggregator()
        {
            Aggregator = new MessageAggregator();
        }

        private MessageAggregator()
        {

        }
    }
}
