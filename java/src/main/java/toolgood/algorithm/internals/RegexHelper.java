/**
 * 正则表达式帮助类
 */
package toolgood.algorithm.internals;

import java.util.regex.Pattern;

public class RegexHelper {
    /**
     * 十六进制正则表达�?
     */
   public static final Pattern HexRegex = Pattern.compile("^[0-9A-Fa-f]+$");
    
    /**
     * 八进制正则表达式
     */
  public  static final Pattern OctRegex = Pattern.compile("^[0-7]+$");
    
    /**
     * 二进制正则表达式
     */
  public  static final Pattern BinRegex = Pattern.compile("^[01]+$");

    /**
     * 日期时间正则表达�?(yyyy-MM-dd HH:mm:ss)
     */
    public static final Pattern dateTimeRegex = Pattern.compile("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d):([012345]?\\d)$");
    
    /**
     * 日期时间正则表达�?(yyyy/MM/dd HH:mm:ss)
     */
   public static final Pattern dateTimeRegex2 = Pattern.compile("^(\\d{4})/(1[012]|0?\\d)/(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d):([012345]?\\d)$");
    
    /**
     * 日期时间正则表达�?(yyyy-MM-dd HH:mm)
     */
   public static final Pattern dateTimeRegex3 = Pattern.compile("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d)$");
    
    /**
     * 日期时间正则表达�?(yyyy/MM/dd HH:mm)
     */
  public  static final Pattern dateTimeRegex4 = Pattern.compile("^(\\d{4})/(1[012]|0?\\d)/(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d)$");

    /**
     * 日期正则表达�?(yyyy-MM-dd)
     */
  public  static final Pattern dateRegex = Pattern.compile("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d)$");
    
    /**
     * 日期正则表达�?(yyyy-MM-dd)
     */
   public static final Pattern dateRegex2 = Pattern.compile("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d)$");

    /**
     * 天时间正则表达式 (dd HH:mm:ss)
     */
  public  static final Pattern dayTimeRegex = Pattern.compile("^(\\d+) (2[0123]|[01]?\\d):([012345]?\\d):([012345]?\\d)$");
    
    /**
     * 天时间正则表达式 (dd HH:mm)
     */
  public  static final Pattern dayTimeRegex2 = Pattern.compile("^(\\d+) (2[0123]|[01]?\\d):([012345]?\\d)$");

    /**
     * 时间正则表达�?(HH:mm:ss)
     */
  public static final Pattern timeRegex = Pattern.compile("^(2[0123]|[01]?\\d):([012345]?\\d):([012345]?\\d)$");
    
    /**
     * 时间正则表达式(HH:mm)
     */
  public  static final Pattern timeRegex2 = Pattern.compile("^(2[0123]|[01]?\\d):([012345]?\\d)$");

    public static boolean IsHex(String value) {
        if (value == null || value.isEmpty()) return false;
        for (char c : value.toCharArray()) {
            if (!IsHexChar(c)) return false;
        }
        return true;
    }

    private static boolean IsHexChar(char c) {
        return (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f');
    }

    public static boolean IsOct(String value) {
        if (value == null || value.isEmpty()) return false;
        for (char c : value.toCharArray()) {
            if (!IsOctChar(c)) return false;
        }
        return true;
    }

    private static boolean IsOctChar(char c) {
        return c >= '0' && c <= '7';
    }

    public static boolean IsBin(String value) {
        if (value == null || value.isEmpty()) return false;
        for (char c : value.toCharArray()) {
            if (!IsBinChar(c)) return false;
        }
        return true;
    }

    private static boolean IsBinChar(char c) {
        return c == '0' || c == '1';
    }
}
