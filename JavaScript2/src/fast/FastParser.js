import { FastLexer, T } from './FastLexer.js';

/**
 * 手写递归下降语法分析器，等价替代 ANTLR 生成的 mathjsParser（对应 g4/mathjs.g4）。
 *
 * 文法（mathjs.g4）只有 **一级二元优先级**，比 math.g4 简单得多：
 *   expr: '(' expr ')' | expr '.' PARAMETER [args] | expr '[' (PARAMETER|expr) ']'
 *       | '!' expr | expr '%' | expr (MOD|SUB|OP) expr | expr '?' expr ':' expr
 *       | PARAMETER [args] | '{' arrayJson '}' | '[' expr ']' | SUB? NUM | STRING | PARAMETER
 *
 * 优先级（高 → 低）：`.` / `[` 后缀 > `!` > `%` 后缀 > 二元运算 > `? :`
 *
 * 与 ANTLR 版的行为对齐点：
 * - **不抛异常**：语法/词法错误通过 listener.syntaxError 上报，随后尽量恢复并继续，
 *   最终仍返回一个语法树对象（ANTLR 的错误恢复同样会返回一棵残缺树）；
 * - `1+`、`(2+3`、`[]`、`{}` 之类的输入都是「有错误但返回结果」。
 */
const PREC_PERCENT = 4;
const PREC_BIN = 2;
const PREC_TERNARY = 1;

const CLOSERS = new Set([T.RPAREN, T.RBRACKET, T.RBRACE, T.COMMA, T.COLON, T.EOF]);

export class FastParser {
    /**
     * @param {string} text 原始公式
     * @param {{syntaxError:Function}} [listener] 错误回调
     */
    constructor(text, listener) {
        this.text = text || '';
        this.listener = listener || null;
        this.tokens = new FastLexer(this.text, listener).tokenize();
        this.index = 0;
    }

    /**
     * 解析整条公式，始终返回一个节点对象（语法错误会先上报）。
     * @returns {{type:string,start:number,end:number}}
     */
    parse() {
        const root = this._expr(0);
        if (!this._isEof()) {
            this._report(this._cur().start, `Unexpected token '${this._tokenText(this._cur())}'.`);
            this._skipToEnd();
        }
        return root;
    }

    // ------------------------------------------------------------------ 表达式

    _expr(minPrecedence) {
        let left;
        if (this._cur().type === T.BANG) {
            // '!' 的优先级高于 '%' 后缀：!a% 解析为 (!a)%，与 ANTLR 的左递归改写一致
            this._next();
            const operand = this._expr(PREC_PERCENT + 1);
            left = this._node('Not', operand.start, operand.end, { children: [operand] });
        } else {
            left = this._primary();
            left = this._postfix(left);
        }
        return this._exprRest(left, minPrecedence);
    }

    _exprRest(left, minPrecedence) {
        while (true) {
            const token = this._cur();
            const type = token.type;

            // 后缀 '%'：后面接不上表达式时按后缀处理（否则是二元取模）
            if (type === T.MOD && !this._canStartExpression(this.index + 1)) {
                if (PREC_PERCENT < minPrecedence) return left;
                this._next();
                left = this._node('Percentage', left.start, token.end, { children: [left] });
                continue;
            }

            if (type === T.MOD || type === T.SUB || type === T.OP) {
                if (PREC_BIN < minPrecedence) return left;
                this._next();
                const right = this._expr(PREC_BIN + 1);
                left = this._node('Binary', left.start, right.end, {
                    op: this._text(token.start, token.end),
                    children: [left, right],
                });
                continue;
            }

            if (type === T.QUESTION) {
                if (PREC_TERNARY < minPrecedence) return left;
                this._next();
                const whenTrue = this._expr(0);
                this._expect(T.COLON);
                const whenFalse = this._expr(PREC_TERNARY + 1);
                left = this._node('If', left.start, whenFalse.end, { children: [left, whenTrue, whenFalse] });
                continue;
            }

            return left;
        }
    }

    /** 后缀链：expr.name / expr.name(...) / expr[index] */
    _postfix(left) {
        while (true) {
            const type = this._cur().type;
            if (type === T.DOT) {
                const dot = this._cur();
                this._next();
                if (this._cur().type !== T.PARAMETER) {
                    this._report(this._cur().start, `Unexpected token '${this._tokenText(this._cur())}', expecting a property name.`);
                    const bad = this._errorNode();
                    left = this._node('Member', left.start, bad.end, { children: [left, bad] });
                    continue;
                }
                const name = this._cur();
                this._next();
                let end = name.end;
                const children = [left];
                if (this._cur().type === T.LPAREN) {
                    end = this._arguments(children);
                }
                left = this._node('Member', left.start, end, {
                    name: this._text(name.start, name.end),
                    children,
                    startOfName: dot.start,
                });
                continue;
            }
            if (type === T.LBRACKET) {
                this._next();
                const indexNode = this._expr(0);
                const close = this._cur();
                this._expect(T.RBRACKET);
                left = this._node('Index', left.start, close.end, { children: [left, indexNode] });
                continue;
            }
            return left;
        }
    }

