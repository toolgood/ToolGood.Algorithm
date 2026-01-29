import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_VALUE extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.isNumber) {
            return args1;
        }
        if (args1.isBoolean) {
            return args1.booleanValue ? Operand.one : Operand.zero;
        }
        if (args1.isNotText) {
            args1.ToText('Function {0} parameter is error!', 'Value');
            if (args1.isError) {
                return args1;
            }
        }

        const textValue = args1.textValue;
        const parsedValue = parseFloat(textValue);
        if (!isNaN(parsedValue)) {
            return Operand.Create(parsedValue);
        }
        return Operand.error('Function {0} parameter is error!', 'Value');
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Value');
    }
}

export { Function_VALUE };
