using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ToolGood.Algorithm
{
    /// <summary>
    /// Modified Base64 for URL applications ('base64url' encoding)
    /// 
    /// See http://tools.ietf.org/html/rfc4648
    /// For more information see http://en.wikipedia.org/wiki/Base64
    /// </summary>
    static class Base64
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
        /// <summary>
        /// Modified Base64 for URL applications ('base64url' encoding)
        /// 
        /// See http://tools.ietf.org/html/rfc4648
        /// For more information see http://en.wikipedia.org/wiki/Base64
        /// </summary>
        /// <param name="base64ForUrlInput"></param>
        /// <returns>Input base64ForUrl encoded string as the original byte array</returns>
        private const string base64 = "===========================================+=+=/0123456789=======ABCDEFGHIJKLMNOPQRSTUVWXYZ====/=abcdefghijklmnopqrstuvwxyz=====";
        public static byte[] FromBase64ForUrlString(string base64ForUrlInput)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in base64ForUrlInput) {
                if ((int)c >= 128) continue;
                var k = base64[c];
                if (k == '=') continue;
                sb.Append(k);
            }
            var len = sb.Length;
            int padChars = (len % 4) == 0 ? 0 : (4 - (len % 4));
            if (padChars > 0) sb.Append(String.Empty.PadRight(padChars, '='));
            return Convert.FromBase64String(sb.ToString());

            //base64ForUrlInput = Regex.Replace(base64ForUrlInput, @"[^a-zA-Z0-9+/\-_]", "");

            //int padChars = (base64ForUrlInput.Length % 4) == 0 ? 0 : (4 - (base64ForUrlInput.Length % 4));
            //StringBuilder result = new StringBuilder(base64ForUrlInput, base64ForUrlInput.Length + padChars);
            //result.Append(String.Empty.PadRight(padChars, '='));
            //result.Replace('-', '+');
            //result.Replace('_', '/');
            //return Convert.FromBase64String(result.ToString());
        }
    }
}
