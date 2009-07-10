using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sem.Sync.SyncBase.Commands
{
    using System.IO;

    using Helpers;

    using Interfaces;

    using Properties;

    public class DeletePattern : ISyncCommand
    {
        public IUiInteraction UiProvider { get; set;}

        public bool ExecuteCommand(IClientBase sourceClient, IClientBase targetClient, IClientBase baseliClient, string sourceStorePath, string targetStorePath, string baselineStorePath, string commandParameter)
        {
            if (targetStorePath == null) throw new InvalidOperationException("targetStorePath is null");
            DeleteFiles(targetStorePath);
            return true;
        }

        /// <summary>
        /// Deletes files from a folder using a search pattern. Use "*" as a place holder for any 
        /// number of any chars; use "?" as a placeholder for a single char.
        /// </summary>
        /// <param name="path">the path including the search pattern</param>
        private static void DeleteFiles(string path)
        {
            var paths = path.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var singlePath in paths)
            {
                var singlePathWithoutSpaces = singlePath.Trim();
                if (!string.IsNullOrEmpty(singlePathWithoutSpaces))
                {
                    SyncTools.EnsurePathExist(Path.GetDirectoryName(singlePathWithoutSpaces));
                    foreach (var file in Directory.GetFiles(Path.GetDirectoryName(singlePathWithoutSpaces), Path.GetFileName(singlePathWithoutSpaces)))
                    {
                        File.Delete(file);
                        // this.LogProcessingEvent(Resources.uiFilesDeleted + ": " + file);
                    }
                }
            }
        }
    }
}
