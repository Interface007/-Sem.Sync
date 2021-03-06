﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionHandler.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   <para>
//   Public handler for exceptions. Also implements the <see cref="IExceptionWriter" /> interface and uses
//   an instance of its own class as an ExceptionWriter by default.
//   </para>
//   <para>
//   The method <see cref="SendPending" /> sends all locally logged exceptions encryped (using <see cref="SimpleCrypto" />)
//   to a WCF server configured in the app.config
//   </para>
//   <remarks>
//   Attention: encryption does not provide privacy or security! Currently there is no check for the
//   authenticity of the WCF server. While the encryption is strong (2048 Bit RSA), there is no prevention of a
//   man in the middle attack - no certificates have been used.
//   </remarks>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.ServiceModel;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    using Sem.GenericHelpers.Entities;
    using Sem.GenericHelpers.ExceptionService;
    using Sem.GenericHelpers.Interfaces;

    /// <summary>
    /// <para>
    /// Public handler for exceptions. Also implements the <see cref="IExceptionWriter"/> interface and uses
    ///     an instance of its own class as an ExceptionWriter by default.
    /// </para>
    /// <para>
    /// The method <see cref="SendPending"/> sends all locally logged exceptions encryped (using <see cref="SimpleCrypto"/>) 
    ///     to a WCF server configured in the app.config
    /// </para>
    /// <remarks>
    /// Attention: encryption does not provide privacy or security! Currently there is no check for the 
    ///     authenticity of the WCF server. While the encryption is strong (2048 Bit RSA), there is no prevention of a
    ///     man in the middle attack - no certificates have been used.
    ///   </remarks>
    /// </summary>
    public class ExceptionHandler : IExceptionWriter
    {
        #region Constants and Fields

        /// <summary>
        ///   The dafault exception handler (using the file system to log exceptions).
        /// </summary>
        private static readonly ExceptionHandler DefaultHandler;

        /// <summary>
        /// Cache for context related values. This cache will only be used in debug builds. In release builds the 
        /// methods for accessing it will be removed via the [Conditional("DEBUG")] attribute.
        /// </summary>
        private static readonly List<KeyValuePair<string, object>> ContextCache = new List<KeyValuePair<string, object>>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes static members of the <see cref = "ExceptionHandler" /> class. <see cref = "ExceptionWriter" /> will
        ///   be initialized with an instance of this class in order to write the exceptions to the path
        ///   "[SpecialFolder.ApplicationData]\Sem.GenericHelpers\\Exceptions". Use the method <see cref = "Clean" /> in order
        ///   to clean up the folder on start of the application - make sure the exceptions have been submitted to
        ///   exceptions@svenerikmatzen.info in order to be reproduced and fixed.
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

        #endregion

        #region Properties

        /// <summary>
        ///   Gets a list of writers that should get the information about exceptions. If one of the
        ///   writers does throw an exception, it will be removed from the list and the exception will be reported
        ///   to the remaining exception writers.
        /// </summary>
        public static List<IExceptionWriter> ExceptionWriter { get; private set; }

        /// <summary>
        ///   Gets or sets the user interface implementation.
        /// </summary>
        public static IUiInteraction UserInterface { get; set; }

        /// <summary>
        ///   Gets or sets the Destination path for saving  exception information.
        /// </summary>
        public string Destination { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Handels exception by assembling an xml string with exception information, writing this string using 
        ///   the object inside the <see cref="ExceptionWriter"/> (list of <see cref="IExceptionWriter"/>) and returns the string.
        ///   <see cref="ProcessAbortException"/> will not be handled - an empty string will be returned. Exception writers that
        ///   throw exceptions while writing will be removed from the <see cref="ExceptionWriter"/> list.
        /// </summary>
        /// <param name="ex">
        /// The exception to be handled. 
        /// </param>
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

            ProcessModule mainModule;
            using (var currentProcess = Process.GetCurrentProcess())
            {
                // we might want to write a dump file (wich is huge!)
                if (ConfigurationManager.AppSettings["WriteDumpFile"] == "true")
                {
                    var outputFileName = Path.Combine(DefaultHandler.Destination, string.Format("{0:yyyy-MM-dd--HH-mm-ss}.dmp", DateTime.Now));
                    DumpWriter.CreateMiniDump(currentProcess, outputFileName);
                }

                mainModule = currentProcess.MainModule;
                var mainModuleName = mainModule.FileName;

                var logEntry = new XElement(
                    "Exception",
                    new XElement(
                        "GenericInfo",
                        new XElement("Timestamp", DateTime.Now),
                        new XElement("ExecutingMainModule", mainModuleName),
                        new XElement("ContextCache", ContextCache)),
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
        }

        /// <summary>
        /// Sends all pending exception information to the server. See <see cref="SendFile"/> for 
        /// details about sending the information.
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
                    Properties.Resources.ThereAreSomeInformationFiles,
                    Properties.Resources.ThereAreSomeInformationFilesTitle))
            {
                files.ForEach(SendFile);
            }
        }

        /// <summary>
        /// Simply suppresses all exceptions of type <typeparamref name="TException"/> if the function <paramref name="check"/> returns true.
        ///   The following code sample does show how to suppress all <see cref="FormatException"/> with a message property containing
        ///   the character "x".
        ///   <code language="c#">
        /// var result = ExceptionHandler.Suppress(
        ///     () =&gt; File.WriteAllText(x, SimpleCrypto.DecryptString(File.ReadAllText(x), key)),
        ///     (FormatException ex) =&gt; ex.Message.Contains("x")));
        /// </code>
        /// </summary>
        /// <typeparam name="TException"> the exception to suppress   </typeparam>
        /// <typeparam name="TResult"> The return type of the function.  </typeparam>
        /// <param name="code"> The code to be executed.   </param>
        /// <param name="check"> The check function - when this expression is true, the exception will be suppressed.  </param>
        /// <returns> The result of the code provided, default of <typeparamref name="TResult"/>. </returns>
        public static TResult Suppress<TException, TResult>(Func<TResult> code, Func<TException, bool> check)
            where TException : Exception
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
        ///   The following code sample does show how to suppress all <see cref="FormatException"/> with a message property containing
        ///   the character "x".
        ///   <code language="c#">
        /// ExceptionHandler.Suppress(
        ///     () =&gt; File.WriteAllText(x, SimpleCrypto.DecryptString(File.ReadAllText(x), key)),
        ///     (FormatException ex) =&gt; ex.Message.Contains("x")));
        /// </code>
        /// </summary>
        /// <typeparam name="T"> the exception to suppress </typeparam>
        /// <param name="code"> The code to be executed. </param>
        /// <param name="check"> The check function - when this expression is true, the exception will be suppressed. </param>
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
        /// Simply suppresses all exceptions of type <typeparamref name="T"/> for the code specified
        ///   by the action <paramref name="code"/>.
        ///   The following code sample does show how to suppress all <see cref="FormatException"/>. See
        ///   <see cref="Suppress{T}(System.Action,System.Func{T,bool})"/> in order to check not only
        ///   the type of the exception, but also add a condition for the exception.
        ///   <code language="c#">
        /// ExceptionHandler.Suppress(
        ///     () =&gt;
        ///     {
        ///     File.WriteAllText(x, SimpleCrypto.DecryptString(File.ReadAllText(x), key));
        ///     });
        /// </code>
        /// </summary>
        /// <typeparam name="T"> The exception to suppress  </typeparam>
        /// <param name="code"> The code to be executed. </param>
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
        /// Stores context specific information for the next coming exception.
        /// This method will be excluded in release builds.
        /// </summary>
        /// <param name="contextValueName">The name of the value to store</param>
        /// <param name="contextValue">The text to store.</param>
        [Conditional("DEBUG")]
        public static void WriteContextEntry(string contextValueName, object contextValue)
        {
            ContextCache.Add(new KeyValuePair<string, object>(contextValueName, contextValue));
        }

        /// <summary>
        /// Clears the context value cache.
        /// This method will be excluded in release builds.
        /// </summary>
        [Conditional("DEBUG")]
        public static void ClearContextCache()
        {
            ContextCache.Clear();
        }

        #endregion

        #region Implemented Interfaces

        #region IExceptionWriter

        /// <summary>
        /// Delete all collected exception data. This will delete all files matching the pattern for 
        ///   exception logging files ("*...*...*.xml").
        /// </summary>
        public void Clean()
        {
            Tools.EnsurePathExist(this.Destination);
            Directory.GetFiles(this.Destination, "*...*...*.xml").ForEach(File.Delete);
        }

        /// <summary>
        /// Reads all collected exception information. 
        ///   Returns NULL in case of no available information.
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
        /// Writes exception information to the files system (<see cref="Destination"/>). Does not throw any exceptions
        ///   but may write exception details into the console window.
        /// </summary>
        /// <param name="information">
        /// The information. 
        /// </param>
        public void Write(XElement information)
        {
            try
            {
                var mainModule = System.Diagnostics.Process.GetCurrentProcess().MainModule;
                var mainModuleName = mainModule.FileName;
                var logName = string.Format(
                    CultureInfo.CurrentCulture,
                    "{1}...{0:yyyy-MM-dd-hh-mm-ss}...{2}.xml",
                    DateTime.Now,
                    mainModuleName,
                    Guid.NewGuid());
                var fileName = Path.GetFileName(logName);
                if (fileName != null)
                {
                    var pathToPersist = Path.Combine(this.Destination, fileName);
                    Tools.EnsurePathExist(this.Destination);
                    File.WriteAllText(pathToPersist, information.ToString());
                }
#if DEBUG
                Console.WriteLine(information);
#endif
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Extracts the <see cref="TechnicalException.RelatedEntities"/> and serializes them.
        /// </summary>
        /// <param name="ex">
        /// The <see cref="TechnicalException"/> to scan for. 
        /// </param>
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
        /// Collects exception information and builds an <see cref="XElement"/> from that information. This information 
        ///   is built recursive, so all inner exceptions will be serialized.
        /// </summary>
        /// <param name="ex">
        /// The exception to scan. 
        /// </param>
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
        /// Sends an exception file if the configuration does permit this to the configured WCF service.
        ///   The public portion of the encryption key (2048 Bit RSA) is read from the service. There is 
        ///   currently no check if the encryption key is authentic - if someone did manipulate your 
        ///   configuration, she/he can implement the same type of service and send a different key, so 
        ///   that she/he can decrypt the information.
        ///   <para>
        /// If you want to, you can simply generate a new key pair, compile the service (which is
        ///     part of this solution) and provide such an exception service by yourself.
        /// </para>
        /// <remarks>
        /// The file is encrypted using 2048-bit RSA key. This sounds strong, but was implemented by a
        ///     non-crypto-programmer: me. So don't be surprised if I did a very silly line of code that totally
        ///     broke my encryption class - but inform me if I did so (and also how to fix it).
        ///   </remarks>
        /// </summary>
        /// <param name="fileName">
        /// the file to be sent
        /// </param>
        private static void SendFile(string fileName)
        {
            var content = File.ReadAllText(fileName);

            // ask the user if it's ok to send exception information
            if (UserInterface == null || !UserInterface.AskForConfirmSendingException(content))
            {
                return;
            }

            // create a new service client
            var sender = new ExceptionServiceClient();

            // todo: encryption - in this case we should use public key encryption
            // send the information and currently don't care about rejected messages
            try
            {
                var key = sender.GetEncryptionKey();
                content = SimpleCrypto.EncryptString(content, key);
                sender.WriteExceptionData(content);
            }
            catch (ProtocolException ex)
            {
                var x = ex.InnerException as System.Net.WebException;
                var r = x == null ? null : x.Response as System.Net.HttpWebResponse;
                if (r != null && r.StatusCode == System.Net.HttpStatusCode.ProxyAuthenticationRequired)
                {
                    var proxyCredentials = new Credentials();
                    var wp = System.Net.WebRequest.DefaultWebProxy;

                    var logonCredentialRequest = new LogonCredentialRequest(
                        proxyCredentials,
                        string.Format(
                            CultureInfo.CurrentCulture,
                            "The proxy server needs your credentials to receive content from {0}.",
                            sender.InnerChannel.RemoteAddress.Uri),
                        sender.InnerChannel.RemoteAddress.Uri.ToString());

                    if (UserInterface.AskForLogOnCredentials(logonCredentialRequest))
                    {
                        if (string.IsNullOrEmpty(proxyCredentials.LogOnDomain))
                        {
                            wp.Credentials = new NetworkCredential(
                                proxyCredentials.LogOnUserId, proxyCredentials.LogOnPassword);
                        }
                        else
                        {
                            wp.Credentials = new NetworkCredential(
                                proxyCredentials.LogOnUserId,
                                proxyCredentials.LogOnPassword,
                                proxyCredentials.LogOnDomain);
                        }

                        sender.WriteExceptionData(content);

                        logonCredentialRequest.SaveCredentials();
                    }

                    return;
                }

                throw;
            }
        }

        /// <summary>
        /// Serializes the object into an XElement
        /// </summary>
        /// <param name="obj">
        /// The object to be serialized.  
        /// </param>
        /// <param name="name">
        /// The variable name of the object in the exception context. 
        /// </param>
        /// <returns>
        /// an XElement containing the data from the object  
        /// </returns>
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

        #endregion
    }
}