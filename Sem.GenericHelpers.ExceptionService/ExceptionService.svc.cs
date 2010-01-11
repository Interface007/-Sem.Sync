namespace Sem.GenericHelpers.ExceptionService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using System.Text;
    using System.IO;
    using System.Configuration;

    public class ExceptionService : IExceptionService
    {
        private static string destinationFolder = ConfigurationManager.AppSettings["ExceptionDestinationFolder"];

        public bool WriteExceptionData(string exceptionData)
        {
            // don't accept more than 10kbytes per message
            if (exceptionData.Length > 10240)
            {
                return false;
            }

            // don't accept more than 1 message per minute = 14 MByte per day maximum
            var fileNamePattern = string.Format("{0:yyyy-MM-dd-HH-mm}*.*", DateTime.Now);
            if (Directory.GetFiles(destinationFolder, fileNamePattern, SearchOption.AllDirectories).Length > 0)
            {
                return false;
            }

            // don't accept more than 100 files (who should handle it?)
            if (Directory.GetFiles(destinationFolder).Length > 100)
            {
                return false;
            }

            // the file name does contain a fresh GUID, so it is unique.
            var fileName = string.Format("{0:yyyy-MM-dd-HH-mm-ss}-{1}.xml", DateTime.Now, Guid.NewGuid().ToString("N"));
            File.WriteAllText(Path.Combine(destinationFolder, fileName), exceptionData);

            return true;
        }
    }
}
