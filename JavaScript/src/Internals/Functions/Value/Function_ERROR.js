import { Function_1 } from '../Function_1';
import { Operand } from '../../../Operand.js';

class Function_ERROR extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    evaluate(engine, tempParameter) {
        const args1 = this.func1.evaluate(engine, tempParameter);
        if (args1.isNotText) {
            args1.toText();
            if (args1.isError) {
                return args1;
            }
        }
        return Operand.error(args1.textValue);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Error');
    }
}

export { Function_ERROR };
