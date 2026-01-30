import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_LCM extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(engine, tempParameter) {
        let args = [];
        for (let i = 0; i < this.funcs.length; i++) {
            let aa = this.funcs[i].Evaluate(engine, tempParameter);
            if (aa.IsError) { return aa; }
            args.push(aa);
        }

        let list = [];
        for (let arg of args) {
            if (arg.IsNotNumber) {
                return engine.createErrorOperand("Function '{0}' parameter is error!", "Lcm");
            }
            list.push(arg.NumberValue);
        }

        if (list.length === 0) {
            return engine.createErrorOperand("Function '{0}' parameter is error!", "Lcm");
        }

        return Operand.Create(Function_LCM.calculateLCM(list));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Lcm");
    }

    // 计算多个数字的最小公倍数
    static calculateLCM(numbers) {
        // 计算两个数的GCD
        let gcdTwo = (a, b) => {
            a = Math.abs(a);
            b = Math.abs(b);
            while (b !== 0) {
                let temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        };

        // 计算两个数的LCM
        let lcmTwo = (a, b) => {
            return Math.abs(a * b) / gcdTwo(a, b);
        };

        // 对数组中的所有数字应用LCM
        return numbers.reduce((acc, current) => lcmTwo(acc, current), numbers[0]);
    }
}

export { Function_LCM };
