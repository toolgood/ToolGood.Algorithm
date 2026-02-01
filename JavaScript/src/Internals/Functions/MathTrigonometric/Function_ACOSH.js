import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_ACOSH extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.a.Evaluate(work, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, 'Acosh');
            if (args1.IsError) {
                return args1;
            }
        }
        let z = args1.NumberValue;
        if (z < 1) {
            return Operand.Error(StringCache.Function_parameter_error, 'Acosh');
        }
        return Operand.Create(Math.acosh(z));
    }
}

export { Function_ACOSH };

