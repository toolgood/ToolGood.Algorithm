import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_DEGREES extends Function_1 {
    get Name() {
        return "Degrees";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.GetNumber_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let z = args1.DoubleValue;
        let r = (z / Math.PI * 180);
        return Operand.Create(r);
    }
}

export { Function_DEGREES };

