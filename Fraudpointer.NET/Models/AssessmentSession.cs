using Newtonsoft.Json;

namespace Fraudpointer.API.Models
{    
    /// <summary>
    /// An Assessment Session is the object returned when you call API.IClient.CreateAssessmentSession(), which is the
    /// first method that you need to call to start interacting with the FraudPointer Server.
    /// </summary>
    /// <remarks>
    /// When you want to start interaction with FraudPointer Server you need to call API.IClient.CreateAssessmentSession(). This
    /// method will return to you an instance of Models.AssessmentSession. After its creation, you use this object in the
    /// following cases:
    /// -# When you want to add a Models.Event object to this Assessment Session.
    /// -# When you want to request a Fraud Assessment
    /// -# When you want to embed the session id in your html content, in the hidden field value that is used by
    ///    FraudPointer javascript <c>fp.js</c>.
    /// </remarks>
    public class AssessmentSession
    {
        /// <summary>
        /// Unique id for the created session returned by the server. 
        /// </summary>
        /// <remarks>
        /// When you call API.IClient.CreateAssessmentSession() you get an instantiated object whose only property is
        /// AssessmentSession.Id. This <c>Id</c> itself, is useful when you want to embed it in your html content, in 
        /// the hidden field value that is used by FraudPointer javascript <c>fp.js</c>
        /// </remarks>
        [JsonProperty(PropertyName="id")]
        public string Id { get; set; }       

    } // AssessmentSession

} // Fraudpointer.API.Models
