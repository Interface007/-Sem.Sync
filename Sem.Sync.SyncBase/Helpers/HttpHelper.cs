//-----------------------------------------------------------------------
// <copyright file="HttpHelper.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.Helpers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;

    using DetailData;
    using Interfaces;

    /// <summary>
    /// This class provides funktionality to get information from the web.
    /// </summary>
    public class HttpHelper
    {
        #region private members
        /// <summary>
        /// Gets or sets a value to determine the path to cache the content
        /// </summary>
        private static readonly string CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SemSyncManager\\Cache");

        /// <summary>
        /// Private cookie store when not using IE cookies
        /// </summary>
        private readonly CookieContainer sessionCookies = new CookieContainer();

        /// <summary>
        /// credentials for the proxy server
        /// </summary>
        private readonly ICredentialAware proxyCredentials = new Credentials();

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHelper"/> class. 
        /// the constructor will switch off certificate validation if IgnoreCertificateError is true
        /// </summary>
        /// <param name="baseUrl">
        /// The base Url that is added if there is no http?:// prifix inside the url.
        /// </param>
        /// <param name="ignoreCertificateErrors">
        /// The ignore Certificate Errors.
        /// </param>
        public HttpHelper(string baseUrl, bool ignoreCertificateErrors)
        {
            this.BaseUrl = baseUrl;
            this.IgnoreCertificateError = ignoreCertificateErrors;
            this.sessionCookies = new CookieContainer();
            ServicePointManager.Expect100Continue = false;

            if (this.IgnoreCertificateError)
            {
                // Hack for debugging purposes to accept Fiddler certificate
                ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, cert, chain, errors) => true;
            }
        }

        #region public propertries
        /// <summary>
        /// Gets or sets the base address for requests
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a certificate error in an
        /// https connection whould be ignored.
        /// debugging using Fiddler needs to bypass certificate approval
        /// set this const to "true" if you want to enable fiddler-debugging
        /// this will prevent checking for man in the middle attack
        /// </summary>
        public bool IgnoreCertificateError { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to activate caching of content 
        /// - this will present refreshing the data use this for debugging purpose 
        /// to not stress the http server
        /// </summary>
        public bool UseCache { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to prevent loading missing entries 
        /// - this is only usefull for debugging purpose if this property is true, we 
        /// will not download missing content
        /// </summary>
        public bool SkipNotCached { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use stored ie cookies instead of 
        /// private session cookies
        /// </summary>
        public bool UseIeCookies { get; set; }

        /// <summary>
        /// Gets or sets the object that is responsible to interact with the user
        /// </summary>
        public IUiInteraction UiDispatcher { get; set; }
        #endregion

        /// <summary>
        /// encodes the input parameter to form-url-encoded
        /// </summary>
        /// <param name="parameter">the content to be encoded</param>
        /// <returns>the encoded string</returns>
        public static string EncodeForPost(string parameter)
        {
            return System.Web.HttpUtility.UrlEncode(parameter);
        }

        /// <summary>
        /// Download content binary
        /// </summary>
        /// <param name="url">the url to access the content</param>
        /// <param name="name">a name for caching - this should correspond to the url</param>
        /// <returns>the binary result of the request without conversion</returns>
        public byte[] GetContentBinary(string url, string name)
        {
            var fileName = CachePathName(name);

            if (this.UseCache && File.Exists(fileName))
            {
                return File.ReadAllBytes(fileName);
            }

            byte[] result = null;
            if (!this.SkipNotCached)
            {
                var receiveStream = this.GetResponseStream(url);
                result = ReadStreamToByteArray(receiveStream, 32768);
                if (!string.IsNullOrEmpty(fileName))
                {
                    SyncTools.EnsurePathExist(Path.GetDirectoryName(fileName));
                    File.WriteAllBytes(fileName, result);
                }
            }

            return result;
        }

        /// <summary>
        /// Download content binary
        /// </summary>
        /// <param name="url">the url to access the content</param>
        /// <param name="name">a name for caching - this should correspond to the url</param>
        /// <param name="postData">the data that should be posted to the server</param>
        /// <returns>the binary result of the request without conversion</returns>
        public byte[] GetContentBinaryPost(string url, string name, string postData)
        {
            var fileName = CachePathName(name);

            if (this.UseCache && fileName != "[NOCACHE]" && File.Exists(fileName))
            {
                return File.ReadAllBytes(fileName);
            }

            byte[] result = null;
            if (!this.SkipNotCached && !string.IsNullOrEmpty(fileName))
            {
                var receiveStream = this.PostResponseStream(url, postData);
                result = ReadStreamToByteArray(receiveStream, 32768);

                File.WriteAllBytes(fileName, result);
            }

            return result;
        }

        /// <summary>
        /// Download content as text
        /// </summary>
        /// <param name="url">the url to access the content</param>
        /// <param name="name">a name for caching - this should correspond to the url</param>
        /// <returns>the text result of the request</returns>
        public string GetContent(string url, string name)
        {
            var fileName = CachePathName(name);
            if (this.UseCache && File.Exists(fileName))
            {
                return File.ReadAllText(fileName);
            }

            var result = string.Empty;
            if (!this.SkipNotCached)
            {
                var receiveStream = this.GetResponseStream(this.BaseUrl + url);
                result = ReadStreamToString(receiveStream);

                this.WriteToCache(name, result);
            }

            return result;
        }

        /// <summary>
        /// Download content as text
        /// </summary>
        /// <param name="url">the url to access the content</param>
        /// <param name="name">a name for caching - this should correspond to the url</param>
        /// <param name="postData">the complete data to be added (including keys and values) as one string</param>
        /// <returns>the text result of the request</returns>
        public string GetContentPost(string url, string name, string postData)
        {
            var fileName = CachePathName(name);
            if (this.UseCache && File.Exists(fileName))
            {
                return File.ReadAllText(fileName);
            }

            var result = string.Empty;
            if (!this.SkipNotCached)
            {
                var receiveStream = this.PostResponseStream(this.BaseUrl + url, postData);
                result = ReadStreamToString(receiveStream);

                this.WriteToCache(name, result);
            }

            return result;
        }

        /// <summary>
        /// reads stream data to string
        /// </summary>
        /// <param name="receiveStream">the stream to read from</param>
        /// <returns>a string representing the resource</returns>
        private static string ReadStreamToString(Stream receiveStream)
        {
            var resultBuilder = new StringBuilder();
            var encode = Encoding.GetEncoding("utf-8");

            // Pipe the stream to a higher level stream reader with the required encoding format
            var readStream = new StreamReader(receiveStream, encode);

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

            return resultBuilder.ToString();
        }

        /// <summary>
        /// Reads data from a stream until the end is reached. The
        /// data is returned as a byte array. An IOException is
        /// thrown if any of the underlying IO calls fail.
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        /// <param name="initialLength">The initial buffer length</param>
        /// <returns>The read stream converted into a byte array.</returns>
        private static byte[] ReadStreamToByteArray(Stream stream, int initialLength)
        {
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
        /// retrieves cookies from IE cache to simulate native IE request
        /// </summary>
        /// <param name="uri">the uri to the web page that should be queried</param>
        /// <returns>a cookie collections with the </returns>
        private static CookieCollection GetCookiesFromIE(Uri uri)
        {
            var cookieFolder = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
            var cookieFiles =
                from s in Directory.GetFiles(cookieFolder, "*", SearchOption.AllDirectories)
                orderby s descending
                select s;

            var cookies = new CookieCollection();
            foreach (var cookie in cookieFiles)
            {
                var filename = Path.GetFileNameWithoutExtension(cookie);
                filename = filename.Substring(filename.IndexOf("@", StringComparison.Ordinal) + 1);
                if (filename.Contains("["))
                {
                    filename = filename.Substring(0, filename.IndexOf("[", StringComparison.Ordinal));
                }

                if (!uri.Host.Contains(filename))
                {
                    continue;
                }

                var fs = new FileStream(cookie, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var data = new byte[fs.Length - 1];
                fs.Read(data, 0, data.Length);
                fs.Close();

                var cookieData = Encoding.ASCII.GetString(data);
                var entries = cookieData.Split('*');
                foreach (var entry in entries)
                {
                    if (entry.Length < 2)
                    {
                        continue;
                    }

                    var cookieObject = (entry.StartsWith("\n", StringComparison.Ordinal) ? entry.Remove(0, 1) : entry).Split('\n');
                    if (cookieObject.Length > 2)
                    {
                        cookies.Add(new Cookie(cookieObject[0], cookieObject[1], null, cookieObject[2]));
                    }
                }
            }

            return cookies;
        }

        /// <summary>
        /// determines the cache path for a given cache item name
        /// </summary>
        /// <param name="name">name of the cache item</param>
        /// <returns>the path if successfull, empty string if no cache should be used</returns>
        private static string CachePathName(string name)
        {
            return 
                name != "[NOCACHE]" 
                ? Path.Combine(CachePath, name.Replace("/", "_").Replace(":", "_")) 
                : string.Empty;
        }

        /// <summary>
        /// Creates a request, posts some data and gets the response stream. This method does use the POST verb of http.
        /// </summary>
        /// <param name="url">the uri to the resource to get</param>
        /// <param name="postData">the data to be posted (must already be encoded using application/x-www-form-urlencoded)</param>
        /// <returns>a stream that represents the binary data</returns>
        private Stream PostResponseStream(string url, string postData)
        {
            var request = this.CreateRequest(url, "POST");
            request.ContentType = "application/x-www-form-urlencoded";

            var encoding = new ASCIIEncoding();
            var loginDataBytes = encoding.GetBytes(postData);
            request.ContentLength = loginDataBytes.Length;

            var stream = request.GetRequestStream();

            stream.Write(loginDataBytes, 0, loginDataBytes.Length);
            stream.Close();

            var objResponse = (HttpWebResponse)request.GetResponse();
            return objResponse.GetResponseStream();
        }

        /// <summary>
        /// Create a request and get the response stream for the GET method
        /// </summary>
        /// <param name="url">url to the page to get</param>
        /// <returns>a stream corresponding to the content at the uri</returns>
        private Stream GetResponseStream(string url)
        {
            HttpWebResponse objResponse;
            var request = this.CreateRequest(url, "GET");

            while (true)
            {
                try
                {
                    objResponse = (HttpWebResponse)request.GetResponse();
                    break;
                }
                catch (WebException ex)
                {
                    if ((this.UiDispatcher != null) &&
                        ((System.Net.HttpWebResponse)ex.Response).StatusCode ==
                        HttpStatusCode.ProxyAuthenticationRequired)
                    {
                        if (this.UiDispatcher.AskForLogOnCredentials(
                            this.proxyCredentials,
                            "The proxy server needs credentials.",
                            this.proxyCredentials.LogOnUserId,
                            this.proxyCredentials.LogOnPassword))
                        {
                            if (string.IsNullOrEmpty(this.proxyCredentials.LogOnDomain))
                            {
                                request.Proxy.Credentials = new NetworkCredential(
                                    this.proxyCredentials.LogOnUserId,
                                    this.proxyCredentials.LogOnPassword);
                            }
                            else
                            {
                                request.Proxy.Credentials = new NetworkCredential(
                                this.proxyCredentials.LogOnUserId,
                                this.proxyCredentials.LogOnPassword,
                                this.proxyCredentials.LogOnDomain);
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return objResponse.GetResponseStream();
        }

        /// <summary>
        /// create the request object
        /// </summary>
        /// <param name="url">url to the resource we want to read</param>
        /// <param name="method">POST / GET or whatever</param>
        /// <returns>a web request object that can be used to read the url content</returns>
        private HttpWebRequest CreateRequest(string url, string method)
        {
            // add base url, if there is no protocol identifier
            var requestUrl = (url.Contains("http:") || url.Contains("https:")) ? url : this.BaseUrl + url;

            // build up request and response
            var request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.Method = method;
            request.AllowAutoRedirect = true;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

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
        /// writes the content to the file system cache
        /// </summary>
        /// <param name="name">the name of the cache item</param>
        /// <param name="result">the content of the cache item</param>
        private void WriteToCache(string name, string result)
        {
            if (this.UseCache)
            {
                var fileName = CachePathName(name);
                if (!string.IsNullOrEmpty(fileName))
                {
                    SyncTools.EnsurePathExist(CachePath);
                    File.WriteAllText(fileName, result);
                }
            }
        }
    }
}