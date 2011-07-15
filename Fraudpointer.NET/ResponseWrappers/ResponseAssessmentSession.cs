using Fraudpointer.API.Models;
using Newtonsoft.Json;

namespace Fraudpointer.API.ResponseWrappers
{
    /// <summary>
    ///   This class is to replicate the server response structure
    /// </summary>
    class ResponseAssessmentSession
    {
        /// <summary>
        ///   Base server response json object
        /// </summary>
        [JsonProperty(PropertyName = "assessment_session")]
        public AssessmentSession AssessmentSession { get; set; }
    }

}
