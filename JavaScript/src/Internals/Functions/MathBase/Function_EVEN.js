import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_EVEN extends Function_1 {
    get Name() {
        return "Even";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.getNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        if (args1.IsNone) { return args1; }

        let n = args1.NumberValue;
        if (n == 0) {
            return Operand.Zero;
        }

        if (n > 0) {
            if (Math.floor(n) == n && Math.abs(n) % 2 == 0) {
                return args1;
            }
            n = Math.ceil(n);
            if (Math.abs(n) % 2 == 0) {
                return Operand.Create(n);
            }
            n++;
            return Operand.Create(n);
        } else {
            if (Math.floor(n) == n && Math.abs(n) % 2 == 0) {
                return args1;
            }
            n = Math.floor(n);
            if (Math.abs(n) % 2 == 0) {
                return Operand.Create(n);
            }
            n--;
            return Operand.Create(n);
        }
    }
}

export { Function_EVEN };

