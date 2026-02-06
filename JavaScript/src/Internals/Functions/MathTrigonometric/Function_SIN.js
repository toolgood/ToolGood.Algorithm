import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_SIN extends Function_1 {
    get Name() {
        return "Sin";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.GetNumber_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        return Operand.Create(Math.sin(args1.DoubleValue));
    }
}

export { Function_SIN };

