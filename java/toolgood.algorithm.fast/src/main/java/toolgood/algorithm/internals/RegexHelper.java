package toolgood.algorithm.internals;

public final class RegexHelper {
    public static boolean IsHex(String value) {
        if (value == null || value.isEmpty()) return false;
        for (int i = 0; i < value.length(); i++) {
            if (!IsHexChar(value.charAt(i))) return false;
        }
        return true;
    }

    private static boolean IsHexChar(char c) {
        return (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f');
    }

    public static boolean IsOct(String value) {
        if (value == null || value.isEmpty()) return false;
        for (int i = 0; i < value.length(); i++) {
            if (!IsOctChar(value.charAt(i))) return false;
        }
        return true;
    }

    private static boolean IsOctChar(char c) {
        return c >= '0' && c <= '7';
    }

    public static boolean IsBin(String value) {
        if (value == null || value.isEmpty()) return false;
        for (int i = 0; i < value.length(); i++) {
            if (!IsBinChar(value.charAt(i))) return false;
        }
        return true;
    }

    private static boolean IsBinChar(char c) {
        return c == '0' || c == '1';
    }

    // 日期/时间解析已改为 MyDate.parse 的字符扫描，原先 10 条日期正则已删除。
}
