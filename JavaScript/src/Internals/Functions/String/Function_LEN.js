import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_LEN extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.isNotText) {
            args1.toText('Function {0} parameter is error!', 'Len');
            if (args1.isError) {
                return args1;
            }
        }
        return Operand.create(args1.textValue.length);
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Len');
    }
}

export { Function_LEN };
