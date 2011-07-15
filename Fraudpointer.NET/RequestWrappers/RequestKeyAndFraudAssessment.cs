using Fraudpointer.API.Models;
using Newtonsoft.Json;

namespace Fraudpointer.API.RequestWrappers
{
    class RequestKeyAndFraudAssessment : RequestKey
    {
        [JsonProperty(PropertyName = "fraud_assessment")]
        public FraudAssessment FraudAssessment { get; set; }
    }
}
