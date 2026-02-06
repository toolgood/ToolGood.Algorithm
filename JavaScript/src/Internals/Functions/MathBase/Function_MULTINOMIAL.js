import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_MULTINOMIAL extends Function_N {
    get Name() {
        return "Multinomial";
    }

    constructor(z) {
        super(z);
    }

    Evaluate(engine, tempParameter) {
        let args = [];
        for (let item of this.z) {
            let aa = item.Evaluate(engine, tempParameter);
            if (aa.IsError) { return aa; }
            args.push(aa);
        }

        let list = [];
        for (let arg of args) {
            if (arg.IsNotNumber) {
                return this.FunctionError();
            }
            list.push(arg.NumberValue);
        }

        let sum = 0;
        let n = 1;
        for (let i = 0; i < list.length; i++) {
            let a = Math.floor(list[i]); // 小于等于0 时，按0处理
            n *= this.calculateFactorial(a);
            sum += a;
        }

        let r = this.calculateFactorial(sum) / n;
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

