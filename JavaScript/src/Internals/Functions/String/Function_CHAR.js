import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_CHAR extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.a.Evaluate(work, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, 'Char');
            if (args1.IsError) {
                return args1;
            }
        }
        let c = String.fromCharCode(args1.IntValue);
        return Operand.Create(c);
    }
}

export { Function_CHAR };

