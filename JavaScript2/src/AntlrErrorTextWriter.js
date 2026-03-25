import ErrorListener from './antlr4/error/ErrorListener.js';

/**
 * AntlrErrorTextWriter
 */
export class AntlrErrorListener extends ErrorListener {
    constructor() {
        super();
    }
    syntaxError(recognizer, offendingSymbol, line, column, msg, e) {
        this.SyntaxError(recognizer, offendingSymbol, line, column, msg, e);
    }
    SyntaxError(recognizer, offendingSymbol, line, column, msg, e) {
    }
}
