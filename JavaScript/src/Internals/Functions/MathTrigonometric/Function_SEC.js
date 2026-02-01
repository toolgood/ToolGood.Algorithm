import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_SEC extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.a.Evaluate(work, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, 'Sec');
            if (args1.IsError) {
                return args1;
            }
        }
        let d = Math.cos(args1.NumberValue);
        if (d === 0) {
            return Operand.Error(StringCache.Function_error, 'Sec');
        }
        return Operand.Create(1.0 / d);
    }
}

export { Function_SEC };

