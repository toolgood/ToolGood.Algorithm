/**
 * 手写词法分析器，等价替代 ANTLR 生成的 mathjsLexer（对应 g4/mathjs.g4）。
 *
 * 与 ANTLR 版的关键点保持一致：
 * - 逐字符按 standardChar 归一化（大小写 / 全角 / ×÷ 等符号），再做词法判定，
 *   因此 "ｓｕｍ（1,2）" 这类全角写法与半角等价；
 * - 中文字符属于 FullWidthLetter，可以直接作标识符（如 "瓦楞"）；
 * - WS / 块注释 / 行注释直接跳过，不产生 Token。
 */

export const T = {
    EOF: 0,
    Invalid: -1,
    LPAREN: 1,
    RPAREN: 2,
    LBRACKET: 3,
    RBRACKET: 4,
    LBRACE: 5,
    RBRACE: 6,
    COMMA: 7,
    DOT: 8,
    QUESTION: 9,
    COLON: 10,
    BANG: 11,
    MOD: 12,   // '%'
    SUB: 13,   // '-'
    OP: 14,    // 其它二元运算符（保留原文）
    NUM: 15,
    STRING: 16,
    PARAMETER: 17,
};

/** 与 AntlrCharStream.standardChar 等价的字符归一化 */
export function standardChar(ch) {
    const code = ch.charCodeAt(0);
    if (code >= 65 && code <= 90) return ch;          // 'A'-'Z'
    if (code <= 127) return ch.toUpperCase();
    if (code <= 65280) {
        switch (ch) {
            case '\u00d7': return '*';   // ×
            case '\u00f7': return '/';   // ÷
            case '\u2018':
            case '\u2019': return '\'';  // ‘ ’
            case '\u201c':
            case '\u201d': return '"';   // “ ”
            case '\u3000': return ' ';   // 全角空格
            case '\u3010': return '[';   // 【
            case '\u3011': return ']';   // 】
            case '\u3014': return '(';   // 〔
            case '\u3015': return ')';   // 〕
            default: return ch;          // 含 CJK：保持原样
        }
    }
    if (code < 65375) return String.fromCharCode(code - 65248).toUpperCase();
    return ch.toUpperCase();
}

function isDigit(c) {
    return c >= '0' && c <= '9';
}

/** 归一化后的字符是否可作标识符首字符（[A-Z_] 或 FullWidthLetter） */
function isIdentifierStart(c) {
    if (typeof c !== 'string') return false;          // 越界时 _ch 返回 -1
    if ((c >= 'A' && c <= 'Z') || c === '_') return true;
    const code = c.charCodeAt(0);
    return (code >= 0x00c0 && code <= 0x00d6)
        || (code >= 0x00d8 && code <= 0x00f6)
        || (code >= 0x00f8 && code <= 0x00ff)
        || (code >= 0x0100 && code <= 0x1fff)
        || (code >= 0x2c00 && code <= 0x2fff)
        || (code >= 0x3040 && code <= 0x318f)
        || (code >= 0x3300 && code <= 0x337f)
        || (code >= 0x3400 && code <= 0x3fff)
        || (code >= 0x4e00 && code <= 0x9fff)
        || (code >= 0xa000 && code <= 0xd7ff)
        || (code >= 0xf900 && code <= 0xfaff)
        || (code >= 0xff00 && code <= 0xfff0);
}

function isIdentifierPart(c) {
    return isDigit(c) || isIdentifierStart(c);
}

export class FastLexer {
    /**
     * @param {string} text 原始公式
     * @param {{syntaxError:Function}} [listener] 词法错误回调
     */
    constructor(text, listener) {
        this.text = text || '';
        this.listener = listener || null;
        this.pos = 0;
    }

    /** 归一化后的字符；越界返回 -1 */
    _ch(index) {
        if (index < 0 || index >= this.text.length) return -1;
        return standardChar(this.text.charAt(index));
    }

