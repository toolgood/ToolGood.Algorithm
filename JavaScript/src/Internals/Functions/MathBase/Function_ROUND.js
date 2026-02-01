import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_ROUND extends Function_2 {
    constructor(z) {
    super(z);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "Round", 1);
            if (args1.IsError) { return args1; }

        if (this.b === null) {
            return Operand.Create(Math.round(args1.NumberValue));
        }
        let args2 = this.b.Evaluate(engine, tempParameter);
            args2 = args2.ToNumber(StringCache.Function_parameter_error, "Round", 2);
            if (args2.IsError) { return args2; }
        let decimalPlaces = args2.IntValue;
        let factor = Math.pow(10, decimalPlaces);
        return Operand.Create(Math.round(args1.NumberValue * factor) / factor);
    }
}

export { Function_ROUND };

