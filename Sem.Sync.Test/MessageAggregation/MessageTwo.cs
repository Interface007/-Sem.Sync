﻿namespace Sem.Sync.Test.Contracts
{
    using System;

    public class MessageTwo
    {
        public MessageTwo(int content)
        {
            this.Content = content;
        }

        public int Content { get; set; }

        public MessageOne SubMessage { get; set; }
    }
}