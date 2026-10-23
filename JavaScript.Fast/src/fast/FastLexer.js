/**
 * 手写词法分析器 —— 等价替换 Antlr4 生成的 mathLexer.js。
 *
 * 与 C# 版 FastLexer 不同，JS 版的文法里**所有标识符都是 PARAMETER**，
 * 函数靠文本查 funcDict 分派，所以这里不需要 300 条关键字表，只有 5 类 token。
 *
 * 归一化（大小写/全角/中文 → '_'）用字符码运算实现，不产生临时字符串。
 */

import { funcDict } from './FunctionDict.js';

export const T_EOF = 0;
export const T_NUM = 1;
export const T_STR = 2;
export const T_ID = 3;
export const T_NULL = 4;

const CH_LF = 10, CH_CR = 13, CH_TAB = 9, CH_FF = 12, CH_SPACE = 32, CH_IDEO_SPACE = 0x3000;

/** 按 CharUtil.standardChar 的规则把字符码归一化，越界返回 -1 */
function stdCode(code) {
    if (code < 0) { return -1; }
    if (code === CH_CR) { return CH_LF; }
    if (code === CH_TAB || code === CH_FF) { return CH_SPACE; }
    if (code < 97) { return code; }
    if (code <= 122) { return code - 32; }
    if (code > 65280 && code < 65375) {
        let c = code - 65248;
        if (c >= 97 && c <= 122) { c -= 32; }
        return c;
    }
    if ((code >= 0xc0 && code <= 0xd6) || (code >= 0xd8 && code <= 0xf6) ||
        (code >= 0xf8 && code <= 0xff) || (code >= 0x100 && code <= 0x1fff) ||
        (code >= 0x2c00 && code <= 0x2fff) || (code >= 0x3040 && code <= 0x318f) ||
        (code >= 0x3300 && code <= 0x337f) || (code >= 0x3400 && code <= 0x3fff) ||
        (code >= 0x4e00 && code <= 0x9fff) || (code >= 0xa000 && code <= 0xd7ff) ||
        (code >= 0xf900 && code <= 0xfaff) || (code >= 0xff00 && code <= 0xfff0)) {
        return 95;
    }
    return code;
}

/** 含点号的函数名（COVARIANCE.P、NORM.S.DIST 等）必须先按整体匹配，否则会被切成 expr.name(...) */
const DOTTED_KEYWORDS = (function () {
    const set = new Set();
    for (const k of Object.keys(funcDict)) {
        if (k.indexOf('.') >= 0) { set.add(k.toUpperCase()); }
    }
    return set;
})();

function isDigit(c) { return c >= 48 && c <= 57; }
function isIdStart(c) { return (c >= 65 && c <= 90) || c === 95; }
function isIdPart(c) { return (c >= 65 && c <= 90) || c === 95 || (c >= 48 && c <= 57); }

export class FastLexer {
    constructor(text) {
        this.text = text;
        this.pos = 0;
        this.error = null;
    }

