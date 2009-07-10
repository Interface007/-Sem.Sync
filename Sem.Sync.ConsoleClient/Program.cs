namespace Sem.Sync.ConsoleClient
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Xml.Serialization;

    using SyncBase;
    using SyncBase.Binding;
    using SyncBase.EventArgs;

    class Program
    {
        static void Main(string[] args)
        {
#if (DEBUG)
            if (args.Length < 1)
            {
                args = new [] { "A Copy to CSV.SyncList" };
            }
#endif

            if (args.Length >= 1)
            {
                var success = false;
                try
                {
                    Console.WriteLine("loading command list: {0}", args[0]);
                    var SyncCommands = LoadSyncList(args[0]);

                    var defaultBaseFolder =
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SemSyncCmd");

                    Console.WriteLine("working folder: {0}", defaultBaseFolder);

                    var engine = new SyncEngine { WorkingFolder = defaultBaseFolder, UiProvider = new UiDispatcher() };
                    ((UiDispatcher)engine.UiProvider).UserDomain = args.Length > 1 ? args[1] : "";
                    ((UiDispatcher)engine.UiProvider).UserName = args.Length > 2 ? args[2] : "";
                    ((UiDispatcher)engine.UiProvider).UserPassword = args.Length > 3 ? args[3] : "";
                    engine.ProcessingEvent += ProcessingEvent;
                    engine.ProgressEvent += ProgressEvent;
            
                    success = engine.Execute(SyncCommands);
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
            Console.ReadLine();
#endif
        }

        private static void ProcessingEvent(object sender, ProcessingEventArgs e)
        {
            Console.WriteLine(e.Message); 
        }

        private static void ProgressEvent(object sender, ProgressEventArgs e)
        {
            Console.WriteLine("{0}% done...", e.PercentageDone); 
        }

        internal static SyncCollection LoadSyncList(string p)
        {
            var formatter = new XmlSerializer(typeof(SyncCollection));
            using (var file = new FileStream(p, FileMode.Open))
            {
                return (SyncCollection)formatter.Deserialize(file);
            }
        }
    }
}
