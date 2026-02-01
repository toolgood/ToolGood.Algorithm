import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_EVEN extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "Even");
            if (args1.IsError) { return args1; }
        let z = args1.NumberValue;
        if (z % 2 == 0) { return args1; }
        z = Math.ceil(z);
        if (z % 2 == 0) { return Operand.Create(z); }
        z++;
        return Operand.Create(z);
    }
}

export { Function_EVEN };

