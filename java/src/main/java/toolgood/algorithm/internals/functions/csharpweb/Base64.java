package toolgood.algorithm.internals.functions.csharpweb;

import java.util.Base64;

/**
 * Modified Base64 for URL applications ('base64url' encoding)
 *
 * See http://tools.ietf.org/html/rfc4648
 * For more information see http://en.wikipedia.org/wiki/Base64
 */
public class Base64 {
    private static final Base64.Encoder encoder = Base64.getEncoder();
    private static final Base64.Decoder decoder = Base64.getDecoder();
    private static final Base64.Encoder urlEncoder = Base64.getUrlEncoder().withoutPadding();
    private static final Base64.Decoder urlDecoder = Base64.getUrlDecoder();
    private static final String base64 = "===========================================+=+=/0123456789=======ABCDEFGHIJKLMNOPQRSTUVWXYZ====/=abcdefghijklmnopqrstuvwxyz=====";

    public static String ToBase64String(byte[] input) {
        return encoder.encodetoString(input);
    }

    public static byte[] FromBase64String(String base64Str) {
        return decoder.decode(base64Str);
    }

    /**
     * Modified Base64 for URL applications ('base64url' encoding)
     *
     * See http://tools.ietf.org/html/rfc4648
     * For more information see http://en.wikipedia.org/wiki/Base64
     * @param input
     * @return Input byte array converted to a base64ForUrl encoded string
     */
    public static String ToBase64ForUrlString(byte[] input) {
        return urlEncoder.encodetoString(input);
    }

    /**
     * Modified Base64 for URL applications ('base64url' encoding)
     *
     * See http://tools.ietf.org/html/rfc4648
     * For more information see http://en.wikipedia.org/wiki/Base64
     * @param base64ForUrlInput
     * @return Input base64ForUrl encoded string as the original byte array
     */
    public static byte[] FromBase64ForUrlString(String base64ForUrlInput) {
        return urlDecoder.decode(base64ForUrlInput);
    }
}
