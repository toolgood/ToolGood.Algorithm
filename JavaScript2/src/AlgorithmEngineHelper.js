import { FastParser } from './fast/FastParser.js';

export class AlgorithmEngineHelper {
    /**
     * 校验公式语法，返回语法树（手写解析器，无 antlr4 依赖）。
     *
     * 约定与 ANTLR 版完全一致：
     * - 空串 / 全空白 → null
     * - 未传 errorListener → null
     * - 有语法错误时通过 errorListener.syntaxError(...) 上报，**仍然返回**语法树
     *   （ANTLR 的错误恢复同样返回一棵残缺树）
     *
     * @param {string} exp 公式
     * @param {{syntaxError:Function}} [errorListener] 错误监听器
     * @returns {{type:string,start:number,end:number}|null}
     */
    static ParseFormula(exp, errorListener) {
        if (!exp || exp.trim() === '') { return null; }
        if (!errorListener) { return null; }

        const parser = new FastParser(exp, errorListener);
        return parser.parse();
    }
}
