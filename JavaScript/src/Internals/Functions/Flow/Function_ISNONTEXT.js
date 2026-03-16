import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ISNONTEXT extends Function_1 {
    get Name() {
        return "IsNonText";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.a.evaluate(engine, tempParameter);
        if (!args1.IsText) {
            return Operand.True;
        }
        return Operand.False;
    }
}

export { Function_ISNONTEXT };

