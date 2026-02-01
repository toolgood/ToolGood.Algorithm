import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_MINUTE extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToMyDate(StringCache.Function_parameter_error, "Minute");
            if (args1.IsError) { return args1; }
        try {
            return Operand.Create(args1.DateValue.getMinutes());
        } catch (e) {
            return Operand.Error(StringCache.Function_error, "Minute");
        }
    }
}

export { Function_MINUTE };

