import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_SIGN extends Function_1 {
    get Name() {
        return "Sign";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.GetNumber_1(engine, tempParameter);
        if (args1.IsError) { return args1; }
        return Operand.Create(Math.sign(args1.NumberValue));
    }
}

export { Function_SIGN };

