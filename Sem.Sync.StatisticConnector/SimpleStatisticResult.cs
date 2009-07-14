namespace Sem.Sync.StatisticConnector
{
    using System.Collections.Generic;

    using GenericHelpers.Entities;

    public class SimpleStatisticResult
    {
        public int NumberOfElements { get; set; }
        public List<KeyValuePair> PropertyUsage { get; set; }
        public ValueAnalysisCounter ValueAnalysis { get; set; }
    }
}
