import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_GCD extends Function_N {
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
                return Operand.Error("Function {0} parameter is error!", "Gcd");
            }
            list.push(arg.NumberValue);
        }

        if (list.length === 0) {
            return Operand.Error("Function {0} parameter is error!", "Gcd");
        }

        return Operand.Create(Function_GCD.calculateGCD(list));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Gcd");
    }

    // 计算多个数字的最大公约数
    static calculateGCD(numbers) {
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

        // 对数组中的所有数字应用GCD
        return numbers.reduce((acc, current) => gcdTwo(acc, current), numbers[0]);
    }
}

export { Function_GCD };
