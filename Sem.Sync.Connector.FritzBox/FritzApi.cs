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
    using System.Xml.Linq;

    using Sem.Sync.Connector.FritzBox.Entities;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using System.IO;

    /// <summary>
    /// </summary>
    public class FritzApi
    {
        private string controlPath = "/upnp/control/phonebook";

        private string fileName = "/phonebook-scpd.xml";

        private string soapNs = "http://schemas.xmlsoap.org/soap/envelope/";
        private string userNs = "urn:schemas-any-com:service:phonebook:1";
        private string action = "OpenPort";
        
        public Uri Host { get; set; }

        public string UserName { get; set; }
        
        public string UserPassword { get; set; }

        private string schema = "urn:schemas-any-com:service:phonebook:1";

        public PhoneBook GetPhoneBook()
        {
            var result = new PhoneBook();
            var xmlCode = this.RequestBook();
            var credentials = new System.Net.NetworkCredential(this.UserName, this.UserPassword);

            var url = new Uri(this.Host, this.controlPath);
            var request = BuildRequest(credentials, url, xmlCode);
            var respons = ReadResponse(request, xmlCode);

            return result;
        }

        private object ReadResponse(WebRequest urlRequest, string sRequest)
        {
            var urlRequestStream = urlRequest.GetRequestStream();
            var urlPostBytes = Encoding.ASCII.GetBytes(sRequest);
            urlRequestStream.Write(urlPostBytes, 0, urlPostBytes.Length);
            urlRequestStream.Close();

            // Response return
            var urlResponse = urlRequest.GetResponse();

            // Read the stuff ...
            var urlReader = new StreamReader(urlResponse.GetResponseStream());

            // Make a string ...
            return urlReader.ReadToEnd();
        }

        private WebRequest BuildRequest(NetworkCredential credentials, Uri url, string requestXml)
        {
            var urlRequest = WebRequest.Create(url) as HttpWebRequest;
            urlRequest.PreAuthenticate = true;
            urlRequest.Credentials = credentials;
            urlRequest.Method = "POST";
            urlRequest.Headers.Add("SOAPACTION", "\"" + userNs + "#" + action + "\"");
            urlRequest.ContentType = @"text/xml; charset=""utf-8""";
            urlRequest.UserAgent = Assembly.GetExecutingAssembly().FullName;
            urlRequest.ContentLength = requestXml.ToString().Length;
            urlRequest.Accept = "text/xml";
            urlRequest.AllowWriteStreamBuffering = true;

            return urlRequest;
        }

        private string RequestBook()
        {
            var requestXml = new XDocument(
                new XElement("Envelope" + soapNs,
                    new XAttribute("encodingStyle" + soapNs, "http://schemas.xmlsoap.org/soap/encoding/"),
                    new XElement("Body" + soapNs,
                        new XElement(action + userNs)))
                );

            return requestXml.ToString();

            
        }
    }
}