/**
 * 正则表达式帮助类
 */
package toolgood.algorithm.internals;

import java.util.regex.Pattern;

public class RegexHelper {
    /**
     * 十六进制正则表达式
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
     * 日期时间正则表达式 (yyyy-MM-dd HH:mm:ss)
     */
    public static final Pattern dateTimeRegex = Pattern.compile("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d):([012345]?\\d)$");
    
    /**
     * 日期时间正则表达式 (yyyy/MM/dd HH:mm:ss)
     */
   public static final Pattern dateTimeRegex2 = Pattern.compile("^(\\d{4})/(1[012]|0?\\d)/(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d):([012345]?\\d)$");
    
    /**
     * 日期时间正则表达式 (yyyy-MM-dd HH:mm)
     */
   public static final Pattern dateTimeRegex3 = Pattern.compile("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d)$");
    
    /**
     * 日期时间正则表达式 (yyyy/MM/dd HH:mm)
     */
  public  static final Pattern dateTimeRegex4 = Pattern.compile("^(\\d{4})/(1[012]|0?\\d)/(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d)$");

    /**
     * 日期正则表达式 (yyyy-MM-dd)
     */
  public  static final Pattern dateRegex = Pattern.compile("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d)$");
    
    /**
     * 日期正则表达式 (yyyy-MM-dd)
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
     * 时间正则表达式 (HH:mm:ss)
     */
  public static final Pattern timeRegex = Pattern.compile("^(2[0123]|[01]?\\d):([012345]?\\d):([012345]?\\d)$");
    
    /**
     * 时间正则表达式 (HH:mm)
     */
  public  static final Pattern timeRegex2 = Pattern.compile("^(2[0123]|[01]?\\d):([012345]?\\d)$");
}
