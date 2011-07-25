using System;
using Newtonsoft.Json;

namespace Fraudpointer.API.Models
{
    /// <summary>
    /// Fraud assesment related case information
    /// </summary>
    public class Case
    {
        /// <summary>
        /// Case resolution
        /// </summary>
        /// <remarks>
        /// The valid values here are:
        /// - Accept
        /// - Review
        /// - Reject
        /// </remarks>
        [JsonProperty(PropertyName = "resolution")]
        public string Resolution { get; set; }

        /// <summary>
        /// Case Id for further reference
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Case status. "Open" or "Closed".
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}