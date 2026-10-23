import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_VALUE extends Function_1 {
    get Name() {
        return "Value";
    }

    constructor(a) {
        super(a);
    }

    evaluate(work, tempParameter) {
        let args1 = this.getText_1(work, tempParameter);
        if (args1.IsError) {
            return args1;
        }

        let parsedValue = this.parseNumber(args1.TextValue);
        if (isNaN(parsedValue)) {
            return this.parameterError(1);
        }
        return Operand.Create(parsedValue);
    }

    // 与 C# decimal.TryParse(NumberStyles.Any, InvariantCulture) 对齐的解析逻辑
    parseNumber(text) {
        // AllowLeadingWhite | AllowTrailingWhite
        let t = text.trim();
        if (t.length === 0) {
            return NaN;
        }
        let negative = false;
        // AllowParentheses:括号表示负数
        if (t.startsWith('(') && t.endsWith(')')) {
            negative = true;
            t = t.substring(1, t.length - 1).trim();
        }
        // AllowCurrencySymbol(InvariantCulture 货币符号 ¤)
        if (t.startsWith('¤')) {
            t = t.substring(1).trim();
        }
        // AllowLeadingSign
        if (t.startsWith('+')) {
            t = t.substring(1);
        } else if (t.startsWith('-')) {
            negative = !negative;
            t = t.substring(1);
        }
        t = t.trim();
        // AllowTrailingSign
        if (t.endsWith('-')) {
            negative = !negative;
            t = t.substring(0, t.length - 1).trim();
        } else if (t.endsWith('+')) {
            t = t.substring(0, t.length - 1).trim();
        }
        // AllowThousands:千分位逗号直接忽略
        t = t.replace(/,/g, '');
        // 严格校验数字格式(AllowDecimalPoint | AllowExponent),避免 parseFloat 部分解析
        if (!/^[+-]?(\d+\.?\d*|\.\d+)([eE][+-]?\d+)?$/.test(t)) {
            return NaN;
        }
        let num = parseFloat(t);
        if (isNaN(num)) {
            return NaN;
        }
        return negative ? -num : num;
    }
}

export { Function_VALUE };
