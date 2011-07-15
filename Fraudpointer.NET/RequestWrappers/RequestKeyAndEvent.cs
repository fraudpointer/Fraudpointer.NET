using Fraudpointer.API.Models;
using Newtonsoft.Json;

namespace Fraudpointer.API.RequestWrappers
{
    class RequestKeyAndEvent : RequestKey
    {
        [JsonProperty(PropertyName = "event")]
        public Event Event { get; set; }
    }
}
