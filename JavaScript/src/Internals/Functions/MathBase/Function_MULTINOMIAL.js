import { Function_N } from '../Function_N.js';

class Function_MULTINOMIAL extends Function_N {
    constructor(funcs) {
        super(funcs);
    }

    evaluate(engine, tempParameter) {
        const args = [];
        for (const item of this._args) {
            const aa = item.evaluate(engine, tempParameter);
            if (aa.IsError) { return aa; }
            args.push(aa);
        }

        const list = [];
        for (const arg of args) {
            if (arg.IsNotNumber) {
                return engine.createErrorOperand("Function '{0}' parameter is error!", "Multinomial");
            }
            list.push(arg.NumberValue);
        }

        let sum = 0;
        let n = 1;
        for (const value of list) {
            const a = Math.max(0, Math.floor(value)); // 小于等于0时按0处理
            n *= Function_MULTINOMIAL.factorial(a);
            sum += a;
        }

        const r = Function_MULTINOMIAL.factorial(sum) / n;
        return engine.createOperand(r);
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
