/**
 * RegexHelper
 */
export class RegexHelper {
    /**
     * 匹配十六进制字符串
     */
    static HexRegex = /^[0-9A-Fa-f]+$/;
    
    /**
     * 匹配八进制字符串
     */
    static OctRegex = /^[0-7]+$/;
    
    /**
     * 匹配二进制字符串
     */
    static BinRegex = /^[01]+$/;
    
    /**
     * 匹配日期时间格式：YYYY-MM-DD HH:MM:SS
     */
    static dateTimeRegex = /^(\d{4})-(1[012]|0?\d)-(30|31|[012]?\d) ([01]?\d|2[0123]):([012345]?\d):([012345]?\d)$/;
    
    /**
     * 匹配日期时间格式：YYYY/MM/DD HH:MM:SS
     */
    static dateTimeRegex2 = /^(\d{4})\/(1[012]|0?\d)\/(30|31|[012]?\d) ([01]?\d|2[0123]):([012345]?\d):([012345]?\d)$/;
    
    /**
     * 匹配日期时间格式：YYYY-MM-DD HH:MM
     */
    static dateTimeRegex3 = /^(\d{4})-(1[012]|0?\d)-(30|31|[012]?\d) ([01]?\d|2[0123]):([012345]?\d)$/;
    
    /**
     * 匹配日期时间格式：YYYY/MM/DD HH:MM
     */
    static dateTimeRegex4 = /^(\d{4})\/(1[012]|0?\d)\/(30|31|[012]?\d) ([01]?\d|2[0123]):([012345]?\d)$/
    
    /**
     * 匹配日期格式：YYYY-MM-DD
     */
    static dateRegex = /^(\d{4})-(1[012]|0?\d)-(30|31|[012]?\d)$/;
    
    /**
     * 匹配日期格式：YYYY-MM-DD
     */
    static dateRegex2 = /^(\d{4})-(1[012]|0?\d)-(30|31|[012]?\d)$/;
    
    /**
     * 匹配日时间格式：DD HH:MM:SS
     */
    static dayTimeRegex = /^(\d+) (2[0123]|[01]?\d):([012345]?\d):([012345]?\d)$/;
    
    /**
     * 匹配日时间格式：DD HH:MM
     */
    static dayTimeRegex2 = /^(\d+) (2[0123]|[01]?\d):([012345]?\d)$/;
    
    /**
     * 匹配时间格式：HH:MM:SS
     */
    static timeRegex = /^(2[0123]|[01]?\d):([012345]?\d):([012345]?\d)$/;
    
    /**
     * 匹配时间格式：HH:MM
     */
    static timeRegex2 = /^(2[0123]|[01]?\d):([012345]?\d)$/;
}
