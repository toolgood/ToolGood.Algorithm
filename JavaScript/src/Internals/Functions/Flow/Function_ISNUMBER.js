import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ISNUMBER extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
        if (args1.IsNumber) {
            return Operand.Create(true);
        }
        return Operand.Create(false);
    }
}

export { Function_ISNUMBER };