    _primary() {
        const token = this._cur();
        switch (token.type) {
            case T.LPAREN: {
                this._next();
                const inner = this._expr(0);
                const close = this._cur();
                this._expect(T.RPAREN);
                return this._node('Bracket', token.start, close.end, { children: [inner] });
            }
            case T.NUM: {
                this._next();
                return this._node('Number', token.start, token.end, {});
            }
            case T.SUB: {
                // 文法只允许 SUB? NUM 形式的负数字面量
                if (this._peek(1).type === T.NUM) {
                    const num = this._peek(1);
                    this._next();
                    this._next();
                    return this._node('Number', token.start, num.end, {});
                }
                this._report(token.start, `Unexpected token '${this._tokenText(token)}', expecting a number.`);
                this._next();
                return this._node('Error', token.start, token.end, {});
            }
            case T.STRING: {
                this._next();
                return this._node('String', token.start, token.end, {});
            }
            case T.PARAMETER: {
                this._next();
                if (this._cur().type === T.LPAREN) {
                    const children = [];
                    const end = this._arguments(children);
                    return this._node('Call', token.start, end, {
                        name: this._text(token.start, token.end),
                        children,
                    });
                }
                return this._node('Parameter', token.start, token.end, {
                    name: this._text(token.start, token.end),
                });
            }
            case T.LBRACE:
                return this._arrayJson();
            case T.LBRACKET:
                return this._array();
            default:
                return this._errorNode();
        }
    }

    /** '(' (expr (',' expr)*)? ')' —— 允许零个参数（与 math.g4 不同） */
    _arguments(children) {
        this._expect(T.LPAREN);
        if (this._cur().type !== T.RPAREN) {
            children.push(this._expr(0));
            while (this._cur().type === T.COMMA) {
                this._next();
                children.push(this._expr(0));   // 参数不允许尾逗号（与 mathjs.g4 一致）
            }
        }
        const close = this._cur();
        this._expect(T.RPAREN);
        return close.end;
    }

    /** '[' expr (',' expr)* ','? ']' —— 至少一个元素，即 [] 非法 */
    _array() {
        const open = this._cur();
        this._next();
        const children = [this._expr(0)];
        while (this._cur().type === T.COMMA) {
            this._next();
            if (this._cur().type === T.RBRACKET) break;
            children.push(this._expr(0));
        }
        const close = this._cur();
        this._expect(T.RBRACKET);
        return this._node('Array', open.start, close.end, { children });
    }

    /** '{' arrayJson (',' arrayJson)* ','? '}' —— 至少一项，即 {} 非法 */
    _arrayJson() {
        const open = this._cur();
        this._next();
        const children = [this._arrayJsonItem()];
        while (this._cur().type === T.COMMA) {
            this._next();
            if (this._cur().type === T.RBRACE) break;
            children.push(this._arrayJsonItem());
        }
        const close = this._cur();
        this._expect(T.RBRACE);
        return this._node('ArrayJson', open.start, close.end, { children });
    }

    /** arrayJson: (NUM | STRING | PARAMETER) ':' expr */
    _arrayJsonItem() {
        const key = this._cur();
        if (key.type !== T.NUM && key.type !== T.STRING && key.type !== T.PARAMETER) {
            const bad = this._errorNode();
            return bad;
        }
        this._next();
        this._expect(T.COLON);
        const value = this._expr(0);
        return this._node('ArrayJsonItem', key.start, value.end, {
            name: this._text(key.start, key.end),
            children: [value],
        });
    }

    // ------------------------------------------------------------------ 工具

    _cur() {
        return this.tokens[this.index];
    }

    _peek(offset) {
        const i = this.index + offset;
        return i < this.tokens.length ? this.tokens[i] : this.tokens[this.tokens.length - 1];
    }

    _isEof() {
        return this._cur().type === T.EOF;
    }

    _next() {
        if (this.index + 1 < this.tokens.length) this.index++;
    }

    _expect(type) {
        if (this._cur().type !== type) {
            this._report(this._cur().start, `Unexpected token '${this._tokenText(this._cur())}'.`);
            return;
        }
        this._next();
    }

    /** 出错时跳过一个 Token；遇到结束符 / 分隔符则不消费，交给上层匹配 */
    _errorNode() {
        const token = this._cur();
        this._report(token.start, token.type === T.EOF
            ? 'Unexpected end of expression.'
            : `Unexpected token '${this._tokenText(token)}'.`);
        if (!CLOSERS.has(token.type)) this._next();
        return this._node('Error', token.start, token.end, {});
    }

    _skipToEnd() {
        while (!this._isEof()) this._next();
    }

    _text(start, end) {
        if (end < start) return '';
        return this.text.substring(start, end + 1);
    }

    _tokenText(token) {
        if (token.type === T.EOF) return '<EOF>';
        return this._text(token.start, token.end);
    }

    _node(type, start, end, extra) {
        const node = { type, start, end };
        if (extra) {
            if (extra.children) node.children = extra.children;
            if (extra.op !== undefined) node.op = extra.op;
            if (extra.name !== undefined) node.name = extra.name;
            if (extra.startOfName !== undefined) node.startOfName = extra.startOfName;
        }
        return node;
    }

    /** 从 tokenIndex 开始的词法单元能否作为一个表达式的开头（用于区分 '%' 后缀与二元） */
    _canStartExpression(tokenIndex) {
        if (tokenIndex >= this.tokens.length) return false;
        const token = this.tokens[tokenIndex];
        switch (token.type) {
            case T.LPAREN:
            case T.LBRACKET:
            case T.LBRACE:
            case T.NUM:
            case T.STRING:
            case T.PARAMETER:
            case T.BANG:
                return true;
            case T.SUB:
                return this.tokens[tokenIndex + 1] !== undefined
                    && this.tokens[tokenIndex + 1].type === T.NUM;
            default:
                return false;
        }
    }

    /** 1 基行号 + 0 基列号，与 ANTLR 的报错位置一致 */
    _report(index, message) {
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
}
