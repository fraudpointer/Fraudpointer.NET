using System;
using Newtonsoft.Json;

namespace Fraudpointer.API.Models
{
    /// <summary>
    /// The Profile used to carry out the Fraud Assessment.
    /// </summary>
    /// <remarks>
    /// A Models.FraudAssessment carries out Assessment using the rules that belong to a specific Profile. 
    /// There are might be many Profile configured in your Account, but only one is selected for the 
    /// Models.FraudAssessment at hand. This is done at run-time usign the Profile Selection Rules.
    /// </remarks>
    public class Profile
    {
        /// <summary>
        /// The unique identifier of the Profile used for Models.FraudAssessment.
        /// </summary>
        [JsonProperty(PropertyName="id")]
        public string Id { get; set; }

        /// <summary>
        /// The name of the Profile used for FraudAssessment. 
        /// </summary>
        [JsonProperty(PropertyName="name")]
        public string Name { get; set; }

        /// <summary>
        /// Last time this Profile was updated
        /// </summary>
        [JsonProperty(PropertyName = "updated_at")]
        public DateTime UpdatedAt { get; set; }

    } // class Profile
    //-----------------

} // namespace Fraudpointer.API.Models
//-------------------------------------
