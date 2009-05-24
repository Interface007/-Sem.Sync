namespace Sem.Sync.SyncBase
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    public class SyncDescription
    {
        [XmlAttribute]
        public string Name { get; set; }
        public SyncCommand Command { get; set; }
        public string CommandParameter { get; set; }
        public string SourceConnector { get; set; }
        public string SourceStorePath { get; set; }
        public string TargetConnector { get; set; }
        public string TargetStorePath { get; set; }
        public string BaselineConnector { get; set; }
        public string BaselineStorePath { get; set; }
    }

    public enum SyncCommand
    {
        CopyNew,
        CopyAll,
        MergeMissing,
        MergeHighEvidence,
        RemoveDuplicatesOnTarget,
        MergeExternal,
        MatchByName,
        NormalizeContent,
        DeletePattern,
        DetectConflicts,
        AskForContinue,
    }
}