    /** 报告词法错误（位置换算成 1 基行号 + 0 基列号，与 ANTLR 一致） */
    _error(index, message) {
        if (!this.listener || typeof this.listener.syntaxError !== 'function') return;
        let line = 1;
        let lineStart = 0;
        for (let i = 0; i < index && i < this.text.length; i++) {
            if (this.text.charAt(i) === '\n') {
                line++;
                lineStart = i + 1;
            }
        }
        this.listener.syntaxError(null, null, line, index - lineStart, message, null);
    }

    /**
     * 扫描全部词法单元。
     * @returns {{type:number, start:number, end:number}[]} 末尾一定带一个 EOF
     */
    tokenize() {
        const out = [];
        while (true) {
            const token = this._next();
            out.push(token);
            if (token.type === T.EOF) return out;
        }
    }

    _next() {
        const text = this.text;
        while (true) {
            if (this.pos >= text.length) {
                return { type: T.EOF, start: this.pos, end: this.pos - 1 };
            }
            const c = this._ch(this.pos);
            if (c === ' ' || c === '\n' || c === '\t' || c === '\r' || c === '\f') {
                this.pos++;
                continue;
            }
            if (c === '/') {
                const c2 = this._ch(this.pos + 1);
                if (c2 === '*') { this._blockComment(); continue; }
                if (c2 === '/') { this._lineComment(); continue; }
            }
            break;
        }

        const start = this.pos;
        const c = this._ch(start);

        if (isDigit(c)) return this._number(start);
        if (c === '\'' || c === '"' || c === '`') return this._string(start, c);
        if (isIdentifierStart(c)) return this._identifier(start);

        // 单字符 / 多字符运算符
        switch (c) {
            case '(': this.pos++; return { type: T.LPAREN, start, end: start };
            case ')': this.pos++; return { type: T.RPAREN, start, end: start };
            case '[': this.pos++; return { type: T.LBRACKET, start, end: start };
            case ']': this.pos++; return { type: T.RBRACKET, start, end: start };
            case '{': this.pos++; return { type: T.LBRACE, start, end: start };
            case '}': this.pos++; return { type: T.RBRACE, start, end: start };
            case ',': this.pos++; return { type: T.COMMA, start, end: start };
            case '.': this.pos++; return { type: T.DOT, start, end: start };
            case '?': this.pos++; return { type: T.QUESTION, start, end: start };
            case ':': this.pos++; return { type: T.COLON, start, end: start };
            case '%': this.pos++; return { type: T.MOD, start, end: start };
            case '-': this.pos++; return { type: T.SUB, start, end: start };
            case '!':
                // '!' 本身不是 OP（OP 需要 '=' 跟随），单独成 BANG
                if (this._ch(this.pos + 1) === '=') return this._op(start);
                this.pos++;
                return { type: T.BANG, start, end: start };
            case '|':
                // 文法只允许 '||'，单个 '|' 是词法错误（ANTLR 同样报 token recognition error 并跳过）
                if (this._ch(this.pos + 1) === '|') return this._op(start);
                this.pos++;
                this._error(start, `token recognition error at: '|'`);
                return this._next();
            case '+': case '*': case '/': case '&': case '>': case '<': case '=':
                return this._op(start);
            default:
                // 例如单独的 '|'（文法只允许 '||'），ANTLR 同样报 token recognition error 并跳过
                this.pos++;
                this._error(start, `token recognition error at: '${text.charAt(start)}'`);
                return this._next();
        }
    }

    _blockComment() {
        this.pos += 2;
        while (this.pos < this.text.length) {
            if (this._ch(this.pos) === '*' && this._ch(this.pos + 1) === '/') {
                this.pos += 2;
                return;
            }
            this.pos++;
        }
    }

    _lineComment() {
        this.pos += 2;
        while (this.pos < this.text.length && this._ch(this.pos) !== '\n') this.pos++;
    }

