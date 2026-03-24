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
        for (let item of this.z) {
            let aa = item.evaluate(engine, tempParameter);
            if (aa.IsError) { return aa; }
            args.push(aa);
        }

        let list = [];
        for (let i = 0; i < args.length; i++) {
            let arg = args[i];
            if (arg.IsNumber==false) {
                return this.functionError();
            }
            let a = Math.floor(arg.NumberValue);
            if (a < 0) {
                return this.parameterError(i + 1);
            }
            list.push(a);
        }

        let sum = 0;
        let n = 1;
        for (let i = 0; i < list.length; i++) {
            let a = list[i];
            n *= this.calculateFactorial(a);
            sum += a;
        }

        let r = this.calculateFactorial(sum) / n;
        return Operand.Create(r);
    }

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

