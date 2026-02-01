import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_FACT extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "Fact");
            if (args1.IsError) { return args1; }

        let z = args1.IntValue;
        if (z < 0) {
            return Operand.Error(StringCache.Function_parameter_error, "Fact");
        }
        let d = 1;
        for (let i = 1; i <= z; i++) {
            d *= i;
        }
        return Operand.Create(d);
    }
}

export { Function_FACT };

