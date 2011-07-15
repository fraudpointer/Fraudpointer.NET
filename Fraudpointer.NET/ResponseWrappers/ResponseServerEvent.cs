using Fraudpointer.API.Models;
using Newtonsoft.Json;

namespace Fraudpointer.API.Responses
{
    /// <summary>
    ///   This class is to replicate the server request structure
    /// </summary>
    class ResponseEvent
    {
        /// <summary>
        ///  Base server request json object
        /// </summary>
        [JsonProperty(PropertyName = "event")]
        public Event Event { get; set; }
    }
}
