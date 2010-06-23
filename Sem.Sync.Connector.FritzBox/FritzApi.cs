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
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using System.Text.RegularExpressions;

    using Sem.GenericHelpers;
    using Sem.GenericHelpers.Exceptions;
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

                try
                {
                    var contact = Tools.LoadFromString<Contact>(x[0].OuterXml);
                    result.Add(contact);
                }
                catch (Exception ex)
                {
                    throw new TechnicalException(
                        "Serialization problem with FritzBox-entity.",
                        ex,
                        new KeyValuePair<string, object>("serialized entity", x[0]));
                }
            }

            return result;
        }

        /// <summary>
        /// Adds all entries of a phone book to the FritzBox.
        /// This does not check for existing entries.
        /// </summary>
        /// <param name="book"> The phone book with the entries to be written. </param>
        /// <returns> A value indicating whether the write operation was successfull. </returns>
        public virtual bool SetPhoneBook(PhoneBook book)
        {
            var port = this.RequestPortFromFritzBox();
            if (port == 0)
            {
                return false;
            }

            using (var clientSocketTcp = new TcpClient())
            {
                clientSocketTcp.Connect(this.Host.Host, port);

                foreach (var entry in book)
                {
                    var phoneBookEntry = Tools.SaveToString(entry);

                    ExpectResult("0000    ", RequestInfo(clientSocketTcp, "08 00"));
                    ExpectResult("0000....", RequestInfo(clientSocketTcp, "02 00 00 00 FF FF", phoneBookEntry));
                    ExpectResult("0000    ", RequestInfo(clientSocketTcp, "09 00"));
                }
            }

            return true;
        }

        /// <summary>
        /// Deletes all entries from the phone book of the box.
        /// </summary>
        /// <returns> True if the deletion was successfully </returns>
        public virtual bool ClearPhoneBook()
        {
            var port = this.RequestPortFromFritzBox();
            if (port == 0)
            {
                return false;
            }

            using (var clientSocketTcp = new TcpClient())
            {
                clientSocketTcp.Connect(this.Host.Host, port);

                ExpectResult("0000", RequestInfo(clientSocketTcp, "08 00"));
                ExpectResult("0000", RequestInfo(clientSocketTcp, "06 00 00 00"));
                ExpectResult("0000", RequestInfo(clientSocketTcp, "09 00"));
            }

            return true;
        }

        /// <summary>
        /// Checks whether the hex encoded byte array matches to the regular expression in <paramref name="matchExpression"/>.
        /// </summary>
        /// <param name="matchExpression"> The regular expression to test. </param>
        /// <param name="value"> The value to be checked. </param>
        /// <exception cref="Exception">In case of a byte array that does notg match. </exception>
        private static void ExpectResult(string matchExpression, byte[] value)
        {
            var hexValue = string.Concat(value.Select(b => b.ToString("X2")).ToArray()).Replace(" ", string.Empty);
            if (!Regex.IsMatch(hexValue, matchExpression.Trim()))
            {
                throw new TechnicalException(
                        "Return value from FritzBox did not macht check-expression.",
                        null,
                        new KeyValuePair<string, object>("check expression", matchExpression),
                        new KeyValuePair<string, object>("value", value));
            }
        }

        /// <summary>
        /// Sends a reqeust to the FritzBox and returns the response.
        /// </summary>
        /// <param name="clientSocketTcp"> The tcp socket client to communicate with the FritzBox (must be open). </param>
        /// <param name="commandAsHex"> The numerical command encoded as a hex string. </param>
        /// <returns> The result from the FritzBox </returns>
        private static byte[] RequestInfo(TcpClient clientSocketTcp, string commandAsHex)
        {
            return RequestInfo(clientSocketTcp, commandAsHex, string.Empty);
        }

        /// <summary>
        /// Sends a reqeust to the FritzBox and returns the response.
        /// </summary>
        /// <param name="clientSocketTcp"> The tcp socket client to communicate with the FritzBox (must be open). </param>
        /// <param name="commandAsHex"> The numerical command encoded as a hex string. </param>
        /// <param name="data"> The additional data to be written. </param>
        /// <returns> The result from the FritzBox </returns>
        private static byte[] RequestInfo(TcpClient clientSocketTcp, string commandAsHex, string data)
        {
            // decode the command and add two bytes in fron of it (for the size of the block that will be sent)
            var request1 = StringToBytes("00 00 " + commandAsHex);

            // convert the data into a byte array
            var request2 = Encoding.UTF8.GetBytes(data);

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

            var response = new byte[responseLength - 2];
            for (var i = 0; i < responseLength - 2; i++)
            {
                response[i] = responseBytesRead[i + 2];
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

        /// <summary>
        /// Requests a communication port from the FritzBox.
        /// </summary>
        /// <returns> The port if the request was successfully, 0 otherwise. </returns>
        private int RequestPortFromFritzBox()
        {
            this.phonebookControl.host = this.Host.Host;
            this.phonebookControl.HTTPpassword = this.UserPassword;
            var phoneBookResult = this.phonebookControl.OpenPort() as Hashtable;
            return
                phoneBookResult != null
                ? int.Parse((string)phoneBookResult["Port"]) :
                0;
        }
    }
}