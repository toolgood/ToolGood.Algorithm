import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_QUOTIENT extends Function_2 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "Quotient", 1);
            if (args1.IsError) { return args1; }
        let args2 = this.b.Evaluate(engine, tempParameter);
            args2 = args2.ToNumber(StringCache.Function_parameter_error, "Quotient", 2);
            if (args2.IsError) { return args2; }

        if (args2.NumberValue == 0) {
            return Operand.Error(StringCache.Function_error, "Quotient");
        }
        return Operand.Create(Math.floor(args1.NumberValue / args2.NumberValue));
    }
}

export { Function_QUOTIENT };

