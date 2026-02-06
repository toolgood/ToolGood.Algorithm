import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_ATAN2 extends Function_2 {
    get Name() {
        return "Atan2";
    }

    constructor(z) {
        super(z);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.GetNumber_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        let args2 = this.GetNumber_2(work, tempParameter);
        if (args2.IsError) { return args2; }
        return Operand.Create(Math.atan2(args1.NumberValue, args2.NumberValue));
    }
}

export { Function_ATAN2 };

