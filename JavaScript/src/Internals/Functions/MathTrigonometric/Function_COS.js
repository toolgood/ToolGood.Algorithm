import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_COS extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.a.Evaluate(work, tempParameter);
            args1 = args1.ToNumber(StringCache.Function_parameter_error, 'Cos');
            if (args1.IsError) {
                return args1;
            }
        return Operand.Create(Math.cos(args1.NumberValue));
    }
}

export { Function_COS };

