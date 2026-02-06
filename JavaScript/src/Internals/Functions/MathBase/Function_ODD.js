import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ODD extends Function_1 {
    get Name() {
        return "Odd";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let n = args1.NumberValue;
        if (n == 0) {
            return Operand.Create(1);
        }
        if (n > 0) {
            return Operand.Create(Math.ceil(n / 2) * 2 - 1);
        }
        return Operand.Create(Math.floor(n / 2) * 2 + 1);
    }
}

export { Function_ODD };

