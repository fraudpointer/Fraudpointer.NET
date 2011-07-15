using System.Runtime.Serialization;
using Fraudpointer.API.Models;
using Newtonsoft.Json;

namespace Fraudpointer.API.Responses
{
    /// <summary>
    ///   This class is to replicate the server request structure
    /// </summary>
    class ResponseFraudAssessment
    {
        /// <summary>
        ///   Base server request json object
        /// </summary>
        [JsonProperty(PropertyName = "fraud_assessment")]
        public FraudAssessment FraudAssessment { get; set; }
    }
}
