import { CharUtil } from './CharUtil.js';

/**
 * This class supports case-insensitive lexing by wrapping an existing
 * char stream and forcing the lexer to see either upper or
 * lowercase characters. Grammar literals should then be either upper or
 * lower case such as 'BEGIN' or 'begin'. The text of the character
 * stream is unaffected. Example: input 'BeGiN' would match lexer rule
 * 'BEGIN' if constructor parameter upper=true but getText() would return
 * 'BeGiN'.
 */
export class AntlrCharStream {
    /**
     * Constructs a new CaseChangingCharStream wrapping the given stream forcing
     * all characters to upper case or lower case.
     * @param {Object} stream The stream to wrap.
     */
    constructor(stream) {
        this.stream = stream;
    }

    /**
     * @returns {number}
     */
    get Index() {
        return this.stream.Index;
    }

    /**
     * @returns {number}
     */
    get Size() {
        return this.stream.Size;
    }

    /**
     * @returns {string}
     */
    get SourceName() {
        return this.stream.SourceName;
    }

    /**
     * Consumes the current character.
     */
    Consume() {
        this.stream.Consume();
    }

    /**
     * Gets the text from the specified interval.
     * @param {Object} interval
     * @returns {string}
     */
    GetText(interval) {
        return this.stream.GetText(interval);
    }

    /**
     * Gets the character at the specified offset.
     * @param {number} i
     * @returns {number}
     */
    LA(i) {
        var c = this.stream.LA(i);

        if (c <= 0) {
            return c;
        }
        return CharUtil.StandardChar(String.fromCharCode(c)).charCodeAt(0);
    }

    /**
     * Marks the current position in the stream.
     * @returns {number}
     */
    Mark() {
        return this.stream.Mark();
    }

    /**
     * Releases the specified marker.
     * @param {number} marker
     */
    Release(marker) {
        this.stream.Release(marker);
    }

    /**
     * Seeks to the specified position in the stream.
     * @param {number} index
     */
    Seek(index) {
        this.stream.Seek(index);
    }
}
