﻿using System;
namespace Sem.Sync.SyncBase.DetailData
{
    public class ContactReference
    {
        public Guid Source { get; set; }
        public Guid Target { get; set; }

        public bool IsBusinessContact { get; set; }
        public bool IsPrivateContact { get; set; }
    }
}
