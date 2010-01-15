// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionHandler.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ExceptionHandler type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Sem.GenericHelpers.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    using Interfaces;

    /// <summary>
    /// Public handler for exceptions. Also implements the <see cref="IExceptionWriter"/> interface and uses
    /// an instance of its own class as an ExceptionWriter by default.
    /// </summary>
    public class ExceptionHandler : IExceptionWriter
    {
        /// <summary>
        /// The dafault exception handler (using the file system to log exceptions).
        /// </summary>
        private static readonly ExceptionHandler DefaultHandler;

        /// <summary>
        /// Initializes static members of the <see cref="ExceptionHandler"/> class. <see cref="ExceptionWriter"/> will
        /// be initialized with an instance of this class in order to write the exceptions to the path
        /// "[SpecialFolder.ApplicationData]\Sem.GenericHelpers\\Exceptions". Use the method <see cref="Clean"/> in order
        /// to clean up the folder on start of the application - make sure the exceptions have been submitted to
        /// exceptions@svenerikmatzen.info in order to be reproduced and fixed.
        /// </summary>
        static ExceptionHandler()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            DefaultHandler = new ExceptionHandler
                {
                    Destination = Path.Combine(appDataPath, "Sem.GenericHelpers\\Exceptions") 
                };

            ExceptionWriter = new List<IExceptionWriter> { DefaultHandler };
        }

        /// <summary>
        /// Gets a list of writers that should get the information about exceptions. If one of the
        /// writers does throw an exception, it will be removed from the list and the exception will be reported
        /// to the remaining exception writers.
        /// </summary>
        public static List<IExceptionWriter> ExceptionWriter { get; private set; }

        /// <summary>
        /// Gets or sets the user interface implementation.
        /// </summary>
        public static IUiInteraction UserInterface { get; set; }

        /// <summary>
        /// Gets or sets the Destination path for saving  exception information.
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Handels exception by assembling an xml string with exception information, writing this string using 
        /// the object inside the <see cref="ExceptionWriter"/> (list of <see cref="IExceptionWriter"/>) and returns the string.
        /// <see cref="ProcessAbortException"/> will not be handled - an empty string will be returned. Exception writers that
        /// throw exceptions while writing will be removed from the <see cref="ExceptionWriter"/> list.
        /// </summary>
        /// <param name="ex"> The exception to be handled. </param>
        /// <returns>
        /// a string containing the xml information written to the <see cref="ExceptionWriter"/> list.
        /// </returns>
        [SuppressMessage("Microsoft.StyleCop.CSharp.ReadabilityRules", "SA1116:SplitParametersMustStartOnLineAfterDeclaration", Justification = "the XElement class perfectly supports floating interfaces")]
        [SuppressMessage("Microsoft.StyleCop.CSharp.ReadabilityRules", "SA1118:ParametersMustNotSpanMultipleLines", Justification = "the XElement class perfectly supports floating interfaces")]
        public static string HandleException(Exception ex)
        {
            // do not write or process abort exceptions
            if ((ex as ProcessAbortException) != null)
            {
                return string.Empty;
            }

            var mainModule = System.Diagnostics.Process.GetCurrentProcess().MainModule;
            var mainModuleName = mainModule == null ? "(undefined)" : mainModule.FileName;
            
            var logEntry = 
                new XElement("Exception",
                    new XElement("GenericInfo",
                        new XElement("Timestamp", DateTime.Now),
                        new XElement("ExecutingMainModule", mainModuleName)),
                    ScanException(ex));

            var exceptionText = logEntry.ToString(SaveOptions.None);

            foreach (var exceptionWriter in ExceptionWriter)
            {
                try
                {
                    exceptionWriter.Write(logEntry);
                }
                catch (Exception writerException)
                {
                    ExceptionWriter.Remove(exceptionWriter);
                    HandleException(
                        new TechnicalException(
                            "Exception while writing exception. The responsible writer will be removed.",
                            writerException,
                            new KeyValuePair<string, object>("responsible writer", exceptionWriter)));
                }
            }
            
            return exceptionText;
        }

        /// <summary>
        /// Simply suppresses all exceptions of type <typeparamref name="TException"/> if the function <paramref name="check"/> returns true.
        /// </summary>
        /// <typeparam name="TException"> the exception to suppress  </typeparam>
        /// <typeparam name="TResult"> The return type of the function. </typeparam>
        /// <param name="code"> The code to be executed.  </param>
        /// <param name="check"> The check function - when this expression is true, the exception will be suppressed. </param>
        /// <returns> The result of the code provided, default of <typeparamref name="TResult"/>. </returns>
        public static TResult Suppress<TException, TResult>(Func<TResult> code, Func<TException, bool> check) where TException : Exception
        {
            try
            {
                return code.Invoke();
            }
            catch (TException ex)
            {
                if (!check(ex))
                {
                    throw;
                }
            }

            return default(TResult);
        }

        /// <summary>
        /// Simply suppresses all exceptions of type <typeparamref name="T"/> if the function <paramref name="check"/> returns true.
        /// </summary>
        /// <typeparam name="T"> the exception to suppress </typeparam>
        /// <param name="code"> The code to be executed. </param>
        /// <param name="check"> The check function - when this expression is true, the exception will be suppressed.</param>
        public static void Suppress<T>(Action code, Func<T, bool> check) where T : Exception
        {
            try
            {
                code.Invoke();
            }
            catch (T ex)
            {
                if (!check(ex))
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Simply suppresses all exceptions of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"> the exception to suppress </typeparam>
        /// <param name="code"> The code to be executed.   </param>
        public static void Suppress<T>(Action code) where T : Exception
        {
            try
            {
                code.Invoke();
            }
            catch (T)
            {
            }
        }

        /// <summary>
        /// Sends all pending exception information to the server.
        /// </summary>
        public static void SendPending()
        {
            var value = ConfigurationManager.AppSettings["SendExceptionDetails"];
            bool doSend;
            if (!bool.TryParse(value, out doSend) || !doSend)
            {
                return;
            }

            Tools.EnsurePathExist(DefaultHandler.Destination);
            var files = Directory.GetFiles(DefaultHandler.Destination, "*...*...*.xml");

            if (files.Length > 0 &&
                UserInterface.AskForConfirm(
                    Sem.GenericHelpers.Properties.Resources.ThereAreSomeInformationFiles,
                    Sem.GenericHelpers.Properties.Resources.ThereAreSomeInformationFilesTitle))
            {
                files.ForEach(SendFile);
            }
        }

        /// <summary>
        /// Writes exception information to the files system (<see cref="Destination"/>). Does not throw any exceptions
        /// but may write exception details into the console window.
        /// </summary>
        /// <param name="information">
        /// The information.
        /// </param>
        public void Write(XElement information)
        {
            try
            {
                var mainModule = System.Diagnostics.Process.GetCurrentProcess().MainModule;
                var mainModuleName = mainModule == null ? "(undefined)" : mainModule.FileName;
                var logName = string.Format(CultureInfo.CurrentCulture, "{1}...{0:yyyy-MM-dd-hh-mm-ss}...{2}.xml", DateTime.Now, mainModuleName, Guid.NewGuid());
                var pathToPersist = Path.Combine(this.Destination, Path.GetFileName(logName));
                Tools.EnsurePathExist(this.Destination);
                File.WriteAllText(pathToPersist, information.ToString());
#if DEBUG
                Console.WriteLine(information);
#endif
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Delete all collected exception data
        /// </summary>
        public void Clean()
        {
            Tools.EnsurePathExist(this.Destination);
            Directory.GetFiles(this.Destination, "*...*...*.xml").ForEach(File.Delete);
        }

        /// <summary>
        /// Reads all collected exception information. 
        /// Returns NULL in case of no available information.
        /// </summary>
        /// <returns>
        /// An <see cref="XElement"/> containing all exception information that should be read.
        /// </returns>
        public XElement Read()
        {
            var result = new XElement("Exceptions");
            Directory.GetFiles(this.Destination, "*...*...*.xml").ForEach(file => result.Add(XElement.Load(file)));
            return result;
        }

        /// <summary>
        /// Collects exception information and builds an <see cref="XElement"/> from that information. This information 
        /// is built recursive, so all inner exceptions will be serialized.
        /// </summary>
        /// <param name="ex"> The exception to scan. </param>
        /// <returns>
        /// The information as an <see cref="XElement"/>.
        /// </returns>
        private static XElement ScanException(Exception ex)
        {
            if (ex == null)
            {
                return null;
            }

            return new XElement(
                "Exception",
                new XElement("SpecificInformation", ex),
                ExtractEntities(ex as TechnicalException),
                ScanException(ex.InnerException));
        }

        /// <summary>
        /// Extracts the <see cref="TechnicalException.RelatedEntities"/> and serializes them.
        /// </summary>
        /// <param name="ex"> The <see cref="TechnicalException"/> to scan for. </param>
        /// <returns>
        /// An <see cref="XElement"/> containing the serialized ralated entities.
        /// </returns>
        private static XElement ExtractEntities(TechnicalException ex)
        {
            if (ex == null)
            {
                return null;
            }
            
            var result = new XElement("RelatedEntities");
            if (ex.RelatedEntities != null)
            {
                foreach (var entity in ex.RelatedEntities)
                {
                    try
                    {
                        result.Add(SerializeToXElement(entity.Value, entity.Key));
                    }
                    catch (Exception)
                    {
                        result.Add(new XElement(entity.Key, entity.Value));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Serializes the object into an XElement
        /// </summary>
        /// <param name="obj"> The object to be serialized.  </param>
        /// <param name="name"> The variable name of the object in the exception context. </param>
        /// <returns> an XElement containing the data from the object  </returns>
        private static XElement SerializeToXElement(object obj, string name)
        {
            var serializer = new XmlSerializer(obj.GetType());
            
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    serializer.Serialize(writer, obj);

                    stream.Position = 0;
                    using (var reader = new XmlTextReader(stream))
                    {
                        var xml = XElement.Load(reader, LoadOptions.None);
                        xml.Add(new XAttribute("name", name));
                        return xml;
                    }
                }
            }
        }
        
        /// <summary>
        /// Sends an exception file if the configuration does permit this to the configured WCF service.
        /// The public portion of the encryption key (2048 Bit RSA) is read from the service. There is 
        /// currently no check if the encryption key is authentic - if someone did manipulate your 
        /// configuration, she/he can implement the same type of service and send a different key, so 
        /// that she/he can decrypt the information.
        /// <para>If you want to, you can simply generate a new key pair, compile the service (which is
        /// part of this solution) and provide such an exception service by yourself.</para>
        /// <remarks>The file is encrypted using 2048-bit RSA key. This sounds strong, but was implemented by a
        /// non-crypto-programmer: me. So don't be surprised if I did a very silly line of code that totally
        /// broke my encryption class - but inform me if I did so (and also how to fix it).</remarks>
        /// </summary>
        /// <param name="fileName">the file to be sent</param>
        private static void SendFile(string fileName)
        {
            var content = File.ReadAllText(fileName);

            // ask the user if it's ok to send exception information
            if (UserInterface == null || !UserInterface.AskForConfirmSendingException(content))
            {
                return;
            }
            
            // create a new service client
            var sender = new ExceptionService.ExceptionServiceClient();
            
            // todo: encryption - in this case we should use public key encryption
            // send the information and currently don't care about rejected messages
            var key = sender.GetEncryptionKey();
            content = SimpleCrypto.EncryptString(content, key);
            sender.WriteExceptionData(content);
        }
    }
}