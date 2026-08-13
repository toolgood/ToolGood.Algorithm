import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_MULTINOMIAL extends Function_N {
    get Name() {
        return "Multinomial";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let args = [];
        let error = this.tryEvaluateAll(engine, tempParameter, args);
        if (error != null) { return error; }

        let list = [];
        for (let arg of args) {
            if (arg.IsNotNumber) {
                return this.functionError();
            }
            list.push(arg.NumberValue);
        }

        let sum = 0;
        let n = 1;
        for (let i = 0; i < list.length; i++) {
            let value = list[i];
            if (value < -2147483648 || value > 2147483647) {
                return this.parameterError(i + 1);
            }
            let a = Math.trunc(value); // (int) 向零截断,对齐 C#
            if (a < 0) {
                return this.parameterError(i + 1);
            }
            n *= this.calculateFactorial(a);
            if (!isFinite(n)) {
                return this.functionError();
            }
            sum += a;
        }

        let r = this.calculateFactorial(sum) / n;
        if (!isFinite(r)) {
            return this.functionError();
        }
        return Operand.Create(r);
    }

    // 计算阶乘
    calculateFactorial(n) {
        if (n <= 1) return 1;
        let result = 1;
        for (let i = 2; i <= n; i++) {
            result *= i;
        }
        return result;
    }
}

export { Function_MULTINOMIAL };

