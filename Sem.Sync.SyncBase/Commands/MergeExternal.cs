namespace Sem.Sync.SyncBase.Commands
{
    using System.Diagnostics;
    using System.IO;

    using Microsoft.Win32;

    using Interfaces;

    public class MergeExternal : ISyncCommand
    {
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
                        {
                            return pathValue.ToString();
                        }
                    }
                }

                return string.Empty;
            }
        }

        public IUiInteraction UiProvider { get; set; }

        public bool ExecuteCommand(IClientBase sourceClient, IClientBase targetClient, IClientBase baseliClient, string sourceStorePath, string targetStorePath, string baselineStorePath, string commandParameter)
        {
            var pathValue = PathToBeyondCompare;
            if (string.IsNullOrEmpty(pathValue))
            {
                return true;
            }

            var process = Process.Start(
                pathValue,
                " \"" + sourceStorePath + "\"" + " \"" + targetStorePath + "\"" + (string.IsNullOrEmpty(baselineStorePath) ? "" : " \"" + baselineStorePath + "\""));

            if (process != null)
            {
                process.WaitForExit();
            }
            return true;
        }
    }
}
