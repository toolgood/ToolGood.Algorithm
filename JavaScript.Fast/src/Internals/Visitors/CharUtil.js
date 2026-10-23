/**
 * CharUtil
 */
export class CharUtil {
    /**
     * Standardizes a character by converting it to uppercase and handling special characters.
     * @param {string} o - The character to standardize.
     * @returns {string} The standardized character.
     */
    static standardChar(o) {
        if (typeof o !== 'string' || o.length !== 1) return o;
        let charCode = o.charCodeAt(0);
        if (charCode === 13) return '\n';                                   // \r
        if (charCode === 9) return ' ';                                     // \t
        if (charCode === 12) return ' ';                                    // \f
        if (charCode < 97) return o;                                        // < 'a'
        if (charCode <= 122) return String.fromCharCode(charCode - 32);     // a-z -> A-Z
        if (charCode > 65280 && charCode < 65375) {                         // 全角(65281-65374) -> 半角
            o = String.fromCharCode(charCode - 65248).toUpperCase();
            charCode = o.charCodeAt(0);
        }
        // 对齐 C# StandardChar2: CJK/希腊/西里尔等区段统一映射为 '_'
        if ((charCode >= 0xc0 && charCode <= 0xd6) ||                       // U+00C0-U+00D6
            (charCode >= 0xd8 && charCode <= 0xf6) ||                       // U+00D8-U+00F6
            (charCode >= 0xf8 && charCode <= 0xff) ||                       // U+00F8-U+00FF
            (charCode >= 0x100 && charCode <= 0x1fff) ||                    // U+0100-U+1FFF
            (charCode >= 0x2c00 && charCode <= 0x2fff) ||                   // U+2C00-U+2FFF
            (charCode >= 0x3040 && charCode <= 0x318f) ||                   // U+3040-U+318F
            (charCode >= 0x3300 && charCode <= 0x337f) ||                   // U+3300-U+337F
            (charCode >= 0x3400 && charCode <= 0x3fff) ||                   // U+3400-U+3FFF
            (charCode >= 0x4e00 && charCode <= 0x9fff) ||                   // U+4E00-U+9FFF
            (charCode >= 0xa000 && charCode <= 0xd7ff) ||                   // U+A000-U+D7FF
            (charCode >= 0xf900 && charCode <= 0xfaff) ||                   // U+F900-U+FAFF
            (charCode >= 0xff00 && charCode <= 0xfff0)) {                   // U+FF00-U+FFF0
            return '_';
        }
        if (charCode < 127) return o;
        if (charCode === 215) return '*';                                   // ×
        if (charCode === 247) return '/';                                   // ÷
        if (charCode === 8216 || charCode === 8217) return '\'';            // ‘ ’
        if (charCode === 8220 || charCode === 8221) return '"';             // “ ”
        if (charCode === 12288) return ' ';                                 // 全角空格
        if (charCode === 12304) return '[';                                 // 【
        if (charCode === 12305) return ']';                                 // 】
        if (charCode === 12308) return '(';                                 // （
        if (charCode === 12309) return ')';                                 // ）
        return o.toUpperCase();
    }

    /**
     * Standardizes a string by converting each character to uppercase and handling special characters.
     * @param {string} s - The string to standardize.
     * @returns {string} The standardized string.
     */
    static standardString(s) {
        var sb = [];
        for(var i = 0; i < s.length; i++) {
            sb.push(this.standardChar(s[i]));
        }
        return sb.join('');
    }

    /**
     * Compares a string with a character for equality after standardization.
     * @param {string} left - The string to compare.
     * @param {string} right - The character to compare with.
     * @returns {boolean} True if the string and character are equal after standardization, false otherwise.
     */
    static equals(left, right) {
        if(left.length != right.length) return false;
        for(var i = 0; i < left.length; i++) {
            if(left[i] != right[i]) {
                var a = this.standardChar(left[i]);
                if(a != right[i]) return false;
            }
        }
        return true;
    }

    /**
     * Compares a string with two other strings for equality after standardization.
     * @param {string} left - The string to compare.
     * @param {string} arg1 - The first string to compare with.
     * @param {string} arg2 - The second string to compare with.
     * @returns {boolean} True if the string is equal to either of the other strings after standardization, false otherwise.
     */
    static equals3(left, arg1, arg2) {
        if(this.equals(left, arg1)) return true;
        if(this.equals(left, arg2)) return true;
        return false;
    }

    /**
     * Compares a string with three other strings for equality after standardization.
     * @param {string} left - The string to compare.
     * @param {string} arg1 - The first string to compare with.
     * @param {string} arg2 - The second string to compare with.
     * @param {string} arg3 - The third string to compare with.
     * @returns {boolean} True if the string is equal to any of the other strings after standardization, false otherwise.
     */
    static equals4(left, arg1, arg2, arg3) {
        if(this.equals(left, arg1)) return true;
        if(this.equals(left, arg2)) return true;
        if(this.equals(left, arg3)) return true;
        return false;
    }
}