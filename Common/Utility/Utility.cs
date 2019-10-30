using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utility
{
    public class Utility
    {
        public static string GenerateAPIKey()
        {

            string APIKey = "";
            using (var cryptoProvider = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                byte[] secretKeyByteArray = new byte[32]; //256 bit
                cryptoProvider.GetBytes(secretKeyByteArray);
                APIKey = System.Convert.ToBase64String(secretKeyByteArray);
            }

            return APIKey;
        }


    }
}
