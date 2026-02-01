import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_SQRT extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "Sqrt");
            if (args1.IsError) { return args1; }
        }
        if (args1.NumberValue < 0) {
            return Operand.Error(StringCache.Function_parameter_error, "Sqrt");
        }
        return Operand.Create(Math.sqrt(args1.NumberValue));
    }
}

export { Function_SQRT };

