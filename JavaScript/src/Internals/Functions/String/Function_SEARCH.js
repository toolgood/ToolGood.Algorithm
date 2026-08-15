import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

class Function_SEARCH extends Function_3 {
    get Name() {
        return "Search";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let args2 = this.getText_2(work, tempParameter);
        if (args2.IsError) { return args2; }

        if (this.c === null || this.c === undefined) {
            let index = this.wildcardIndexOf(args2.TextValue, args1.TextValue, 0);
            if (index < 0) {
                // 未找到:Excel 模式(索引从1开始)返回错误,C# 模式(索引从0开始)返回 -1
                return work.ExcelIndex === 1 ? this.functionError() : Operand.Create(-1);
            }
            return Operand.Create(index + work.ExcelIndex);
        }
        let args3 = this.getNumber_3(work, tempParameter);
        if (args3.IsError) { return args3; }
        let startIndex = args3.IntValue - work.ExcelIndex;
        if (startIndex < 0 || startIndex >= args2.TextValue.length) {
            return this.parameterError(3);
        }
        let p2 = this.wildcardIndexOf(args2.TextValue, args1.TextValue, startIndex);
        if (p2 < 0) {
            return work.ExcelIndex === 1 ? this.functionError() : Operand.Create(-1);
        }
        return Operand.Create(p2 + work.ExcelIndex);
    }

    /**
     * 在指定起始位置起做大小写不敏感的通配符查找,支持 Excel 的 ? 与 * 及 ~ 转义。
     * 使用手写匹配器替代正则,避免每次调用都编译正则的开销。
     */
    wildcardIndexOf(text, pattern, startIndex) {
        if (startIndex < 0) { startIndex = 0; }
        if (startIndex > text.length) { return -1; }

        const tokens = this.parseWildcardPattern(pattern);
        if (tokens.length === 0) { return startIndex; }

        for (let start = startIndex; start <= text.length; start++) {
            if (this.matchWildcard(text, tokens, start)) {
                return start;
            }
        }
        return -1;
    }

    parseWildcardPattern(pattern) {
        const list = [];
        for (let i = 0; i < pattern.length; i++) {
            const c = pattern[i];
            if (c === '~') {
                if (i + 1 < pattern.length && (pattern[i + 1] === '?' || pattern[i + 1] === '*' || pattern[i + 1] === '~')) {
                    list.push({ kind: 'literal', value: pattern[i + 1].toUpperCase() });
                    i++;
                } else {
                    list.push({ kind: 'literal', value: '~' });
                }
            } else if (c === '*') {
                list.push({ kind: 'anySeq' });
            } else if (c === '?') {
                list.push({ kind: 'anyOne' });
            } else {
                list.push({ kind: 'literal', value: c.toUpperCase() });
            }
        }
        return list;
    }

    matchWildcard(text, tokens, start) {
        let ti = 0;
        let si = start;
        let starToken = -1;
        let starMatch = 0;

        while (ti < tokens.length) {
            if (si < text.length) {
                const tok = tokens[ti];
                if (tok.kind === 'literal') {
                    if (text[si].toUpperCase() === tok.value) {
                        ti++;
                        si++;
                    } else if (starToken !== -1) {
                        ti = starToken + 1;
                        si = ++starMatch;
                    } else {
                        return false;
                    }
                } else if (tok.kind === 'anyOne') {
                    ti++;
                    si++;
                } else {
                    starToken = ti;
                    starMatch = si;
                    ti++;
                }
            } else {
                break;
            }
        }

        while (ti < tokens.length && tokens[ti].kind === 'anySeq') {
            ti++;
        }
        return ti === tokens.length;
    }
}

export { Function_SEARCH };
