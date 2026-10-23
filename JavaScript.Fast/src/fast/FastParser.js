/**
 * 手写递归下降语法分析器 —— 等价替换 Antlr4 生成的 mathParser.js。
 *
 * 对应 JS 版文法（math.g4 的 JS 变体）：expr 是左递归规则，Antlr 会改写成带优先级
 * 的循环，这里用优先级爬升还原同一套优先级：
 *   后缀(.name( / [..] / .name) > ! > %(后缀) > * / % > + - & > 比较 > 等值 > && > || > ?:
 *
 * 输出是轻量 AST（普通对象），由 FastAstBuilder / FastTreeBuilder 转成最终结果。
 */
import { FastLexer, T_EOF, T_NUM, T_STR, T_ID, T_NULL } from './FastLexer.js';

const PREC_UNARY = 14;
const PREC_PERCENT = 13;
const PREC_MUL = 12;
const PREC_ADD = 11;
const PREC_REL = 10;
const PREC_EQ = 9;
const PREC_AND = 8;
const PREC_OR = 7;
const PREC_IF = 6;

const MAX_DEPTH = 5000;

function binaryPrecedence(op) {
    switch (op) {
        case '*': case '/': case '%': return PREC_MUL;
        case '+': case '-': case '&': return PREC_ADD;
        case '>': case '>=': case '<': case '<=': return PREC_REL;
        case '=': case '!=': return PREC_EQ;
        case '&&': return PREC_AND;
        case '||': return PREC_OR;
        case '?': return PREC_IF;
        default: return 0;
    }
}

export class FastParser {
    constructor(text) {
        this.text = text;
        this.tokens = [];
        this.i = 0;
        this.depth = 0;
    }

    /** 解析公式，失败抛 Error */
    static parse(text) {
        const p = new FastParser(text);
        p.tokens = new FastLexer(text).tokenize();
        const ast = p.parseExpr(0);
        if (p.cur.t !== T_EOF) { throw p.unexpected(); }
        return ast;
    }

    // ---------------------------------------------------------------- 语法

    parseExpr(minPrec) {
        if (++this.depth > MAX_DEPTH) {
            throw new Error(`Expression is nested too deeply (more than ${MAX_DEPTH} levels).`);
        }
        let left;
        const tk = this.cur;

        if (tk.t === '!') {
            this.next();
            const operand = this.parseExpr(PREC_UNARY);
            left = { k: 'not', s: tk.s, e: operand.e, a: operand };
        } else if (tk.t === '(') {
            this.next();
            const inner = this.parseExpr(0);
            this.expect(')');
            left = { k: 'bracket', s: inner.s, e: inner.e, a: inner };
        } else {
            left = this.parsePostfix(this.parsePrimary());
        }

        left = this.parseBinaryRest(left, minPrec);
        this.depth--;
        return left;
    }

    parseBinaryRest(left, minPrec) {
        while (true) {
            const tk = this.cur;
            let prec = binaryPrecedence(tk.t);
            let postfixPercent = false;
            if (tk.t === '%' && !this.canStartExpression(this.i + 1)) {
                prec = PREC_PERCENT;
                postfixPercent = true;
            }
            if (prec === 0 || prec < minPrec) { return left; }

            this.next();
            if (postfixPercent) {
                left = { k: 'pct', s: left.s, e: tk.e, a: left };
                continue;
            }
            if (tk.t === '?') {
                // 与 Antlr 生成代码一致：中间项按 0 级解析、末项按 prec+1 解析（左结合）
                const whenTrue = this.parseExpr(0);
                this.expect(':');
                const whenFalse = this.parseExpr(prec + 1);
                left = { k: 'if', s: left.s, e: whenFalse.e, a: left, b: whenTrue, c: whenFalse };
                continue;
            }
            const right = this.parseExpr(prec + 1);
            left = { k: 'bin', op: tk.t, s: left.s, e: right.e, a: left, b: right };
        }
    }

    parsePostfix(left) {
        while (true) {
            const tk = this.cur;
            if (tk.t === '.') {
                this.next();
                const name = this.cur;
                if (name.t !== T_ID) { throw this.unexpected(); }
                this.next();
                if (this.cur.t === '(') {
                    const r = this.parseArgs(name, left);
                    left = r;
                } else {
                    left = { k: 'prop', s: left.s, e: name.e, a: left, name: this.slice(name) };
                }
            } else if (tk.t === '[') {
                this.next();
                const index = this.parseExpr(0);
                this.expect(']');
                left = { k: 'idx', s: left.s, e: index.e, a: left, b: index };
            } else {
                return left;
            }
        }
    }

