import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_ROUNDDOWN extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsNotNumber) {
            args1 = args1.ToNumber(StringCache.Function_parameter_error, "RoundDown", 1);
            if (args1.IsError) { return args1; }
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotNumber) {
            args2 = args2.ToNumber(StringCache.Function_parameter_error, "RoundDown", 2);
            if (args2.IsError) { return args2; }
        }
        if (args1.NumberValue == 0.0) {
            return args1;
        }
        let a = Math.pow(10, args2.IntValue);
        let b = args1.NumberValue;

        let result = b >= 0 ? Math.floor(b * a) / a : Math.ceil(b * a) / a;
        return Operand.Create(result);
    }
}

export { Function_ROUNDDOWN };

