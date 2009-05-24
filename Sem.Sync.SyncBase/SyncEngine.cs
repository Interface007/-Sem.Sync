using System.Globalization;
using Sem.Sync.SyncBase.Properties;

namespace Sem.Sync.SyncBase
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    using Microsoft.Win32;

    using EventArgs;
    using Binding;
    using Helpers;
    using Interfaces;

    public class SyncEngine : SyncComponent
    {
        private bool versionOutdated;

        public SyncEngine()
        {
            versionOutdated = VersionCheck.Check();
        }

        public event EventHandler<QueryForLogOnCredentialsEventArgs> QueryForLogOnCredentialsEvent;

        public IMergeConflictResolver ConflictSolver { get; set; }

        /// <summary>
        /// Gets or sets a value that represents the file system working folder. Use 
        /// {FS:WorkingFolder} inside the source or target path to access this directory.
        /// </summary>
        public string WorkingFolder { get; set; }

        /// <summary>
        /// Determines the registry path to the value that holds the path to BeyondCompare
        /// </summary>
        private const string BeyondComparePath = "Software\\Scooter Software\\Beyond Compare 3";

        /// <summary>
        /// Gets the file system path to the external program BeyondCompare
        /// </summary>
        private static string PathToBeyondCompare
        {
            get
            {
                var key = Registry.LocalMachine.OpenSubKey(BeyondComparePath);
                if (key != null)
                {
                    var pathValue = key.GetValue("ExePath");
                    if (pathValue != null)
                    {
                        if (File.Exists(pathValue.ToString()))
                            return pathValue.ToString();
                    }
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// execute the commands stored inside the list
        /// </summary>
        /// <param name="syncList">the list to execute</param>
        public bool Execute(SyncCollection syncList)
        {
            var itemsDone = 0;
            var itemsMax = syncList.Count;

            LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiStartingProcessing, itemsMax));

            foreach (var item in syncList)
            {
                UpdateProgress(itemsDone * 100 / itemsMax);
                if (!this.Execute(item))
                {
                    LogProcessingEvent(Resources.uiProcessingCanceled);
                    UpdateProgress(0);
                    return false;
                }
                itemsDone++;
            }

            UpdateProgress(itemsDone * 100 / itemsMax);
            return true;
        }

        /// <summary>
        /// Executes a command that (mostly) have a source and a target - see the documentations 
        /// of SyncCommand enumeration
        /// </summary>
        /// <param name="item">a synchronisation command that includes the source, destination, parameters and a command</param>
        public bool Execute(SyncDescription item)
        {
            var continueExecution = true;

            // create classes according to the description
            var sourceClient = Factory.GetNewObject<IClientBase>(item.SourceConnector);
            var targetClient = Factory.GetNewObject<IClientBase>(item.TargetConnector);
            var baseliClient = Factory.GetNewObject<IClientBase>(item.BaselineConnector);

            // wire up events
            WireUpEvents(sourceClient, true);
            WireUpEvents(targetClient, true);
            WireUpEvents(baseliClient, true);

            if (versionOutdated)
                LogProcessingEvent("Version of sync engine is outdated - please update!");

            // process paths to replace token
            item.SourceStorePath = ReplacePathToken(item.SourceStorePath);
            item.TargetStorePath = ReplacePathToken(item.TargetStorePath);
            item.BaselineStorePath = ReplacePathToken(item.BaselineStorePath);

            // select the specified command
            switch (item.Command)
            {
                case SyncCommand.CopyAll:
                    if (targetClient == null) throw new InvalidOperationException("item.targetClient is null");
                    if (sourceClient == null) throw new InvalidOperationException("item.sourceClient is null");
                    targetClient.AddRange(
                        sourceClient.GetAll(item.SourceStorePath),
                        item.TargetStorePath);
                    break;

                case SyncCommand.MergeMissing:
                    if (targetClient == null) throw new InvalidOperationException("item.targetClient is null");
                    if (sourceClient == null) throw new InvalidOperationException("item.sourceClient is null");
                    targetClient.MergeMissingRange(
                        sourceClient.GetAll(item.SourceStorePath),
                        item.TargetStorePath);
                    break;

                case SyncCommand.RemoveDuplicatesOnTarget:
                    if (targetClient == null) throw new InvalidOperationException("item.targetClient is null");
                    targetClient.RemoveDuplicates(item.TargetStorePath);
                    break;

                case SyncCommand.MergeExternal:
                    MergeFiles(item.SourceStorePath, item.TargetStorePath, item.BaselineStorePath);
                    break;

                case SyncCommand.MergeHighEvidence:
                    if (targetClient == null) throw new InvalidOperationException("item.targetClient is null");
                    if (sourceClient == null) throw new InvalidOperationException("item.sourceClient is null");
                    targetClient.WriteRange(
                        targetClient.GetAll(item.TargetStorePath)
                            .MergeHighEvidence(sourceClient.GetAll(item.SourceStorePath))
                        , item.TargetStorePath);
                    break;

                case SyncCommand.NormalizeContent:
                    if (targetClient == null) throw new InvalidOperationException("item.targetClient is null");
                    if (sourceClient == null) throw new InvalidOperationException("item.sourceClient is null");
                    targetClient.WriteRange(
                        targetClient.Normalize(targetClient.GetAll(item.TargetStorePath))
                        , item.SourceStorePath);

                    break;

                case SyncCommand.MatchByName:
                    if (targetClient == null) throw new InvalidOperationException("item.targetClient is null");
                    if (sourceClient == null) throw new InvalidOperationException("item.sourceClient is null");
                    if (item.SourceStorePath == null) throw new InvalidOperationException("item.SourceStorePath is null");
                    if (item.TargetStorePath == null) throw new InvalidOperationException("item.TargetStorePath is null");
                    targetClient.WriteRange(
                        MatchByName(
                            sourceClient.GetAll(item.SourceStorePath),
                            targetClient.GetAll(item.TargetStorePath)),
                        item.TargetStorePath);
                    break;

                case SyncCommand.DeletePattern:
                    if (item.TargetStorePath == null) throw new InvalidOperationException("item.TargetStorePath is null");
                    DeleteFiles(item.TargetStorePath);
                    break;

                case SyncCommand.DetectConflicts:
                    // todo: this is currently specific to StdContact-elements, so we need to generalize it.
                    if (targetClient == null) throw new InvalidOperationException("item.targetClient is null");
                    if (sourceClient == null) throw new InvalidOperationException("item.sourceClient is null");
                    if (item.SourceStorePath == null) throw new InvalidOperationException("item.SourceStorePath is null");
                    if (item.TargetStorePath == null) throw new InvalidOperationException("item.TargetStorePath is null");

                    var targetList = targetClient.GetAll(item.TargetStorePath);
                    var mergeResultList =
                    ConflictSolver.PerformMerge(
                        SyncTools.DetectConflicts(
                            SyncTools.BuildConflictTestContainerList(
                                sourceClient.GetAll(item.SourceStorePath),
                                targetList,
                                (baseliClient == null) ? null : baseliClient.GetAll(item.BaselineStorePath),
                                typeof(StdContact)
                            ),
                            true
                        )
                        , targetList
                    );

                    // only write to target if we did get a merge result
                    if (mergeResultList != null)
                        targetClient.WriteRange(mergeResultList, item.TargetStorePath);

                    break;

                case SyncCommand.AskForContinue:
                    // todo: delegate this message box to the ui elements (this assembly should not directly contain ui interaction)
                    continueExecution =
                        MessageBox.Show(
                            item.CommandParameter,
                            targetClient.FriendlyClientName,
                            MessageBoxButtons.OKCancel)
                        == DialogResult.OK;
                    break;

                default:
                    break;
            }

            WireUpEvents(sourceClient, false);
            WireUpEvents(targetClient, false);
            WireUpEvents(baseliClient, false);

            return continueExecution;
        }

        private void WireUpEvents(IClientBase client, bool addEvent)
        {
            if (client == null) return;

            if (addEvent)
            {
                client.ProcessingEvent += this.LogProcessingEvent;
                client.QueryForLoginCredentialsEvent += this.QueryForLogOnCredentialsEvent;
            }
            else
            {
                client.ProcessingEvent -= this.LogProcessingEvent;
                client.QueryForLoginCredentialsEvent -= this.QueryForLogOnCredentialsEvent;
            }
        }

        private string ReplacePathToken(string path)
        {
            return (path ?? "").Replace("{FS:WorkingFolder}", this.WorkingFolder);
        }

        private void DeleteFiles(string path)
        {
            SyncTools.EnsurePathExist(Path.GetDirectoryName(path));
            foreach (var file in Directory.GetFiles(Path.GetDirectoryName(path), Path.GetFileName(path)))
            {
                File.Delete(file);
            }
            this.LogProcessingEvent(Resources.uiFilesDeleted);
        }

        private static List<StdElement> MatchByName(IEnumerable<StdElement> source, List<StdElement> target)
        {
            foreach (var item in target)
            {
                var corresponding = (from element in source
                                     where element.Id == item.Id
                                     select element).FirstOrDefault();

                // if there is someone with the same id, we do not need to match
                if (corresponding != null) continue;

                // try it by full name
                // or try it by full name without academic title
                corresponding = (from element in source
                                 where element.ToString() == item.ToString()
                                 select element).FirstOrDefault()
                                 ?? (from element in source
                                     where element.ToStringSimple() == item.ToStringSimple()
                                     select element).FirstOrDefault();


                // if we did find the name, we match using the Id
                if (corresponding != null)
                {
                    item.Id = corresponding.Id;
                }
            }

            return target;
        }

        private static void MergeFiles(string source, string target, string baseline)
        {
            var pathValue = PathToBeyondCompare;
            if (string.IsNullOrEmpty(pathValue)) return;

            var process = System.Diagnostics.Process.Start(
                pathValue,
                " \"" + source + "\"" +
                " \"" + target + "\"" +
                (string.IsNullOrEmpty(baseline) ? "" : " \"" + baseline + "\"")
                );

            if (process != null)
                process.WaitForExit();
        }
    }
}
