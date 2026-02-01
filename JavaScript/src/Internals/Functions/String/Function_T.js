import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_T extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
        if (args1.IsText) {
            return args1;
        }
        return Operand.Create('');
    }
}

export { Function_T };

