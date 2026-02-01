import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_POWER extends Function_2 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "Power", 1);
            if (args1.IsError) { return args1; }
        }
        let args2 = this.b.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber(StringCache.Function_parameter_error, "Power", 2);
            if (args2.IsError) { return args2; }
        }
        return Operand.Create(Math.pow(args1.NumberValue, args2.NumberValue));
    }
}

export { Function_POWER };

