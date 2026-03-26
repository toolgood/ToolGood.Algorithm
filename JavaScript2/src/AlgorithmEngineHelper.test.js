import { AlgorithmEngineHelper } from './AlgorithmEngineHelper.js';

// 简单的错误监听器实现
class TestErrorListener {
    constructor() {
        this.errors = [];
    }
    syntaxError(recognizer, offendingSymbol, line, column, msg, e) {
        this.errors.push({ line, column, msg });
    }
    reportAmbiguity(recognizer, dfa, startIndex, stopIndex, exact, ambigAlts, configs) {}
    reportAttemptingFullContext(recognizer, dfa, startIndex, stopIndex, conflictingAlts, configs) {}
    reportContextSensitivity(recognizer, dfa, startIndex, stopIndex, prediction, configs) {}
}

describe('AlgorithmEngineHelper', () => {
    describe('ParseFormula', () => {
        test('should return null for empty expression', () => {
            const errorListener = new TestErrorListener();
            const result = AlgorithmEngineHelper.ParseFormula('', errorListener);
            expect(result).toBeNull();
        });

        test('should return null for whitespace-only expression', () => {
            const errorListener = new TestErrorListener();
            const result = AlgorithmEngineHelper.ParseFormula('   ', errorListener);
            expect(result).toBeNull();
        });

        test('should return null when no error listener provided', () => {
            const result = AlgorithmEngineHelper.ParseFormula('1+1');
            expect(result).toBeNull();
        });

        test('should parse valid expression successfully', () => {
            const errorListener = new TestErrorListener();
            const result = AlgorithmEngineHelper.ParseFormula('1+1', errorListener);
            expect(result).toBeDefined();
            expect(errorListener.errors.length).toBe(0);
        });

        test('should parse complex expression successfully', () => {
            const errorListener = new TestErrorListener();
            const result = AlgorithmEngineHelper.ParseFormula('(2+3)*4/2', errorListener);
            expect(result).toBeDefined();
            expect(errorListener.errors.length).toBe(0);
        });

        test('should report syntax error for invalid expression', () => {
            const errorListener = new TestErrorListener();
            const result = AlgorithmEngineHelper.ParseFormula('1+', errorListener);
            expect(result).toBeDefined(); // 即使有错误，也会返回结果
            expect(errorListener.errors.length).toBeGreaterThan(0);
        });
    });
});