    parsePrimary() {
        const tk = this.cur;
        switch (tk.t) {
            case T_NUM:
                this.next();
                return { k: 'num', s: tk.s, e: tk.e };

            case '-': {
                // 文法只允许 OPSUB? NUM 形式的负数字面量
                this.next();
                if (this.cur.t !== T_NUM) { throw this.unexpected(); }
                const num = this.cur;
                this.next();
                return { k: 'num', s: tk.s, e: num.e };
            }
            case T_STR:
                this.next();
                return { k: 'str', s: tk.s, e: tk.e };

            case T_NULL:
                this.next();
                return { k: 'null', s: tk.s, e: tk.e };

            case T_ID: {
                this.next();
                if (this.cur.t === '(') {
                    return this.parseArgs(tk, null);
                }
                return { k: 'id', s: tk.s, e: tk.e, name: this.slice(tk) };
            }
            case '{':
                return this.parseJson();
            case '[':
                return this.parseArray();
            default:
                throw this.unexpected();
        }
    }

    /** 解析 (args)；recv 非空表示 expr.name(...) 形式，接收者作为第一个参数 */
    parseArgs(nameTok, recv) {
        this.expect('(');
        const args = [];
        if (recv !== null) { args.push(recv); }
        if (this.cur.t !== ')') {
            args.push(this.parseExpr(0));
            while (this.cur.t === ',') {
                this.next();
                args.push(this.parseExpr(0));
            }
        }
        this.expect(')');
        return {
            k: 'call',
            s: recv !== null ? recv.s : nameTok.s,
            e: this.lastEnd,
            name: this.slice(nameTok),
            nameS: nameTok.s,
            nameE: nameTok.e,
            args: args,
        };
    }

    parseArray() {
        const tk = this.cur;
        this.next();
        const items = [this.parseExpr(0)];
        while (this.cur.t === ',') {
            this.next();
            if (this.cur.t === ']') { break; }   // 允许尾逗号
            items.push(this.parseExpr(0));
        }
        this.expect(']');
        return { k: 'array', s: tk.s, e: this.lastEnd, items: items };
    }

    parseJson() {
        const tk = this.cur;
        this.next();
        const items = [];
        while (true) {
            const key = this.cur;
            if (key.t !== T_NUM && key.t !== T_STR && key.t !== T_ID && key.t !== T_NULL) {
                throw this.unexpected();
            }
            this.next();
            this.expect(':');
            const value = this.parseExpr(0);
            items.push({ k: 'jsonitem', s: key.s, e: value.e, key: this.slice(key), a: value });
            if (this.cur.t !== ',') { break; }
            this.next();
            if (this.cur.t === '}') { break; }   // 允许尾逗号
        }
        this.expect('}');
        return { k: 'json', s: tk.s, e: this.lastEnd, items: items };
    }

    // ---------------------------------------------------------------- 辅助

    get cur() { return this.tokens[this.i]; }

    get lastEnd() {
        return this.i > 0 ? this.tokens[this.i - 1].e : -1;
    }

    next() {
        if (this.i + 1 < this.tokens.length) { this.i++; }
    }

    expect(t) {
        if (this.cur.t !== t) { throw this.unexpected(); }
        this.next();
    }

    slice(tok) {
        return tok.e < tok.s ? '' : this.text.substring(tok.s, tok.e + 1);
    }

    unexpected() {
        const tk = this.cur;
        if (tk.t === T_EOF) {
            return new Error(`Unexpected end of expression at position ${this.text.length}.`);
        }
        return new Error(`Unexpected token '${this.slice(tk)}' at position ${tk.s}.`);
    }

    /** 从 tokenIndex 开始能否作为一个表达式的开头 */
    canStartExpression(tokenIndex) {
        if (tokenIndex >= this.tokens.length) { return false; }
        const tk = this.tokens[tokenIndex];
        switch (tk.t) {
            case '(': case T_NUM: case T_STR: case T_ID: case T_NULL:
            case '!': case '{': case '[':
                return true;
            case '-':
                return tokenIndex + 1 < this.tokens.length && this.tokens[tokenIndex + 1].t === T_NUM;
            default:
                return false;
        }
    }
}
