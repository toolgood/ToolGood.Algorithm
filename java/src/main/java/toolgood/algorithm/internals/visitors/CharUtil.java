package toolgood.algorithm.internals.visitors;

public final class CharUtil {
    public static char StandardChar(char o) {
        if (o < 'a') return o;
        if (o <= 'z') return (char) (o - 32);
        if (o < 127) return o;
        switch (o) {
            case '\u00D7': return '*';
            case '\u00F7': return '/';
            case '\u2018':
            case '\u2019': return '\'';
            case '\u201C':
            case '\u201D': return '"';
            case '\u3000': return ' ';
            case '\u3010': return '[';
            case '\u3011': return ']';
            case '\uFF08': return '(';
            case '\uFF09': return ')';
        }
        if (o > 65280 && o < 65375) {
            o = (char) (o - 65248);
        }
        return Character.toUpperCase(o);
    }

    public static boolean Equals(String left, char right) {
        if (left.length() != 1) return false;
        return left.charAt(0) == right || StandardChar(left.charAt(0)) == right;
    }

    public static boolean Equals(String left, String right) {
        if (left.length() != right.length()) return false;
        for (int i = 0; i < left.length(); i++) {
            char l = left.charAt(i);
            char r = right.charAt(i);
            if (l == r) continue;
            l = StandardChar(l);
            if (l != r) return false;
        }
        return true;
    }

    public static boolean Equals(String left, String option1, String option2, String option3) {
        return Equals(left, option1) || Equals(left, option2) || Equals(left, option3);
    }
}
