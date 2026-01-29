/**
 * CharUtil
 */
export class CharUtil {
    /**
     * Standardizes a character by converting it to uppercase and handling special characters.
     * @param {string} o - The character to standardize.
     * @returns {string} The standardized character.
     */
    static StandardChar(o) {
        if(o <= 'Z') return o;
        if(o.charCodeAt(0) <= 127) return o.toUpperCase();
        if(o.charCodeAt(0) <= 65280) {
            if(o == '×') return '*';//215
            if(o == '÷') return '/';//247
            if(o == '‘') return '\'';//8216
            if(o == '’') return '\'';//8217
            if(o == '“') return '"';//8220
            if(o == '”') return '"';//8221
            if(o.charCodeAt(0) == 12288) return String.fromCharCode(32);
            if(o == '【') return '[';//12304
            if(o == '】') return ']';//12305
            if(o == '〔') return '(';//12308
            if(o == '〕') return ')';//12309
            return o;
        } else if(o.charCodeAt(0) < 65375) {
            o = String.fromCharCode(o.charCodeAt(0) - 65248);
        }
        return o.toUpperCase();
    }

    /**
     * Standardizes a string by converting each character to uppercase and handling special characters.
     * @param {string} s - The string to standardize.
     * @returns {string} The standardized string.
     */
    static StandardString(s) {
        var sb = [];
        for(var i = 0; i < s.length; i++) {
            sb.push(this.StandardChar(s[i]));
        }
        return sb.join('');
    }

    /**
     * Compares a string with a character for equality after standardization.
     * @param {string} left - The string to compare.
     * @param {string} right - The character to compare with.
     * @returns {boolean} True if the string and character are equal after standardization, false otherwise.
     */
    static Equals(left, right) {
        if(left.length != 1) return false;
        if(left[0] != right) {
            var a = this.StandardChar(left[0]);
            if(a != right) return false;
        }
        return true;
    }

    /**
     * Compares two strings for equality after standardization.
     * @param {string} left - The first string to compare.
     * @param {string} right - The second string to compare.
     * @returns {boolean} True if the strings are equal after standardization, false otherwise.
     */
    static EqualsStrings(left, right) {
        if(left.length != right.length) return false;
        for(var i = 0; i < left.length; i++) {
            if(left[i] != right[i]) {
                var a = this.StandardChar(left[i]);
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
    static Equals3(left, arg1, arg2) {
        if(this.EqualsStrings(left, arg1)) return true;
        if(this.EqualsStrings(left, arg2)) return true;
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
    static Equals4(left, arg1, arg2, arg3) {
        if(this.EqualsStrings(left, arg1)) return true;
        if(this.EqualsStrings(left, arg2)) return true;
        if(this.EqualsStrings(left, arg3)) return true;
        return false;
    }
}
