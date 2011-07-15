using System;
using System.IO;
using System.Net;
using System.Text;
using Fraudpointer.API.Helpers;
using Fraudpointer.API.Models;
using Fraudpointer.API.RequestWrappers;
using Fraudpointer.API.Responses;
using Fraudpointer.API.ResponseWrappers;
using Newtonsoft.Json;

namespace Fraudpointer.API.Clients
{
    class Client : IClient
    {
        #region private attributes

        private readonly string _baseUrl;
        private readonly string _apiKey;

        #endregion

        #region public methods

        public Client(string baseUrl, string apiKey)
        {
            _baseUrl = baseUrl;
            _apiKey = apiKey;
        } // Client class constructor

        public AssessmentSession CreateAssessmentSession()
        {
            try
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(_baseUrl + "/assessment_sessions");
                // Request
                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";
                webRequest.Accept = "application/json";
                webRequest.ContentType = "application/json; charset=utf-8";
                webRequest.Timeout = 5000;
                // Request content
                var k = new RequestKey { Key = _apiKey };
                var kSerialized = JsonConvert.SerializeObject(k);
                byte[] bodyBytes = Encoding.UTF8.GetBytes(kSerialized);
                using(var requestStream = webRequest.GetRequestStream())
                {
                    requestStream.Write(bodyBytes, 0, bodyBytes.Length);
                }
                // Response
                using(var webResponse = (HttpWebResponse)webRequest.GetResponse())
                using (var respStream = webResponse.GetResponseStream())
                {
                    // Understand the response
                    if (respStream == null) return null;
                    var responseBody = new StreamReader(respStream).ReadToEnd();
                    var responseAssessmentSession = JsonConvert.DeserializeObject<ResponseAssessmentSession>(responseBody);
                    return responseAssessmentSession.AssessmentSession;
                }
            }
            catch (Exception ex)
            {
                throw new ClientException("Error while trying to create assessment session!", ex);
            }
        } // CreateAssessmentSession()

        public Event AppendEventToAssessmentSession(AssessmentSession assessmentSession, Event paramEvent)
        {
            HttpWebRequest webRequest = null;
            HttpWebResponse webResponse = null;
            try
            {
                webRequest = (HttpWebRequest)WebRequest.Create(_baseUrl + "/assessment_sessions/" + assessmentSession.Id + "/events");
                webRequest.Method = "POST";
                var k = new RequestKeyAndEvent {Key = _apiKey, Event = paramEvent};
                var kSerialized = JsonConvert.SerializeObject(k);

                webRequest.ContentType = "application/json";
                webRequest.Accept = "application/json";
                webRequest.ContentType = "application/json; charset=utf-8";

                byte[] bodyBytes = Encoding.UTF8.GetBytes(kSerialized);
                webRequest.GetRequestStream().Write(bodyBytes, 0, bodyBytes.Length);
                webRequest.GetRequestStream().Close();

                webResponse = (HttpWebResponse)webRequest.GetResponse();
                //Console.WriteLine("HTTP/{0} {1} {2}", resp.ProtocolVersion, (int)resp.StatusCode, resp.StatusDescription);

                string responseBody = "";
                Stream respStream = webResponse.GetResponseStream();
                if (respStream != null)
                {
                    responseBody = new StreamReader(respStream).ReadToEnd();
                    //Console.WriteLine(responseBody);
                    ResponseEvent levent = JsonConvert.DeserializeObject<ResponseEvent>(responseBody);
                    //Console.WriteLine(assessmentSession);
                    return levent.Event;
                }

                return null;

            }
            catch (Exception ex)
            {
                throw new ClientException("Error while trying to create assessment session!", ex);
            }
            finally
            {
                try
                {
                    if (webRequest != null && webRequest.GetRequestStream() != null)
                    {
                        webRequest.GetRequestStream().Close();
                    }
                    if (webResponse != null)
                    {
                        webResponse.Close();
                    }
                }
                catch (Exception ex2)
                {
                }
            }
        } // AppendEventToAssessmentSession ()
        //-------------------------------------

        public FraudAssessment CreateFraudAssessment(AssessmentSession assessmentSession, bool interim)
        {
            HttpWebRequest webRequest = null;
            HttpWebResponse webResponse = null;
            try
            {
                webRequest = (HttpWebRequest)WebRequest.Create(_baseUrl + "/assessment_sessions/" + assessmentSession.Id + "/fraud_assessments");
                webRequest.Method = "POST";
                var k = new RequestKeyAndFraudAssessment();
                k.Key = _apiKey;
                k.FraudAssessment = new FraudAssessment { Interim = interim };
                var kSerialized = JsonConvert.SerializeObject(k);

                webRequest.ContentType = "application/json";
                webRequest.Accept = "application/json";
                webRequest.ContentType = "application/json; charset=utf-8";

                byte[] bodyBytes = Encoding.UTF8.GetBytes(kSerialized);
                webRequest.GetRequestStream().Write(bodyBytes, 0, bodyBytes.Length);
                webRequest.GetRequestStream().Close();

                webResponse = (HttpWebResponse)webRequest.GetResponse();
                //Console.WriteLine("HTTP/{0} {1} {2}", resp.ProtocolVersion, (int)resp.StatusCode, resp.StatusDescription);

                string responseBody = "";
                Stream respStream = webResponse.GetResponseStream();
                if (respStream != null)
                {
                    responseBody = new StreamReader(respStream).ReadToEnd();
                    //Console.WriteLine(responseBody);
                    ResponseFraudAssessment serverFraudAssessment = JsonConvert.DeserializeObject<ResponseFraudAssessment>(responseBody);
                    //Console.WriteLine(assessmentSession);
                    return serverFraudAssessment.FraudAssessment;
                }
                return null;

            }
            catch(WebException webEx)
            {
                var errorMessage = GetErrorMessage(webEx.Response);
                throw new ClientException(errorMessage??webEx.Message, webEx);
            }
            catch (Exception ex)
            {
                throw new ClientException("Error while trying to create assessment session!", ex);
            }
            finally
            {
                try
                {
                    if (webRequest != null && webRequest.GetRequestStream() != null)
                    {
                        webRequest.GetRequestStream().Close();
                    }
                    if (webResponse != null)
                    {
                        webResponse.Close();
                    }
                }
                catch (Exception ex2)
                {
                }
            }
        } // CreateFraudAssessment()
        //---------------------------

        public string CreditCardHash(string creditCardNumber)
        {
            var salt = "HVK+gw==";
            var hasher = new SaltedHasher();
            return hasher.GetHashForSpecifiedSaltString(creditCardNumber, salt).HashString;
        } // CreditCardHash()
        //---------------------

        #endregion

        #region private methods

        private string GetErrorMessage(WebResponse response)
        {
            if (response == null)
                return null;
            using(var stream = response.GetResponseStream())
            {
                if (stream == null)
                    return null;
                var responseBody = new StreamReader(stream).ReadToEnd();
                return responseBody;
            }
        }

        #endregion


    }
}
