import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_ROUNDUP extends Function_2 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "RoundUp", 1);
            if (args1.IsError) { return args1; }
        let args2 = this.b.Evaluate(engine, tempParameter);
            args2 = args2.ToNumber(StringCache.Function_parameter_error, "RoundUp", 2);
            if (args2.IsError) { return args2; }
        if (args1.NumberValue == 0.0) { return args1; }
        let a = Math.pow(10, args2.IntValue);
        let b = args1.NumberValue;

        let t = (Math.ceil(Math.abs(b) * a)) / a;
        if (b > 0) return Operand.Create(t);
        return Operand.Create(-t);
    }
}

export { Function_ROUNDUP };

