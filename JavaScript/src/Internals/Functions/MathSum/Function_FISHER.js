import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_FISHER extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToNumber(StringCache.Function_parameter_1_error, 'Fisher');
            if (args1.IsError) {
                return args1;
            }
        let x = args1.NumberValue;
        if (x >= 1 || x <= -1) {
            return Operand.Error(StringCache.Function_parameter_1_error, 'Fisher');
        }
        let n = 0.5 * Math.log((1 + x) / (1 - x));
        return Operand.Create(n);
    }
}

export { Function_FISHER };

