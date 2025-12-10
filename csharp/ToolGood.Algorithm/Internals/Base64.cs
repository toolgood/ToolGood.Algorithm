using System;
using System.Text;

namespace ToolGood.Algorithm.Internals
{
    /// <summary>
    /// Modified Base64 for URL applications ('base64url' encoding)
    ///
    /// See http://tools.ietf.org/html/rfc4648
    /// For more information see http://en.wikipedia.org/wiki/Base64
    /// </summary>
    internal static class Base64
    {
        public static string ToBase64String(byte[] input)
        {
            return Convert.ToBase64String(input);
        }

        public static byte[] FromBase64String(string base64)
        {
            return FromBase64ForUrlString(base64);
        }

        /// <summary>
        /// Modified Base64 for URL applications ('base64url' encoding)
        ///
        /// See http://tools.ietf.org/html/rfc4648
        /// For more information see http://en.wikipedia.org/wiki/Base64
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Input byte array converted to a base64ForUrl encoded string</returns>
        public static string ToBase64ForUrlString(byte[] input)
        {
            StringBuilder result = new StringBuilder(Convert.ToBase64String(input).TrimEnd('='));
            result.Replace('+', '-');
            result.Replace('/', '_');
            return result.ToString();
        }

        private const string base64 = "===========================================+=+=/0123456789=======ABCDEFGHIJKLMNOPQRSTUVWXYZ====/=abcdefghijklmnopqrstuvwxyz=====";

        /// <summary>
        /// Modified Base64 for URL applications ('base64url' encoding)
        ///
        /// See http://tools.ietf.org/html/rfc4648
        /// For more information see http://en.wikipedia.org/wiki/Base64
        /// </summary>
        /// <param name="base64ForUrlInput"></param>
        /// <returns>Input base64ForUrl encoded string as the original byte array</returns>
        public static byte[] FromBase64ForUrlString(string base64ForUrlInput)
        {
            var sb = new StringBuilder();
            foreach (var c in base64ForUrlInput) {
                if ((int)c >= 128) continue;
                var k = base64[c];
                if (k == '=') continue;
                sb.Append(k);
            }
            var len = sb.Length;
            int padChars = (len % 4) == 0 ? 0 : (4 - (len % 4));
            if (padChars > 0) sb.Append(string.Empty.PadRight(padChars, '='));
            return Convert.FromBase64String(sb.ToString());
        }
    }
}