using System;
using System.Security.Cryptography;
using System.Text;

namespace Fraudpointer.API.Helpers
{
    class SaltedHasher
    {
        HashAlgorithm HashProvider;
        int SaltLength;

        /// <summary>
        /// The constructor takes a HashAlgorithm as a parameter.
        /// </summary>
        /// <param name="hashAlgorithm">
        /// A <see cref="HashAlgorithm"/> hashAlgorihm which is derived from HashAlgorithm. C# provides
        /// the following classes: SHA1Managed,SHA256Managed, SHA384Managed, SHA512Managed and MD5CryptoServiceProvider
        /// </param>

        public SaltedHasher(HashAlgorithm hashAlgorithm, int theSaltLength)
        {
            HashProvider = hashAlgorithm;
            SaltLength = theSaltLength;
        }

        /// <summary>
        /// Default constructor which initialises the SaltedHash with the SHA256Managed algorithm
        /// and a Salt of 4 bytes ( or 4*8 = 32 bits)
        /// </summary>

        public SaltedHasher()
            : this(new SHA256Managed(), 4)
        {
        }

        /// <summary>
        /// The routine provides a hash value as a string for a given data string and salt string
        /// </summary>
        /// <param name="data">data</param>
        /// <param name="SaltString">Supplied Salt value assumed to be base64 encoded string</param>
        public HashAndSalt GetHashForSpecifiedSaltString(string data, string SaltString)
        {

            byte[] Salt = Convert.FromBase64String(SaltString);

            return new HashAndSalt(ComputeHash(Encoding.UTF8.GetBytes(data), Salt), Salt);

        }

        /// <summary>
        /// The actual hash calculation is shared by both GetHashAndSalt and the VerifyHash functions
        /// </summary>
        /// <param name="Data">A byte array of the Data to Hash</param>
        /// <param name="Salt">A byte array of the Salt to add to the Hash</param>
        /// <returns>A byte array with the calculated hash</returns>

        private byte[] ComputeHash(byte[] Data, byte[] Salt)
        {
            // Allocate memory to store both the Data and Salt together
            byte[] DataAndSalt = new byte[Data.Length + SaltLength];

            // Copy both the data and salt into the new array
            Array.Copy(Data, DataAndSalt, Data.Length);
            Array.Copy(Salt, 0, DataAndSalt, Data.Length, SaltLength);

            // Calculate the hash
            // Compute hash value of our plain text with appended salt.
            return HashProvider.ComputeHash(DataAndSalt);
        }
    }
}
