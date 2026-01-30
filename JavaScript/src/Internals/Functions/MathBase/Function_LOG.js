import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_LOG extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error2, "Log", 1);
            if (args1.IsError) { return args1; }
        }
        if (this.func2 !== null) {
            let args2 = this.func2.Evaluate(engine, tempParameter);
            if (args2.IsNotNumber) {
                args2 = args2.ToNumber(StringCache.Function_parameter_error2, "Log", 2);
                if (args2.IsError) { return args2; }
            }
            return Operand.Create(Math.log(args1.DoubleValue) / Math.log(args2.DoubleValue));
        }
        return Operand.Create(Math.log10(args1.DoubleValue));
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, "Log");
    }
}

export { Function_LOG };
