/**
 * 错误监听器。
 *
 * 原实现继承 ANTLR 的 ErrorListener，现已去掉 antlr4 依赖，
 * 但**方法签名保持完全一致**，调用方（含 HtmlEditor）无需改动。
 */
export class AntlrErrorListener {
    constructor() {
    }

    syntaxError(recognizer, offendingSymbol, line, column, msg, e) {
        this.SyntaxError(recognizer, offendingSymbol, line, column, msg, e);
    }

    SyntaxError(recognizer, offendingSymbol, line, column, msg, e) {
    }

    reportAmbiguity(recognizer, dfa, startIndex, stopIndex, exact, ambigAlts, configs) {
    }

    reportAttemptingFullContext(recognizer, dfa, startIndex, stopIndex, conflictingAlts, configs) {
    }

    reportContextSensitivity(recognizer, dfa, startIndex, stopIndex, prediction, configs) {
    }
}
