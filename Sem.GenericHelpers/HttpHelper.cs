﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpHelper.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This class provides funktionality to get information from the web.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    using Sem.GenericHelpers.Entities;
    using Sem.GenericHelpers.Interfaces;
    using Sem.GenericHelpers.Properties;

    /// <summary>
    /// This class provides funktionality to get information from the web.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The class has been developed to work with the implementation of the currently included connectors.
    ///     If you have special needs (like better cookie-management), you need to write your own class. 
    ///     ASP.Net sites as well as other sites using hidden input fields for storing session information are
    ///     currently not supported. This is a feature that is currently under investigation.
    /// </para>
    /// <para>
    /// The functionality includes:
    ///     <list type="bullets">
    /// <item>
    /// GET and POST requests
    /// </item>
    /// <item>
    /// support for encoding post parameters using url encoding - see <see cref="EncodeForPost"/>
    /// </item>
    /// <item>
    /// getting data as text or binary - see <see cref="GetContent(string,string)"/> and <see cref="GetContentBinary(string)"/>
    /// </item>
    /// <item>
    /// optionally accepting untrusted certificates with http (to support fiddler debugging) - see <see cref="IgnoreCertificateError"/>
    /// </item>
    /// <item>
    /// using the IE cookie cache or a "private" cookie cache - see <see cref="UseIeCookies"/>
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// reading the content of an http resource can be written in one line of code:
    ///   <code>
    /// var response = new HttpHelper(baseUrl, false).GetContent(HttpUrlLogonRequest, "[NOCACHE]");
    ///   </code>
    /// In this example the second parameter "[NOCACHE]" does specify that this content should not be cached by the library. For more 
    ///   information on using the caching see <see cref="UseCache"/>.
    /// </example>
    public class HttpHelper : IHttpHelper
    {
        #region Constants and Fields

        /// <summary>
        ///   cache hint string constant to specify that this item should not be cached at all
        /// </summary>
        public const string CacheHintNoCache = "[NOCACHE]";

        /// <summary>
        ///   Gets or sets a value to determine the path to cache the content
        /// </summary>
        private static readonly string CachePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SemSyncManager\\Cache");

        /// <summary>
        ///   Regular expressions to detect redirect content
        /// </summary>
        private static readonly Regex[] RedirectExtractors = new[]
            {
                new Regex(@"<script>window.location.replace\(""(.*)""\);</script>"), 
                new Regex(@"<meta http-equiv=""refresh"" content=""0;url=(.*?)"" />")
            };

        /// <summary>
        ///   Private cookie store when not using IE cookies
        /// </summary>
        private readonly CookieContainer sessionCookies = new CookieContainer();

        /// <summary>
        ///   credentials for the proxy server
        /// </summary>
        private ICredentialAware proxyCredentials = Factory.CreateTypeInstance<Credentials>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes static members of the <see cref = "HttpHelper" /> class.
        /// </summary>
        static HttpHelper()
        {
            DefaultInstance = (HttpHelper)Factory.CreateTypeInstance(typeof(HttpHelper), string.Empty, false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHelper"/> class. 
        ///   the constructor will switch off certificate validation if IgnoreCertificateError is true
        /// </summary>
        /// <param name="baseUrl">
        /// The base Url that is added if there is no http?:// prefix inside the url. If there is a 
        ///   full qualified url, this parameter is ignored.
        /// </param>
        /// <param name="ignoreCertificateErrors">
        /// By setting this value to true, server certificate 
        ///   errors are ignored. This is particular useful in case of debugging https-connections using
        ///   fiddler, because the certificate generated by fiddler is normally not trusted.
        /// </param>
        /// <remarks>
        /// The examples have been included with the methods and properties.
        /// </remarks>
        public HttpHelper(string baseUrl, bool ignoreCertificateErrors)
        {
            this.BaseUrl = baseUrl;
            this.IgnoreCertificateError = ignoreCertificateErrors;
            this.sessionCookies = Factory.CreateTypeInstance<CookieContainer>();
            ServicePointManager.Expect100Continue = false;

            if (this.IgnoreCertificateError)
            {
                // Hack for debugging purposes to accept Fiddler certificate
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, errors) => true;
            }

            this.ContentCredentials = Factory.CreateTypeInstance<Credentials>();
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the Default Instance for requesting information from the network without any further configuration.
        /// </summary>
        public static IHttpHelper DefaultInstance { get; set; }

        /// <summary>
        ///   Gets or sets the base address for requests
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        ///   Gets or sets the credentials for the content server
        /// </summary>
        public ICredentialAware ContentCredentials { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether a certificate error in an
        ///   https connection whould be ignored.
        ///   debugging using Fiddler needs to bypass certificate approval
        ///   set this const to "true" if you want to enable fiddler-debugging
        ///   this will prevent checking for man in the middle attack
        /// </summary>
        public bool IgnoreCertificateError { get; set; }

        /// <summary>
        ///   Gets or sets the last content downloaded to extract information (can be reused for extraction).
        /// </summary>
        public string LastExtractContent { get; set; }

        /// <summary>
        ///   Gets or sets the string uniquely represented on the log on form
        /// </summary>
        public string[] LogOnFormDetectionString { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to prevent loading missing entries 
        ///   - this is only usefull for debugging purpose if this property is true, we 
        ///   will not download missing content
        /// </summary>
        public bool SkipNotCached { get; set; }

        /// <summary>
        ///   Gets or sets the object that is responsible to interact with the user
        /// </summary>
        public IUiInteraction UiDispatcher { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to activate caching of content 
        ///   - this will present refreshing the data use this for debugging purpose 
        ///   to not stress the http server
        /// </summary>
        public bool UseCache { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to use stored ie cookies instead of 
        ///   private session cookies
        /// </summary>
        public bool UseIeCookies { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// encodes the input parameter to form-url-encoded
        /// </summary>
        /// <param name="parameter"> the content to be encoded </param>
        /// <returns>the encoded string </returns>
        public static string EncodeForPost(string parameter)
        {
            return System.Web.HttpUtility.UrlEncode(parameter);
        }

        /// <summary>
        /// replaces string.format parameters in a string using the encoded values of the supplied strings
        ///   This will call EncodeForPost() for each parameter before replacing the {0}, {1}... in the url
        /// </summary>
        /// <param name="url"> the string that should get the parameters </param>
        /// <param name="values"> the parameter strings that should be encoded and inserted into the string </param>
        /// <returns> the processed data string </returns>
        /// <example>
        /// The following code will show you the preparation of logon-parameters:
        ///   <code>
        /// // prepare the post data for log on
        ///     var HttpDataLogonRequest = "op=logon&amp;dest=%2Fapp%2Fuser%3Fop%3Dhome&amp;logon_user_name={0}&amp;logon_password={1}";
        ///     var postData = HttpHelper.PreparePostData(
        ///     HttpDataLogonRequest,
        ///     this.LogOnUserId,
        ///     this.LogOnPassword);
        ///   </code>
        /// As you can see, the String.Format()-style can be used to specify parameters for the string insert.
        /// </example>
        public static string PreparePostData(string url, params string[] values)
        {
            var encodedValues = new string[values.Length];

            for (var i = 0; i < values.Length; i++)
            {
                encodedValues[i] = EncodeForPost(values[i]);
            }

            return string.Format(CultureInfo.CurrentCulture, url, encodedValues);
        }

        /// <summary>
        /// replaces string.format parameters in a string using the encoded values of the supplied objects.
        ///   This will call EncodeForPost() for each parameter before replacing the {0}, {1}... in the url
        /// </summary>
        /// <param name="url"> the string that should get the parameters </param>
        /// <param name="values"> the parameter objects that should be casted to strings, encoded and inserted into the string </param>
        /// <returns> the processed data string </returns>
        public static string PreparePostData(string url, params object[] values)
        {
            var encodedValues = new string[values.Length];

            for (var i = 0; i < values.Length; i++)
            {
                encodedValues[i] = EncodeForPost(values[i].ToString());
            }

            return string.Format(CultureInfo.CurrentCulture, url, encodedValues);
        }

        /// <summary>
        /// Download content as text
        /// </summary>
        /// <param name="url"> the url to access the content </param>
        /// <returns> the text result of the request </returns>
        public string GetContent(string url)
        {
            return this.GetContent(url, string.Empty, string.Empty);
        }

        /// <summary>
        /// Download content as text
        /// </summary>
        /// <param name="url"> the url to access the content </param>
        /// <param name="name"> a name for caching - this should correspond to the url </param>
        /// <param name="referer"> the url of the referer to add </param>
        /// <returns> the text result of the request </returns>
        public string GetContent(string url, string name, string referer)
        {
            var uri = this.CreateUri(url);
            var fileName = this.CachePathName(name, uri, string.Empty);
            string result;

            if (this.ReadFromCache(fileName, out result, uri))
            {
                return result;
            }

            if (!this.SkipNotCached)
            {
                string encoding;
                using (var receiveStream = this.GetResponseStream(uri, referer, out encoding))
                {
                    result = ReadStreamToString(receiveStream, encoding);

                    var redirectUrl = string.Empty;
                    foreach (var extractor in RedirectExtractors)
                    {
                        var redirectMatch = extractor.Match(result);
                        if (redirectMatch.Groups.Count <= 1)
                        {
                            continue;
                        }

                        redirectUrl = redirectMatch.Groups[1].ToString().Replace(@"\/", "/");
                        break;
                    }

                    if (!string.IsNullOrEmpty(redirectUrl))
                    {
                        result = this.GetContent(redirectUrl, name, referer);
                    }
                }

                this.WriteToCache(fileName, result, uri);
            }

            return result;
        }

        /// <summary>
        /// Download content binary
        /// </summary>
        /// <param name="url"> the url to access the content </param>
        /// <returns> the binary result of the request without conversion </returns>
        public byte[] GetContentBinary(string url)
        {
            return this.GetContentBinary(url, string.Empty);
        }

        /// <summary>
        /// Download content binary
        /// </summary>
        /// <param name="url"> the url to access the content  </param>
        /// <param name="name"> a name for caching - this should correspond to the url  </param>
        /// <param name="referer"> The referer to be added.  </param>
        /// <returns> the binary result of the request without conversion  </returns>
        public byte[] GetContentBinary(string url, string name, string referer)
        {
            var uri = this.CreateUri(url);
            var fileName = this.CachePathName(name, uri, string.Empty);
            byte[] result;

            if (this.ReadFromCacheBinary(fileName, out result, uri))
            {
                return result;
            }

            if (!this.SkipNotCached)
            {
                string encoding;
                using (var receiveStream = this.GetResponseStream(uri, referer, out encoding))
                {
                    result = ReadStreamToByteArray(receiveStream, 32768);
                    this.WriteToCache(fileName, result, uri);
                }
            }

            return result;
        }

        /// <summary>
        /// Download content as text and extracts all strings matching a regex (the first group is returned in a list of strings)
        /// </summary>
        /// <param name="url"> the url to access the content   </param>
        /// <param name="regularExpression"> The regular Expression to extract the data.   </param>
        /// <param name="result"> The list of strings with the extracted data.  </param>
        /// <returns> the text result of the request   </returns>
        public bool GetExtract(string url, string regularExpression, out List<string> result)
        {
            return this.GetExtract(url, regularExpression, out result, string.Empty, string.Empty);
        }

        /// <summary>
        /// Download content as text and extracts all strings matching a regex (the first group is returned in a list of strings)
        /// </summary>
        /// <param name="url"> the url to access the content   </param>
        /// <param name="regularExpression"> The regular Expression to extract the data.   </param>
        /// <param name="result"> The list of strings with the extracted data.  </param>
        /// <param name="name"> a name for caching - this should correspond to the url </param>
        /// <param name="referer"> the url of the referer to add </param>
        /// <returns> the text result of the request   </returns>
        public bool GetExtract(
            string url, string regularExpression, out List<string> result, string name, string referer)
        {
            result = new List<string>();
            this.LastExtractContent = this.GetContent(url, name, referer);

            if (this.LogOnFormDetectionString.Any(item => this.LastExtractContent.Contains(item)))
            {
                return false;
            }

            var listMatches = Regex.Matches(this.LastExtractContent, regularExpression, RegexOptions.Singleline);
            foreach (Match match in listMatches)
            {
                var newItem = match.Groups[1].ToString();
                if (!result.Contains(newItem))
                {
                    result.Add(newItem);
                }
            }

            return true;
        }

        #endregion

        #region Implemented Interfaces

        #region IHttpHelper

        /// <summary>
        /// Download content as text
        /// </summary>
        /// <param name="url"> the url to access the content </param>
        /// <param name="name"> a name for caching - this should correspond to the url </param>
        /// <returns> the text result of the request </returns>
        public string GetContent(string url, string name)
        {
            return this.GetContent(url, name, string.Empty);
        }

        /// <summary>
        /// Download content binary
        /// </summary>
        /// <param name="url"> the url to access the content </param>
        /// <param name="name"> a name for caching - this should correspond to the url </param>
        /// <returns> the binary result of the request without conversion </returns>
        public byte[] GetContentBinary(string url, string name)
        {
            return this.GetContentBinary(url, name, string.Empty);
        }

        /// <summary>
        /// Download content binary
        /// </summary>
        /// <param name="url"> the url to access the content </param>
        /// <param name="name"> a name for caching - this should correspond to the url </param>
        /// <param name="postData"> the data that should be posted to the server </param>
        /// <returns> the binary result of the request without conversion </returns>
        public byte[] GetContentBinaryPost(string url, string name, string postData)
        {
            var uri = this.CreateUri(url);
            var fileName = this.CachePathName(name, uri, postData);
            byte[] result;
            if (this.ReadFromCacheBinary(fileName, out result, uri))
            {
                return result;
            }

            if (!this.SkipNotCached && !string.IsNullOrEmpty(fileName))
            {
                string encoding;
                using (var receiveStream = this.PostResponseStream(uri, postData, out encoding))
                {
                    result = ReadStreamToByteArray(receiveStream, 32768);
                }

                this.WriteToCache(fileName, result, uri);
            }

            return result;
        }

        /// <summary>
        /// Download content as text
        /// </summary>
        /// <param name="url"> the url to access the content </param>
        /// <param name="name"> a name for caching - this should correspond to the url </param>
        /// <param name="postData"> the complete data to be added (including keys and values) as one string </param>
        /// <returns> the text result of the request </returns>
        public string GetContentPost(string url, string name, string postData)
        {
            var uri = this.CreateUri(url);
            var fileName = this.CachePathName(name, uri, postData);
            string result;

            if (this.ReadFromCache(fileName, out result, uri))
            {
                return result;
            }

            if (!this.SkipNotCached)
            {
                string encoding;
                using (var receiveStream = this.PostResponseStream(uri, postData, out encoding))
                {
                    result = ReadStreamToString(receiveStream, encoding);

                    var redirectUrl = string.Empty;
                    foreach (var extractor in RedirectExtractors)
                    {
                        var redirectMatch = extractor.Match(result);
                        if (redirectMatch.Groups.Count <= 1)
                        {
                            continue;
                        }

                        redirectUrl = redirectMatch.Groups[1].ToString().Replace(@"\/", "/");
                        break;
                    }

                    if (!string.IsNullOrEmpty(redirectUrl))
                    {
                        result = this.GetContent(redirectUrl, name, url);
                    }
                }

                this.WriteToCache(fileName, result, uri);
            }

            return result;
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// retrieves cookies from IE cache to simulate native IE request
        /// </summary>
        /// <param name="uri"> the uri to the web page that should be queried </param>
        /// <returns> a cookie collections with the  </returns>
        private static CookieCollection GetCookiesFromIE(Uri uri)
        {
            var cookieFolder = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
            var cookieFiles = from s in Directory.GetFiles(cookieFolder, "*", SearchOption.AllDirectories)
                              orderby s descending
                              select s;

            var cookies = Factory.CreateTypeInstance<CookieCollection>();
            foreach (var cookie in cookieFiles)
            {
                var filename = Path.GetFileNameWithoutExtension(cookie);
                if (filename == null)
                {
                    continue;
                }

                filename = filename.Substring(filename.IndexOf("@", StringComparison.Ordinal) + 1);
                if (filename.Contains("["))
                {
                    filename = filename.Substring(0, filename.IndexOf("[", StringComparison.Ordinal));
                }

                if (!uri.Host.Contains(filename))
                {
                    continue;
                }

                byte[] data;
                using (var fs = new FileStream(cookie, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    data = new byte[fs.Length - 1];
                    fs.Read(data, 0, data.Length);
                    fs.Close();
                }

                var cookieData = Encoding.ASCII.GetString(data);
                var entries = cookieData.Split('*');
                foreach (var entry in entries)
                {
                    if (entry.Length < 2)
                    {
                        continue;
                    }

                    var cookieObject =
                        (entry.StartsWith("\n", StringComparison.Ordinal) ? entry.Remove(0, 1) : entry).Split('\n');
                    if (cookieObject.Length > 2)
                    {
                        cookies.Add(new Cookie(cookieObject[0], cookieObject[1], null, cookieObject[2]));
                    }
                }
            }

            return cookies;
        }

        /// <summary>
        /// Reads data from a stream until the end is reached. The
        ///   data is returned as a byte array. An IOException is
        ///   thrown if any of the underlying IO calls fail.
        /// </summary>
        /// <param name="stream"> The stream to read data from </param>
        /// <param name="initialLength">The initial buffer length </param>
        /// <returns> The read stream converted into a byte array. </returns>
        private static byte[] ReadStreamToByteArray(Stream stream, int initialLength)
        {
            if (stream == null)
            {
                return new byte[] { };
            }

            var buffer = new byte[initialLength];
            var read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read != buffer.Length)
                {
                    continue;
                }

                var nextByte = stream.ReadByte();

                // End of stream? If so, we're done
                if (nextByte == -1)
                {
                    return buffer;
                }

                // Nope. Resize the buffer, put in the byte we've just
                // read, and continue
                var newBuffer = new byte[buffer.Length * 2];
                Array.Copy(buffer, newBuffer, buffer.Length);
                newBuffer[read] = (byte)nextByte;
                buffer = newBuffer;
                read++;
            }

            stream.Close();

            // Buffer is now too big. Shrink it.
            var ret = new byte[read];
            Array.Copy(buffer, ret, read);

            return ret;
        }

        /// <summary>
        /// reads stream data to string
        /// </summary>
        /// <param name="receiveStream"> the stream to read from </param>
        /// <param name="encoding"> the encoding of the result string </param>
        /// <returns>a string representing the resource </returns>
        private static string ReadStreamToString(Stream receiveStream, string encoding)
        {
            if (receiveStream == null)
            {
                return string.Empty;
            }

            var resultBuilder = new StringBuilder();
            var encode = Encoding.GetEncoding(string.IsNullOrEmpty(encoding) ? "utf-8" : encoding);

            // Pipe the stream to a higher level stream reader with the required encoding format
            using (var readStream = new StreamReader(receiveStream, encode))
            {
                // Read 256 charcters at a time
                var read = new char[256];
                var count = readStream.Read(read, 0, 256);
                while (count > 0)
                {
                    var str = new string(read, 0, count);
                    resultBuilder.Append(str);
                    count = readStream.Read(read, 0, 256);
                }

                receiveStream.Close();
                readStream.Close();
            }

            return resultBuilder.ToString();
        }

        /// <summary>
        /// determines the cache path for a given cache item name
        /// </summary>
        /// <param name="name"> name of the cache item  </param>
        /// <param name="url"> the url the content is located  </param>
        /// <param name="postData"> The post Data.  </param>
        /// <returns> the path if successfull, empty string if no cache should be used  </returns>
        private string CachePathName(string name, Uri url, string postData)
        {
            ////if (string.IsNullOrEmpty(name))
            ////{
            ////    Tools.DebugWriteLine("name not specified for http-request (=> non-cachable): " + url.AbsoluteUri);
            ////    return string.Empty;
            ////}

            if (!this.UseCache || name.Contains(CacheHintNoCache))
            {
                return string.Empty;
            }

            var sessionData = postData + url;
            sessionData = this.sessionCookies.GetCookies(url).Cast<Cookie>()
                                .Aggregate(
                                    sessionData, 
                                    (current, cookie) => current + (cookie.Name + "=" + cookie.Value));

            var hash = Tools.GetSha1Hash(sessionData);

            var result = Tools.ReplaceInvalidFileCharacters(name + "$$" + this.BaseUrl + "$$" + hash);
            result = Path.Combine(CachePath, result + ".cacheitem");

            return result;
        }

        /// <summary>
        /// Determines if reading from cache is allowed
        /// </summary>
        /// <param name="fileName"> the name of the cache-file </param>
        /// <returns> true if reading the cache is allowed </returns>
        private bool CacheReadAllowed(string fileName)
        {
            return this.UseCache && fileName != null && !string.IsNullOrEmpty(fileName) &&
                   !fileName.Contains(CacheHintNoCache) && File.Exists(fileName);
        }

        /// <summary>
        /// create the request object
        /// </summary>
        /// <param name="uri"> url to the resource we want to read </param>
        /// <param name="method"> POST / GET or whatever </param>
        /// <returns> a web request object that can be used to read the url content </returns>
        private HttpWebRequest CreateRequest(Uri uri, string method)
        {
            return this.CreateRequest(uri, method, string.Empty);
        }

        /// <summary>
        /// create the request object
        /// </summary>
        /// <param name="requestUrl"> url to the resource we want to read </param>
        /// <param name="method"> POST / GET or whatever </param>
        /// <param name="referer"> the referer to add to the header </param>
        /// <returns> a web request object that can be used to read the url content </returns>
        private HttpWebRequest CreateRequest(Uri requestUrl, string method, string referer)
        {
            // build up request and response
            var request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.Method = method;
            request.AllowAutoRedirect = false;
            request.UseDefaultCredentials = true;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            if (!string.IsNullOrEmpty(referer))
            {
                request.Referer = referer;
            }

            // we have some common headers that we might use (including cookies)
            if (this.UseIeCookies)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(GetCookiesFromIE(request.RequestUri));
            }
            else
            {
                request.CookieContainer = this.sessionCookies;
            }

            request.Headers.Add("Accept-Language", "de");
            request.Headers.Add("Accept-Charset", "utf-8");

            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; OfficeLiveConnector.1.3; OfficeLivePatch.0.0; .NET CLR 3.5.30729; .NET CLR 3.0.30618; InfoPath.2)";

            return request;
        }

        /// <summary>
        /// Creates a new Uri by adding the base url if needed.
        /// </summary>
        /// <param name="url"> The absolute or relative url.  </param>
        /// <returns> the uri matching the specified url  </returns>
        private Uri CreateUri(string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            return new Uri(url.Contains("://") ? url : this.BaseUrl + url);
        }

        /// <summary>
        /// Create a request and get the response stream for the GET method
        /// </summary>
        /// <param name="url"> url to the page to get </param>
        /// <param name="referer"> specifies the referer (url this request came from) to add to the request </param>
        /// <param name="encoding"> the encoding used for the text </param>
        /// <returns> a stream corresponding to the content at the uri </returns>
        private Stream GetResponseStream(Uri url, string referer, out string encoding)
        {
            HttpWebResponse objResponse;
            var request = this.CreateRequest(url, "GET", referer);

            var logonCredentialRequest = new LogonCredentialRequest(
                this.proxyCredentials,
                string.Format(
                    CultureInfo.CurrentCulture,
                    Resources.TheProxyServerNeedsYourCredentials,
                    url.Host,
                    request.Proxy.GetProxy(url).Host),
                request.Proxy.GetProxy(url).Host);

            while (true)
            {
                try
                {
                    objResponse = GetHttpResponse(request);
                    encoding = objResponse.CharacterSet;
                    logonCredentialRequest.SaveCredentials();
                    break;
                }
                catch (WebException ex)
                {
                    if ((this.UiDispatcher != null) && ex.Response != null &&
                        ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.ProxyAuthenticationRequired)
                    {
                        request = this.CreateRequest(url, "GET", referer);

                        if (this.UiDispatcher.AskForLogOnCredentials(logonCredentialRequest))
                        {
                            this.proxyCredentials = logonCredentialRequest.LogOnCredentials;

                            if (string.IsNullOrEmpty(logonCredentialRequest.LogOnCredentials.LogOnDomain))
                            {
                                request.Proxy.Credentials =
                                    new NetworkCredential(
                                        logonCredentialRequest.LogOnCredentials.LogOnUserId,
                                        logonCredentialRequest.LogOnCredentials.LogOnPassword);
                            }
                            else
                            {
                                request.Proxy.Credentials =
                                    new NetworkCredential(
                                        logonCredentialRequest.LogOnCredentials.LogOnUserId,
                                        logonCredentialRequest.LogOnCredentials.LogOnPassword,
                                        logonCredentialRequest.LogOnCredentials.LogOnDomain);
                            }
                        }
                        else
                        {
                            encoding = string.Empty;
                            return null;
                        }
                    }
                    else
                    {
                        if (ex.Response != null)
                        {
                            if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                            {
                                if (this.ContentCredentials.LogOnDomain == "[GOOGLE]")
                                {
                                    request = this.CreateRequest(url, "GET", referer);
                                    request.Headers.Add("Authorization", "GoogleLogin auth=" + this.ContentCredentials.LogOnPassword);
                                }
                            }
                        }

                        if ((this.UiDispatcher != null) &&
                            (ex.Status == WebExceptionStatus.ConnectFailure ||
                             ex.Status == WebExceptionStatus.NameResolutionFailure))
                        {
                            if (
                                this.UiDispatcher.AskForConfirm(
                                    string.Format(
                                        Resources.UserMessageConnectionProblemQuestionRetry,
                                        url.Host,
                                        ex.Status),
                                    Resources.UserMessageConnectionProblemTitle))
                            {
                                continue;
                            }
                        }

                        encoding = string.Empty;
                        return null;
                    }
                }
            }

            return objResponse.GetResponseStream();
        }

        private HttpWebResponse GetHttpResponse(HttpWebRequest request)
        {
            HttpWebResponse objResponse;
            objResponse = (HttpWebResponse)request.GetResponse();
            if (objResponse.Cookies.Count == 0 && objResponse.Headers.AllKeys.Contains("Set-Cookie"))
            {
                var regexp = new Regex("(?<name>[^=]+)=(?<val>[^;]+)[^,]+,?");
                var cookies = objResponse.Headers["Set-Cookie"];
                var keyValue = regexp.Matches(cookies);
                foreach (Match cookieValue in keyValue)
                {
                    this.sessionCookies.Add(new Cookie(cookieValue.Groups["name"].ToString(), cookieValue.Groups["val"].ToString(), "", "xing.com"));
                }
            }

            if (objResponse.Headers.AllKeys.Contains("Location"))
            {
                var location = objResponse.Headers["Location"];
                if (!location.StartsWith("http"))
                {
                    location = this.BaseUrl + location;
                }

                var request2 = this.CreateRequest(new Uri(location), "GET", objResponse.ResponseUri.ToString());
                objResponse = GetHttpResponse(request2);
            }

            return objResponse;
        }

        /// <summary>
        /// Creates a request, posts some data and gets the response stream. This method does use the POST verb of http.
        /// </summary>
        /// <param name="uri"> the uri to the resource to get </param>
        /// <param name="postData"> the data to be posted (must already be encoded using application/x-www-form-urlencoded) </param>
        /// <param name="encoding"> the encoding of the text content </param>
        /// <returns> a stream that represents the binary data </returns>
        private Stream PostResponseStream(Uri uri, string postData, out string encoding)
        {
            var request = this.CreateRequest(uri, "POST");
            request.ContentType = "application/x-www-form-urlencoded";

            var postEncoding = new ASCIIEncoding();
            var logonDataBytes = postEncoding.GetBytes(postData);
            request.ContentLength = logonDataBytes.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(logonDataBytes, 0, logonDataBytes.Length);
                stream.Close();
            }

            var objResponse = GetHttpResponse(request);
            encoding = objResponse.CharacterSet;
            return objResponse.GetResponseStream();
        }

        /// <summary>
        /// writes the content to the file system cache
        /// </summary>
        /// <param name="fileName"> the file name and path of the cache item  </param>
        /// <param name="result"> the content of the cache item to be written  </param>
        /// <param name="uri"> the url to write the content for  </param>
        /// <returns> a value indicating whether the cache read did succeed.  </returns>
        private bool ReadFromCache(string fileName, out string result, Uri uri)
        {
            result = string.Empty;
            if (!this.CacheReadAllowed(fileName))
            {
                return false;
            }

            ResponseCacheItem cacheItem;

            Tools.EnsurePathExist(CachePath);
            if (File.Exists(fileName))
            {
                using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    cacheItem = (ResponseCacheItem)new XmlSerializer(typeof(ResponseCacheItem)).Deserialize(file);
                }

                foreach (var cookie in cacheItem.Cookies)
                {
                    this.sessionCookies.Add(uri, new Cookie(cookie.Key, cookie.Value));
                }

                result = Encoding.UTF32.GetString(cacheItem.Content);
            }

            return true;
        }

        /// <summary>
        /// writes the content to the file system cache
        /// </summary>
        /// <param name="fileName"> the file name and path of the cache item </param>
        /// <param name="result"> the content of the cache item to be written </param>
        /// <param name="uri"> the url to write the content for </param>
        /// <returns> a value indicating whether the cache read did succeed.  </returns>
        private bool ReadFromCacheBinary(string fileName, out byte[] result, Uri uri)
        {
            result = new byte[] { };
            if (!this.CacheReadAllowed(fileName))
            {
                return false;
            }

            ResponseCacheItem cacheItem;

            Tools.EnsurePathExist(CachePath);
            using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                cacheItem = (ResponseCacheItem)new XmlSerializer(typeof(ResponseCacheItem)).Deserialize(file);
            }

            foreach (var cookie in cacheItem.Cookies)
            {
                this.sessionCookies.Add(uri, new Cookie(cookie.Key, cookie.Value));
            }

            result = cacheItem.Content;
            return true;
        }

        /// <summary>
        /// writes the content to the file system cache
        /// </summary>
        /// <param name="fileName"> the file name and path of the cache item </param>
        /// <param name="result"> the content of the cache item to be written </param>
        /// <param name="url"> the url to write the content for </param>
        private void WriteToCache(string fileName, string result, Uri url)
        {
            this.WriteToCache(fileName, Encoding.UTF32.GetBytes(result), url);
        }

        /// <summary>
        /// writes the content to the file system cache
        /// </summary>
        /// <param name="fileName"> the file name and path of the cache item </param>
        /// <param name="result"> the content of the cache item to be written </param>
        /// <param name="url"> the url to write the content for </param>
        private void WriteToCache(string fileName, byte[] result, Uri url)
        {
            if (!this.UseCache || string.IsNullOrEmpty(fileName))
            {
                return;
            }

            var cookieList =
                (from Cookie cookie in this.sessionCookies.GetCookies(url)
                 select new KeyValuePair(cookie.Name, cookie.Value)).ToList();

            var cacheItem = new ResponseCacheItem { Content = result, Cookies = cookieList };

            Tools.EnsurePathExist(CachePath);
            using (var file = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                new XmlSerializer(typeof(ResponseCacheItem)).Serialize(file, cacheItem);
            }
        }

        #endregion
    }
}