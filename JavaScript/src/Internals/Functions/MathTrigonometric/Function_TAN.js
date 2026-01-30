import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_TAN extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, 'Tan');
            if (args1.IsError) {
                return args1;
            }
        }
        return Operand.Create(Math.tan(args1.DoubleValue));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Tan');
    }
}

export { Function_TAN };
