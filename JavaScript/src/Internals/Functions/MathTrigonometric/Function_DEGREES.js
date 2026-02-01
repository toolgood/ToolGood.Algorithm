import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_DEGREES extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.a.Evaluate(work, tempParameter);
            args1 = args1.ToNumber(StringCache.Function_parameter_error, 'Degrees');
            if (args1.IsError) {
                return args1;
            }
        let z = args1.NumberValue;
        let r = (z / Math.PI * 180);
        return Operand.Create(r);
    }
}

export { Function_DEGREES };

