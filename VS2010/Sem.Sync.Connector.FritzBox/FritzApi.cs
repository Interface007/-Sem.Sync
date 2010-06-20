// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FritzApi.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the FritzApi type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.FritzBox
{
    using System;
    using System.Collections;
    using System.Net.Sockets;
    using System.Text;

    using Sem.GenericHelpers;
    using Sem.Sync.Connector.FritzBox.Entities;

    /// <summary>
    /// Wrapper for the <see cref="FritzBoxNET"/> classes.
    /// </summary>
    public class FritzApi
    {
        /// <summary>
        /// Control connection for FritzBox - this will query the connection parameters for the TCP client
        /// </summary>
        private readonly FritzBoxNET.UPnP.Phonebook phonebookControl = new FritzBoxNET.UPnP.Phonebook();

        /// <summary>
        /// Data connection for FritzBox - this will get the data from the box
        /// </summary>
        private readonly FritzBoxNET.Network.Phonebook phonebookAccess = new FritzBoxNET.Network.Phonebook();

        /// <summary>
        /// Gets or sets the host setting for the fritz box.
        /// </summary>
        public Uri Host { get; set; }

        /// <summary>
        /// Gets or sets the fritz box password.
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        /// Gets the phone book entries of phonebook 0
        /// </summary>
        /// <returns> a list of deserialized entries </returns>
        public PhoneBook GetPhoneBook()
        {
            var result = new PhoneBook();

            this.phonebookControl.host = this.Host.Host;
            this.phonebookControl.HTTPpassword = this.UserPassword;
            var phoneBookResult = this.phonebookControl.OpenPort() as Hashtable;

            this.phonebookAccess.host = this.Host.Host;
            if (this.phonebookAccess.port == "0")
            {
                if (phoneBookResult != null)
                {
                    this.phonebookAccess.port = (string)phoneBookResult["Port"];
                }
            }

            var phonebookEntryCount = (int)this.phonebookAccess.GetEntryCount();

            for (var i = 0; i < phonebookEntryCount; i++)
            {
                var x = this.phonebookAccess.GetEntry(i) as System.Xml.XmlNodeList;
                if (x == null || x.Count < 1)
                {
                    continue;
                }

                var contact = Tools.LoadFromString<Contact>(x[0].OuterXml);
                result.Add(contact);
            }

            return result;
        }

        public bool SetPhoneBook(PhoneBook book)
        {
            this.phonebookControl.host = this.Host.Host;
            this.phonebookControl.HTTPpassword = this.UserPassword;
            var phoneBookResult = this.phonebookControl.OpenPort() as Hashtable;
            if (phoneBookResult == null)
            {
                return false;
            }

            var port = int.Parse((string)phoneBookResult["Port"]);
            using (var clientSocketTcp = new TcpClient())
            {
                clientSocketTcp.Connect(this.Host.Host, port);

                foreach (var entry in book)
                {
                    var phoneBookEntry = Tools.SaveToString(entry);

                    var responseString1 = RequestInfo(clientSocketTcp, "08 00");
                    var responseString2 = RequestInfo(clientSocketTcp, "02 00 00 00 FF FF", phoneBookEntry);
                    var responseString3 = RequestInfo(clientSocketTcp, "09 00");
                }
            }

            return true;
        }

        private static string RequestInfoString(TcpClient clientSocketTcp, string strInput)
        {
            return Encoding.ASCII.GetString(RequestInfo(clientSocketTcp, strInput, string.Empty));
        }

        private static string RequestInfoString(TcpClient clientSocketTcp, string strInput, string phoneBookEntry)
        {
            return Encoding.ASCII.GetString(RequestInfo(clientSocketTcp, strInput, phoneBookEntry));
        }

        private static byte[] RequestInfo(TcpClient clientSocketTcp, string strInput)
        {
            return RequestInfo(clientSocketTcp, strInput, string.Empty);
        }

        private static byte[] RequestInfo(TcpClient clientSocketTcp, string strInput, string phoneBookEntry)
        {
            // decode the command and add two bytes in fron of it (for the size of the block that will be sent)
            var request1 = StringToBytes("00 00 " + strInput);
            
            // convert the data into a byte array
            var request2 = Encoding.UTF8.GetBytes(phoneBookEntry);

            // determine full length
            var length = request1.Length + request2.Length;
            
            // combine both data
            var request = new byte[length];
            request1.CopyTo(request, 0);
            request2.CopyTo(request, request1.Length);

            // set the request length into the first two bytes
            request[0] = BitConverter.GetBytes(length)[0];
            request[1] = BitConverter.GetBytes(length)[1];
            
            // get the stream to communicate with
            var remoteStream = clientSocketTcp.GetStream();
            
            // Write request and flush it
            remoteStream.Write(request, 0, request.Length);
            remoteStream.Flush();

            // Read response
            var responseBytesRead = new byte[clientSocketTcp.ReceiveBufferSize];
            var responseLength = remoteStream.Read(responseBytesRead, 0, clientSocketTcp.ReceiveBufferSize);

            var response = new byte[responseLength];
            for (int i = 0; i < responseLength; i++)
            {
                response[i] = responseBytesRead[i];
            }

            return response;
        }

        /// <summary>
        /// Restores a byte array from a hex encoded string (ignores space in encoded string)
        /// </summary>
        /// <param name="strInput"> The hex encoded input string. </param>
        /// <returns> the byte array </returns>
        private static byte[] StringToBytes(string strInput)
        {
            strInput = strInput.Replace(" ", string.Empty);

            // allocate byte array based on half of string length
            var numBytes = strInput.Length / 2;
            var bytes = new byte[numBytes];

            // loop through the string - 2 bytes at a time converting it to decimal equivalent and store in byte array
            // x variable used to hold byte array element position
            for (var x = 0; x < numBytes; ++x)
            {
                bytes[x] = Convert.ToByte(strInput.Substring(x * 2, 2), 16);
            }

            // return the finished byte array of decimal values
            return bytes;
        }
    }
}