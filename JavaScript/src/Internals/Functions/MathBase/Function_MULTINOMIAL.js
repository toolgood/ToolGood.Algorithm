import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_MULTINOMIAL extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    Evaluate(engine, tempParameter) {
        let args = [];
        for (let item of this.funcs) {
            let aa = item.Evaluate(engine, tempParameter);
            if (aa.IsError) { return aa; }
            args.push(aa);
        }

        let list = [];
        for (let arg of args) {
            if (arg.IsNotNumber) {
                return Operand.Error("Function '{0}' parameter is error!", "Multinomial");
            }
            list.push(arg.NumberValue);
        }

        let sum = 0;
        let n = 1;
        for (let value of list) {
            let a = Math.max(0, Math.floor(value)); // 小于等于0时按0处理
            n *= Function_MULTINOMIAL.factorial(a);
            sum += a;
        }

        let r = Function_MULTINOMIAL.factorial(sum) / n;
        return Operand.Create(r);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Multinomial");
    }

    // 计算阶乘
    static factorial(n) {
        if (n <= 1) return 1;
        let result = 1;
        for (let i = 2; i <= n; i++) {
            result *= i;
        }
        return result;
    }
}

export { Function_MULTINOMIAL };
