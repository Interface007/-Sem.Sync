// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncEngine.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Microsoft.Win32;

    using Binding;
    using DetailData;
    using EventArgs;
    using Helpers;
    using Interfaces;
    using Properties;

    /// <summary>
    /// The sync engine is the heart of the library. This engine does coordinate the work in
    /// processes between the connectors. 
    /// </summary>
    /// <remarks>
    /// <para>Using the <see cref="Execute(Sem.Sync.SyncBase.SyncDescription)"/> method
    /// the calling process can execute a single <see cref="SyncDescription"/> which represents a single
    /// part of work. The command that can be executed are described inside the enumeration <see cref="SyncCommand"/>.</para>
    /// <para>The overload <see cref="Execute(Sem.Sync.SyncBase.Binding.SyncCollection)"/> does provide
    /// a way to execute a collection of <see cref="SyncDescription"/> in a sequence.</para>
    /// </remarks>
    public class SyncEngine : SyncComponent
    {
        /// <summary>
        /// Determines the registry path to the value that holds the path to BeyondCompare
        /// </summary>
        private const string BeyondComparePath = "Software\\Scooter Software\\Beyond Compare 3";

        /// <summary>
        /// will be set when the first command is executed
        /// </summary>
        private bool versionOutdated;

        /// <summary>
        /// flag for already executed version check
        /// </summary>
        private bool versionChecked;

        /// <summary>
        /// Will be raised in the event of needed log on credentials
        /// </summary>
        public event EventHandler<QueryForLogOnCredentialsEventArgs> QueryForLogOnCredentialsEvent;

        /// <summary>
        /// Gets or sets the object that will be responsible for routing UI interaction requests from connectors
        /// </summary>
        public IUiInteraction UiProvider { get; set; }

        /// <summary>
        /// Gets or sets a value that represents the file system working folder. Use 
        /// {FS:WorkingFolder} inside the source or target path to access this directory.
        /// The user of this class is responsible to use specify a usefull working folder.
        /// </summary>
        public string WorkingFolder { get; set; }

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
                        {
                            return pathValue.ToString();
                        }
                    }
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// execute the commands stored inside the list
        /// </summary>
        /// <param name="syncList"> the list to execute </param>
        /// <returns> a value specifying if the execution should continue </returns>
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
        /// <param name="item">
        /// a synchronisation command that includes the source, destination, parameters and a command
        /// </param>
        /// <returns>
        /// a value whether the execution should continue (true) or should abort (false)
        /// </returns>
        public bool Execute(SyncDescription item)
        {
            if (!this.versionChecked)
            {
                this.versionOutdated = !VersionCheck.Check(this.UiProvider);
                this.versionChecked = true;
            }

            var continueExecution = true;

            // create classes according to the description
            var sourceClient = Factory.GetNewObject<IClientBase>(item.SourceConnector);
            var targetClient = Factory.GetNewObject<IClientBase>(item.TargetConnector);
            var baseliClient = Factory.GetNewObject<IClientBase>(item.BaselineConnector);

            // wire up events
            this.WireUpEvents(sourceClient, true);
            this.WireUpEvents(targetClient, true);
            this.WireUpEvents(baseliClient, true);

            if (this.versionOutdated)
            {
                LogProcessingEvent("Version of sync engine is outdated - please update!");
            }

            // process paths to replace token
            item.SourceStorePath = this.ReplacePathToken(item.SourceStorePath);
            item.TargetStorePath = this.ReplacePathToken(item.TargetStorePath);
            item.BaselineStorePath = this.ReplacePathToken(item.BaselineStorePath);

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
                    MergeFilesBeyondCompare(item.SourceStorePath, item.TargetStorePath, item.BaselineStorePath);
                    break;

                case SyncCommand.MergeHighEvidence:
                    if (targetClient == null) throw new InvalidOperationException("item.targetClient is null");
                    if (sourceClient == null) throw new InvalidOperationException("item.sourceClient is null");
                    targetClient.WriteRange(
                        targetClient.GetAll(item.TargetStorePath)
                            .MergeHighEvidence(sourceClient.GetAll(item.SourceStorePath)),
                            item.TargetStorePath);
                    break;

                case SyncCommand.NormalizeContent:
                    if (targetClient == null) throw new InvalidOperationException("item.targetClient is null");
                    if (sourceClient == null) throw new InvalidOperationException("item.sourceClient is null");
                    targetClient.WriteRange(
                        targetClient.Normalize(targetClient.GetAll(item.TargetStorePath)),
                        item.SourceStorePath);

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

                case SyncCommand.MatchByProfileId:
                    if (targetClient == null) throw new InvalidOperationException("item.targetClient is null");
                    if (sourceClient == null) throw new InvalidOperationException("item.sourceClient is null");
                    if (item.SourceStorePath == null) throw new InvalidOperationException("item.SourceStorePath is null");
                    if (item.TargetStorePath == null) throw new InvalidOperationException("item.TargetStorePath is null");
                    targetClient.WriteRange(
                        MatchByProfileId(
                            sourceClient.GetAll(item.SourceStorePath),
                            baseliClient.GetAll(item.BaselineStorePath)),
                        item.TargetStorePath);
                    break;

                case SyncCommand.MatchManually:
                    if (targetClient == null) throw new InvalidOperationException("item.targetClient is null");
                    if (sourceClient == null) throw new InvalidOperationException("item.sourceClient is null");
                    if (baseliClient == null) throw new InvalidOperationException("item.baseliClient is null");
                    if (item.BaselineStorePath == null) throw new InvalidOperationException("item.SourceStorePath is null");
                    if (item.SourceStorePath == null) throw new InvalidOperationException("item.SourceStorePath is null");
                    if (item.TargetStorePath == null) throw new InvalidOperationException("item.TargetStorePath is null");

                    var targetMatchList = targetClient.GetAll(item.TargetStorePath);
                    var matchResultList =
                    this.UiProvider.PerformEntityMerge(
                        sourceClient.GetAll(item.SourceStorePath),
                        targetMatchList,
                        baseliClient.GetAll(item.BaselineStorePath));

                    // only write to target if we did get a merge result
                    if (targetMatchList != null)
                        targetClient.WriteRange(targetMatchList, item.TargetStorePath);

                    // only write to target if we did get a merge result
                    if (matchResultList != null)
                        baseliClient.WriteRange(matchResultList, item.BaselineStorePath);

                    break;

                case SyncCommand.DeletePattern:
                    if (item.TargetStorePath == null) throw new InvalidOperationException("item.TargetStorePath is null");
                    this.DeleteFiles(item.TargetStorePath);
                    break;

                case SyncCommand.DetectConflicts:
                    // todo: this is currently specific to StdContact-elements, so we need to generalize it.
                    if (targetClient == null) throw new InvalidOperationException("item.targetClient is null");
                    if (sourceClient == null) throw new InvalidOperationException("item.sourceClient is null");
                    if (item.SourceStorePath == null) throw new InvalidOperationException("item.SourceStorePath is null");
                    if (item.TargetStorePath == null) throw new InvalidOperationException("item.TargetStorePath is null");

                    var targetList = targetClient.GetAll(item.TargetStorePath);
                    var mergeResultList =
                        this.UiProvider.PerformAttributeMerge(
                            SyncTools.DetectConflicts(
                                SyncTools.BuildConflictTestContainerList(
                                    sourceClient.GetAll(item.SourceStorePath),
                                    targetList,
                                    (baseliClient == null) ? null : baseliClient.GetAll(item.BaselineStorePath),
                                    typeof(StdContact)),
                                true),
                            targetList);

                    // only write to target if we did get a merge result
                    if (mergeResultList != null)
                    {
                        targetClient.WriteRange(mergeResultList, item.TargetStorePath);
                    }

                    break;

                case SyncCommand.AskForContinue:
                    if (this.UiProvider != null)
                    {
                        continueExecution = this.UiProvider.AskForConfirm(
                            item.CommandParameter, targetClient.FriendlyClientName);
                    }

                    break;

                case SyncCommand.OpenDocument:
                    if (!string.IsNullOrEmpty(item.CommandParameter))
                    {
                            Process.Start(new ProcessStartInfo(this.ReplacePathToken(item.CommandParameter)));
                    }
                    break;

                default:
                    break;
            }

            this.WireUpEvents(sourceClient, false);
            this.WireUpEvents(targetClient, false);
            this.WireUpEvents(baseliClient, false);

            return continueExecution;
        }

        /// <summary>
        /// attached or detaches event handlers between sync engine and client implementation
        /// </summary>
        /// <param name="client">the client that provides events this engine should subscribe to</param>
        /// <param name="addEvent">a value to specify if the events should be attached (true) or detached (false)</param>
        private void WireUpEvents(IClientBase client, bool addEvent)
        {
            if (client == null)
            {
                return;
            }

            if (addEvent)
            {
                client.ProcessingEvent += this.LogProcessingEvent;
                client.QueryForLogonCredentialsEvent += this.QueryForLogOnCredentialsEvent;
            }
            else
            {
                client.ProcessingEvent -= this.LogProcessingEvent;
                client.QueryForLogonCredentialsEvent -= this.QueryForLogOnCredentialsEvent;
            }
        }

        /// <summary>
        /// Replaces known string token like the token {FS:WorkingFolder} that represents the
        /// current working folder for the file system
        /// </summary>
        /// <param name="path">the string that might contain token</param>
        /// <returns>the processed string containing the token values instead of the token</returns>
        private string ReplacePathToken(string path)
        {

            return (path ?? string.Empty).Replace("{FS:WorkingFolder}", this.WorkingFolder);
        }

        /// <summary>
        /// Deletes files from a folder using a search pattern. Use "*" as a place holder for any 
        /// number of any chars; use "?" as a placeholder for a single char.
        /// </summary>
        /// <param name="path">the path including the search pattern</param>
        private void DeleteFiles(string path)
        {
            SyncTools.EnsurePathExist(Path.GetDirectoryName(path));
            foreach (var file in Directory.GetFiles(Path.GetDirectoryName(path), Path.GetFileName(path)))
            {
                File.Delete(file);
            }
            this.LogProcessingEvent(Resources.uiFilesDeleted);
        }

        /// <summary>
        /// Automatically matches without user interaction entities by <see cref="object.ToString"/> and (with lower 
        /// priority) by <see cref="StdElement.ToStringSimple"/>
        /// </summary>
        /// <param name="source">the list of <see cref="StdElement"/> that contains the source 
        /// (this will not be changed)</param>
        /// <param name="target">the list of <see cref="StdElement"/> that contains the target 
        /// (here the <see cref="StdElement.Id"/> will be changed if a match is found in the source)</param>
        /// <returns>the modified list of elements from the <paramref name="target"/></returns>
        private static List<StdElement> MatchByName(IEnumerable<StdElement> source, List<StdElement> target)
        {
            // ReSharper disable AccessToModifiedClosure
            foreach (var item in target)
            {
                var corresponding = (from element in source
                                     where element.Id == item.Id
                                     select element).FirstOrDefault();

                // if there is someone with the same id, we do not need to match
                if (corresponding != null)
                {
                    continue;
                }

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
            // ReSharper restore AccessToModifiedClosure

            return target;
        }

        /// <summary>
        /// Automatically matches without user interaction entities by <see cref="StdContact.PersonalProfileIdentifiers"/>.
        /// </summary>
        /// <param name="baseline">the list of <see cref="StdElement"/> that contains the source of the baseline
        /// (this will not be changed, but need to contain entries of type <see cref="MatchingEntry"/>)</param>
        /// <param name="target">the list of <see cref="StdContact"/> that contains the target 
        /// (here the <see cref="StdElement.Id"/> will be changed if a match is found in the baseline)</param>
        /// <returns>the modified list of elements from the <paramref name="target"/></returns>
        private static List<StdElement> MatchByProfileId(List<StdElement> target, IEnumerable<StdElement> baseline)
        {
            // ReSharper disable AccessToModifiedClosure
            foreach (var item in target)
            {
                var corresponding = (from element in baseline
                                     where ((MatchingEntry)element).ProfileId.MatchesAny(((StdContact)item).PersonalProfileIdentifiers)
                                     select element).FirstOrDefault();

                // if there is one with a matching profile id, 
                // we overwrite the id
                if (corresponding != null)
                {
                    item.Id = corresponding.Id;
                }
            }
            // ReSharper restore AccessToModifiedClosure

            return target;
        }

        /// <summary>
        /// Performs a Merge operation between two files or three files. This operation does not 
        /// use client implementations but file system paths only.
        /// </summary>
        /// <param name="source">the path of the source list to merge</param>
        /// <param name="target">the path of the target list to merge</param>
        /// <param name="baseline">the path of the baseline list to merge</param>
        private void MergeFilesBeyondCompare(string source, string target, string baseline)
        {
            var pathValue = PathToBeyondCompare;
            if (string.IsNullOrEmpty(pathValue))
            {
                return;
            }

            var process = Process.Start(
                pathValue,
                " \"" + source + "\"" + " \"" + target + "\"" + (string.IsNullOrEmpty(baseline) ? "" : " \"" + baseline + "\""));

            if (process != null)
            {
                process.WaitForExit();
            }

            this.LogProcessingEvent("Process started: " + pathValue);
        }
    }
}
