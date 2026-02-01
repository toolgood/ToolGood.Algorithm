import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_LOG extends Function_2 {
    constructor(funcs) {
    super(funcs);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "Log", 1);
            if (args1.IsError) { return args1; }
        }
        if (this.func2 !== null) {
            let args2 = this.func2.Evaluate(engine, tempParameter);
            if (args2.IsNotNumber) {
                args2 = args2.ToNumber(StringCache.Function_parameter_error, "Log", 2);
                if (args2.IsError) { return args2; }
            }
            return Operand.Create(Math.log(args1.NumberValue) / Math.log(args2.NumberValue));
        }
        return Operand.Create(Math.log10(args1.NumberValue));
    }
}

export { Function_LOG };

