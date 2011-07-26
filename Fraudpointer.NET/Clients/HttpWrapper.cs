using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Fraudpointer.API.Clients
{
    /// <summary>
    /// A wrapper around common http calls
    /// </summary>
    class HttpWrapper : IHttpWrapper
    {
        /// <summary>
        /// Base url used in all requests
        /// </summary>
        private readonly string _baseUrl;

        /// <summary>
        /// Constructor with base url
        /// </summary>
        /// <param name="baseUrl">Url used as a base in all request done in this HttpWrapper</param>
        public HttpWrapper(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        /// <summary>
        /// Post an object as JSON to the given url
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="urlPath">Path to post to</param>
        /// <param name="thingToPost">Object to post in JSON</param>
        /// <returns>An object of the given type.</returns>
        public T Post<T>(string urlPath,object thingToPost)
        {
            var webRequest = PrepareWebRequest(urlPath);

            PostObject(thingToPost, webRequest);

            return DeserializeResponse<T>(webRequest);
        }

        /// <summary>
        /// Gets an object as JSON from the given url
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="urlPath">Path to get from</param>
        /// <returns>An object of the given type.</returns>
        public T Get<T>(string urlPath)
        {
            var webRequest = PrepareWebRequest(urlPath);

            return DeserializeResponse<T>(webRequest);
        }

        /// <summary>
        /// Post an object using json in the current request
        /// </summary>
        /// <param name="objectToPost">An object graph we want to post</param>
        /// <param name="webRequest"></param>
        private void PostObject(object objectToPost, HttpWebRequest webRequest)
        {
            webRequest.Method = "POST";

            var kSerialized = JsonConvert.SerializeObject(objectToPost);
            byte[] bodyBytes = Encoding.UTF8.GetBytes(kSerialized);
            using (var requestStream = webRequest.GetRequestStream())
            {
                requestStream.Write(bodyBytes, 0, bodyBytes.Length);
            }
        }

        /// <summary>
        /// Builds an <see cref="HttpWebRequest"/> based on a url path to the fraudpointer service.
        /// </summary>
        /// <param name="url">url path to the fraudpointer service</param>
        /// <returns>a new HttpWebRequest for the given url.</returns>
        private HttpWebRequest PrepareWebRequest(string url)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(_baseUrl + url);

            webRequest.ContentType = "application/json";
            webRequest.Accept = "application/json";
            webRequest.ContentType = "application/json; charset=utf-8";
            webRequest.Method = "GET";
            webRequest.Timeout = 5000;

            return webRequest;
        }

        /// <summary>
        /// Deserializes a json response from fraudpointer
        /// </summary>
        /// <typeparam name="T">What kind of object shuld the method deserialize,</typeparam>
        /// <param name="webRequest">The request where we're expectiong the result</param>
        /// <returns></returns>
        private T DeserializeResponse<T>(HttpWebRequest webRequest)
        {
            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            using (var responseStream = webResponse.GetResponseStream())
            {
                if (responseStream == null)
                    throw new Exception("Unexpected empty response");

                var responseBody = new StreamReader(responseStream).ReadToEnd();
                var serverFraudAssessment = responseBody.DeserializeJson<T>();
                return serverFraudAssessment;
            }
        }
    }
}
