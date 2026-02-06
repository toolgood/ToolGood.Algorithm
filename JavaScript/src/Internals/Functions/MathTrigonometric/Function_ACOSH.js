import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ACOSH extends Function_1 {
    get Name() {
        return "Acosh";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.GetNumber_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let z = args1.DoubleValue;
        if (z < 1) {
            return this.FunctionError();
        }
        return Operand.Create(Math.acosh(z));
    }
}

export { Function_ACOSH };

