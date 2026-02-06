import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';


class Function_NOT extends Function_1 {
    get Name() {
        return "Not";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetBoolean_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        return args1.BooleanValue ? Operand.False : Operand.True;
    }
}

export { Function_NOT };

