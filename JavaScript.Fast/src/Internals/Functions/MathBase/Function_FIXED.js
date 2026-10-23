import { Function_3 } from '../Function_3.js';
import { Operand } from '../../../Operand.js';

// 模拟 C# Math.Round(x, 0, MidpointRounding.AwayFromZero)
function roundAwayFromZero(x) {
    return x < 0 ? -Math.round(-x) : Math.round(x);
}

class Function_FIXED extends Function_3 {
    get Name() {
        return "Fixed";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let num = 2;
        if (this.b !== null && this.b !== undefined) {
            let args2 = this.getNumber_2(engine, tempParameter);
            if (args2.IsError) { return args2; }
            num = args2.IntValue;
            // Excel 支持负数 decimals(向左取整),如 FIXED(1234.567,-1)="1,230",范围与 ROUND 一致
            if (num < -15 || num > 15) {
                return this.parameterError(2);
            }
        }

        let s = args1.NumberValue;
        if (num >= 0) {
            let factor = Math.pow(10, num);
            s = roundAwayFromZero(s * factor) / factor;
        } else {
            // Math.Round(decimal, int) 只支持非负位数,负数位数(向左取整)改用先除后乘
            let factor = Math.pow(10, -num);
            s = roundAwayFromZero(s / factor) * factor;
        }
        let no = false;
        if (this.c !== null && this.c !== undefined) {
            let args3 = this.getBoolean_3(engine, tempParameter);
            if (args3.IsError) { return args3; }
            no = args3.BooleanValue;
        }
        if (no === false) {
            // 负数位数取整后无小数位,用 0 位小数保持千分位
            let formatted = (num < 0 ? s.toFixed(0) : s.toFixed(num));
            let parts = formatted.split('.');
            parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
            return Operand.Create(parts.join('.'));
        }
        return Operand.Create(s.toString());
    }
}

export { Function_FIXED };
