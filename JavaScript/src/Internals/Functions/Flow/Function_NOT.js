import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_NOT extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotBoolean) {
            args1 = args1.ToBoolean(StringCache.Function_parameter_error, "Not");
            if (args1.IsError) { return args1; }
        }
        return Operand.Create(!args1.BooleanValue);
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Not");
    }
}

export { Function_NOT };
