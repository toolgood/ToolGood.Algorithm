import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_NOT extends Function_1 {
    constructor(a) {
        super(a);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToBoolean(StringCache.Function_parameter_error, "Not");
            if (args1.IsError) { return args1; }
        return Operand.Create(!args1.BooleanValue);
    }
}

export { Function_NOT };