    _string(start, quote) {
        this.pos++;
        while (true) {
            if (this.pos >= this.text.length) {
                this._error(start, 'token recognition error at: \'unterminated string\'');
                return { type: T.STRING, start, end: this.text.length - 1 };
            }
            const c = this._ch(this.pos);
            this.pos++;
            if (c === quote) break;
            if (c === '\\') {
                if (this.pos >= this.text.length) {
                    this._error(start, 'token recognition error at: \'unterminated string\'');
                    return { type: T.STRING, start, end: this.text.length - 1 };
                }
                this.pos++;
            }
        }
        return { type: T.STRING, start, end: this.pos - 1 };
    }

    _number(start) {
        this.pos++;
        while (isDigit(this._ch(this.pos))) this.pos++;

        let c = this._ch(this.pos);
        if (c === '.' && isDigit(this._ch(this.pos + 1))) {
            this.pos += 2;
            while (isDigit(this._ch(this.pos))) this.pos++;
            c = this._ch(this.pos);
        }

        if (c === 'E') {
            const c2 = this._ch(this.pos + 1);
            if (isDigit(c2) || c2 === '+' || c2 === '-') {
                this.pos += 2;
                while (isDigit(this._ch(this.pos))) this.pos++;
            }
            return { type: T.NUM, start, end: this.pos - 1 };
        }

        // 单位后缀： [KDCM]? 'M' [23]? | 'M'? 'L' | 'K'? 'G' | 'T'
        if (c === 'C' || c === 'D') {
            if (this._ch(this.pos + 1) === 'M') {
                this.pos += 2;
                const c3 = this._ch(this.pos);
                if (c3 === '2' || c3 === '3') this.pos++;
            }
        } else if (c === 'M') {
            this.pos++;
            const c2 = this._ch(this.pos);
            if (c2 === 'M') {
                this.pos++;
                const c3 = this._ch(this.pos);
                if (c3 === '2' || c3 === '3') this.pos++;
            } else if (c2 === '2' || c2 === '3' || c2 === 'L') {
                this.pos++;
            }
        } else if (c === 'K') {
            const c2 = this._ch(this.pos + 1);
            if (c2 === 'M') {
                this.pos += 2;
                const c3 = this._ch(this.pos);
                if (c3 === '2' || c3 === '3') this.pos++;
            } else if (c2 === 'G') {
                this.pos += 2;
            }
        } else if (c === 'G' || c === 'T' || c === 'L') {
            this.pos++;
        }

        return { type: T.NUM, start, end: this.pos - 1 };
    }

    _identifier(start) {
        this.pos++;
        while (isIdentifierPart(this._ch(this.pos))) this.pos++;
        return { type: T.PARAMETER, start, end: this.pos - 1 };
    }

    /** 二元运算符： [>=<!] '=' '='? | 单字符 [+ * \/ & > < =] | '<>' | '&&' | '||' */
    _op(start) {
        const c = this._ch(start);
        const c2 = this._ch(start + 1);
        const c3 = this._ch(start + 2);
        if (c === '<' && c2 === '>') {
            this.pos = start + 2;
            return { type: T.OP, start, end: this.pos - 1 };
        }
        if (c === '&' && c2 === '&') {
            this.pos = start + 2;
            return { type: T.OP, start, end: this.pos - 1 };
        }
        if (c === '|' && c2 === '|') {
            this.pos = start + 2;
            return { type: T.OP, start, end: this.pos - 1 };
        }
        if ((c === '>' || c === '=' || c === '<' || c === '!') && c2 === '=') {
            this.pos = start + 2;
            if (c3 === '=') this.pos = start + 3;
            return { type: T.OP, start, end: this.pos - 1 };
        }
        // 单字符运算符： + * / & > < =
        this.pos = start + 1;
        return { type: T.OP, start, end: start };
    }
}
