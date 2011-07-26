using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fraudpointer.API.Clients
{
    /// <summary>
    /// Client string extensions
    /// </summary>
    static class StringExtensions
    {
        /// <summary>
        /// Deserializes a JSON String.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T DeserializeJson<T>(this string src)
        {
            //Note: the ida behind this simple method was to wrap how deserialization 
            //is done in order to test it later without copying and pasting code.
            return JsonConvert.DeserializeObject<T>(src, new IsoDateTimeConverter());
        }
    }
}
