import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_ODD extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "Odd");
            if (args1.IsError) { return args1; }
        }
        let z = args1.NumberValue;
        if (Math.abs(z % 2) == 1) { return args1; }
        z = Math.ceil(z);
        if (z % 2 == 1) { return Operand.Create(z); }
        z++;
        return Operand.Create(z);
    }
}

export { Function_ODD };

