namespace Sem.Sync.SyncBase.Commands
{
    using System;
    using Interfaces;

    public class MatchManually : ISyncCommand
    {
        /// <summary>
        /// Gets or sets the object that will be responsible for routing UI interaction requests from connectors
        /// </summary>
        public IUiInteraction UiProvider { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceClient"></param>
        /// <param name="targetClient"></param>
        /// <param name="baseliClient"></param>
        /// <param name="sourceStorePath"></param>
        /// <param name="targetStorePath"></param>
        /// <param name="baselineStorePath"></param>
        public bool ExecuteCommand(IClientBase sourceClient, IClientBase targetClient, IClientBase baseliClient, string sourceStorePath, string targetStorePath, string baselineStorePath, string commandParameter)
        {
            if (targetClient == null) throw new InvalidOperationException("item.targetClient is null");
            if (sourceClient == null) throw new InvalidOperationException("item.sourceClient is null");
            if (baseliClient == null) throw new InvalidOperationException("item.baseliClient is null");
            if (baselineStorePath == null) throw new InvalidOperationException("sourceStorePath is null");
            if (sourceStorePath == null) throw new InvalidOperationException("sourceStorePath is null");
            if (targetStorePath == null) throw new InvalidOperationException("targetStorePath is null");

            var targetMatchList = targetClient.GetAll(targetStorePath);
            var matchResultList =
                this.UiProvider.PerformEntityMerge(
                    sourceClient.GetAll(sourceStorePath),
                    targetMatchList,
                    baseliClient.GetAll(baselineStorePath));

            // only write to target if we did get a merge result
            if (targetMatchList != null)
                targetClient.WriteRange(targetMatchList, targetStorePath);

            // only write to target if we did get a merge result
            if (matchResultList != null)
                baseliClient.WriteRange(matchResultList, baselineStorePath);
            return true;
        }
    }
}
