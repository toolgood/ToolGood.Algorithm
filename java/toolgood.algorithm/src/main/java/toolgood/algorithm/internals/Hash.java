package toolgood.algorithm.internals;

import toolgood.algorithm.internals.Crc8Hash;
import toolgood.algorithm.internals.Crc16Hash;
import toolgood.algorithm.internals.MD5Hash;
import java.util.zip.CRC32;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;

static class Hash {
    public static String GetCrc8String(final byte[] buffer) {
        final int b = Crc8Hash.calcCrc8(buffer);
        return BitConverter.ToString(new byte[] { b });
    }

    public static String GetCrc16String(final byte[] buffer) {
        final int buffer1 = Crc16Hash.CRC16_CCITT(buffer);
        return BitConverter.ToString(buffer1);
    }

    public static String GetCrc32String(final byte[] buffer) {
        CRC32 c = new CRC32();
        c.reset();// Resets CRC-32 to initial value.
        c.update(buffer, 0, buffer.length);// 将数据丢入CRC32解码器
        int value = (int) c.getValue();

        // final Crc32Hash crc32 = new Crc32Hash();
        // crc32.Append(buffer);
        // final byte[] retVal = crc32.Finish();
        return BitConverter.ToString(value).Replace("-", "");
    }

    public static String GetMd5String(final byte[] buffer) {
        return MD5Hash.encrypt(buffer);
    }

    public static String GetSha1String(final byte[] buffer) {
        final SHA1 sha512 = new SHA1Managed();
        final byte[] retVal = sha512.ComputeHash(buffer); // 计算指定Stream 对象的哈希值
        sha512.Dispose();
        return BitConverter.ToString(retVal).Replace("-", "");
    }

    public static String GetSha256String(final byte[] buffer) {
        final SHA256 sha512 = new SHA256Managed();
        final byte[] retVal = sha512.ComputeHash(buffer); // 计算指定Stream 对象的哈希值
        sha512.Dispose();
        return BitConverter.ToString(retVal).Replace("-", "");
    }

    public static String GetSha512String(final byte[] buffer) {
        final SHA512 sha512 = new SHA512Managed();
        final byte[] retVal = sha512.ComputeHash(buffer); // 计算指定Stream 对象的哈希值
        sha512.Dispose();
        return BitConverter.ToString(retVal).Replace("-", "");
    }

    public static String GetHmacMd5String(final byte[] buffer, final String secret)
    {
        final byte[] keyByte = System.Text.Encoding.UTF8.GetBytes(secret ?? "");
        using (HMACMD5 hmacsha256 = new HMACMD5(keyByte)) {
            byte[] hashmessage = hmacsha256.ComputeHash(buffer);
            return BitConverter.ToString(hashmessage).Replace("-", "");
        }
    }

    public static String GetHmacSha1String(final byte[] buffer, final String secret)
    {
        final byte[] keyByte = System.Text.Encoding.UTF8.GetBytes(secret ?? "");
        using (HMACSHA1 hmacsha256 = new HMACSHA1(keyByte)) {
            byte[] hashmessage = hmacsha256.ComputeHash(buffer);
            return BitConverter.ToString(hashmessage).Replace("-", "");
        }
    }

    public static String GetHmacSha256String(final byte[] buffer, final String secret)
    {
        final byte[] keyByte = System.Text.Encoding.UTF8.GetBytes(secret ?? "");
        using (HMACSHA256 hmacsha256 = new HMACSHA256(keyByte)) {
            byte[] hashmessage = hmacsha256.ComputeHash(buffer);
            return BitConverter.ToString(hashmessage).Replace("-", "");
        }
    }

    public static String GetHmacSha512String(final byte[] buffer, final String secret)
    {
        final byte[] keyByte = System.Text.Encoding.UTF8.GetBytes(secret ?? "");
        using (HMACSHA512 hmacsha256 = new HMACSHA512(keyByte)) {
            byte[] hashmessage = hmacsha256.ComputeHash(buffer);
            return BitConverter.ToString(hashmessage).Replace("-", "");
        }
    }

}
