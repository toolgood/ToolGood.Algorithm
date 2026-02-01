import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_LEFT extends Function_2 {
    constructor(funcs) {
    super(funcs);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotText) {
            args1 = args1.ToText(StringCache.Function_parameter_error, 'Left', 1);
            if (args1.IsError) {
                return args1;
            }
        }
        if (args1.TextValue.length === 0) {
            return Operand.Create('');
        }
        if (this.func2 === null) {
            return Operand.Create(args1.TextValue.substring(0, 1));
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber(StringCache.Function_parameter_error, 'Left', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        let length = Math.min(args2.IntValue, args1.TextValue.length);
        return Operand.Create(args1.TextValue.substring(0, length));
    }
}

export { Function_LEFT };

