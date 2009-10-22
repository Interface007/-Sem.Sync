using System;
namespace Sem.Sync.SyncBase.DetailData
{
    public class BusinessHistoryEntry
    {
        public string BusinessCompanyName { get; set; }
        public string BusinessDepartment { get; set; }
        public string BusinessPosition { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
