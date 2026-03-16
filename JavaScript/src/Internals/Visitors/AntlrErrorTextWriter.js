import ErrorListener from './../../antlr4/error/ErrorListener.js';

/**
 * AntlrErrorTextWriter
 */
export class AntlrErrorTextWriter extends ErrorListener {
    constructor() {
        super();
        this.IsError = false;
        this.ErrorMsg = null;
    }
    syntaxError(recognizer, offendingSymbol, line, column, msg, e) {
        this.IsError = true;
        this.ErrorMsg = msg;
    }
}
