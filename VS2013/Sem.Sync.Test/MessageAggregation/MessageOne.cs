﻿namespace Sem.Sync.Test.Contracts
{
    public class MessageOne
    {
        public MessageOne(string content)
        {
            this.Content = content;
        }

        public string Content { get; set; }

        public override string ToString()
        {
            return this.Content;
        }
    }
}