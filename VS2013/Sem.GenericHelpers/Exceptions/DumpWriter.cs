namespace Sem.GenericHelpers.Exceptions
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;

    internal class DumpWriter
    {
        [DllImport("DbgHelp.dll", SetLastError = true)]
        private static extern bool MiniDumpWriteDump(IntPtr hProcess, int processId, IntPtr fileHandle, int dumpType, IntPtr excepInfo, IntPtr userInfo, IntPtr extInfo);

        public static void CreateMiniDump(Process process, string outputFileName)
        {
            using (var stream = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
            {

#pragma warning disable 612,618
                // todo: the handle has to be received in a save manner
                var dangerousGetHandle = stream.Handle;
#pragma warning restore 612,618

                MiniDumpWriteDump(
                    process.Handle, 
                    process.Id, 
                    dangerousGetHandle,
                    (int)(MiniDumpType.MiniDumpWithFullMemory | MiniDumpType.MiniDumpWithHandleData) /* use 0x00000006 */, 
                    IntPtr.Zero, 
                    IntPtr.Zero, 
                    IntPtr.Zero);
            }
        }
    }
}