    /** 扫完整串，返回 token 数组；出错抛 Error */
    tokenize() {
        const s = this.text;
        const n = s.length;
        const out = [];
        let i = 0;

        while (i < n) {
            let c = stdCode(s.charCodeAt(i));

            if (c === CH_SPACE || c === CH_LF || c === 9 || c === 12 || s.charCodeAt(i) === CH_IDEO_SPACE) {
                i++;
                continue;
            }
            // 注释
            if (c === 47) {
                const c2 = i + 1 < n ? stdCode(s.charCodeAt(i + 1)) : -1;
                if (c2 === 42) {
                    i += 2;
                    while (i < n && !(s.charCodeAt(i) === 42 && i + 1 < n && s.charCodeAt(i + 1) === 47)) { i++; }
                    i = Math.min(n, i + 2);
                    continue;
                }
                if (c2 === 47) {
                    i += 2;
                    while (i < n && s.charCodeAt(i) !== CH_LF) { i++; }
                    continue;
                }
            }
            // 数字
            if (isDigit(c)) {
                const start = i;
                i++;
                while (i < n && isDigit(stdCode(s.charCodeAt(i)))) { i++; }
                let nc = i < n ? stdCode(s.charCodeAt(i)) : -1;
                if (nc === 46 && i + 1 < n && isDigit(stdCode(s.charCodeAt(i + 1)))) {
                    i += 2;
                    while (i < n && isDigit(stdCode(s.charCodeAt(i)))) { i++; }
                    nc = i < n ? stdCode(s.charCodeAt(i)) : -1;
                }
                if (nc === 69) {  // 'E'
                    const c2 = i + 1 < n ? stdCode(s.charCodeAt(i + 1)) : -1;
                    if (isDigit(c2) || c2 === 43 || c2 === 45) {
                        i += 2;
                        while (i < n && isDigit(stdCode(s.charCodeAt(i)))) { i++; }
                    }
                } else if (nc === 67 || nc === 68) {          // C / D  ->  CM / DM
                    if (i + 1 < n && stdCode(s.charCodeAt(i + 1)) === 77) {
                        i += 2;
                        const c3 = i < n ? stdCode(s.charCodeAt(i)) : -1;
                        if (c3 === 50 || c3 === 51) { i++; }   // 2 / 3
                    }
                } else if (nc === 77) {                        // M
                    i++;
                    const c2 = i < n ? stdCode(s.charCodeAt(i)) : -1;
                    if (c2 === 77) {
                        i++;
                        const c3 = i < n ? stdCode(s.charCodeAt(i)) : -1;
                        if (c3 === 50 || c3 === 51) { i++; }
                    } else if (c2 === 50 || c2 === 51 || c2 === 76) { i++; }
                } else if (nc === 75) {                        // K
                    const c2 = i + 1 < n ? stdCode(s.charCodeAt(i + 1)) : -1;
                    if (c2 === 77) {
                        i += 2;
                        const c3 = i < n ? stdCode(s.charCodeAt(i)) : -1;
                        if (c3 === 50 || c3 === 51) { i++; }
                    } else if (c2 === 71) { i += 2; }          // KG
                } else if (nc === 71 || nc === 84 || nc === 76) {  // G / T / L
                    i++;
                }
                out.push({ t: T_NUM, s: start, e: i - 1 });
                continue;
            }
            // 字符串
            if (c === 39 || c === 34 || c === 96) {
                const quote = c;
                const start = i;
                i++;
                let closed = false;
                while (i < n) {
                    const ci = stdCode(s.charCodeAt(i));
                    i++;
                    if (ci === quote) { closed = true; break; }
                    if (ci === 92 && i < n) { i++; }
                }
                if (!closed) { throw new Error(`Unterminated string literal at position ${start}.`); }
                out.push({ t: T_STR, s: start, e: i - 1 });
                continue;
            }
            // 标识符（含中文/全角）
            if (isIdStart(c)) {
                const start = i;
                i++;
                while (i < n && isIdPart(stdCode(s.charCodeAt(i)))) { i++; }

                // 允许带点：COVARIANCE.P / NORM.S.DIST —— 最长优先
                let e0 = i, e1 = -1, e2 = -1;
                if (i + 1 < n && stdCode(s.charCodeAt(i)) === 46 && isIdStart(stdCode(s.charCodeAt(i + 1)))) {
                    let q = i + 2;
                    while (q < n && isIdPart(stdCode(s.charCodeAt(q)))) { q++; }
                    e1 = q;
                    if (q + 1 < n && stdCode(s.charCodeAt(q)) === 46 && isIdStart(stdCode(s.charCodeAt(q + 1)))) {
                        let r = q + 2;
                        while (r < n && isIdPart(stdCode(s.charCodeAt(r)))) { r++; }
                        e2 = r;
                    }
                }
                if (e2 > 0 && DOTTED_KEYWORDS.has(s.substring(start, e2).toUpperCase())) { i = e2; }
                else if (e1 > 0 && DOTTED_KEYWORDS.has(s.substring(start, e1).toUpperCase())) { i = e1; }
                else { i = e0; }

                const raw = s.substring(start, i);
                out.push({ t: raw.toUpperCase() === 'NULL' ? T_NULL : T_ID, s: start, e: i - 1 });
                continue;
            }
            // 运算符
            const start = i;
            const op = this._readOperator(s, n, i);
            if (op === null) {
                throw new Error(`Unexpected character '${s[i]}' at position ${i}.`);
            }
            i += op[1];
            out.push({ t: op[0], s: start, e: i - 1 });
        }
        out.push({ t: T_EOF, s: n, e: n - 1 });
        return out;
    }

    /** 返回 [token 文本, 消耗字符数]；无法识别返回 null */
    _readOperator(s, n, i) {
        const c = stdCode(s.charCodeAt(i));
        const c2 = i + 1 < n ? stdCode(s.charCodeAt(i + 1)) : -1;
        const c3 = i + 2 < n ? stdCode(s.charCodeAt(i + 2)) : -1;
        switch (c) {
            case 40: return ['(', 1];
            case 41: return [')', 1];
            case 91: return ['[', 1];
            case 93: return [']', 1];
            case 123: return ['{', 1];
            case 125: return ['}', 1];
            case 44: return [',', 1];
            case 46: return ['.', 1];
            case 63: return ['?', 1];
            case 58: return [':', 1];
            case 43: return ['+', 1];
            case 45: return ['-', 1];
            case 42: return ['*', 1];
            case 47: return ['/', 1];
            case 37: return ['%', 1];
            case 33: // '!'   -> '!' | '!=' | '!=='
                if (c2 === 61) { return c3 === 61 ? ['!=', 3] : ['!=', 2]; }
                return ['!', 1];
            case 61: // '='   -> '=' | '==' | '==='
                if (c2 === 61) { return c3 === 61 ? ['=', 3] : ['=', 2]; }
                return ['=', 1];
            case 60: // '<'   -> '<' | '<=' | '<>'
                if (c2 === 61) { return ['<=', 2]; }
                if (c2 === 62) { return ['!=', 2]; }
                return ['<', 1];
            case 62: // '>'
                return c2 === 61 ? ['>=', 2] : ['>', 1];
            case 38: // '&'
                return c2 === 38 ? ['&&', 2] : ['&', 1];
            case 124: // '|'
                return c2 === 124 ? ['||', 2] : null;
            default:
                return null;
        }
    }
}
