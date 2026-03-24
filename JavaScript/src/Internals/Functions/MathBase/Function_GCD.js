import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_GCD extends Function_N {
    get Name() {
        return "Gcd";
    }

    constructor(z) {
        super(z);
    }

    evaluate(engine, tempParameter) {
        let args = [];
        for (let i = 0; i < this.z.length; i++) {
            let aa = this.getNumber(engine, tempParameter, i);
            if (aa.IsError) { return aa; }
            args.push(aa);
        }

        let list = [];
        for (let i = 0; i < args.length; i++) {
            let arg = args[i];
            if (arg.IsNumber==false) {
                return this.functionError();
            }
            if (arg.NumberValue < 0) {
                return this.parameterError(i + 1);
            }
            list.push(arg.NumberValue);
        }

        return Operand.Create(this.calculateGCD(list));
    }

    calculateGCD(numbers) {
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

        return numbers.reduce((acc, current) => gcdTwo(acc, current), numbers[0]);
    }
}

export { Function_GCD };

