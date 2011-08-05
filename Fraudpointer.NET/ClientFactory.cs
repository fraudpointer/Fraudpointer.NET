using Fraudpointer.API.Clients;

namespace Fraudpointer.API
{
    /// <summary>
    /// This is a helping class that instantiates an object that implements IClient interface.
    /// </summary>
    /// <remarks>
    /// Use this method to start with FraudPointer API. It will give you an object that properly implements IClient interface.
    /// </remarks>    
    public class ClientFactory
    {
        /// <summary>
        /// Instantiates an object that properly implements IClient interface.
        /// </summary>
        /// <remarks>
        /// Start your FraudPointer API usage from here. It will give you an instance of an object that properly implements the IClient interface.
        /// 
        /// Here is an example of usage:
        /// <code>
        /// IClient client = ClientFactory.Construct("https://production.fraudpointer.com/api",
        ///                                          "8312453678901abcdef123456789328");    
        /// </code>
        /// Note that the object returned uses a default timeout of 5 seconds in its communication with the Fraudpointer Server.
        /// If you want to set the timeout in a different value, use the ClientFactory.Construct(string baseUrl, string apiKey, int webRequestTimeout) method
        /// instead, giving the timeout (in milliseconds) as an input parameter.
        /// </remarks>
        /// <param name="baseUrl">This is the URL of the FraudPointer API Service. It has to have the value:
        /// <code>https://production.fraudpointer.com/api</code>
        /// </param>
        /// <param name="apiKey">This should be the API KEY that corresponds to the domain that you integrate FraudPointer API for.
        /// The API KEY is automatically generated when you register an Account in <a href="https://production.fraudpointer.com/a/new" target="_blank">FraudPointer Registration Web Form</a>.
        /// Note that your Account in FraudPointer service might have more than one domains registered. You need to provide here the API KEY that
        /// corresponds to the domain that you are going to integrate this client with.</param>
        /// <returns>A valid IClient instantiated object. This is a newly created object to work with the FraudPointer Service.</returns>
        public static IClient Construct(string baseUrl, string apiKey)
        {

            return new Client(new HttpWrapper(baseUrl), apiKey);            

        } // Construct static method
        //---------------------------

        /// <summary>
        /// Similar to ClientFactory.Construct(string baseUrl, string apiKey) but takes as extra input a web request timeout in
        /// milliseconds.
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="apiKey"></param>
        /// <param name="webRequestTimeout"></param>
        /// <returns></returns>
        public static IClient Construct(string baseUrl, string apiKey, int webRequestTimeout)
        {

            return new Client(new HttpWrapper(baseUrl, webRequestTimeout), apiKey);

        } // Construct static method that takes as input a webRequestTimeout in milliseconds
        //----------------------------------------------------------------------------------

    } // class ClientFactory
    //-----------------------

} // namespace Fraudpointer.API
//------------------------------
