import CharStream from './antlr4/CharStream.js';
import Token from './antlr4/Token.js';

/**
 * This class supports case-insensitive lexing by wrapping an existing
 * char stream and forcing the lexer to see either upper or
 * lowercase characters. Grammar literals should then be either upper or
 * lower case such as 'BEGIN' or 'begin'. The text of the character
 * stream is unaffected. Example: input 'BeGiN' would match lexer rule
 * 'BEGIN' if constructor parameter upper=true but getText() would return
 * 'BeGiN'.
 */
export class AntlrCharStream extends CharStream {
    constructor(data, decodeToUnicodeCodePoints) {
		super(data, decodeToUnicodeCodePoints);
	}
     LA(offset) {
         if (offset === 0) {
             return 0; // undefined
         }
         if (offset < 0) {
             offset += 1; // e.g., translate LA(-1) to use offset=0
         }
         let pos = this._index + offset - 1;
         if (pos < 0 || pos >= this._size) { // invalid
             return Token.EOF;
         }
         let c=this.data[pos];
        return AntlrCharStream.standardChar(String.fromCharCode(c)).charCodeAt(0);
     }
    static standardChar(o) {
        if (typeof o !== 'string' || o.length !== 1) return o;
        let charCode = o.charCodeAt(0);
        if (charCode >= 65 && charCode <= 90) return o;
        if (charCode <= 127) return o.toUpperCase();
        if (charCode <= 65280) {
            if (o == '×') return '*';//215
            if (o == '÷') return '/';//247
            if (o == '‘') return '\'';//8216
            if (o == '’') return '\'';//8217
            if (o == '"') return '"';//8220
            if (o == '"') return '"';//8221
            if (charCode == 12288) return String.fromCharCode(32);
            if (o == '【') return '[';//12304
            if (o == '】') return ']';//12305
            if (o == '〔') return '(';//12308
            if (o == '〕') return ')';//12309
            return o;
        } else if (charCode < 65375) {
            o = String.fromCharCode(charCode - 65248);
        }
        return o.toUpperCase();
    }
}
