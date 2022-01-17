package toolgood.algorithm.internals;

import java.util.zip.CRC32;

import javax.crypto.Mac;
import javax.crypto.SecretKey;
import javax.crypto.spec.SecretKeySpec;

import java.security.InvalidKeyException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;

class Hash {
    public static String GetCrc32String(final byte[] buffer) {
        CRC32 c = new CRC32();
        c.reset();// Resets CRC-32 to initial value.
        c.update(buffer, 0, buffer.length);// 将数据丢入CRC32解码器
        int value = (int) c.getValue();
        return byteArrayToHexString(toHH(value));
    }

    public static String GetMd5String(final byte[] buffer) throws NoSuchAlgorithmException {
        MessageDigest m = MessageDigest.getInstance("MD5");
        m.update(buffer);
        byte[] result = m.digest();
        return byteArrayToHexString(result);
    }

    public static String GetSha1String(final byte[] buffer) throws NoSuchAlgorithmException {
        MessageDigest mDigest = MessageDigest.getInstance("SHA1");
        byte[] result = mDigest.digest(buffer);
        return byteArrayToHexString(result);
    }

    public static String GetSha256String(final byte[] buffer) throws NoSuchAlgorithmException {
        MessageDigest mDigest = MessageDigest.getInstance("SHA-256");
        byte[] result = mDigest.digest(buffer);
        return byteArrayToHexString(result);
    }

    public static String GetSha512String(final byte[] buffer) throws NoSuchAlgorithmException {
        MessageDigest mDigest = MessageDigest.getInstance("SHA-512");
        byte[] result = mDigest.digest(buffer);
        return byteArrayToHexString(result);
    }

    public static String GetHmacMd5String(final byte[] buffer) throws NoSuchAlgorithmException {
        MessageDigest mDigest = MessageDigest.getInstance("MD5");
        byte[] result = mDigest.digest(buffer);
        return byteArrayToHexString(result);    }

    public static String GetHmacMd5String(final byte[] buffer, final String secret) throws NoSuchAlgorithmException,
            InvalidKeyException {
        SecretKey secretKey = new SecretKeySpec(secret.getBytes(), "HmacMD5");
        Mac mac = Mac.getInstance(secretKey.getAlgorithm());
        mac.init(secretKey);
        byte[] result = mac.doFinal(buffer);
        return byteArrayToHexString(result);
    }

    public static String GetHmacSha1String(final byte[] buffer, final String secret)
            throws NoSuchAlgorithmException, InvalidKeyException
    {
        SecretKey secretKey = new SecretKeySpec(secret.getBytes(), "HmacSHA1");
        Mac mac = Mac.getInstance(secretKey.getAlgorithm());
        mac.init(secretKey);
        byte[] result= mac.doFinal(buffer);
        return byteArrayToHexString(result);
    }

    public static String GetHmacSha256String(final byte[] buffer, final String secret) throws NoSuchAlgorithmException,
            InvalidKeyException
    {
        SecretKey secretKey = new SecretKeySpec(secret.getBytes(), "HmacSHA256");
        Mac mac = Mac.getInstance(secretKey.getAlgorithm());
        mac.init(secretKey);
        byte[] result= mac.doFinal(buffer);
        return byteArrayToHexString(result);
    }

    public static String GetHmacSha512String(final byte[] buffer, final String secret) throws NoSuchAlgorithmException,
            InvalidKeyException
    {
        SecretKey secretKey = new SecretKeySpec(secret.getBytes(), "HmacSHA512");
        Mac mac = Mac.getInstance(secretKey.getAlgorithm());
        mac.init(secretKey);
        byte[] result= mac.doFinal(buffer);
        return byteArrayToHexString(result);
    }

      private static byte[] toHH(int n) {  
        byte[] b = new byte[4];  
        b[3] = (byte) (n & 0xff);  
        b[2] = (byte) (n >> 8 & 0xff);  
        b[1] = (byte) (n >> 16 & 0xff);  
        b[0] = (byte) (n >> 24 & 0xff);  
        return b;  
      }

    private static String byteArrayToHexString(byte[] b) {
        StringBuffer sb = new StringBuffer(b.length * 2);
        for (int i = 0; i < b.length; i++) {
            int v = b[i] & 0xff;
            if (v < 16) {
                sb.append('0');
            }
            sb.append(Integer.toHexString(v));
        }
        return sb.toString().toUpperCase();
    }

}
