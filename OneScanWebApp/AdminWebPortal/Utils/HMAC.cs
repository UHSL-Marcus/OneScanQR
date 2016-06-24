using System.Security.Cryptography;
using System.Text;

namespace AdminWebPortal.Utils
{
    class HMAC
    {
        public static string Hash(string input, string key)
        {
            byte[] hash = null;
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            using (HMACSHA1 hmac = new HMACSHA1(keyBytes))
            {
                hash = hmac.ComputeHash(inputBytes);
            }

            StringBuilder hex = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
                hex.AppendFormat("{0:x2}", b);

            return hex.ToString();
        }

        public static bool ValidateHash(string input, string key, string hmac)
        {
            string calcHmac = Hash(input, key);
            if (calcHmac.Equals(hmac))
                return true;
            else
                return false;
        }
    }
}
