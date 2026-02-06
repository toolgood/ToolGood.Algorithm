import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_VALUE extends Function_1 {
    get Name() {
        return "Value";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.a.Evaluate(work, tempParameter);
        if (args1.IsNumber) {
            return args1;
        }
        if (args1.isBoolean) {
            return args1.BooleanValue ? Operand.one : Operand.zero;
        }
            args1 = args1.ToText(this.FunctionError, 'Value');
            if (args1.IsError) {
                return args1;
            }

        let TextValue = args1.TextValue;
        let parsedValue = parseFloat(TextValue);
        if (!isNaN(parsedValue)) {
            return Operand.Create(parsedValue);
        }
        return Operand.Error(this.FunctionError, 'Value');
    }
}

export { Function_VALUE };

