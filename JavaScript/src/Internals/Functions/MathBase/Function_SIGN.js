import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_SIGN extends Function_1 {
    constructor(func1) {
        super(func1);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "Sign");
            if (args1.IsError) { return args1; }
        }
        return Operand.Create(Math.sign(args1.NumberValue));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Sign");
    }
}

export { Function_SIGN };
