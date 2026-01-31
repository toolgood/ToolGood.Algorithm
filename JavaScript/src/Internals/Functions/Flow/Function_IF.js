import { Function_3 } from '../Function_3.js';
import { StringCache } from '../../../Internals/StringCache.js';
import { Operand } from '../../../Operand.js';

class Function_IF extends Function_3 {
    constructor(func1, func2, func3) {
        super(func1, func2, func3);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotBoolean) {
            args1 = args1.ToBoolean(StringCache.Function_parameter_error, "If", 1);
            if (args1.IsError) { return args1; }
        }
        if (args1.BooleanValue) {
            return this.func2.Evaluate(engine, tempParameter);
        }
        if (this.func3 === null) {
            return Operand.Create(false);
        }
        return this.func3.Evaluate(engine, tempParameter);
    }
}

export { Function_IF };

