using Newtonsoft.Json;

namespace Fraudpointer.API.RequestWrappers
{
    class RequestKey
    {
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }
    }
}
