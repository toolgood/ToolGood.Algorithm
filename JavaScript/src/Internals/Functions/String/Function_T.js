import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_T extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.isText) {
            return args1;
        }
        return Operand.create('');
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'T');
    }
}

export { Function_T };
