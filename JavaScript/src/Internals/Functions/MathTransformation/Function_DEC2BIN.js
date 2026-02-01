import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_DEC2BIN extends Function_2 {
    constructor(z) {
    super(z);
  }

    Evaluate(work, tempParameter) {
        let args1 = this.a.Evaluate(work, tempParameter);
            args1 = args1.ToNumber(StringCache.Function_parameter_error, 'DEC2BIN', 1);
            if (args1.IsError) {
                return args1;
            }
        let num = Math.floor(args1.NumberValue).toString(2);
        if (this.b !== null) {
            let args2 = this.b.Evaluate(work, tempParameter);
                args2 = args2.ToNumber(StringCache.Function_parameter_error, 'DEC2BIN', 2);
                if (args2.IsError) {
                    return args2;
                }
            if (num.length > args2.IntValue) {
                return Operand.Error(StringCache.Function_parameter_error, 'DEC2BIN', 2);
            }
            return Operand.Create(num.padStart(args2.IntValue, '0'));
        }
        return Operand.Create(num);
    }
}

export { Function_DEC2BIN };

