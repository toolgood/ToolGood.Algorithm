import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_PERMUT extends Function_2 {
    constructor(funcs) {
    super(funcs);
  }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "Permut", 1);
            if (args1.IsError) { return args1; }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber(StringCache.Function_parameter_error, "Permut", 2);
            if (args2.IsError) { return args2; }
        }

        let total = args1.IntValue;
        let count = args2.IntValue;

        let sum = 1;
        for (let i = 0; i < count; i++) {
            sum *= (total - i);
        }
        return Operand.Create(sum);
    }
}

export { Function_PERMUT };

