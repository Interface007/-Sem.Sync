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
    using System.Reflection;

    using Binding;

    using GenericHelpers;
    using GenericHelpers.EventArgs;
    using GenericHelpers.Exceptions;
    using GenericHelpers.Interfaces;

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
        #region fields
        /// <summary>
        /// the factory to create the classes
        /// </summary>
        private readonly Factory factory = new Factory("Sem.Sync.SyncBase");

        /// <summary>
        /// will be set when the first command is executed
        /// </summary>
        private bool versionOutdated;

        /// <summary>
        /// flag for already executed version check
        /// </summary>
        private bool versionChecked;

        /// <summary>
        /// the number of commands in the sequence
        /// </summary>
        private int numberOfCommandsInSequence;

        /// <summary>
        /// the percentage of work already done
        /// </summary>
        private int percentageOfSequenceDone;
        #endregion

        #region events
        /// <summary>
        /// Will be raised in the event of needed log on credentials
        /// </summary>
        public event EventHandler<QueryForLogOnCredentialsEventArgs> QueryForLogOnCredentialsEvent;
        #endregion

        /// <summary>
        /// Gets or sets a value that represents the file system working folder. Use 
        /// {FS:WorkingFolder} inside the source or target path to access this directory.
        /// The user of this class is responsible to use specify a usefull working folder.
        /// </summary>
        public string WorkingFolder { get; set; }

        /// <summary>
        /// execute the commands stored inside the list
        /// </summary>
        /// <param name="syncList"> the list to execute </param>
        /// <returns> a value specifying if the execution should continue </returns>
        public bool Execute(SyncCollection syncList)
        {
            var itemsDone = 0;
            this.numberOfCommandsInSequence = syncList.Count;

            LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiStartingProcessing, this.percentageOfSequenceDone));

            foreach (var item in syncList)
            {
                this.percentageOfSequenceDone = itemsDone * 100 / this.numberOfCommandsInSequence;
                UpdateProgress(this.percentageOfSequenceDone);
                if (!this.Execute(item))
                {
                    LogProcessingEvent(Resources.uiProcessingCanceled);
                    UpdateProgress(0);
                    return false;
                }

                itemsDone++;
            }

            UpdateProgress(100);
            return true;
        }

        /// <summary>
        /// Replaces known string token like the token {FS:WorkingFolder} that represents the
        /// current working folder for the file system
        /// </summary>
        /// <param name="path">the string that might contain token</param>
        /// <returns>the processed string containing the token values instead of the token</returns>
        public string ReplacePathToken(string path)
        {
            return (path ?? string.Empty)
                .Replace("{FS:WorkingFolder}", this.WorkingFolder)
                .Replace("{FS:ApplicationFolder}", Path.GetDirectoryName(Assembly.GetCallingAssembly().CodeBase));
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "in this method exceptions are just logged - it's not acceptable to interrupt batch execution in case of a 'minor' issue.")]
        public bool Execute(SyncDescription item)
        {
            if (!this.versionChecked)
            {
                this.versionOutdated = !new VersionCheck().Check(this.UiProvider);
                this.versionChecked = true;
            }

            var continueExecution = true;

            // create classes according to the description
            var sourceClient = this.SetupConnector(item.SourceConnector, item.SourceCredentials);
            var targetClient = this.SetupConnector(item.TargetConnector, item.TargetCredentials);
            var baseliClient = this.SetupConnector(item.BaselineConnector, item.BaselineCredentials);

            if (this.versionOutdated)
            {
                LogProcessingEvent("Version of sync engine is outdated - please update!");
            }

            // process paths to replace token
            var sourceStorePath = this.ReplacePathToken(item.SourceStorePath);
            var targetStorePath = this.ReplacePathToken(item.TargetStorePath);
            var baselineStorePath = this.ReplacePathToken(item.BaselineStorePath);

            try
            {
                var command = this.factory.GetNewObject<ISyncCommand>(string.Format(CultureInfo.CurrentCulture, "Sem.Sync.SyncBase.Commands.{0}, Sem.Sync.SyncBase", item.Command));
                var commandAsComponent = command as SyncComponent;

                if (command != null)
                {
                    command.UiProvider = this.UiProvider;

                    // wire up events
                    this.WireUpEvents(sourceClient, true);
                    this.WireUpEvents(targetClient, true);
                    this.WireUpEvents(baseliClient, true);
                    this.WireUpEvents(commandAsComponent, true);

                    // execute the command with the connectors
                    continueExecution = command.ExecuteCommand(sourceClient, targetClient, baseliClient, sourceStorePath, targetStorePath, baselineStorePath, this.ReplacePathToken(item.CommandParameter));

                    this.WireUpEvents(commandAsComponent, false);
                }
            }
            catch (Exception ex)
            {
                this.CheckForInteropAssemblies(ex as FileNotFoundException);

                ExceptionHandler.HandleException(
                    new TechnicalException(
                        "exception while processing SyncDescription",
                        ex,
                        new KeyValuePair<string, object>("SyncDescription", item)));
                this.LogException(ex);
            }
            finally
            {
                // detach events
                this.WireUpEvents(sourceClient, false);
                this.WireUpEvents(targetClient, false);
                this.WireUpEvents(baseliClient, false);
            }

            return continueExecution;
        }

        /// <summary>
        /// Generates a connector and inserts some information
        /// </summary>
        /// <param name="typeName"> The type name to generate. </param>
        /// <param name="credentials"> The credentials to add to the connector . </param>
        /// <returns> The connector that is set up </returns>
        public StdClient SetupConnector(string typeName, ICredentialAware credentials)
        {
            var client = this.factory.GetNewObject<StdClient>(typeName);
            if (client != null && credentials != null)
            {
                client.LogOnUserId = credentials.LogOnUserId;
                client.LogOnPassword = credentials.LogOnPassword;
                client.LogOnDomain = credentials.LogOnDomain;
            }

            return client;
        }

        /// <summary>
        /// attached or detaches event handlers between sync engine and client implementation
        /// </summary>
        /// <param name="component">the client/command that provides events this engine should subscribe to</param>
        /// <param name="addEvent">a value to specify if the events should be attached (true) or detached (false)</param>
        private void WireUpEvents(SyncComponent component, bool addEvent)
        {
            if (component == null)
            {
                return;
            }

            var client = component as IClientBase;
            var clientBase = component as StdClient;

            if (addEvent)
            {
                component.ProcessingEvent += this.LogProcessingEvent;
                component.ProgressEvent += this.HandleProgressEvent;

                if (clientBase != null)
                {
                    clientBase.UiDispatcher = this.UiProvider;
                }

                if (client != null)
                {
                    client.QueryForLogonCredentialsEvent += this.QueryForLogOnCredentialsEvent;
                }
            }
            else
            {
                component.ProcessingEvent -= this.LogProcessingEvent;
                component.ProgressEvent -= this.HandleProgressEvent;

                if (client != null)
                {
                    client.QueryForLogonCredentialsEvent -= this.QueryForLogOnCredentialsEvent;
                }
            }
        }

        /// <summary>
        /// Handels the <see cref="SyncComponent.ProgressEvent"/>.
        /// </summary>
        /// <param name="sender"> The sender object. </param>
        /// <param name="e"> The event argument. </param>
        private void HandleProgressEvent(object sender, ProgressEventArgs e)
        {
            this.UpdateProgress(this.percentageOfSequenceDone
                                + (e.PercentageDone / (this.numberOfCommandsInSequence + 1)));
        }

        /// <summary>
        /// Checks if the exception is highly porpable from a missing interop assembly and opens a link for the download.
        /// </summary>
        /// <param name="ex"> The exception to check. </param>
        private void CheckForInteropAssemblies(FileNotFoundException ex)
        {
            if (ex == null || !ex.Message.Contains("Microsoft.Office.Interop"))
            {
                return;
            }

            if (!this.UiProvider.AskForConfirm(Resources.MissingInteropQuestion, Resources.MissingInteropQuestionTitle))
            {
                return;
            }

            // Office 2003
            if (ex.Message.Contains("Version=11"))
            {
                Process.Start("http://www.microsoft.com/downloads/details.aspx?familyid=3c9a983a-ac14-4125-8ba0-d36d67e0f4ad&displaylang=en");
            }

            // Office 2007
            if (ex.Message.Contains("Version=12"))
            {
                Process.Start("http://www.microsoft.com/downloads/details.aspx?FamilyID=59daebaa-bed4-4282-a28c-b864d8bfa513&displaylang=en");
            }

            // Office 2010
            if (ex.Message.Contains("Version=14"))
            {
                Process.Start("http://go.microsoft.com/fwlink/?LinkId=166026");
            }
        }
    }
}
