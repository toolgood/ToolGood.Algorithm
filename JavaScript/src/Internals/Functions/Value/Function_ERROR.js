import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_ERROR extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1.ToText();
            if (args1.IsError) {
                return args1;
            }
        }
        return Operand.error(args1.TextValue);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Error');
    }
}

export { Function_ERROR };
