package toolgood.algorithm.internals;

public class CharUtil {
    public static char StandardChar(char c)
    {
        if (c <= 0) return c;
        char o = (char)c;
        if (o == '‘') return '\'';
        if (o == '’') return '\'';
        if (o == '“') return '"';
        if (o == '”') return '"';
        if (o == '〔') return '(';
        if (o == '〕') return ')';
        if (o == '＝') return '=';
        if (o == '＋') return '+';
        if (o == '－') return '-';
        if (o == '×') return '*';
        if (o == '÷') return '/';
        if (o == '／') return '/';

        if (c == 12288) {
            o = (char)32;
        } else if (c > 65280 && c < 65375) {
            o = (char)(c - 65248);
        }
        return Character.toUpperCase(o);
    }

    private static boolean EqualsOnce(String left, String right)
    {
        if (left.length() != right.length()) return false;
        for (int i = 0; i < left.length(); i++) {
            if (left.charAt(i)  != right.charAt(i)) {
                char a = StandardChar(left.charAt(i));
                char b = StandardChar(right.charAt(i));
                if (a != b) return false;
            }
        }
        return true;
    }

    public static boolean Equals(String left, String right)
    {
        if (left == null) return false;
        if (right == null) return false;
        return EqualsOnce(left, right);
    }
    public static boolean Equals(String left, String arg1, String arg2)
    {
        if (left == null) return false;
        if (arg1 != null && EqualsOnce(left, arg1))
            return true;
        if (arg2 != null && EqualsOnce(left, arg2))
            return true;
        return false;
    }

    public static boolean Equals(String left, String arg1, String arg2, String arg3)
    {
        if (left == null) return false;
        if (arg1 != null && EqualsOnce(left, arg1))
            return true;
        if (arg2 != null && EqualsOnce(left, arg2))
            return true;
        if (arg3 != null && EqualsOnce(left, arg3))
            return true;
        return false;
    }

}
