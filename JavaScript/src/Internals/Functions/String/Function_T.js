import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_T extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.isText) {
            return args1;
        }
        return Operand.Create('');
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'T');
    }
}

export { Function_T };
