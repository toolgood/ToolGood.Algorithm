import { Function_1 } from '../Function_1.js';
import { StringCache } from '../../../Internals/StringCache.js';
import { Operand } from '../../../Operand.js';

class Function_Percentage extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "Percentage");
            if (args1.IsError) { return args1; }
        return Operand.Create(args1.NumberValue / 100.0);
    }
}

export { Function_Percentage };

