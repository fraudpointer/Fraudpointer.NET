using Fraudpointer.API.Helpers;

namespace Fraudpointer.API.Extensions
{
    /// <summary>
    /// Extension methods for the String class.
    /// </summary>
    public static class StringExtensions
    {
        public static string CreditCardHash(this string creditCardNumber)
        {
            const string salt = "HVK+gw==";
            var hasher = new SaltedHasher();
            return hasher.GetHashForSpecifiedSaltString(creditCardNumber, salt).HashString;
        } 
    }
}
