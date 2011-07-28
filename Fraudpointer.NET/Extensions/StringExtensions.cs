using Fraudpointer.API.Helpers;

namespace Fraudpointer.API.Extensions
{
    /// <summary>
    /// Extension methods for the String class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Use this method to generate a hash of a credit card number. Use the generated hash to send the encrypted credit card number
        /// to FraudPointer Server instead of the credit card number itself.
        /// </summary>
        /// <remarks>
        /// FraudPointer Server tries to identify the existence of the same credit card number in various transactions. These
        /// transactions either take place during the same session or take place in different sessions, but at the same time, or 
        /// took place in a session in the past. However, FraudPointer Server does not want to store the credit card numbers in clear format 
        /// and it does not need to do that in order to accomplish its goal. Hence, you need to encrypt them using the method provided here.
        /// Note that hash is one-way encryption method and FraudPointer Server cannot derive the credit card number from the hash.
        /// 
        /// This method does not communicate with the FraudPointer Server to generate the hash. Works locally.
        /// </remarks>
        /// <param name="creditCardNumber">The credit card number that you want to get its hash value.</param>
        /// <returns>The hash of the credit card number.</returns>
        /// <exception cref="API.ClientException">It may throw a ClientException if an error occurs</exception>>
        public static string CreditCardHash(this string creditCardNumber)
        {
            const string salt = "HVK+gw==";
            var hasher = new SaltedHasher();
            return hasher.GetHashForSpecifiedSaltString(creditCardNumber, salt).HashString;
        } 
    }
}
