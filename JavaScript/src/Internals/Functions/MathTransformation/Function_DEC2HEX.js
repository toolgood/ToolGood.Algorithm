import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_DEC2HEX extends Function_2 {
    constructor(z) {
    super(z);
  }

    Evaluate(work, tempParameter) {
        let args1 = this.a.Evaluate(work, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, 'DEC2HEX', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        let num = parseInt(args1.IntValue).toString(16).toUpperCase();
        if (this.b !== null) {
            let args2 = this.b.Evaluate(work, tempParameter);
            if (args2.IsNotNumber) {
                args2 = args2.ToNumber(StringCache.Function_parameter_error, 'DEC2HEX', 2);
                if (args2.IsError) {
                    return args2;
                }
            }
            if (num.length > args2.IntValue) {
                return Operand.Create(num.padLeft(args2.IntValue, '0'));
            }
            return Operand.Error(StringCache.Function_parameter_error, 'DEC2HEX', 2);
        }
        return Operand.Create(num);
    }
}

export { Function_DEC2HEX };

