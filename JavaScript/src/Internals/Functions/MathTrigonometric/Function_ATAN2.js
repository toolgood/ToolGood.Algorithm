import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_ATAN2 extends Function_2 {
    constructor(z) {
    super(z);
  }

    Evaluate(work, tempParameter) {
        let args1 = this.a.Evaluate(work, tempParameter);
            args1 = args1.ToNumber(StringCache.Function_parameter_error, 'Atan2', 1);
            if (args1.IsError) {
                return args1;
            }
        let args2 = this.b.Evaluate(work, tempParameter);
            args2 = args2.ToNumber(StringCache.Function_parameter_error, 'Atan2', 2);
            if (args2.IsError) {
                return args2;
            }
        return Operand.Create(Math.atan2(args1.NumberValue, args2.NumberValue));
    }
}

export { Function_ATAN2 };

