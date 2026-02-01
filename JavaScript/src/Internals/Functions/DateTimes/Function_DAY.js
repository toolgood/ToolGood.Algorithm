import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_DAY extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
        if (args1.IsNotDate) {
            args1 = args1.ToMyDate(StringCache.Function_parameter_error, "Day");
            if (args1.IsError) { return args1; }
        }
        try {
            return Operand.Create(args1.DateValue.ToDateTime().getDate());
        } catch (e) {
            return Operand.Error(StringCache.Function_error, "Day");
        }
    }
}

export { Function_DAY };

