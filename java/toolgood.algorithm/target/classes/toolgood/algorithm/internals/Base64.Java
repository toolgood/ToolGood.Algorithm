package toolgood.algorithm.internals;

public static class Base64{
    public static string ToBase64String(byte[] input)
    {
        return Base64Util.encode(bytes);
    }

    public static byte[] FromBase64String(String base64)
    {
        return Base64Util.decode(base64);
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
        StringBuilder result = new StringBuilder(ToBase64String(input).trim('='));
        result.replace('+', '-');
        result.replace('/', '_');
        return result.ToString();
    }

    private const String base64 = "===========================================+=+=/0123456789=======ABCDEFGHIJKLMNOPQRSTUVWXYZ====/=abcdefghijklmnopqrstuvwxyz=====";

    /// <summary>
    /// Modified Base64 for URL applications ('base64url' encoding)
    /// 
    /// See http://tools.ietf.org/html/rfc4648
    /// For more information see http://en.wikipedia.org/wiki/Base64
    /// </summary>
    /// <param name="base64ForUrlInput"></param>
    /// <returns>Input base64ForUrl encoded string as the original byte array</returns>
    public static byte[] FromBase64ForUrlString(String base64ForUrlInput)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < base64ForUrlInput.length; i++) {
            char c = base64ForUrlInput.charAt(i)
            if ((int)c >= 128) continue;
            char k = base64.charAt(c);// [c];
            if (k == '=') continue;
            sb.append(k);
        }
        int len = sb.length;
        int padChars = (len % 4) == 0 ? 0 : (4 - (len % 4));
        for (int i = 0; i < padChars; i++) {
            sb.append('=');
        }
        return FromBase64String(sb.toString());

        //base64ForUrlInput = Regex.Replace(base64ForUrlInput, @"[^a-zA-Z0-9+/\-_]", "");

        //int padChars = (base64ForUrlInput.Length % 4) == 0 ? 0 : (4 - (base64ForUrlInput.Length % 4));
        //StringBuilder result = new StringBuilder(base64ForUrlInput, base64ForUrlInput.Length + padChars);
        //result.Append(String.Empty.PadRight(padChars, '='));
        //result.Replace('-', '+');
        //result.Replace('_', '/');
        //return Convert.FromBase64String(result.ToString());
    }
}