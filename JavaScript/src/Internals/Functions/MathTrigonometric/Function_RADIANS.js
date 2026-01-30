import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_RADIANS extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, 'Radians');
            if (args1.IsError) {
                return args1;
            }
        }
        let r = args1.DoubleValue / 180 * Math.PI;
        return Operand.Create(r);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Radians');
    }
}

export { Function_RADIANS };
