import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../StringCache.js';

class Function_LOWER extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_1_error, 'Lower');
            if (args1.IsError) {
                return args1;
            }
        }
        return Operand.Create(args1.TextValue.toLowerCase());
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Lower');
    }
}

export { Function_LOWER };
