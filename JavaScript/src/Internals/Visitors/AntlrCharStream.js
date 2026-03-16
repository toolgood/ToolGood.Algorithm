import { CharUtil } from './CharUtil.js';
import CharStream from './../../antlr4/CharStream.js';
import Token from './../../antlr4/Token.js';

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
        return CharUtil.standardChar(String.fromCharCode(c)).charCodeAt(0);
     }
}
