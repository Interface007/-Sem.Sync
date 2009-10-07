// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Main execution class that will run the program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.ConsoleClient
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Xml.Serialization;

    using GenericHelpers.EventArgs;

    using SyncBase;
    using SyncBase.Binding;

    /// <summary>
    /// Main execution class that will run the program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Executes the list of commands specified as a path to the file containing the serialized list in the first parameter.
        /// </summary>
        /// <param name="args">
        /// The parameter that contains the path to the list of serialized commands.
        /// </param>
        public static void Main(string[] args)
        {
#if (DEBUG)
            if (args.Length < 1)
            {
                args = new[] { "A Copy fromAccess.SyncList" };
            }
#endif

            if (args.Length >= 1)
            {
                var success = false;
                try
                {
                    Console.WriteLine("loading command list: {0}", args[0]);
                    var syncCommands = LoadSyncList(args[0]);

                    var defaultBaseFolder =
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SemSyncCmd");

                    Console.WriteLine("working folder: {0}", defaultBaseFolder);

                    var engine = new SyncEngine { WorkingFolder = defaultBaseFolder, UiProvider = new UiDispatcher() };
                    ((UiDispatcher)engine.UiProvider).UserDomain = args.Length > 1 ? args[1] : string.Empty;
                    ((UiDispatcher)engine.UiProvider).UserName = args.Length > 2 ? args[2] : string.Empty;
                    ((UiDispatcher)engine.UiProvider).UserPassword = args.Length > 3 ? args[3] : string.Empty;
                    engine.ProcessingEvent += ProcessingEvent;
                    engine.ProgressEvent += ProgressEvent;
            
                    success = engine.Execute(syncCommands);
                    engine.ProcessingEvent -= ProcessingEvent;
                    engine.ProgressEvent -= ProgressEvent;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("exception while execution: \n{0}", ex);
                }

                Console.WriteLine("Execution status: {0}", success ? "success" : "failed");
            }
            else
            {
                Console.WriteLine(
                    "usage: {0} [PathOfCommandList]",
                    Path.GetFileNameWithoutExtension(Assembly.GetAssembly(typeof(Program)).CodeBase));
            }

#if (DEBUG)
            // wait for user input if in debug build
            Console.WriteLine("Finished - Press ENTER to close the window");
            Console.ReadLine();
#endif
        }

        /// <summary>
        /// Logs the processing event
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e"> The processing event arguments that do include the message to be logged. </param>
        private static void ProcessingEvent(object sender, ProcessingEventArgs e)
        {
            Console.WriteLine(e.Message); 
        }

        /// <summary>
        /// Logs the progress event
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e"> The processing event arguments that do include the percentageof work done. </param>
        private static void ProgressEvent(object sender, ProgressEventArgs e)
        {
            Console.WriteLine("{0}% done...", e.PercentageDone); 
        }

        /// <summary>
        /// Loads the list of serialized commands.
        /// </summary>
        /// <param name="pathToFile"> The path to the file to be read. </param>
        /// <returns>the deserialized list of sync commands.</returns>
        private static SyncCollection LoadSyncList(string pathToFile)
        {
            var formatter = new XmlSerializer(typeof(SyncCollection));
            using (var file = new FileStream(pathToFile, FileMode.Open))
            {
                return (SyncCollection)formatter.Deserialize(file);
            }
        }
    }
}
