package toolgood.algorithm.internals;

import java.util.regex.Pattern;

final class RegexHelper {
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

    public static final Pattern dateTimeRegex = Pattern.compile("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d):([012345]?\\d)$");
    public static final Pattern dateTimeRegex2 = Pattern.compile("^(\\d{4})/(1[012]|0?\\d)/(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d):([012345]?\\d)$");
    public static final Pattern dateTimeRegex3 = Pattern.compile("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d)$");
    public static final Pattern dateTimeRegex4 = Pattern.compile("^(\\d{4})/(1[012]|0?\\d)/(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d)$");

    public static final Pattern dateRegex = Pattern.compile("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d)$");
    public static final Pattern dateRegex2 = Pattern.compile("^(\\d{4})/(1[012]|0?\\d)/(30|31|[012]?\\d)$");

    public static final Pattern dayTimeRegex = Pattern.compile("^(\\d+) (2[0123]|[01]?\\d):([012345]?\\d):([012345]?\\d)$");
    public static final Pattern dayTimeRegex2 = Pattern.compile("^(\\d+) (2[0123]|[01]?\\d):([012345]?\\d)$");

    public static final Pattern timeRegex = Pattern.compile("^(2[0123]|[01]?\\d):([012345]?\\d):([012345]?\\d)$");
    public static final Pattern timeRegex2 = Pattern.compile("^(2[0123]|[01]?\\d):([012345]?\\d)$");
}
