import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_VALUE extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNumber) {
            return args1;
        }
        if (args1.isBoolean) {
            return args1.BooleanValue ? Operand.one : Operand.zero;
        }
        if (args1.IsNotText) {
            args1.ToText('Function {0} parameter is error!', 'Value');
            if (args1.IsError) {
                return args1;
            }
        }

        let TextValue = args1.TextValue;
        let parsedValue = parseFloat(TextValue);
        if (!isNaN(parsedValue)) {
            return Operand.Create(parsedValue);
        }
        return Operand.error('Function {0} parameter is error!', 'Value');
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Value');
    }
}

export { Function_VALUE };
