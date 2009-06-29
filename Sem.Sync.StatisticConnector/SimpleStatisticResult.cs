namespace Sem.Sync.StatisticConnector
{
    using System.Collections.Generic;

    using SyncBase.DetailData;

    public class SimpleStatisticResult
    {
        public int NumberOfElements { get; set; }
        public List<object> PropertyUsage { get; set; }
    }
}
