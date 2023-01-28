using System.Security.Cryptography;
using System.Text;

namespace CRD.Utils
{
    public static class StringUtils
    {
        public static String GetHash<T>(this string stringToHash) where T : HashAlgorithm
        {
            using (var stream = GenerateStreamFromString(stringToHash))
            {
                return GetHash<T>(stream);
            }
        }
        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static String GetHash<T>(this Stream stream) where T : HashAlgorithm
        {
            var sb = new StringBuilder();
            HashAlgorithm crypt = null;

            if (typeof(T) == typeof(MD5))
                crypt = MD5.Create();
            if (typeof(T) == typeof(SHA1))
                crypt = SHA1.Create();
            if (typeof(T) == typeof(SHA256))
                crypt = SHA256.Create();
            if (typeof(T) == typeof(SHA512))
                crypt = SHA512.Create();

            var hashBytes = crypt.ComputeHash(stream);
            foreach (byte bt in hashBytes)
            {
                sb.Append(bt.ToString("x2"));
            }

            crypt.Dispose();
            return sb.ToString();
        }
    }
}
