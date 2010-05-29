// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenericClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This class is the client class for handling elements. This class leaks some of the contact
//   related features of "ContactClient", but provides the ability to handle other types that do
//   inherit from StdElement
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Ftp
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;

    using Sem.GenericHelpers;
    using Sem.GenericHelpers.Exceptions;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.DetailData;

    #endregion usings

    /// <summary>
    /// This class is the client class for handling elements. This class leaks some of the contact 
    ///   related features of "ContactClient", but provides the ability to handle other types that do 
    ///   inherit from StdElement
    /// </summary>
    [ClientStoragePathDescription(
        Mandatory = true,
        Default = @"ftp://ftp.myserver.com/Elements.xml",
        ReferenceType = ClientPathType.Undefined)]
    [ConnectorDescription(
        DisplayName = "FTP generic client",
        NeedsCredentials = true,
        NeedsCredentialsDomain = false,
        CanReadCalendarEntries = true,
        CanWriteCalendarEntries = true)]
    public class GenericClient : StdClient
    {
        /// <summary>
        /// The length of the transfer buffer
        /// </summary>
        private const int BufferLength = 2048;

        #region Properties

        /// <summary>
        ///   Gets the user friendly name of the connector
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "FTP Generic Connector - does not provide type specific features";
            }
        }

        #endregion

        #region Implemented Interfaces

        #endregion

        #region Methods

        /// <summary>
        /// Overrides the method to read the full list of data.
        /// </summary>
        /// <param name="clientFolderName">
        /// the full name including path of the file that does contain the contacts.
        /// </param>
        /// <param name="result">
        /// A list of StdElements that will get the new imported entries.
        /// </param>
        /// <returns>
        /// The list with the added contacts
        /// </returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            // Get the object used to communicate with the server.
            var request = this.GetRequest(clientFolderName);
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            // This example assumes the FTP site uses anonymous logon.
            using (var stream = new MemoryStream())
            {
                Stream responseStream;
                var fileString = string.Empty;

                // we suppress file not found exceptions here
                ExceptionHandler.Suppress<WebException>(
                    () =>
                    {
                        responseStream = request.GetResponse().GetResponseStream();
                        responseStream.CopyTo(stream);
                        stream.Position = 0;
                        fileString = Encoding.UTF8.GetString(stream.ToArray());
                    },
                    ex => ex.Message.Contains("(550) File unavailable"));

                return Tools.LoadFromString<List<StdElement>>(fileString) ?? result;
            }
        }

        /// <summary>
        /// Overrides the method to write the full list of data.
        /// </summary>
        /// <param name="elements">
        /// The elements to be exported. 
        /// </param>
        /// <param name="clientFolderName">
        /// the full name including path of the file that will get the contacts while exporting data.
        /// </param>
        /// <param name="skipIfExisting">
        /// this value is not used in this client.
        /// </param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            this.DeleteServerFile(clientFolderName);

            var state = new FtpState();
            var request = this.GetRequest(clientFolderName);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // Store the request in the object that we pass 
            // into the asynchronous operations.
            state.Request = request;
            state.Content = Tools.SaveToString(elements, true);

            // Get the event to wait on.
            var waitObject = state.OperationComplete;

            // Asynchronously get the stream for the file contents.
            request.BeginGetRequestStream(this.EndGetStreamCallback, state);

            // Block the current thread until all operations are complete.
            waitObject.WaitOne();

            // The operations either completed or threw an exception.
            if (state.OperationException != null)
            {
                throw state.OperationException;
            }

            this.LogProcessingEvent("The operation completed - {0}", state.StatusDescription);
        }

        private void EndGetStreamCallback(IAsyncResult ar)
        {
            var state = (FtpState)ar.AsyncState;

            Stream requestStream;

            // End the asynchronous call to get the request stream.
            try
            {
                requestStream = state.Request.EndGetRequestStream(ar);

                // Copy the file contents to the request stream.
                var buffer = new byte[BufferLength];
                var count = 0;
                int readBytes;
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(state.Content));
                var lastProgressEvent = DateTime.Now;
                do
                {
                    readBytes = stream.Read(buffer, 0, BufferLength);
                    requestStream.Write(buffer, 0, readBytes);
                    count += readBytes;
                    if ((DateTime.Now - lastProgressEvent).Duration().Seconds <= 2)
                    {
                        continue;
                    }

                    lastProgressEvent = DateTime.Now;
                    this.LogProcessingEvent("Writing {0} bytes to the stream.", count);
                }
                while (readBytes != 0);

                // IMPORTANT: Close the request stream before sending the request.
                requestStream.Close();

                // Asynchronously get the response to the upload request.
                state.Request.BeginGetResponse(this.EndGetResponseCallback, state);
            }
            catch (Exception e)
            {
                // Return exceptions to the main application thread.
                this.LogProcessingEvent("Could not get the request stream.");
                state.OperationException = e;
                state.OperationComplete.Set();
                return;
            }
        }

        /// <summary>
        /// The EndGetResponseCallback method completes a call to BeginGetResponse.
        /// </summary>
        /// <param name="ar">result of the async operation</param>
        private void EndGetResponseCallback(IAsyncResult ar)
        {
            var state = (FtpState)ar.AsyncState;
            FtpWebResponse response;
            try
            {
                response = (FtpWebResponse)state.Request.EndGetResponse(ar);
                response.Close();
                state.StatusDescription = response.StatusDescription;
            }
            catch (Exception ex)
            {
                // Return exceptions to the main application thread.
                this.LogException(ex, "Error getting response.");
                state.OperationException = ex;
            }
            finally
            {
                // Signal the main application thread that 
                // the operation is complete.
                state.OperationComplete.Set();
            }
        }

        private void DeleteServerFile(string clientFolderName)
        {
            var request = this.GetRequest(clientFolderName);
            request.Method = WebRequestMethods.Ftp.DeleteFile;

            ExceptionHandler.Suppress<WebException>(
                () =>
                {
                    var response = (FtpWebResponse)request.GetResponse();
                    this.LogProcessingEvent(UserStrings.MessageFileDeletedStatus, response.StatusDescription);
                    response.Close();
                },
                ex => ex.Message.Contains("(550) File unavailable"));
        }

        private FtpWebRequest GetRequest(string clientFolderName)
        {
            var serverUri = new Uri(clientFolderName);

            // The serverUri parameter should start with the "ftp://" scheme.
            if (serverUri.Scheme != Uri.UriSchemeFtp)
            {
                return null;
            }

            // setup the FTP connection
            var request = (FtpWebRequest)WebRequest.Create(serverUri);
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = true;
            request.Credentials = new NetworkCredential(this.LogOnUserId, this.LogOnPassword);
            
            return request;
        }

        #endregion
    }
}
