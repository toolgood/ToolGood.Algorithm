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
     */
    wildcardIndexOf(text, pattern, startIndex) {
        const regex = new RegExp(this.wildcardToRegex(pattern), 'ig');
        regex.lastIndex = startIndex;
        const match = regex.exec(text);
        if (!match) { return -1; }
        return match.index;
    }

    wildcardToRegex(pattern) {
        const sb = [];
        for (let i = 0; i < pattern.length; i++) {
            const c = pattern[i];
            if (c === '~') {
                if (i + 1 < pattern.length && (pattern[i + 1] === '?' || pattern[i + 1] === '*' || pattern[i + 1] === '~')) {
                    sb.push(this.escapeRegExpChar(pattern[i + 1]));
                    i++;
                } else {
                    sb.push(this.escapeRegExpChar('~'));
                }
            } else if (c === '*') {
                sb.push('[\\s\\S]*');
            } else if (c === '?') {
                sb.push('[\\s\\S]');
            } else {
                sb.push(this.escapeRegExpChar(c));
            }
        }
        return sb.join('');
    }

    escapeRegExpChar(c) {
        return /[.*+?^${}()|[\]\\]/.test(c) ? '\\' + c : c;
    }
}

export { Function_SEARCH };

