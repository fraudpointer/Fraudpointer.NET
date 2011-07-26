namespace Fraudpointer.API.Clients
{
    public interface IHttpWrapper
    {
        /// <summary>
        /// Post an object as JSON to the given url
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="urlPath">Path to post to</param>
        /// <param name="thingToPost">Object to post in JSON</param>
        /// <returns>An object of the given type.</returns>
        T Post<T>(string urlPath,object thingToPost);

        /// <summary>
        /// Gets an object as JSON from the given url
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="urlPath">Path to get from</param>
        /// <returns>An object of the given type.</returns>
        T Get<T>(string urlPath);
    }
}