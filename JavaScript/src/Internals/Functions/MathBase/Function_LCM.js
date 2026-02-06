import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_LCM extends Function_N {
    get Name() {
        return "Lcm";
    }

    constructor(z) {
        super(z);
    }

    Evaluate(engine, tempParameter) {
        let args = [];
        for (let i = 0; i < this.z.length; i++) {
            let aa = this.GetNumber_1(engine, tempParameter, i);
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

        return Operand.Create(this.calculateLCM(list));
    }

    // 计算多个数字的最小公倍数
    calculateLCM(numbers) {
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

