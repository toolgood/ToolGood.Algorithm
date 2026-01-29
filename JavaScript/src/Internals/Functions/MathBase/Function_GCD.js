import { Function_N } from '../Function_N.js';

class Function_GCD extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(engine, tempParameter) {
        const args = [];
        for (let i = 0; i < this._args.length; i++) {
            const aa = this._args[i].Evaluate(engine, tempParameter);
            if (aa.IsError) { return aa; }
            args.push(aa);
        }

        const list = [];
        for (const arg of args) {
            if (arg.IsNotNumber) {
                return engine.createErrorOperand("Function '{0}' parameter is error!", "Gcd");
            }
            list.push(arg.NumberValue);
        }

        if (list.length === 0) {
            return engine.createErrorOperand("Function '{0}' parameter is error!", "Gcd");
        }

        return engine.createOperand(Function_GCD.calculateGCD(list));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Gcd");
    }

    // 计算多个数字的最大公约数
    static calculateGCD(numbers) {
        // 计算两个数的GCD
        const gcdTwo = (a, b) => {
            a = Math.abs(a);
            b = Math.abs(b);
            while (b !== 0) {
                const temp = b;
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
