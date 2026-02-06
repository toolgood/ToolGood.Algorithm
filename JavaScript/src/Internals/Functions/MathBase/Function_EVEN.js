import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_EVEN extends Function_1 {
    get Name() {
        return "Even";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        let n = args1.NumberValue;
        if (n == 0) {
            return Operand.Zero;
        }
        if (n > 0) {
            return Operand.Create(Math.ceil(n / 2) * 2);
        }
        return Operand.Create(Math.floor(n / 2) * 2);
    }
}

export { Function_EVEN };

