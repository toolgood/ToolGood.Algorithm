/**
 * AntlrErrorTextWriter
 */
export class AntlrErrorTextWriter {
    /**
     * @param {Object} formatProvider
     */
    constructor(formatProvider = null) {
        this.IsError = false;
        this.ErrorMsg = null;
    }

    /**
     * Gets the encoding.
     * @returns {string}
     */
    get Encoding() {
        return 'UTF-8';
    }

    /**
     * Writes a line of text.
     * @param {string} value
     */
    WriteLine(value) {
        this.IsError = true;
        this.ErrorMsg = value;
    }
    syntaxError(recognizer, offendingSymbol, line, column, msg, e) {
        this.IsError = true;
        this.ErrorMsg = msg;
    }

    reportAttemptingFullContext(recognizer, dfa, startIndex, stopIndex, conflictingAlts, conflictState) {
        // 空实现，因为我们不需要处理这个事件
    }

    reportContextSensitivity(recognizer, dfa, startIndex, stopIndex, prediction, acceptState) {
        // 空实现，因为我们不需要处理这个事件
    }

    reportAmbiguity(recognizer, dfa, startIndex, stopIndex, exact, ambigAlts, configs) {
        // 空实现，因为我们不需要处理这个事件
    }

}
