import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_LN extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "Ln");
            if (args1.IsError) { return args1; }
        }
        let z = args1.NumberValue;
        if (z <= 0) {
            return Operand.Error(StringCache.Function_parameter_error, "Ln");
        }
        return Operand.Create(Math.log(z));
    }
}

export { Function_LN };

