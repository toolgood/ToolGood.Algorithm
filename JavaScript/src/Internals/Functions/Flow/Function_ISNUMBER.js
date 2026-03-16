import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ISNUMBER extends Function_1 {
    get Name() {
        return "IsNumber";
    }

    constructor(a) {
        super(a);
    }

    evaluate(engine, tempParameter) {
        let args1 = this.a.evaluate(engine, tempParameter);
        if (args1.IsNumber) {
            return Operand.True;
        }
        return Operand.False;
    }
}

export { Function_ISNUMBER };

