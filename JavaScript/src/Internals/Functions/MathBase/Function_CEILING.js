import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_CEILING extends Function_2 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "Ceiling", 1);
            if (args1.IsError) { return args1; }
        }

        if (this.b === null) {
            return Operand.Create(Math.ceil(args1.NumberValue));
        }

        let args2 = this.b.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber(StringCache.Function_parameter_error, "Ceiling", 2);
            if (args2.IsError) { return args2; }
        }
        let b = args2.NumberValue;
        if (b == 0) {
            return Operand.Create(0);
        }
        if (b < 0) {
            return Operand.Error(StringCache.Function_parameter_error, "Ceiling", 2);
        }

        let a = args1.NumberValue;
        let d = Math.ceil(a / b) * b;
        return Operand.Create(d);
    }
}

export { Function_CEILING };

