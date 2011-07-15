using System;

namespace Fraudpointer.API.Helpers
{
    class HashAndSalt
    {
        public byte[] Hash { get; set; }
        public byte[] Salt { get; set; }
        public string HashString
        {
            get { return Convert.ToBase64String(Hash); }
        }
        public string SaltString
        {
            get { return Convert.ToBase64String(Salt); }
        }

        public HashAndSalt()
        {
        }

        public HashAndSalt(byte[] hash, byte[] salt)
        {
            Hash = hash;
            Salt = salt;
        }
    }
}
