import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

// 模拟 C# Math.Round(x, 0, MidpointRounding.AwayFromZero)
function roundAwayFromZero(x) {
    return x < 0 ? -Math.round(-x) : Math.round(x);
}

class Function_ROUND extends Function_2 {
    get Name() {
        return "Round";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        if (this.b === null || this.b === undefined) {
            return Operand.Create(roundAwayFromZero(args1.NumberValue));
        }
        let args2 = this.getNumber_2(engine, tempParameter);
        if (args2.IsError) { return args2; }
        let digits = args2.IntValue;
        if (digits < -15 || digits > 15) {
            return this.parameterError(2);
        }
        let num = args1.NumberValue;
        if (digits >= 0) {
            let factor = Math.pow(10, digits);
            return Operand.Create(roundAwayFromZero(num * factor) / factor);
        }
        // Math.Round(decimal, int) 只支持非负位数,负数位数(向左取整)改用先除后乘
        let factor = Math.pow(10, -digits);
        return Operand.Create(roundAwayFromZero(num / factor) * factor);
    }
}

export { Function_ROUND };
