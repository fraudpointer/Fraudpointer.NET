using System;
using System.IO;
using System.Net;
using Fraudpointer.API.Helpers;
using Fraudpointer.API.Models;
using Fraudpointer.API.RequestWrappers;
using Fraudpointer.API.Responses;
using Fraudpointer.API.ResponseWrappers;


namespace Fraudpointer.API.Clients
{
    /// <summary>
    /// Maps methods to fraud pointer HTTP API actions.
    /// </summary>
    class Client : IClient
    {
        #region private attributes

        private readonly HttpWrapper _http;
        private readonly string _apiKey;

        #endregion

        #region public methods

        public Client(HttpWrapper http, string apiKey)
        {
            _http = http;
            _apiKey = apiKey;
        } // Client class constructor

        public AssessmentSession CreateAssessmentSession()
        {
            return CommonTryCatch( () => 
            {
                // Request content
                var k = new RequestKey { Key = _apiKey };

                return  _http.Post<ResponseAssessmentSession>("/assessment_sessions",k).AssessmentSession;
            },
            "Error while trying to create assessment session!");
        }

        public Event AppendEventToAssessmentSession(AssessmentSession assessmentSession, Event paramEvent)
        {
            return CommonTryCatch( () => 
            {
                var url = String.Format("/assessment_sessions/{0}/events", assessmentSession.Id);

                var k = new RequestKeyAndEvent {Key = _apiKey, Event = paramEvent};

                return _http.Post<ResponseEvent>(url,k).Event;
            },
            "Error while trying to create assessment session!");
        } 

        public FraudAssessment GetFraudAssesment(AssessmentSession assessmentSession, string assesmentId)
        {
            return CommonTryCatch( () => 
            {
                var url = String.Format("/assessment_sessions/{0}/fraud_assessments/{1}?key={2}",
                                                    assessmentSession.Id, assesmentId, _apiKey);

                return _http.Get<ResponseFraudAssessment>(url).FraudAssessment;

            }
            ,"Error while trying to get assessment result!");
        }

        public FraudAssessment CreateFraudAssessment(AssessmentSession assessmentSession, bool interim)
        {
            return CommonTryCatch( () => 
            {
                
                var url = String.Format("/assessment_sessions/{0}/fraud_assessments", assessmentSession.Id);

                var k = new RequestKeyAndFraudAssessment{
                    Key = _apiKey, 
                    FraudAssessment = new FraudAssessment
                                          {
                                              Interim = interim
                                          }
                };

                return _http.Post<ResponseFraudAssessment>(url,k).FraudAssessment;

            },
            "Error while trying to create fraud assesment!");
        }
        #endregion

        #region Misc functions
        //Note: This should be an Extension method. Is not a part of the client.
        public string CreditCardHash(string creditCardNumber)
        {
            const string salt = "HVK+gw==";
            var hasher = new SaltedHasher();
            return hasher.GetHashForSpecifiedSaltString(creditCardNumber, salt).HashString;
        } 
        #endregion

        #region private methods
        /// <summary>
        /// Wraps an action in a common try catch clause
        /// </summary>
        /// <param name="action"></param>
        /// <param name="errorMessage"></param>
        private T CommonTryCatch<T>(Func<T> action,string errorMessage)
        {
            try
            {
                return action();
            }
            catch (WebException webEx)
            {
                var error = GetErrorMessage(webEx.Response);
                throw new ClientException(error ?? webEx.Message, webEx);
            }
            catch (Exception ex)
            {
                throw new ClientException(errorMessage, ex);
            }
        }


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
