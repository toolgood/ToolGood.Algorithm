import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_TRIM extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.isNotText) {
            args1.ToText('Function {0} parameter is error!', 'Trim');
            if (args1.isError) {
                return args1;
            }
        }
        return Operand.Create(args1.textValue.trim());
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Trim');
    }
}

export { Function_TRIM };
