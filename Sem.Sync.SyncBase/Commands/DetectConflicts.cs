namespace Sem.Sync.SyncBase.Commands
{
    using System;

    using Helpers;
    using Interfaces;

    public class DetectConflicts: ISyncCommand
    {
        public IUiInteraction UiProvider { get; set; }

        public bool ExecuteCommand(IClientBase sourceClient, IClientBase targetClient, IClientBase baseliClient, string sourceStorePath, string targetStorePath, string baselineStorePath, string commandParameter)
        {
            // todo: this is currently specific to StdContact-elements, so we need to generalize it.
            if (targetClient == null) throw new InvalidOperationException("item.targetClient is null");
            if (sourceClient == null) throw new InvalidOperationException("item.sourceClient is null");
            if (sourceStorePath == null) throw new InvalidOperationException("sourceStorePath is null");
            if (targetStorePath == null) throw new InvalidOperationException("targetStorePath is null");

            var targetList = targetClient.GetAll(targetStorePath);
            var mergeResultList =
                this.UiProvider.PerformAttributeMerge(
                    SyncTools.DetectConflicts(
                        SyncTools.BuildConflictTestContainerList(
                            sourceClient.GetAll(sourceStorePath),
                            targetList,
                            (baseliClient == null) ? null : baseliClient.GetAll(baselineStorePath),
                            typeof(StdContact)),
                        true),
                    targetList);

            // only write to target if we did get a merge result
            if (mergeResultList != null)
            {
                targetClient.WriteRange(mergeResultList, targetStorePath);
            }

            return true;
        }
    }
}
