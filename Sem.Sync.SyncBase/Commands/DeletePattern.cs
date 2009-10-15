// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeletePattern.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This command deletes files specified by one or more path pattern separated by a line break
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Commands
{
    using System;
    using System.Globalization;
    using System.IO;

    using GenericHelpers;
    using GenericHelpers.Interfaces;

    using Interfaces;

    using Properties;

    /// <summary>
    /// This command deletes files specified by one or more path pattern separated by a line break
    /// </summary>
    public class DeletePattern : SyncComponent, ISyncCommand
    {
        /// <summary>
        /// This command deletes files specified by one or more path pattern separated by a line break.
        /// Deletes files from a folder using a search pattern. Use "*" as a place holder for any 
        /// number of any chars; use "?" as a placeholder for a single char.
        /// </summary>
        /// <param name="sourceClient">The source client.</param>
        /// <param name="targetClient">The target client.</param>
        /// <param name="baseliClient">The baseline client.</param>
        /// <param name="sourceStorePath">The source storage path.</param>
        /// <param name="targetStorePath">The target storage path.</param>
        /// <param name="baselineStorePath">The baseline storage path.</param>
        /// <param name="commandParameter">The command parameter.</param>
        /// <returns> True if the response from the <see cref="SyncComponent.UiProvider"/> is "continue" </returns>
        public bool ExecuteCommand(IClientBase sourceClient, IClientBase targetClient, IClientBase baseliClient, string sourceStorePath, string targetStorePath, string baselineStorePath, string commandParameter)
        {
            if (string.IsNullOrEmpty(commandParameter))
            {
                commandParameter = targetStorePath;
            }

            if (string.IsNullOrEmpty(commandParameter))
            {
                throw new InvalidOperationException("commandParameter is null");
            }

            var deletionCounter = 0;
            var paths = commandParameter.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var singlePath in paths)
            {
                var singlePathWithoutSpaces = singlePath.Trim();
                if (!string.IsNullOrEmpty(singlePathWithoutSpaces))
                {
                    Tools.EnsurePathExist(Path.GetDirectoryName(singlePathWithoutSpaces));
                    foreach (var file in Directory.GetFiles(Path.GetDirectoryName(singlePathWithoutSpaces), Path.GetFileName(singlePathWithoutSpaces)))
                    {
                        File.Delete(file);
                        deletionCounter++;
                        this.LogProcessingEvent(Resources.uiFilesDeleted + ": " + file);
                    }
                }
            }

            this.LogProcessingEvent(Resources.uiFilesDeleted + ": " + deletionCounter.ToString(CultureInfo.CurrentCulture));
            return true;
        }
    }
}
