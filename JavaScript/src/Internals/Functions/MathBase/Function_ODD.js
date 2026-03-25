import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ODD extends Function_1 {
    get Name() {
        return "Odd";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let n = args1.NumberValue;
        if (n == 0) {
            return Operand.Create(1);
        }

        if (n > 0) {
            if (Math.floor(n) == n && Math.abs(n) % 2 == 1) {
                return args1;
            }
            n = Math.ceil(n);
            if (Math.abs(n) % 2 == 1) {
                return Operand.Create(n);
            }
            n++;
            return Operand.Create(n);
        } else {
            if (Math.floor(n) == n && Math.abs(n) % 2 == 1) {
                return args1;
            }
            n = Math.floor(n);
            if (Math.abs(n) % 2 == 1) {
                return Operand.Create(n);
            }
            n--;
            return Operand.Create(n);
        }
    }
}

export { Function_ODD };

